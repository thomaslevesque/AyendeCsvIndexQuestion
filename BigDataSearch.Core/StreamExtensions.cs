using System;
using System.IO;
using System.Text;

namespace BigDataSearch.Core
{
    static class StreamExtensions
    {
        public static byte[] ReadBytes(this Stream stream, int count)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");
            byte[] buffer = new byte[count];
            int nRead, totalRead = 0;
            while (totalRead < count && (nRead = stream.Read(buffer, totalRead, count - totalRead)) != 0)
            {
                totalRead += nRead;
            }
            if (count > totalRead)
            {
                Array.Resize(ref buffer, totalRead);
            }
            return buffer;
        }

        private static T ReadValue<T>(this Stream stream, Func<byte[], int, T> func, int length)
            where T : struct
        {
            var buffer = stream.ReadBytes(length);
            if (buffer.Length < length)
                throw new InvalidOperationException(
                    string.Format("Cannot read {0} from stream: end of stream", typeof(T).Name));
            return func(buffer, 0);
        }

        public static long ReadInt64(this Stream stream)
        {
            return stream.ReadValue(BitConverter.ToInt64, 8);
        }

        public static int ReadInt32(this Stream stream)
        {
            return stream.ReadValue(BitConverter.ToInt32, 4);
        }

        public static string ReadString(this Stream stream, int byteLength, Encoding encoding)
        {
            var buffer = stream.ReadBytes(byteLength);
            if (buffer.Length < byteLength)
                throw new InvalidOperationException(
                    string.Format("Cannot read {0} bytes from stream: end of stream", byteLength));
            return encoding.GetString(buffer);
        }

        private static int WriteValue<T>(this Stream stream, T value, Func<T, byte[]> func)
        {
            var buffer = func(value);
            stream.Write(buffer, 0, buffer.Length);
            return buffer.Length;
        }

        public static void WriteInt64(this Stream stream, long value)
        {
            stream.WriteValue(value, BitConverter.GetBytes);
        }

        public static void WriteInt32(this Stream stream, int value)
        {
            stream.WriteValue(value, BitConverter.GetBytes);
        }

    }
}
