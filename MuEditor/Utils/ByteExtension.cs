using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuEditor.Utils
{
    static class ByteExtension
    {
        public static void WriteBits(this ref byte variable, byte[] value, int pos)
        {
            for (var i = pos; i < value.Length + pos; i++)
            {
                variable.WriteBit(value[i - pos], i);
            }
        }

        public static void WriteBits(this ref byte variable, uint value, int pos, uint count)
        {
            for (var i = pos; i < count + pos; i++)
            {
                variable.WriteBit(value, i);
            }
        }

        public static byte[] ReadBits(this byte variable, int pos, uint count)
        {
            var result = new List<byte>();
            for (var i = pos; i < count + pos; i++)
            {
                result.Add(variable.ReadBit(i));
            }

            return result.ToArray();
        }

        public static void WriteBit(this ref byte variable, uint value, int pos)
        {
            if (value == 1)
            {
                variable |= (byte) (1 << pos);
                return;
            }

            variable &= (byte) ~(1 << pos);
        }

        public static byte ReadBit(this byte variable, int pos)
        {
            return (byte) ((variable >> pos) & 1);
        }
    }
}