using System.Security.Cryptography;
using System.Text;
using TuduBot.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace TuduBot.Infrastructure.Security;

public class CryptoService : ICryptoService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public CryptoService(IConfiguration configuration)
    {
        var keyString = configuration["Encryption:AesKey"];
        var ivString = configuration["Encryption:AesIV"];

        if (string.IsNullOrWhiteSpace(keyString) || string.IsNullOrWhiteSpace(ivString))
            throw new InvalidOperationException("Encryption key or IV is missing in configuration");

        _key = Convert.FromBase64String(keyString);
        _iv = Convert.FromBase64String(ivString);
    }

    public string Encrypt(string input)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;
        aes.Mode = CipherMode.CBC;

        var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(input);

        var encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        return Convert.ToBase64String(encrypted);
    }

    public string Decrypt(string input)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;
        aes.Mode = CipherMode.CBC;

        var decryptor = aes.CreateDecryptor();
        var encryptedBytes = Convert.FromBase64String(input);

        var decrypted = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
        return Encoding.UTF8.GetString(decrypted);
    }
}
