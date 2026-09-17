#!/usr/bin/env python3
"""Resolve Infra-CSharp ref from Require.yml.

Usage:
  resolve-infra-ref.py REQUIRE.yml --default
      CI: read Default.Infra-CSharp (branch | v*.*.* | git short SHA).
  resolve-infra-ref.py REQUIRE.yml TAG --strict
      Release: find Require[] item whose Tag equals TAG; Infra-CSharp uses the same value format.

Writes ref= (and tag= for Release) to $GITHUB_OUTPUT.

INFRA_REPO env (default xuzhuoxi/Infra-CSharp) is used when value is a branch name.
"""
from __future__ import annotations

import os
import re
import subprocess
import sys

import yaml

DEFAULT_INFRA_REPO = "xuzhuoxi/Infra-CSharp"
TAG_RE = re.compile(r"^v\d+\.\d+\.\d+(?:[-+][0-9A-Za-z.]+)?$")
SHA_RE = re.compile(r"^[0-9a-fA-F]{7,40}$")


def fail(message: str) -> int:
    print(f"::error::{message}")
    print(message, file=sys.stderr)
    return 1


def load_yaml(path: str):
    with open(path, encoding="utf-8") as f:
        return yaml.safe_load(f) or {}


def write_output(**fields: str) -> None:
    github_output = os.environ.get("GITHUB_OUTPUT")
    if not github_output:
        return
    with open(github_output, "a", encoding="utf-8") as fh:
        for key, value in fields.items():
            fh.write(f"{key}={value}\n")


def is_branch_name(name: str) -> bool:
    proc = subprocess.run(
        ["git", "check-ref-format", "--allow-onelevel", name],
        capture_output=True,
    )
    return proc.returncode == 0


def remote_has_branch(repo: str, name: str) -> bool:
    remote = f"https://github.com/{repo}.git"
    # 不要在本仓库 gitdir 下跑：checkout v7 的 includeIf 会带上 GITHUB_TOKEN。
    cwd = os.environ.get("RUNNER_TEMP") or "/tmp"
    proc = subprocess.run(
        ["git", "ls-remote", "--exit-code", "--heads", remote, f"refs/heads/{name}"],
        capture_output=True,
        text=True,
        cwd=cwd,
    )
    return proc.returncode == 0


def resolve_infra_value(raw: str, *, source: str, extra: dict[str, str] | None = None) -> int:
    raw = raw.strip()
    extra = extra or {}
    repo = (os.environ.get("INFRA_REPO") or DEFAULT_INFRA_REPO).strip()

    if TAG_RE.match(raw):
        write_output(ref=raw, kind="tag", **extra)
        print(f"Using Infra-CSharp tag {raw} ({source})")
        return 0

    if SHA_RE.match(raw):
        write_output(ref=raw, kind="sha", **extra)
        print(f"Using Infra-CSharp commit {raw} ({source})")
        return 0

    if not is_branch_name(raw):
        return fail(
            f"{source}={raw} 无效，工作流已中止。"
            "取值须为分支名、v*.*.* tag，或 git 提交短哈希。"
        )

    if not remote_has_branch(repo, raw):
        return fail(f"Infra-CSharp 仓库 {repo} 没有分支 {raw}，工作流已中止。")

    write_output(ref=raw, kind="branch", **extra)
    print(f"Using Infra-CSharp branch {raw} (latest, {source})")
    return 0


def resolve_default(data: dict) -> int:
    block = data.get("Default")
    if not isinstance(block, dict):
        return fail("Require.yml 缺少 Default 对象，工作流已中止。")
    raw = str(block.get("Infra-CSharp") or "").strip()
    if not raw:
        return fail("Require.yml 的 Default.Infra-CSharp 为空，工作流已中止。")
    return resolve_infra_value(raw, source="Require.yml Default.Infra-CSharp")


def resolve_require_tag(data: dict, want: str, strict: bool) -> int:
    items = data.get("Require") if isinstance(data, dict) else None
    if not isinstance(items, list) or not items:
        return fail("Require.yml 中没有有效的 Require 数组，工作流已中止。")

    if want:
        match = next((item for item in items if str(item.get("Tag", "")).strip() == want), None)
        if match is None:
            known = ", ".join(str(item.get("Tag", "?")) for item in items)
            return fail(f"Require.yml 中找不到 Tag 为 {want} 的项，工作流已中止。已有 Tag: {known}")
    elif strict:
        return fail("未提供要查找的 tag，工作流已中止。")
    else:
        match = items[-1]
        print(f"未指定 tag，使用 Require.yml 最后一项 Tag={match.get('Tag')}")

    selected = str(match.get("Tag") or "").strip()
    infra = str(match.get("Infra-CSharp") or "").strip()
    if not infra:
        return fail(f"Tag {selected} 缺少 Infra-CSharp 字段，工作流已中止。")

    return resolve_infra_value(
        infra,
        source=f"Require.yml Tag={selected} Infra-CSharp",
        extra={"tag": selected},
    )


def main() -> int:
    argv = sys.argv[1:]
    use_default = "--default" in argv
    strict = "--strict" in argv
    args = [a for a in argv if a not in ("--default", "--strict")]
    if not args:
        return fail("用法: resolve-infra-ref.py REQUIRE.yml [--default | TAG [--strict]]")

    path, want = args[0], (args[1].strip() if len(args) > 1 else "")
    try:
        data = load_yaml(path)
    except FileNotFoundError:
        return fail(f"找不到 {path}，工作流已中止。")
    except yaml.YAMLError as exc:
        return fail(f"Require.yml 不是有效的 YAML：{exc}")

    if not isinstance(data, dict):
        return fail("Require.yml 根节点必须是对象，工作流已中止。")

    if use_default:
        return resolve_default(data)
    return resolve_require_tag(data, want, strict)


if __name__ == "__main__":
    raise SystemExit(main())
