using Microsoft.AspNetCore.Identity;
using NewsPortal.Models;

namespace NewsPortal.Auth
{
    public class AccountService
    {
        private readonly JwtService _jwtService;
        private readonly string _userName = "admin";
        private readonly string _passwordHash;

        public AccountService(JwtService jwtService)
        {
            _jwtService = jwtService;

            _passwordHash = "AQAAAAIAAYagAAAAEGPsYjneutZSpFoP7R2FTV5bX1INWavBddbiLdUb/edFTMtT0cZb61twzOgusvza+Q==";

            Console.WriteLine("AccountService initialized");
        }

        public async Task<string> Login(string userName, string password)
        {
            try
            {
                Console.WriteLine("=== LOGIN ATTEMPT ===");
                Console.WriteLine($"Username: '{userName}' == 'admin': {userName == "admin"}");

                if (userName != _userName)
                {
                    Console.WriteLine("❌ Username mismatch!");
                    throw new Exception("Unauthorized");
                }

                Console.WriteLine("✅ Username correct");


                Console.WriteLine($"Password hash length: {_passwordHash?.Length}");
                Console.WriteLine($"Hash starts with: {_passwordHash?.Substring(0, 20)}...");
                Console.WriteLine($"Hash ends with: ...{_passwordHash?.Substring(_passwordHash.Length - 20)}");

                var result = new PasswordHasher<object>().VerifyHashedPassword(null, _passwordHash, password);
                Console.WriteLine($"Password verification result: {result}");

                if (result == PasswordVerificationResult.Success)
                {
                    Console.WriteLine("✅ Password correct!");

                    var tempAccount = new Account(
                        Guid.NewGuid(),
                        _userName,
                        "Administrator",
                        ""
                    );

                    var token = _jwtService.GenerateToken(tempAccount);
                    Console.WriteLine("✅ Token generated successfully");

                    return token;
                }

                Console.WriteLine("❌ Password verification failed!");
                throw new Exception("Unauthorized");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Exception: {ex.Message}");
                Console.WriteLine($"Exception type: {ex.GetType().Name}");
                throw;
            }
        }

        // Метод для проверки хеша
        public void ValidateHash()
        {
            try
            {
                Console.WriteLine("=== VALIDATING HASH ===");
                Console.WriteLine($"Hash: {_passwordHash}");
                Console.WriteLine($"Length: {_passwordHash?.Length}");

                // Пробуем проверить хеш с пустым паролем
                var testResult = new PasswordHasher<object>().VerifyHashedPassword(null, _passwordHash, "");
                Console.WriteLine($"Test verification: {testResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Hash validation failed: {ex.Message}");
            }
        }
    }
}