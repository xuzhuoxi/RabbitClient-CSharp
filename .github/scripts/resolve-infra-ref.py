#!/usr/bin/env python3
"""Resolve Infra-CSharp tag from Require.yml.

Usage:
  resolve-infra-ref.py REQUIRE.yml [TAG] [--strict]

Writes ref= and tag= to $GITHUB_OUTPUT.
If TAG is empty and --strict is not set, uses the last Require array item.
"""
from __future__ import annotations

import os
import sys

import yaml


def fail(message: str) -> int:
    print(f"::error::{message}")
    print(message, file=sys.stderr)
    return 1


def main() -> int:
    strict = "--strict" in sys.argv[1:]
    args = [a for a in sys.argv[1:] if a != "--strict"]
    if not args:
        return fail("用法: resolve-infra-ref.py REQUIRE.yml [TAG] [--strict]")

    path, want = args[0], (args[1].strip() if len(args) > 1 else "")
    try:
        with open(path, encoding="utf-8") as f:
            data = yaml.safe_load(f) or {}
    except FileNotFoundError:
        return fail(f"找不到 {path}，工作流已中止。")
    except yaml.YAMLError as exc:
        return fail(f"Require.yml 不是有效的 YAML：{exc}")

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

    github_output = os.environ.get("GITHUB_OUTPUT")
    if github_output:
        with open(github_output, "a", encoding="utf-8") as fh:
            fh.write(f"ref={infra}\n")
            fh.write(f"tag={selected}\n")
    print(f"Using Infra-CSharp {infra} (Require.yml Tag={selected})")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
