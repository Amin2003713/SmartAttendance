using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Shifty.Common.General;

namespace Shifty.Common.Utilities;

public static class SecurityExtensions
{
    public static SymmetricSecurityKey GenerateShuffledKey(this HttpContext context)
    {

        var headers    = context.Request.Headers;
        var domainId   = headers.ContainsKey("--tenant--") ? headers["--tenant--"].ToString() : "DefaultDomain";

        // Concatenate domainId with SecretKey
        var combinedKey = domainId + ApplicationConstant.JwtSettings.SecretKey;
                                                                                                    
        // Perform bitwise shifting and shuffling based on IP hash
        var shuffledKey = combinedKey.ToCharArray();
        for (var i = 0; i < shuffledKey.Length; i++)
        {
            shuffledKey[i] = (char)(shuffledKey[i] & 0xFF);            // XOR with IP Hash
            shuffledKey[i] = (char)(shuffledKey[i] << 1 | shuffledKey[i] >> 7); // Bitwise shift
        }


        var a = new string(shuffledKey);
        // Convert to a secure hash
        using var sha256 = SHA256.Create();
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new string(shuffledKey)));
    }
}