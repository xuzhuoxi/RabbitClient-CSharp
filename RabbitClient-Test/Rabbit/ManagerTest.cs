using System;
using System.IO;
using System.Threading.Tasks;
using JLGames.Infra.Event;
using JLGames.Infra.Net;
using JLGames.RabbitClient;
using JLGames.RabbitClient.Home;

namespace JLGames.RabbitClientTest.Rabbit;

[TestFixture]
[Category("RunOnlyThis")] // 依赖本机 Rabbit-Home / Rabbit-Server 服务，CI 中跳过
public class ManagerTest
{
    private static readonly string s_BasePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent!.Parent!.Parent!.FullName;

    private const string c_HomeUrl = "http://127.0.0.1:9000";
    private const bool c_UsePost = false;
    private const bool c_EnableKey = true;
    private const bool c_IsPemKey = true;

    private const string c_PlatformId = "main01";
    private const string c_TypeName = "Rabbit-Server";

    [Test]
    public async Task TestManagerLink()
    {
        var pubKeyPath = Path.Combine(s_BasePath!, "Resources/Home", "x509_public.pem");
        var httpProxy = new HttpClientProxy(c_HomeUrl);
        var manager = new RabbitClientManager(httpProxy, c_HomeUrl, c_UsePost, c_EnableKey, c_IsPemKey, pubKeyPath, null);
        manager.OnceEventListener(RabbitClientManagerEvents.EventOnProgressHome, OnManagerProgressHome);
        manager.OnceEventListener(RabbitClientManagerEvents.EventOnProgressServer, OnManagerProgressServer);
        manager.OnceEventListener(RabbitClientManagerEvents.EventOnConnectFinish, OnLinkFinish);
        await manager.ConnectThroughHome(c_PlatformId, c_TypeName, true);
        await Task.Delay(5000);
    }

    private void OnManagerProgressHome(EventData evd)
    {
        var info = (RabbitClientManagerEvents.ProgressEventData<QueryResult>)evd.Data;
        TestContext.Progress.WriteLine($"OnManagerProgressHome: {info}");
    }

    private void OnManagerProgressServer(EventData evd)
    {
        var info = (RabbitClientManagerEvents.ProgressEventData<SocketEvents.SocketConnEventInfo>)evd.Data;
        TestContext.Progress.WriteLine($"OnManagerProgressServer: {info}");
    }

    private void OnLinkFinish(EventData evd)
    {
        TestContext.Progress.WriteLine($"OnLinkFinish: Suc={evd.Data}");
    }
}
