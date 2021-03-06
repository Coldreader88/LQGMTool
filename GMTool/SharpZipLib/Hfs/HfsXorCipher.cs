﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace ICSharpCode.SharpZipLib.Hfs
{
    static class HfsXorCipher
    {
        public static byte[] XorTruths; // extracted, simplified truths

        static HfsXorCipher()
        {
            try
            {
                Assembly self = Assembly.GetExecutingAssembly();

                Stream xortruths = self.GetManifestResourceStream("ICSharpCode.SharpZipLib.XorTruths.bin");
                XorTruths = new byte[xortruths.Length];
                xortruths.Read(XorTruths, 0, XorTruths.Length);
            }
            catch (Exception ex)
            {
            	throw new Exception("Unable to extract Xor bin: " + ex);
            }
        }

        public static void XorBlockWithKey(byte[] buffer, byte[] key, int src_position)
        {
            Debug.Assert(key.Length == 4 || key.Length == 4096);

            for (int x = 0; x < buffer.Length; x++)
            {
                buffer[x] ^= key[(src_position + x) & (key.Length - 1)];
            }
        }

        public static bool ValidateCompSig(byte[] buffer, out UInt32 decompressed)
        {
            UInt32 magic = BitConverter.ToUInt32(buffer, 0);
            UInt32 decomp = BitConverter.ToUInt32(buffer, 4);

            decompressed = decomp;
            return (magic == HfsConstants.CompSignature);
        }

        public static byte[] BruteforceInnerKey(byte[] buffer, int start)
        {
            byte[] key = new byte[4];

            for (int i = 0; i <= 3; i++)
            {
                int keypos = (int)((start + i) & 3);

                for (byte z = 1; z < 0xFF; z++)
                {
                    byte test = (byte)(buffer[i] ^ z);

                    if ((i == 0 && test == 'c') || (i == 1 && test == 'o') || (i == 2 && test == 'm') || (i == 3 && test == 'p'))
                    {
                        key[keypos] = z;
                    }
                }
            }

            return key;
        }

    }
}
