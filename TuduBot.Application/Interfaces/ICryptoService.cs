
namespace TuduBot.Application.Interfaces;
public interface ICryptoService
{
    string Encrypt(string input);
    string Decrypt(string input);
}
