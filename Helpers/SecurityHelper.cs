using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace PresupuestoMVC.Helpers
{
    public static class SecurityHelper
    {
        public static string HashPassword(string password)
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

        public static bool VerifyPassword(string password, string storedHash)
        {
            try
            {
                var parts = storedHash.Split(':');
                if (parts.Length != 2)
                    return false;

                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] storedHashBytes = Convert.FromBase64String(parts[1]);

                var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
                {
                    Salt = salt,
                    DegreeOfParallelism = 2,
                    MemorySize = 32768,
                    Iterations = 3
                };

                byte[] computedHash = argon2.GetBytes(32);

                return CryptographicOperations.FixedTimeEquals(computedHash, storedHashBytes);
            }
            catch
            {
                return false;
            }
        }
    }

}
