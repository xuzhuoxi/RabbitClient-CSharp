using System;
using System.IO;
using System.Threading.Tasks;
using JLGames.Infra.Event;
using JLGames.Infra.Net;
using JLGames.RabbitClient;
using JLGames.RabbitClient.Home;

namespace JLGames.RabbitClientTest.Rabbit;

[TestFixture]
public class ManagerTest
{
    private static readonly string m_BasePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent!.Parent!.Parent!.FullName;

    private const string m_HomeUrl = "http://127.0.0.1:9000";
    private const bool m_UsePost = false;
    private const bool m_EnableKey = true;
    private const bool m_IsPemKey = true;

    private const string m_PlatformId = "main01";
    private const string m_TypeName = "Rabbit-Server";

    [Test]
    public async Task TestManagerLink()
    {
        var pubKeyPath = Path.Combine(m_BasePath!, "Resources/Home", "x509_public.pem");
        var manager = new RabbitClientManager(m_HomeUrl, m_UsePost, m_EnableKey, m_IsPemKey, pubKeyPath);
        manager.OnceEventListener(RabbitClientManagerEvents.EventOnProgressHome, OnManagerProgressHome);
        manager.OnceEventListener(RabbitClientManagerEvents.EventOnProgressServer, OnManagerProgressServer);
        manager.OnceEventListener(RabbitClientManagerEvents.EventOnConnectFinish, OnLinkFinish);
        await manager.ConnectThroughHome(m_PlatformId, m_TypeName, true);
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