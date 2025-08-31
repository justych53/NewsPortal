using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace NewsPortal.Auth
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            var authSettings = configuration.GetSection(nameof(AuthSettings)).Get<AuthSettings>();

            Console.WriteLine($"=== JWT CONFIGURATION ===");
            Console.WriteLine($"Secret key: {authSettings.SecretKey}");
            Console.WriteLine($"Secret key length: {authSettings.SecretKey?.Length}");

            var keyBytes = Encoding.UTF8.GetBytes(authSettings.SecretKey);
            Console.WriteLine($"Key bytes length: {keyBytes.Length * 8} bits");

            serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                        ClockSkew = TimeSpan.Zero
                    };


                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"🔴 AUTH FAILED: {context.Exception.Message}");
                            Console.WriteLine($"Exception type: {context.Exception.GetType().Name}");

                            if (context.Exception is SecurityTokenExpiredException)
                                Console.WriteLine("❌ Token expired");
                            else if (context.Exception is SecurityTokenInvalidSignatureException)
                                Console.WriteLine("❌ Invalid signature");
                            else if (context.Exception is SecurityTokenInvalidIssuerException)
                                Console.WriteLine("❌ Invalid issuer");
                            else if (context.Exception is SecurityTokenNoExpirationException)
                                Console.WriteLine("❌ No expiration");

                            return Task.CompletedTask;
                        },

                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("🟢 TOKEN VALIDATED SUCCESSFULLY");
                            var jwtToken = context.SecurityToken as JwtSecurityToken;
                            Console.WriteLine($"Token expiry: {jwtToken?.ValidTo}");

                            foreach (var claim in context.Principal.Claims)
                            {
                                Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
                            }
                            return Task.CompletedTask;
                        },

                        OnMessageReceived = context =>
                        {
                            if (!string.IsNullOrEmpty(context.Token))
                            {
                                Console.WriteLine($"🔑 TOKEN RECEIVED: {context.Token}");

                                try
                                {
                                    var tokenHandler = new JwtSecurityTokenHandler();
                                    var validationParameters = new TokenValidationParameters
                                    {
                                        ValidateIssuerSigningKey = true,
                                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                                        ValidateIssuer = false,
                                        ValidateAudience = false,
                                        ValidateLifetime = true,
                                        ClockSkew = TimeSpan.Zero
                                    };

                                    var principal = tokenHandler.ValidateToken(context.Token, validationParameters, out var validatedToken);
                                    Console.WriteLine("🟢 MANUAL VALIDATION SUCCESS");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"🔴 MANUAL VALIDATION FAILED: {ex.Message}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("❌ NO TOKEN RECEIVED");
                            }
                            return Task.CompletedTask;
                        },

                        OnChallenge = context =>
                        {
                            Console.WriteLine($"🚨 CHALLENGE: {context.Error}");
                            Console.WriteLine($"Description: {context.ErrorDescription}");
                            return Task.CompletedTask;
                        }
                    };
                });

            return serviceCollection;
        }
    }
}