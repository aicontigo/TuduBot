using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace TuduBot.Tests.Security;
public static class TestConfigFactory
{
    public static IConfiguration CreateValid()
    {
        var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)); // 32 bytes
        var iv = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16)); // 16 bytes

        var data = new Dictionary<string, string?>
        {
            { "Encryption:AesKey", key },
            { "Encryption:AesIV", iv }
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(data!)
            .Build();
    }
}
