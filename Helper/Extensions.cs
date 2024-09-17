using Cafe_NET_API.Entities;

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
    }
}
