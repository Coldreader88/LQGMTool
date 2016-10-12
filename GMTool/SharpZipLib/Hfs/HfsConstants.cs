using System;
using System.Text;
using System.Threading;

#if NETCF_1_0 || NETCF_2_0
using System.Globalization;
#endif

namespace ICSharpCode.SharpZipLib.Hfs
{

    #region Enumerations

    /// <summary>
    /// The kind of compression used for an entry in an archive
    /// </summary>
    public enum CompressionMethod
    {
        /// <summary>
        /// A direct copy of the file contents is held in the archive
        /// </summary>
        Stored = 0
    }

    #endregion

    /// <summary>
    /// This class contains constants used for Hfs format files
    /// </summary>
    public sealed class HfsConstants
    {
        #region Versions
        /// <summary>
        /// The version made by field for entries in the central header when created by this library
        /// </summary>
        /// <remarks>
        /// This is also the Hfs version for the library when comparing against the version required to extract
        /// for an entry.  See .
        /// </remarks>
        public const int VersionMadeBy = 20; // was 45 before AES

        /// <summary>
        /// The version made by field for entries in the central header when created by this library
        /// </summary>
        /// <remarks>
        /// This is also the Hfs version for the library when comparing against the version required to extract
        /// for an entry.  See HfsInputStream.CanDecompressEntry.
        /// </remarks>
        [Obsolete("Use VersionMadeBy instead")]
        public const int VERSION_MADE_BY = 10;

        #endregion

        #region Header Sizes
        /// <summary>
        /// Size of local entry header (excluding variable length fields at end)
        /// </summary>
        public const int LocalHeaderBaseSize = 30;

        /// <summary>
        /// Size of local entry header (excluding variable length fields at end)
        /// </summary>
        [Obsolete("Use LocalHeaderBaseSize instead")]
        public const int LOCHDR = 30;

        /// <summary>
        /// Size of central header entry (excluding variable fields)
        /// </summary>
        public const int CentralHeaderBaseSize = 46;

        /// <summary>
        /// Size of central header entry
        /// </summary>
        [Obsolete("Use CentralHeaderBaseSize instead")]
        public const int CENHDR = 46;

        /// <summary>
        /// Size of end of central record (excluding variable fields)
        /// </summary>
        public const int EndOfCentralRecordBaseSize = 22;

        /// <summary>
        /// Size of end of central record (excluding variable fields)
        /// </summary>
        [Obsolete("Use EndOfCentralRecordBaseSize instead")]
        public const int ENDHDR = 22;

        #endregion

        #region Header Signatures

        /// <summary>
        /// Signature for comp
        /// </summary>
        public const int CompSignature = 'c' | ('o' << 8) | ('m' << 16) | ('p' << 24);

        /// <summary>
        /// Signature for local entry header
        /// </summary>
        public const int LocalHeaderSignature = 'H' | ('F' << 8) | (1 << 16) | (2 << 24);

        /// <summary>
        /// Signature for local entry header
        /// </summary>
        [Obsolete("Use LocalHeaderSignature instead")]
        public const int LOCSIG = 'H' | ('F' << 8) | (1 << 16) | (2 << 24);

        /// <summary>
        /// Signature for central header
        /// </summary>
        [Obsolete("Use CentralHeaderSignature instead")]
        public const int CENSIG = LocalHeaderSignature;

        /// <summary>
        /// Signature for central header
        /// </summary>
        public const int CentralHeaderSignature = LocalHeaderSignature;


        /// <summary>
        /// End of central directory record signature
        /// </summary>
        public const int EndOfCentralDirectorySignature = 'H' | ('F' << 8) | (5 << 16) | (6 << 24);

        /// <summary>
        /// End of central directory record signature
        /// </summary>
        [Obsolete("Use EndOfCentralDirectorySignature instead")]
        public const int ENDSIG = 'H' | ('F' << 8) | (5 << 16) | (6 << 24);
        #endregion

#if NETCF_1_0 || NETCF_2_0
		// This isnt so great but is better than nothing.
        // Trying to work out an appropriate OEM code page would be good.
        // 850 is a good default for english speakers particularly in Europe.
		static int defaultCodePage = CultureInfo.CurrentCulture.TextInfo.ANSICodePage;
#else
        static int defaultCodePage = Thread.CurrentThread.CurrentCulture.TextInfo.OEMCodePage;
#endif

