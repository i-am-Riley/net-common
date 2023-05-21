using Rileysoft.Common.Extensions;

namespace Rileysoft.Common.Tests.Extensions
{
    [TestClass]
    public class LEB128Tests
    {
        [DataTestMethod]
        [DataRow(new byte[] { 0x01 }, 1UL)]
        [DataRow(new byte[] { 0x80, 0x01 }, 128UL)]
        [DataRow(new byte[] { 0xE5, 0x8E, 0x26 }, 624485UL)]
        [DataRow(new byte[] { 0xA3, 0xE0, 0xD4, 0xB9, 0xBF, 0x86, 0x02 }, 9019283812387UL)]
        public void ReadUnsignedLEB128_WithValidStream_ReturnsDecodedValue(byte[] input, ulong expected)
        {
            using (MemoryStream stream = new MemoryStream(input))
            {
                ulong decodedValue = stream.ReadUnsignedLEB128();

                Assert.AreEqual(expected, decodedValue);
            }
        }

        [DataTestMethod]
        [DataRow(new byte[] { 0x01 }, 1UL)]
        [DataRow(new byte[] { 0x80, 0x01 }, 128UL)]
        [DataRow(new byte[] { 0xE5, 0x8E, 0x26 }, 624485UL)]
        [DataRow(new byte[] { 0xA3, 0xE0, 0xD4, 0xB9, 0xBF, 0x86, 0x02 }, 9019283812387UL)]
        public void WriteUnsignedLEB128_WithValidStream_ReturnsCorrectValue(byte[] expected, ulong input)
        {
            byte[] buffer = new byte[16];
            int bytesWritten;
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                bytesWritten = stream.WriteUnsignedLEB128(input);
            }

            Assert.AreEqual(expected.Length, bytesWritten);
            for (int i = 0; i < bytesWritten; i++)
            {
                Assert.AreEqual(expected[i], buffer[i], $"Byte {i} is incorrect.");
            }
        }


        [DataTestMethod]
        [DataRow(new byte[] { 0x7F }, -1L)]
        [DataRow(new byte[] { 0xC0, 0xC4, 0x07 }, 123456)]
        [DataRow(new byte[] { 0xC0, 0xBB, 0x78 }, -123456L)]
        [DataRow(new byte[] { 0xDD, 0x9F, 0xAB, 0xC6, 0xC0, 0xF9, 0x7D }, -9019283812387L)]
        public void ReadSignedLEB128_WithValidStream_ReturnsDecodedValue(byte[] input, long expected)
        {
            using (MemoryStream stream = new MemoryStream(input))
            {
                long decodedValue = stream.ReadSignedLEB128();

                Assert.AreEqual(expected, decodedValue);
            }
        }

        [DataTestMethod]
        [DataRow(new byte[] { 0x7F }, -1L)]
        [DataRow(new byte[] { 0xC0, 0xC4, 0x07 }, 123456)]
        [DataRow(new byte[] { 0xC0, 0xBB, 0x78 }, -123456L)]
        [DataRow(new byte[] { 0xDD, 0x9F, 0xAB, 0xC6, 0xC0, 0xF9, 0x7D }, -9019283812387L)]
        public void WriteSignedLEB128_WithValidStream_ReturnsCorrectValue(byte[] expected, long input)
        {
            byte[] buffer = new byte[16];
            int bytesWritten;
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                bytesWritten = stream.WriteSignedLEB128(input);
            }

            Assert.AreEqual(expected.Length, bytesWritten);
            for (int i = 0; i < bytesWritten; i++)
            {
                Assert.AreEqual(expected[i], buffer[i], $"Byte {i} is incorrect.");
            }
        }
    }
}
