using System.Collections.Generic;
using JLGames.RabbitClient.Server.MMO;

namespace JLGames.RabbitClientTest.MMO;

[TestFixture]
public class MmoUnitTest
{
    [Test]
    public void V2Int_ComponentsEqualityAndIndex()
    {
        var v = new V2Int(3, 4);
        Assert.That(v.x, Is.EqualTo(3));
        Assert.That(v.y, Is.EqualTo(4));
        Assert.That(v[0], Is.EqualTo(3));
        Assert.That(v[1], Is.EqualTo(4));
        v.Set(1, 2);
        Assert.That(v, Is.EqualTo(new V2Int(1, 2)));
        Assert.That(new V2Int(0, 0), Is.EqualTo(V2Int.zero));
        Assert.Throws<System.IndexOutOfRangeException>(() =>
        {
            var unused = v[2];
        });
    }

    [Test]
    public void V3Int_ComponentsAndDefaultZ()
    {
        var v = new V3Int(1, 2);
        Assert.That(v.z, Is.EqualTo(0));
        v.Set(7, 8, 9);
        Assert.That(v.x, Is.EqualTo(7));
        Assert.That(v.y, Is.EqualTo(8));
        Assert.That(v.z, Is.EqualTo(9));
        Assert.That(v, Is.EqualTo(new V3Int(7, 8, 9)));
        Assert.That(v[2], Is.EqualTo(9));
    }

    [Test]
    public void VarSet_StoresPrimitivesVectorsAndStamps()
    {
        var set = new VarSet(true);
        set.SetVar("hp", 100);
        set.SetVar("pos", new V2Int(1, 2), 12345L);
        set.SetVar("loc", new V3Int(3, 4, 5));
        set.SetVars(new Dictionary<string, object> { { "name", "p01" } });

        Assert.That(set.GetValue<int>("hp"), Is.EqualTo(100));
        Assert.That(set.GetValue<V2Int>("pos"), Is.EqualTo(new V2Int(1, 2)));
        Assert.That(set.GetValueStamp("pos"), Is.EqualTo(12345L));
        Assert.That(set.GetValue<V3Int>("loc"), Is.EqualTo(new V3Int(3, 4, 5)));
        Assert.That(set.GetValue<string>("name"), Is.EqualTo("p01"));
        Assert.IsTrue(set.CheckKey("hp"));
        Assert.Greater(set.Size, 0);

        set.DeleteVar("hp", true);
        Assert.IsFalse(set.CheckKey("hp"));

        var encoded = set.EncodeToBytes();
        var restored = new VarSet(true);
        restored.DecodeFromBytes(encoded);
        Assert.That(restored.GetValue<string>("name"), Is.EqualTo("p01"));
        Assert.That(restored.GetValue<V2Int>("pos"), Is.EqualTo(new V2Int(1, 2)));
    }

    [Test]
    public void VarData_UpdatesStampOnValueAssign()
    {
        var data = new VarData<int> { Key = "hp", Type = VarType.Forever };
        data.Value = 10;
        var first = data.Stamp;
        Assert.That(data.Key, Is.EqualTo("hp"));
        Assert.That(data.Type, Is.EqualTo(VarType.Forever));
        Assert.That(data.Value, Is.EqualTo(10));
        Assert.Greater(first, 0L);
        data.Value = 11;
        Assert.GreaterOrEqual(data.Stamp, first);
    }
}
