using Cafe_NET_API.Entities;
using System.Security.Cryptography;
using System.Text;

namespace Cafe_NET_API.Helper
{
    public static class Extensions
    {
        public static string ToSafeString(this Guid? obj)
        {
            if (obj == null) return string.Empty;

            return ((Guid)obj).ToString();
        }

        public static string ToSafeString(this Guid obj)
        {
            return obj.ToString();
        }

        public static string ToHexString(this Guid? obj)
        {
            if(obj == null) return null;

            var uuidByteArray = ((Guid)obj).ToByteArray();
            string uuidHex = BitConverter.ToString(uuidByteArray).Replace("-", string.Empty);

            return uuidHex;
        }

        public static string ToHexString(this Guid obj)
        {
            var uuidByteArray = obj.ToByteArray();
            string uuidHex = BitConverter.ToString(uuidByteArray).Replace("-", string.Empty);

            return uuidHex;
        }

        public static string ToHash(this string password)
        {
            SHA512 sha512 = SHA512.Create();
            sha512.ComputeHash(Encoding.ASCII.GetBytes(Convert.ToString(password)));
            var encryptStr = Encoding.ASCII.GetString(sha512.Hash);

            if(!string.IsNullOrWhiteSpace(encryptStr) && encryptStr.Length > 64)
            {
                encryptStr = encryptStr.Substring(0, 64);
            }

            return encryptStr;
        }
    }
}
