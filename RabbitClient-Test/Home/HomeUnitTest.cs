using System;
using System.IO;
using JLGames.Infra.Net;
using JLGames.RabbitClient;
using JLGames.RabbitClient.Home;

namespace JLGames.RabbitClientTest.Home;

[TestFixture]
public class HomeUnitTest
{
    private bool m_SavedLittleEndian;

    [SetUp]
    public void SetUp()
    {
        m_SavedLittleEndian = RabbitHomeDefaults.LittleEndian;
    }

    [TearDown]
    public void TearDown()
    {
        RabbitHomeDefaults.SetLittleEndian(m_SavedLittleEndian);
    }

    [Test]
    public void HomeSettings_StoresUrlAndKeyOptions()
    {
        var settings = new HomeSettings("http://home.example", true, true, false);
        settings.SetPublicKeyPath("pub.pem");
        settings.SetPublicKeyContent("CONTENT");

        Assert.That(settings.HomeUrl, Is.EqualTo("http://home.example"));
        Assert.IsTrue(settings.UsePost);
        Assert.IsTrue(settings.EnableKey);
        Assert.IsFalse(settings.IsPemKey);
        Assert.That(settings.PublicKeyPath, Is.EqualTo("pub.pem"));
        Assert.That(settings.PublicKeyContent, Is.EqualTo("CONTENT"));
    }

    [Test]
    public void QueryRouteInfo_SerializesPlatformAndType()
    {
        var info = new QueryRouteInfo { PlatformId = "main01", TypeName = "Rabbit-Server" };
        var json = info.ToJsonString();
        Assert.That(json, Does.Contain("main01"));
        Assert.That(json, Does.Contain("Rabbit-Server"));
    }

    [Test]
    public void HomeResponseInfo_ParsesJson()
    {
        var info = HomeResponseInfo.FromJsonString("{\"code\":404,\"value\":\"err\",\"other\":\"x\"}");
        Assert.That(info.ExtCode, Is.EqualTo(404));
        Assert.That(info.Info, Is.EqualTo("err"));
        Assert.That(info.Other, Is.EqualTo("x"));
    }

    [Test]
    public void QueryRouteBackInfo_ParsesJsonAndSkipsDecryptWhenKeyOff()
    {
        var back = QueryRouteBackInfo.FromJsonString(
            "{\"id\":\"i1\",\"pid\":\"main01\",\"type-name\":\"Rabbit-Server\",\"open-network\":\"tcp\",\"open-addr\":\"127.0.0.1:7000\",\"open-key-on\":false,\"open-sk\":\"\"}");
        Assert.That(back.Id, Is.EqualTo("i1"));
        Assert.That(back.PlatformId, Is.EqualTo("main01"));
        Assert.That(back.TypeName, Is.EqualTo("Rabbit-Server"));
        Assert.That(back.OpenNetwork, Is.EqualTo("tcp"));
        Assert.That(back.OpenAddr, Is.EqualTo("127.0.0.1:7000"));
        Assert.IsFalse(back.OpenKeyOn);
        Assert.IsTrue(back.ComputeOpenSk(null));
    }

    [Test]
    public void RabbitHomeDefaults_SetLittleEndian()
    {
        RabbitHomeDefaults.SetLittleEndian(false);
        Assert.IsFalse(RabbitHomeDefaults.LittleEndian);
        RabbitHomeDefaults.SetLittleEndian(true);
        Assert.IsTrue(RabbitHomeDefaults.LittleEndian);
        Assert.That(RabbitHomeDefaults.HttpKeyQuery, Is.EqualTo("q"));
        Assert.That(RabbitHomeDefaults.HttpPatternRoute, Is.EqualTo("/route"));
    }

    [Test]
    public void RabbitHomeUtils_ReturnsNullWhenKeyMissing()
    {
        var settings = new HomeSettings("http://home.example", false, true, true);
        Assert.IsNull(RabbitHomeUtils.LoadHomePublicRsa(settings));
        Assert.IsNull(RabbitHomeUtils.LoadHomePublicRsa(true, null, null));
    }

    [Test]
    public void RabbitHomeUtils_LoadsPemPublicKeyFromTestResources()
    {
        var pemPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Home", "x509_public.pem");
        Assert.IsTrue(File.Exists(pemPath), $"missing {pemPath}");
        var cipher = RabbitHomeUtils.LoadHomePublicRsaWithPath(true, pemPath);
        Assert.IsNotNull(cipher);
    }

    [Test]
    public void RabbitHomeClient_ConstructsWithoutSendingRequest()
    {
        using var client = new RabbitHomeClient("http://home.example/", false);
        Assert.That(client.HomeUrl, Is.EqualTo("http://home.example/"));
    }

    [Test]
    public void RabbitClientManager_ConstructsWithoutConnecting()
    {
        var proxy = new HttpClientProxy("http://home.example");
        using var manager = new RabbitClientManager(proxy, "http://home.example", false, false, true, null, null);
        Assert.That(manager.HomeSettings.HomeUrl, Is.EqualTo("http://home.example"));
        Assert.IsNotNull(manager.HomeClient);
        Assert.IsNull(manager.SocketClient);
    }
}
