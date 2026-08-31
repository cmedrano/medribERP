using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PresupuestoMVC.Data;
using PresupuestoMVC.Helpers;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PresupuestoMVC.Services
{
    public class LoginService : ILoginService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _jwtExpiryMinutes;
        private readonly int _refreshTokenExpiryDays = 7; // Refresh token dura 7 días
        private readonly int _passwordResetTokenExpiryMinutes = 30; // El enlace de recuperación dura 30 minutos

        public LoginService(AppDbContext context, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _jwtKey = configuration["JwtSettings:Key"]!;
            _jwtIssuer = configuration["JwtSettings:Issuer"]!;
            _jwtAudience = configuration["JwtSettings:Audience"]!;
            _jwtExpiryMinutes = configuration.GetValue<int>("JwtSettings:ExpiryInMinutes");
        }

        public async Task<LoginResponseDto> LoginAsync(LoginViewRequest loginRequest)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == loginRequest.Username);

                // Verificar si el usuario existe
                if (user == null)
                    throw new UnauthorizedAccessException("Usuario incorrecto");

                // Verifico contraseña
                bool isPasswordValid = VerifyPassword(loginRequest.Password, user.UserPasswordHash);

                if (!isPasswordValid)
                    throw new UnauthorizedAccessException("Contraseña incorrecta");

                // Generar token JWT y RefreshToken para el usuario autenticado
                var token = await GenerateJwtToken(user);
                var refreshToken = await GenerateRefreshToken(user.Id);

                // Retornar respuesta con el token JWT y información del usuario
                return new LoginResponseDto
                {
                    Token = token, // Token JWT generado
                    RefreshToken = refreshToken.Token, // Refresh token generado
                    Expiration = DateTime.UtcNow.AddMinutes(_jwtExpiryMinutes), // Fecha de expiración del token
                    Username = user.UserName // Nombre de usuario
                };
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        // Método para verificar contraseñas con Argon2
        private bool VerifyPassword(string password, string storedHash)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                // Dividir el hash almacenado en salt y hash
                var parts = storedHash.Split(':');
                if (parts.Length != 2)
                    return false;

                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] storedHashBytes = Convert.FromBase64String(parts[1]);

                // Calcular el hash de la contraseña proporcionada con la misma sal
                var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
                {
                    Salt = salt,
                    DegreeOfParallelism = 2,    // Reducir de 4 a 2
                    MemorySize = 32768,         // Reducir de 65536 a 32768 (32MB)
                    Iterations = 3              // Reducir de 4 a 3
                };

                byte[] computedHash = argon2.GetBytes(32);

                Console.WriteLine($"Argon2 verification took: {stopwatch.ElapsedMilliseconds}ms");

                // Comparar los hashes de manera segura
                return CryptographicOperations.FixedTimeEquals(computedHash, storedHashBytes);
            }
            catch
            {
                return false;
            }
        }

        // Genera el JwtToken para ese Usuario
        private async Task<string> GenerateJwtToken(User user)
        {
            // Crear manejador de tokens JWT
            var tokenHandler = new JwtSecurityTokenHandler();

            // Obtener la clave secreta desde configuración y convertirla a bytes
            var key = Encoding.ASCII.GetBytes(_jwtKey);

            // Obtener módulos del usuario
            var modules = _context.AreasPerUser
            .Where(x => x.UserId == user.Id)
            .Select(x => x.Module.Name)
            .ToList();

            var companyName = await _context.Companies
            .Where(c => c.Id == user.CompanyId)
            .Select(c => c.CompanyName)
            .FirstOrDefaultAsync();

            // Crear lista de claims
            var claims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName), // Claim con el nombre de usuario
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Claim con el ID del usuario
               new Claim(ClaimTypes.Role, user.Role.ToString()),
               new Claim("CompanyId", user.CompanyId.ToString()),
               new Claim("CompanyName", companyName.ToString() ?? string.Empty)
            };

            // Agregar módulos
            foreach (var module in modules)
            {
                claims.Add(new Claim("Module", module));
            }

            // Crear token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtExpiryMinutes),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            // Crear el token JWT basado en la configuración
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Convertir el token a string para devolverlo
            return tokenHandler.WriteToken(token);
        }

        // Generar Refresh Token
        private async Task<RefreshToken> GenerateRefreshToken(int userId)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays)
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        // Refresh Token
        public async Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshViewRequest request)
        {
            var storedRefreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt =>
                    rt.Token == request.RefreshToken &&
                    !rt.IsRevoked &&
                    rt.ExpiresAt > DateTime.UtcNow);

            if (storedRefreshToken == null)
                throw new UnauthorizedAccessException("Refresh token inválido");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == storedRefreshToken.UserId);

            if (user == null)
                throw new UnauthorizedAccessException("Usuario no encontrado");

            // Revocar el refresh token usado (rotación opcional)
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);

            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
                await _context.SaveChangesAsync();
            }

            // Generar nuevo access token
            var newAccessToken = await GenerateJwtToken(user);

            // Generar NUEVO refresh token (rotación - más seguro)
            var newRefreshToken = await GenerateRefreshToken(user.Id);

            return new RefreshTokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token,
                Expiration = DateTime.UtcNow.AddMinutes(_jwtExpiryMinutes)
            };
        }

        // Logout (revocar refresh token)
        public async Task RevokeRefreshTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);

            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterViewRequest registerRequest)
        {
            // Validar si el usuario ya existe
            var userExiste = await _context.Users
                .AnyAsync(u => u.UserName == registerRequest.UserName || u.UserEmail == registerRequest.Email);

            if (userExiste)
                throw new InvalidOperationException("El nombre de usuario o correo electrónico ya está en uso.");

            // Hashear la contraseña
            var passwordHash = HashPassword(registerRequest.Password);

            // Crear nuevo usuario
            var user = new User
            {
                UserName = registerRequest.UserName,
                UserEmail = registerRequest.Email,
                UserPasswordHash = passwordHash,
                CompanyId = registerRequest.CompanyId,
                Role = registerRequest.Role,
                CreateDate = DateTime.UtcNow
            };

            // Guardar en la base de datos
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var createdUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == registerRequest.UserName);

            var area = new AreasPerUser
            {
                UserId = createdUser!.Id,
                ModuleId = (int)registerRequest.Role
            };
            _context.AreasPerUser.Add(area);
            await _context.SaveChangesAsync();

            return new RegisterResponseDto
            {
                Message = "Usuario registrado exitosamente",
                UserName = createdUser.UserName,
                CreatedAt = createdUser.CreateDate
            };
        }

        public async Task<User> GetByEmailAsync(RecoverViewModel viewModel)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserEmail == viewModel.Email);
        }

        // Genera y persiste un token de recuperación de contraseña para el usuario
        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            // Invalido cualquier token previo aún vigente para este usuario
            var previousTokens = await _context.PasswordResetTokens
                .Where(t => t.UserId == user.Id && !t.IsUsed)
                .ToListAsync();

            foreach (var previousToken in previousTokens)
            {
                previousToken.IsUsed = true;
            }

            var resetToken = new PasswordResetToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
                    .Replace("+", "-").Replace("/", "_").Replace("=", ""),
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_passwordResetTokenExpiryMinutes),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.PasswordResetTokens.Add(resetToken);
            await _context.SaveChangesAsync();

            return resetToken.Token;
        }

        // Envía el correo con el enlace para restablecer la contraseña, vía Resend
        public async Task SendPasswordResetEmailAsync(string email, string resetLink)
        {
            var info = GetResendInfoMail();

            if (string.IsNullOrWhiteSpace(info.ApiKey))
                throw new InvalidOperationException("Falta la clave de Resend para el entorno actual.");

            using var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", info.ApiKey);

            var payload = new
            {
                from = info.FromEmail,
                to = new[] { email },
                subject = "🔑 Recuperación de contraseña",
                html = $"<p>Recibimos una solicitud para restablecer tu contraseña.</p>" +
                       $"<p>Hacé clic en el siguiente enlace para crear una nueva contraseña. Este enlace vence en {_passwordResetTokenExpiryMinutes} minutos:</p>" +
                       $"<p><a href=\"{resetLink}\">{resetLink}</a></p>" +
                       $"<p>Si no solicitaste este cambio, podés ignorar este correo.</p>"
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await http.PostAsync("https://api.resend.com/emails", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Error al enviar email vía Resend: {response.StatusCode} - {responseBody}");
        }

        // Valida el token de recuperación y actualiza la contraseña del usuario
        public async Task<bool> ResetPasswordAsync(ResetPasswordViewRequest model)
        {
            var resetToken = await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.Token == model.Token);

            if (resetToken == null || resetToken.IsUsed || resetToken.ExpiresAt < DateTime.UtcNow)
                throw new InvalidOperationException("El enlace de recuperación es inválido o ha expirado.");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == resetToken.UserId);

            if (user == null)
                throw new InvalidOperationException("El usuario no existe.");

            user.UserPasswordHash = HashPasswordHelper.GetHashPassword(model.NewPassword);
            resetToken.IsUsed = true;

            await _context.SaveChangesAsync();
            return true;
        }

        private InfoMail GetResendInfoMail()
        {
            var envName = _environment?.EnvironmentName
                ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? "Production";

            var isTestEnv = string.Equals(envName, "Development", StringComparison.OrdinalIgnoreCase)
                || string.Equals(envName, "Test", StringComparison.OrdinalIgnoreCase)
                || envName.Contains("Test", StringComparison.OrdinalIgnoreCase);

            var apiKey = isTestEnv
                ? Environment.GetEnvironmentVariable("RESEND_API_KEY_TEST")
                : Environment.GetEnvironmentVariable("RESEND_API_KEY_PRODUCTION");

            var fromEmail = isTestEnv
                ? "dmarc@tupresupuestotest.online"
                : "dmarc@erp.medribsoftware.com";

            return new InfoMail
            {
                FromEmail = fromEmail,
                ApiKey = apiKey ?? string.Empty
            };
        }

        private class InfoMail
        {
            public string FromEmail { get; set; } = string.Empty;
            public string ApiKey { get; set; } = string.Empty;
        }

        // Método para hashear contraseñas con Argon2 (formato estándar)
        private string HashPassword(string password)
        {
            // Crear una sal (salt) aleatoria para mayor seguridad
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Configurar Argon2 con parámetros seguros
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 2,    // Reducir de 4 a 2
                MemorySize = 32768,         // Reducir de 65536 a 32768 (32MB)
                Iterations = 3              // Reducir de 4 a 3
            };

            // Generar el hash y devolver el formato estándar
            byte[] hash = argon2.GetBytes(32);

            // Para obtener el formato estándar, necesitamos construirlo manualmente
            // o usar una alternativa más simple
            return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

    }
}
