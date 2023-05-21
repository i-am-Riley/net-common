namespace Rileysoft.Common.Extensions
{
    public static partial class StreamExtensions
    {
        public static ulong ReadUnsignedLEB128(this Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (!stream.CanRead)
                throw new ArgumentException("Cannot read from stream.");

            ulong result = 0;
            int shift = 0;
            byte b;
            do
            {
                if (shift >= 64) throw new OverflowException("LEB128 value is too large for the ulong data type.");
                b = (byte)stream.ReadByte();
                result |= (ulong)(b & 0x7F) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);
            return result;
        }

        public static long ReadSignedLEB128(this Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (!stream.CanRead)
                throw new ArgumentException("Cannot read from stream.");

            long result = 0;
            int shift = 0;
            byte b;
            do
            {
                if (shift >= 64) throw new OverflowException("LEB128 value is too large for the long data type.");
                b = (byte)stream.ReadByte();
                result |= (long)(b & 0x7F) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);

            // If the sign bit is set and we have not used all bits,
            // then this number is negative and we need to sign-extend it.
            if ((b & 0x40) != 0 && shift < 64)
            {
                result |= -1L << shift;
            }
            return result;
        }

        public static int WriteUnsignedLEB128(this Stream output, ulong value)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            if (!output.CanWrite)
                throw new ArgumentException("Cannot write to stream");

            int bytesWritten = 0;
            do
            {
                byte temp = (byte)(value & 0x7F);
                value >>= 7;
                if (value != 0)
                {
                    temp |= 0x80;
                }
                output.WriteByte(temp);
                bytesWritten++;
            } while (value != 0);
            return bytesWritten;
        }

        public static int WriteSignedLEB128(this Stream output, long value)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            if (!output.CanWrite)
                throw new ArgumentException("Cannot write to stream");

            bool more = true;
            int bytesWritten = 0;
            while (more)
            {
                byte temp = (byte)(value & 0x7F);
                value >>= 7;
                more = !((((temp & 0x40) > 0) && value == -1)
                        || ((temp & 0x40) == 0 && value == 0));

                if (more)
                {
                    temp |= 0x80;
                }
                output.WriteByte(temp);
                bytesWritten++;
            }

            return bytesWritten;
        }
    }
}
