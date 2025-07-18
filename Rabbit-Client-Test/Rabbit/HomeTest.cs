using System;
using System.IO;
using System.Threading.Tasks;
using JLGames.Infra.Crypto.Key;
using JLGames.RabbitClient.Home;

namespace JLGames.RabbitClientTest.Rabbit;

[TestFixture]
public class HomeTest
{
    private static readonly string s_BasePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent!.Parent!.Parent!.FullName;

    private const string c_HomeUrl = "http://127.0.0.1:9000";
    private const bool c_UsePost = false;
    private const bool c_EnableKey = true;
    private const bool c_IsPemKey = true;

    private const string c_PlatformId = "main01";
    private const string c_TypeName = "Rabbit-Server";

    [Test]
    public async Task TestQueryHome()
    {
        var queryInfo = new QueryRouteInfo { PlatformId = c_PlatformId, TypeName = c_TypeName };
        await DoQueryHome(queryInfo);
    }

    [Test]
    public async Task TestQueryHomeWithTempKey()
    {
        var queryInfo = new QueryRouteInfo
        {
            PlatformId = c_PlatformId, TypeName = c_TypeName,
            TempAesKey = KeyDerivation.DeriveKeyPbkdf2StrDefault("hello world")
        };
        await DoQueryHome(queryInfo);
    }

    private async Task DoQueryHome(QueryRouteInfo queryRouteInfo)
    {
        var pubKeyPath = Path.Combine(s_BasePath!, "Resources/Home", "x509_public.pem");
        var homeSettings = new HomeSettings(c_HomeUrl, c_UsePost, c_EnableKey, c_IsPemKey, pubKeyPath);

        var homeClient = new RabbitHomeClient(homeSettings.HomeUrl, homeSettings.UsePost);
        QueryResult result;
        if (homeSettings.EnableKey)
        {
            result = await homeClient.QueryFromHome(queryRouteInfo, homeSettings.IsPemKey, homeSettings.PublicKeyPath);
        }
        else
        {
            result = await homeClient.QueryFromHome(queryRouteInfo);
        }

        HandleHomeResponse(result);
    }

    private void HandleHomeResponse(QueryResult result)
    {
        TestContext.Progress.WriteLine(result.ToString());
    }
}
