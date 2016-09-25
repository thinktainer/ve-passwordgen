namespace Thinktainer.Ve.HOTP
{
    public interface IPasswordGenerator
    {
        string GeneratePassword(string userId);

        bool IsValidPassword(string userId, string password);
    }

    public class PasswordGenerator : IPasswordGenerator
    {
        public string GeneratePassword(string userId)
        {
            return TOTP.Generate(userId);
        }

        public bool IsValidPassword(string userId, string password)
        {
            return password == TOTP.Generate(userId);
        }
    }
}