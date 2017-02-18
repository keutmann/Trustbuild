using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustchainCore.Extensions
{
    public static class MemoryStreamExtensions
    {
        public static void WriteBytes(this MemoryStream ms, byte[] data)
        {
            ms.Write(data, 0, data.Length);
        }
        public static void WriteString(this MemoryStream ms, string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            ms.WriteBytes(bytes);
        }
        public static void WriteInteger(this MemoryStream ms, int num)
        {
            var bytes = BitConverter.GetBytes(num);
            ms.WriteBytes(bytes);
        }
    }
}
