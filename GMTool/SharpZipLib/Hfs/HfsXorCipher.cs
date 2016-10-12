using System;
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
        public static UInt32[] ChecksumTruths; // extracted (simplified?) checksum truths
        static HfsXorCipher()
        {
            try
            {
                Assembly self = Assembly.GetExecutingAssembly();

                Stream xortruths = self.GetManifestResourceStream("ICSharpCode.SharpZipLib.XorTruths.bin");
                XorTruths = new byte[xortruths.Length];
                xortruths.Read(XorTruths, 0, XorTruths.Length);

                Stream checksumtruths = self.GetManifestResourceStream("ICSharpCode.SharpZipLib.ChecksumTruths.bin");
                byte[] checksumBuffer = new byte[checksumtruths.Length];
                checksumtruths.Read(checksumBuffer, 0, checksumBuffer.Length);

                ChecksumTruths = new UInt32[checksumBuffer.Length / 4];

                for (int i = 0; i < ChecksumTruths.Length; i++)
                {
                    ChecksumTruths[i] = BitConverter.ToUInt32(checksumBuffer, i * 4);
                }
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

        public static UInt32 XorRollWithKey(byte[] buffer, int limit, UInt32[] key, UInt32 checksum)
        {
            for (int x = 0; x < limit; x++)
            {
                checksum = key[buffer[x] ^ (byte)(checksum & 0xFF)] ^ (checksum >> 8);
            }

            return checksum;
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
