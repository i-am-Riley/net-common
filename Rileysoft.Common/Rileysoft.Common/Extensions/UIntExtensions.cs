namespace Rileysoft.Common.Extensions
{
    public static class UIntExtensions
    {
        public static string ToStringHexLE(this uint value)
        {
            return BitConverter.GetBytes(value).ToStringHexLE();
        }

        public static string ToStringHexBE(this uint value)
        {
            return BitConverter.GetBytes(value).ToStringHexBE();
        }
    }
}
