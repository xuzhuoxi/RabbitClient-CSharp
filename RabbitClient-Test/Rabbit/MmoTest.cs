using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using JLGames.Infra.Event;
using JLGames.Infra.Mathx;
using JLGames.Infra.Net;
using JLGames.Infra.TinyJson;
using JLGames.RabbitClient;
using JLGames.RabbitClient.Server;
using JLGames.RabbitClient.Server.Message;

namespace JLGames.RabbitClientTest.Rabbit;

public struct MmoPlayerEnter
{
    [DataMember(Name = "pid")] public string PlayerId;
    [DataMember(Name = "rid")] public string RoomId;
}

public struct MmoPlayerLeave
{
    [DataMember(Name = "pid")] public string PlayerId;
}

[TestFixture]
[Category("RunOnlyThis")] // 依赖本机 Rabbit-Home / Rabbit-Server 服务，CI 中跳过
public class MmoTest
{
    // 转接信息
    private static readonly string s_BasePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent!.Parent!.Parent!.FullName;
    private const string c_HomeUrl = "http://127.0.0.1:9000";
    private const bool c_UsePost = false;
    private const bool c_EnableKey = true;
    private const bool c_IsPemKey = true;
    private const string c_PlatformId = "main01";
    private const string c_TypeName = "Rabbit-Server";
    private RabbitClientManager m_RabbitClientManager;

    // MMO信息
    private const string c_Mmo = "Mmo";
    private const string c_ProtoJoin = "PJ";
    private const string c_ProtoEnterRoom = "PER";
    private const string c_ProtoLeaveRoom = "PLR";
    private const string c_ProtoPlayerVar = "PV";
    private const string c_ProtoRoomVar = "RV";
    private static readonly string[] s_PlayerIds = { "player01" };
    private static readonly string[] s_RoomIds = { "r111", "r112", "r121", "r122", "r221", "r222", "other1", "other2" };

    [Test]
    public async Task TestManagerLink()
    {
        var pubKeyPath = Path.Combine(s_BasePath!, "Resources/Home", "x509_public.pem");
        var httpProxy = new HttpClientProxy(c_HomeUrl);
        var manager = new RabbitClientManager(httpProxy, c_HomeUrl, c_UsePost, c_EnableKey, c_IsPemKey, pubKeyPath, null);
        m_RabbitClientManager = manager;
        manager.OnceEventListener(RabbitClientManagerEvents.EventOnConnectFinish, OnLinkFinish);
        await manager.ConnectThroughHome(c_PlatformId, c_TypeName, true);
        await Task.Delay(50000);
    }

    private void OnLinkFinish(EventData evd)
    {
        TestContext.Progress.WriteLine($"OnLinkFinish: Suc={evd.Data}");
        m_RabbitClientManager.SocketClient.EventDispatcher.AddEventListener(RabbitSocketClientEvents.EventOnClientReceiveMessage, OnSocketClientMessage);
        foreach (var playerId in s_PlayerIds)
        {
            new Thread(async () => { await Run(playerId); }).Start();
        }
    }

    private void OnSocketClientMessage(EventData evd)
    {
        IRabbitResponseMsg msg = (IRabbitResponseMsg)evd.Data;
        msg.StartReadData();
        var extension = msg.Extension;
        var pid = msg.ProtoId;
        var code = msg.RsCode;
        var cid = msg.ClientId;
        TestContext.Progress.WriteLine($"{{Name={extension}, Pid={pid}, Code={code}, Cid={cid}}}");
    }

    private async Task Run(string playerId)
    {
        var delay = new Random().Next(1000, 5000);
        JoinWorld(playerId, s_RoomIds[0]);
        await Task.Delay(delay);
        for (var index = 1; index < s_RoomIds.Length; index++)
        {
            EnterRoom(playerId, s_RoomIds[index]);
            await Task.Delay(delay);
        }

        LeaveRoom(playerId);
    }

    private void JoinWorld(string playerId, string roomId)
    {
        var msg = new RabbitMessageWriter(RabbitServerDefaults.LittleEndian);
        msg.WriteHeader(c_Mmo, c_ProtoJoin, playerId);
        var data = new MmoPlayerEnter
        {
            PlayerId = playerId,
            RoomId = roomId
        };
        var jsonStr = data.ToJson();
        TestContext.Progress.WriteLine($"JoinWorld: {jsonStr}");
        msg.WriteData(jsonStr);
        m_RabbitClientManager.SocketClient.SendMessage(msg);
    }

    private void EnterRoom(string playerId, string roomId)
    {
        var msg = new RabbitMessageWriter(RabbitServerDefaults.LittleEndian);
        msg.WriteHeader(c_Mmo, c_ProtoEnterRoom, playerId);
        var data = new MmoPlayerEnter
        {
            PlayerId = playerId,
            RoomId = roomId
        };
        var jsonStr = data.ToJson();
        TestContext.Progress.WriteLine($"EnterRoom: {jsonStr}");
        msg.WriteData(jsonStr);
        m_RabbitClientManager.SocketClient.SendMessage(msg);
    }

    private void LeaveRoom(string playerId)
    {
        var msg = new RabbitMessageWriter(RabbitServerDefaults.LittleEndian);
        msg.WriteHeader(c_Mmo, c_ProtoLeaveRoom, playerId);
        var data = new MmoPlayerLeave
        {
            PlayerId = playerId,
        };
        var jsonStr = data.ToJson();
        TestContext.Progress.WriteLine($"LeaveRoom: {jsonStr}");
        msg.WriteData(jsonStr);
        m_RabbitClientManager.SocketClient.SendMessage(msg);
    }
}
