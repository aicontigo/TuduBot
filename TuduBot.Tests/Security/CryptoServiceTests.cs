using AwesomeAssertions;
using TuduBot.Infrastructure.Security;
using System.Security.Cryptography;

namespace TuduBot.Tests.Security;

public class CryptoServiceTests
{
    [Fact]
    public void Encrypt_and_decrypt_should_return_original_value()
    {
        var config = TestConfigFactory.CreateValid();
        var crypto = new CryptoService(config);

        var input = "todoist_test_api_key";
        var encrypted = crypto.Encrypt(input);
        var decrypted = crypto.Decrypt(encrypted);

        decrypted.Should().Be(input);
        encrypted.Should().NotBe(input);
    }

    [Fact]
    public void Decrypt_should_throw_on_invalid_input()
    {
        var config = TestConfigFactory.CreateValid();
        var crypto = new CryptoService(config);

        var act = () => crypto.Decrypt("not-base64");

        act.Should().Throw<FormatException>();
    }

    [Fact]
    public void Decrypt_with_wrong_key_should_fail()
    {
        var config1 = TestConfigFactory.CreateValid();
        var crypto1 = new CryptoService(config1);

        var config2 = TestConfigFactory.CreateValid(); // another key
        var crypto2 = new CryptoService(config2);

        var encrypted = crypto1.Encrypt("secret");

        var act = () => crypto2.Decrypt(encrypted);

        act.Should().Throw<CryptographicException>();
    }
}
