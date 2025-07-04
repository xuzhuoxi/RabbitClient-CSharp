using System;
using System.IO;
using System.Threading.Tasks;
using JLGames.Infra.Crypto.Key;
using JLGames.RabbitClient.Home;

namespace JLGames.RabbitClientTest.Rabbit;

[TestFixture]
public class HomeTest
{
    private static readonly string m_BasePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent!.Parent!.Parent!.FullName;

    private const string m_HomeUrl = "http://127.0.0.1:9000";
    private const bool m_UsePost = false;
    private const bool m_EnableKey = true;
    private const bool m_IsPemKey = true;

    private const string m_PlatformId = "main01";
    private const string m_TypeName = "Rabbit-Server";

    [Test]
    public async Task TestQueryHome()
    {
        var queryInfo = new QueryRouteInfo { PlatformId = m_PlatformId, TypeName = m_TypeName };
        await DoQueryHome(queryInfo);
    }

    [Test]
    public async Task TestQueryHomeWithTempKey()
    {
        var queryInfo = new QueryRouteInfo
        {
            PlatformId = m_PlatformId, TypeName = m_TypeName,
            TempAesKey = KeyDerivation.DeriveKeyPbkdf2StrDefault("hello world")
        };
        await DoQueryHome(queryInfo);
    }

    private async Task DoQueryHome(QueryRouteInfo queryRouteInfo)
    {
        var pubKeyPath = Path.Combine(m_BasePath!, "Resources/Home", "x509_public.pem");
        var homeSettings = new HomeSettings(m_HomeUrl, m_UsePost, m_EnableKey, m_IsPemKey, pubKeyPath);

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