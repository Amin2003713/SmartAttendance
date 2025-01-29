using System;
using System.Security.Cryptography;
using System.Text;
using Shifty.Common.General;

namespace Shifty.Common.Utilities
{
    public static class PasswordGenerator
    {
        // Define character sets
        private const string LowercaseLetters  = "abcdefghijklmnopqrstuvwxyz";
        private const string UppercaseLetters  = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Numbers           = "0123456789";
        private const string SpecialCharacters = "!@#$%^&*()_+-=[]{}|;:,.<>?";

        // Generate a random password
        public static string GeneratePassword(int length = 8)
        {
            Console.WriteLine($"Environment is  : {ApplicationConstant.IsDevelopment}");
            if(ApplicationConstant.IsDevelopment)
                return "@Shifty403";


            if (length < 8)
            {
                throw new ArgumentException("Password length must be at least 8 characters.");
            }

            // Combine all character sets
            var allCharacters = LowercaseLetters + UppercaseLetters + Numbers + SpecialCharacters;

            // Use a cryptographically secure random number generator
            using var rng      = new RNGCryptoServiceProvider();
            var       password = new StringBuilder();

            // Ensure the password contains at least one character from each set
            password.Append(GetRandomCharacter(rng , LowercaseLetters));
            password.Append(GetRandomCharacter(rng , UppercaseLetters));
            password.Append(GetRandomCharacter(rng , Numbers));
            password.Append(GetRandomCharacter(rng , SpecialCharacters));

            // Fill the rest of the password with random characters
            for (var i = 4; i < length; i++)
            {
                password.Append(GetRandomCharacter(rng , allCharacters));
            }

            // Shuffle the password to ensure randomness
            return Shuffle(password.ToString());
        }

        // Get a random character from a given character set
        private static char GetRandomCharacter(RNGCryptoServiceProvider rng , string characterSet)
        {
            var randomBytes = new byte[4];
            rng.GetBytes(randomBytes);
            var randomIndex = BitConverter.ToUInt32(randomBytes , 0) % (uint)characterSet.Length;
            return characterSet[(int)randomIndex];
        }

        // Shuffle the characters in the password
        private static string Shuffle(string str)
        {
            var array = str.ToCharArray();
            using (var rng = new RNGCryptoServiceProvider())
            {
                var n = array.Length;
                while (n > 1)
                {
                    var randomBytes = new byte[4];
                    rng.GetBytes(randomBytes);
                    var k = (int)(BitConverter.ToUInt32(randomBytes , 0) % (uint)n);
                    n--;
                    (array[n] , array[k]) = (array[k] , array[n]);
                }
            }

            return new string(array);
        }
    }
}