        /// <summary>
        /// Default encoding used for string conversion.  0 gives the default system OEM code page.
        /// Dont use unicode encodings if you want to be Hfs compatible!
        /// Using the default code page isnt the full solution neccessarily
        /// there are many variable factors, codepage 850 is often a good choice for
        /// European users, however be careful about compatability.
        /// </summary>
        public static int DefaultCodePage
        {
            get
            {
                return defaultCodePage;
            }
            set
            {
                defaultCodePage = value;
            }
        }

        /// <summary>Convert obfuscated string</summary>
        public static string ConvertToStringObfs(byte[] data, int count, int src_offset)
        {
            if (data == null)
            {
                return string.Empty;
            }

            HfsXorCipher.XorBlockWithKey(data, HfsXorCipher.XorTruths, src_offset);

            return Encoding.GetEncoding(DefaultCodePage).GetString(data, 0, count);
        }

        /// <summary>
        /// Convert a portion of a byte array to a string.
        /// </summary>		
        /// <param name="data">
        /// Data to convert to string
        /// </param>
        /// <param name="count">
        /// Number of bytes to convert starting from index 0
        /// </param>
        /// <returns>
        /// data[0]..data[length - 1] converted to a string
        /// </returns>
        public static string ConvertToString(byte[] data, int count)
        {
            if (data == null)
            {
                return string.Empty;
            }

            return Encoding.GetEncoding(DefaultCodePage).GetString(data, 0, count);
        }

        /// <summary>
        /// Convert a byte array to string
        /// </summary>
        /// <param name="data">
        /// Byte array to convert
        /// </param>
        /// <returns>
        /// <paramref name="data">data</paramref>converted to a string
        /// </returns>
        public static string ConvertToString(byte[] data)
        {
            if (data == null)
            {
                return string.Empty;
            }
            return ConvertToString(data, data.Length);
        }

        /// <summary>
        /// Convert a byte array to string
        /// </summary>
        /// <param name="flags">The applicable general purpose bits flags</param>
        /// <param name="data">
        /// Byte array to convert
        /// </param>
        /// <param name="count">The number of bytes to convert.</param>
        /// <returns>
        /// <paramref name="data">data</paramref>converted to a string
        /// </returns>
        public static string ConvertToStringExt(int flags, byte[] data, int count)
        {
            if (data == null)
            {
                return string.Empty;
            }

            return ConvertToString(data, count);
        }

        /// <summary>
        /// Convert a byte array to string
        /// </summary>
        /// <param name="data">
        /// Byte array to convert
        /// </param>
        /// <param name="flags">The applicable general purpose bits flags</param>
        /// <returns>
        /// <paramref name="data">data</paramref>converted to a string
        /// </returns>
        public static string ConvertToStringExt(int flags, byte[] data)
        {
            if (data == null)
            {
                return string.Empty;
            }

            return ConvertToString(data, data.Length);
        }

        /// <summary>
        /// Convert a string to a byte array
        /// </summary>
        /// <param name="str">
        /// String to convert to an array
        /// </param>
        /// <returns>Converted array</returns>
        public static byte[] ConvertToArray(string str)
        {
            if (str == null)
            {
                return new byte[0];
            }

            return Encoding.GetEncoding(DefaultCodePage).GetBytes(str);
        }

        /// <summary>
        /// Convert a string to a byte array
        /// </summary>
        /// <param name="flags">The applicable general purpose bits flags</param>
        /// <param name="str">
        /// String to convert to an array
        /// </param>
        /// <returns>Converted array</returns>
        public static byte[] ConvertToArray(int flags, string str)
        {
            if (str == null)
            {
                return new byte[0];
            }

            return ConvertToArray(str);
        }


        /// <summary>
        /// Initialise default instance of <see cref="HfsConstants">HfsConstants</see>
        /// </summary>
        /// <remarks>
        /// Private to prevent instances being created.
        /// </remarks>
        HfsConstants()
        {
            // Do nothing
        }
    }
}
