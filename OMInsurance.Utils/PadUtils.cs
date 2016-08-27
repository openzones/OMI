using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.Utils
{
    public static class PadUtils
    {
        public static byte[] Pad(byte[] input, int boundary, byte padValue)
        {
            int outboundBytes = input.Length % boundary;
            if (outboundBytes == 0)
            {
                return input;
            }

            int padSize = boundary - outboundBytes;
            byte[] output = new byte[input.Length + padSize];

            Array.Copy(input, 0, output, 0, input.Length);
            for (int i = input.Length; i < input.Length + padSize; i++)
            {
                output[i] = padValue;
            }

            return output;
        }

        public static byte[] Unpad(byte[] input, byte padValue)
        {
            int realLength = input.Length;
            for (int i = input.Length - 1; i >= 0 && input[i] == padValue; i--)
            {
                realLength = i;
            }

            byte[] output = new byte[realLength];
            Array.Copy(input, 0, output, 0, output.Length);

            return output;
        }
    }
}
