using System.Collections.Generic;

namespace Texture
{
    public static class DXTHeuristics
    {
        private static readonly List<byte[]> dxt5 = new List<byte[]>()
        {
      new byte[8]
      {
        4,
        41,
        3,
        41,
        170,
        170,
        170,
        170
      },
      new byte[8]
      {
        243,
        156,
        211,
        156,
        170,
        170,
        170,
        170
      },
      new byte[6]
      {
        73,
        146,
        36,
        73,
        146,
        36
      },
      new byte[8]
      {
        byte.MaxValue,
        byte.MaxValue,
        0,
        0,
        0,
        0,
        0,
        0
      },
      new byte[8]
      {
        0,
        1,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue
      },
      new byte[8]
      {
        0,
        5,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue
      }
    };
        private static readonly List<byte[]> dxt3 = new List<byte[]>()
    {
      new byte[8]
      {
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue
      }
    };
        private static int CountHits(byte[] input, List<byte[]> data)
        {
            int num = 0;
            for (int index1 = 0; index1 < data.Count; ++index1)
            {
                byte[] numArray = data[index1];
                for (int index2 = 8 - numArray.Length; index2 < input.Length; index2 += 16)
                {
                    bool flag = false;
                    for (int index3 = 0; index3 < numArray.Length; ++index3)
                    {
                        if ((int)input[index2 + index3] != numArray[index3])
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                        ++num;
                }
            }
            return num;
        }

        public static void CountHits(byte[] input, ref int dxt3Count, ref int dxt5Count)
        {
            dxt3Count += CountHits(input, dxt3);
            dxt5Count += CountHits(input, dxt5);
        }
    }
}
