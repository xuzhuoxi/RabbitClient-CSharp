using JLGames.RabbitClient.Server;
using JLGames.RabbitClient.Server.Message;

namespace JLGames.RabbitClientTest.Server;

[TestFixture]
public class MessageUnitTest
{
    private bool m_SavedLittleEndian;
    private bool m_SavedApmMode;

    [SetUp]
    public void SetUp()
    {
        m_SavedLittleEndian = RabbitServerDefaults.LittleEndian;
        m_SavedApmMode = RabbitServerDefaults.ApmMode;
    }

    [TearDown]
    public void TearDown()
    {
        RabbitServerDefaults.SetLittleEndian(m_SavedLittleEndian);
        RabbitServerDefaults.SetConnectApiMode(m_SavedApmMode);
    }

    [Test]
    public void RabbitMessageHeader_BuildsProtoUid()
    {
        var header = new RabbitMessageHeader();
        header.SetHeaderInfo("Mmo", "PJ", "client-1");
        Assert.That(header.Extension, Is.EqualTo("Mmo"));
        Assert.That(header.ProtoId, Is.EqualTo("PJ"));
        Assert.That(header.ClientId, Is.EqualTo("client-1"));
        Assert.That(header.ProtoUid, Is.EqualTo("Mmo:PJ"));
    }

    [Test]
    public void RabbitMessageWriterReader_RoundtripsHeaderAndPayload()
    {
        var writer = new RabbitMessageWriter();
        writer.WriteHeader("Mmo", "PJ", "cid");
        writer.WriteData("hello");
        writer.WriteData(42);

        var reader = new RabbitMessageReader();
        reader.SetMessageBytes(writer.ToMessageBytes());
        reader.StartReadData();

        Assert.That(reader.Extension, Is.EqualTo("Mmo"));
        Assert.That(reader.ProtoId, Is.EqualTo("PJ"));
        Assert.That(reader.ClientId, Is.EqualTo("cid"));
        Assert.That(reader.ProtoUid, Is.EqualTo("Mmo:PJ"));
        Assert.That(reader.ReadString(), Is.EqualTo("hello"));
        Assert.That(reader.ReadInt32(), Is.EqualTo(42));
        Assert.IsFalse(reader.Next);
    }

    [Test]
    public void RabbitRequestMsg_WritesHeaderThenBody()
    {
        var req = new RabbitRequestMsg();
        req.SetProtoInfo("Mmo", "PER");
        req.SetClientId("p01");
        req.StartWriteData();
        req.WriteRequestBase("r111");

        var reader = new RabbitMessageReader();
        reader.SetMessageBytes(req.ToMessageBytes());
        reader.StartReadData();
        Assert.That(reader.Extension, Is.EqualTo("Mmo"));
        Assert.That(reader.ProtoId, Is.EqualTo("PER"));
        Assert.That(reader.ClientId, Is.EqualTo("p01"));
        Assert.That(reader.ReadString(), Is.EqualTo("r111"));
    }

    [Test]
    public void RabbitResponseMsg_ReadsHeaderAndStatusCode()
    {
        var writer = new RabbitMessageWriter();
        writer.WriteHeader("Mmo", "PJ", "cid");
        writer.WriteData(200);
        writer.WriteData("ok");

        var resp = new RabbitResponseMsg();
        resp.SetMessageBytes(writer.ToMessageBytes());
        resp.StartReadData();
        Assert.That(resp.Extension, Is.EqualTo("Mmo"));
        Assert.That(resp.ProtoId, Is.EqualTo("PJ"));
        Assert.That(resp.RsCode, Is.EqualTo(200));
        Assert.That(resp.ReadString(), Is.EqualTo("ok"));

        var clone = resp.Clone();
        Assert.That(clone.RsCode, Is.EqualTo(200));
        Assert.That(clone.ReadString(), Is.EqualTo("ok"));
    }

    [Test]
    public void RabbitServerDefaults_CanChangeEndianAndApiMode()
    {
        RabbitServerDefaults.SetLittleEndian(false);
        RabbitServerDefaults.SetConnectApiMode(true);
        Assert.IsFalse(RabbitServerDefaults.LittleEndian);
        Assert.IsTrue(RabbitServerDefaults.ApmMode);
        RabbitServerDefaults.SetLittleEndian(true);
        RabbitServerDefaults.SetConnectApiMode(false);
        Assert.IsTrue(RabbitServerDefaults.LittleEndian);
        Assert.IsFalse(RabbitServerDefaults.ApmMode);
    }
}
