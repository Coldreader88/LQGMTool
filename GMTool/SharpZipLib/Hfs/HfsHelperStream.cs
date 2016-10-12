using System;
using System.IO;
using System.Text;

namespace ICSharpCode.SharpZipLib.Hfs
{

    /// <summary>
    /// Holds data pertinent to a data descriptor.
    /// </summary>
    public class DescriptorData
    {
        /// <summary>
        /// Get /set the compressed size of data.
        /// </summary>
        public long CompressedSize
        {
            get { return compressedSize; }
            set { compressedSize = value; }
        }

        /// <summary>
        /// Get / set the uncompressed size of data
        /// </summary>
        public long Size
        {
            get { return size; }
            set { size = value; }
        }

        /// <summary>
        /// Get /set the crc value.
        /// </summary>
        public long Crc
        {
            get { return crc; }
            set { crc = (value & 0xffffffff); }
        }

        #region Instance Fields
        long size;
        long compressedSize;
        long crc;
        #endregion
    }

    class EntryPatchData
    {
        public long SizePatchOffset
        {
            get { return sizePatchOffset_; }
            set { sizePatchOffset_ = value; }
        }

        public long CrcPatchOffset
        {
            get { return crcPatchOffset_; }
            set { crcPatchOffset_ = value; }
        }

        #region Instance Fields
        long sizePatchOffset_;
        long crcPatchOffset_;
        #endregion
    }

    /// <summary>
    /// This class assists with writing/reading from Hfs files.
    /// </summary>
    internal class HfsHelperStream : Stream
    {
        #region Constructors
        /// <summary>
        /// Initialise an instance of this class.
        /// </summary>
        /// <param name="name">The name of the file to open.</param>
        public HfsHelperStream(string name)
        {
            stream_ = new FileStream(name, FileMode.Open, FileAccess.ReadWrite);
            isOwner_ = true;
        }

        /// <summary>
        /// Initialise a new instance of <see cref="HfsHelperStream"/>.
        /// </summary>
        /// <param name="stream">The stream to use.</param>
        public HfsHelperStream(Stream stream)
        {
            stream_ = stream;
        }
        #endregion

        /// <summary>
        /// Get / set a value indicating wether the the underlying stream is owned or not.
        /// </summary>
        /// <remarks>If the stream is owned it is closed when this instance is closed.</remarks>
        public bool IsStreamOwner
        {
            get { return isOwner_; }
            set { isOwner_ = value; }
        }

        #region Base Stream Methods
        public override bool CanRead
        {
            get { return stream_.CanRead; }
        }

        public override bool CanSeek
        {
            get { return stream_.CanSeek; }
        }

#if !NET_1_0 && !NET_1_1 && !NETCF_1_0
        public override bool CanTimeout
        {
            get { return stream_.CanTimeout; }
        }
#endif

        public override long Length
        {
            get { return stream_.Length; }
        }

        public override long Position
        {
            get { return stream_.Position; }
            set { stream_.Position = value; }
        }

        public override bool CanWrite
        {
            get { return stream_.CanWrite; }
        }

        public override void Flush()
        {
            stream_.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return stream_.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            stream_.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return stream_.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            stream_.Write(buffer, offset, count);
        }

        /// <summary>
        /// Close the stream.
        /// </summary>
        /// <remarks>
        /// The underlying stream is closed only if <see cref="IsStreamOwner"/> is true.
        /// </remarks>
        override public void Close()
        {
            Stream toClose = stream_;
            stream_ = null;
            if (isOwner_ && (toClose != null))
            {
                isOwner_ = false;
                toClose.Close();
            }
        }
        #endregion

        // Write the local file header
        // TODO: HfsHelperStream.WriteLocalHeader is not yet used and needs checking for HfsFile and HfsOuptutStream usage
        void WriteLocalHeader(HfsEntry entry, EntryPatchData patchData)
        {
            CompressionMethod method = entry.CompressionMethod;
            bool headerInfoAvailable = true; // How to get this?

            WriteLEInt(HfsConstants.LocalHeaderSignature);

            WriteLEShort(entry.Version);
            WriteLEShort(entry.Flags);
            WriteLEShort((byte)method);
            WriteLEInt((int)entry.DosTime);

            if (headerInfoAvailable == true)
            {
                WriteLEInt((int)entry.CompressedSize);
                WriteLEInt((int)entry.Size);
            }
            else
            {
                if (patchData != null)
                {
                    patchData.CrcPatchOffset = stream_.Position;
                }
                WriteLEInt(0);	// Crc

                if (patchData != null)
                {
                    patchData.SizePatchOffset = stream_.Position;
                }

                WriteLEInt(0);	// Compressed size
                WriteLEInt(0);	// Uncompressed size
            }

            byte[] name = HfsConstants.ConvertToArray(entry.Flags, entry.Name);

            if (name.Length > 0xFFFF)
            {
                throw new HfsException("Entry name too long.");
            }

            byte[] extra = new byte[0];

            WriteLEShort(name.Length);
            WriteLEShort(extra.Length);

            if (name.Length > 0)
            {
                stream_.Write(name, 0, name.Length);
            }

            if (extra.Length > 0)
            {
                stream_.Write(extra, 0, extra.Length);
            }
        }

        /// <summary>
        /// Locates a block with the desired <paramref name="signature"/>.
        /// </summary>
        /// <param name="signature">The signature to find.</param>
        /// <param name="endLocation">Location, marking the end of block.</param>
        /// <param name="minimumBlockSize">Minimum size of the block.</param>
        /// <param name="maximumVariableData">The maximum variable data.</param>
        /// <returns>Eeturns the offset of the first byte after the signature; -1 if not found</returns>
        public long LocateBlockWithSignature(int signature, long endLocation, int minimumBlockSize, int maximumVariableData)
        {
            long pos = endLocation - minimumBlockSize;
            if (pos < 0)
            {
                return -1;
            }

            long giveUpMarker = Math.Max(pos - maximumVariableData, 0);

            // TODO: This loop could be optimised for speed.
            do
            {
                if (pos < giveUpMarker)
                {
                    return -1;
                }
                Seek(pos--, SeekOrigin.Begin);
            } while (ReadLEInt() != signature);

            return Position;
        }

        /// <summary>
        /// Write the required records to end the central directory.
        /// </summary>
        /// <param name="noOfEntries">The number of entries in the directory.</param>
        /// <param name="sizeEntries">The size of the entries in the directory.</param>
        /// <param name="startOfCentralDirectory">The start of the central directory.</param>
        /// <param name="comment">The archive comment.  (This can be null).</param>
        /// <param name="obfuscationKey">obfs key</param>
        public void WriteEndOfCentralDirectory(long noOfEntries, long sizeEntries,
            long startOfCentralDirectory, byte[] comment, uint obfuscationKey)
        {

            if ((noOfEntries >= 0xffff) ||
                (startOfCentralDirectory >= 0xffffffff) ||
                (sizeEntries >= 0xffffffff))
            {
                throw new HfsException("No HFS64");
            }

            WriteLEInt(HfsConstants.EndOfCentralDirectorySignature);

            // TODO: HfsFile Multi disk handling not done
            WriteLEShort(0);                    // number of this disk
            WriteLEShort(0);                    // no of disk with start of central dir


            // Number of entries
            if (noOfEntries >= 0xffff)
            {
                WriteLEUshort(0xffff);  // Hfs64 marker
                WriteLEUshort(0xffff);
            }
            else
            {
                WriteLEShort((short)noOfEntries);          // entries in central dir for this disk
                WriteLEShort((short)noOfEntries);          // total entries in central directory
            }

            // Size of the central directory
            if (sizeEntries >= 0xffffffff)
            {
                WriteLEUint(0xffffffff);    // Hfs64 marker
            }
            else
            {
                WriteLEInt((int)sizeEntries);
            }


            // offset of start of central directory
            if (startOfCentralDirectory >= 0xffffffff)
            {
                WriteLEUint(0xffffffff);    // Hfs64 marker
            }
            else
            {
                WriteLEInt((int)startOfCentralDirectory);
            }

            int commentLength = (comment != null) ? comment.Length : 0;

            if (commentLength > 0xffff)
            {
                throw new HfsException(string.Format("Comment length({0}) is too long can only be 64K", commentLength));
            }

            WriteLEShort(commentLength);

            if (commentLength > 0)
            {
                Write(comment, 0, comment.Length);
            }

            if (obfuscationKey > 0)
            {
                WriteLEInt(0); // TODO
            }

        }

        #region LE value reading/writing
        /// <summary>
        /// Read an unsigned short in little endian byte order.
        /// </summary>
        /// <returns>Returns the value read.</returns>
        /// <exception cref="IOException">
        /// An i/o error occurs.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// The file ends prematurely
        /// </exception>
        public int ReadLEShort()
        {
            int byteValue1 = stream_.ReadByte();

            if (byteValue1 < 0)
            {
                throw new EndOfStreamException();
            }

            int byteValue2 = stream_.ReadByte();
            if (byteValue2 < 0)
            {
                throw new EndOfStreamException();
            }

            return byteValue1 | (byteValue2 << 8);
        }

        /// <summary>
        /// Read an int in little endian byte order.
        /// </summary>
        /// <returns>Returns the value read.</returns>
        /// <exception cref="IOException">
        /// An i/o error occurs.
        /// </exception>
        /// <exception cref="System.IO.EndOfStreamException">
        /// The file ends prematurely
        /// </exception>
        public int ReadLEInt()
        {
            return ReadLEShort() | (ReadLEShort() << 16);
        }

        /// <summary>
        /// Read a long in little endian byte order.
        /// </summary>
        /// <returns>The value read.</returns>
        public long ReadLELong()
        {
            return (uint)ReadLEInt() | ((long)ReadLEInt() << 32);
        }

        /// <summary>
        /// Write an unsigned short in little endian byte order.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteLEShort(int value)
        {
            stream_.WriteByte((byte)(value & 0xff));
            stream_.WriteByte((byte)((value >> 8) & 0xff));
        }

        /// <summary>
        /// Write a ushort in little endian byte order.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteLEUshort(ushort value)
        {
            stream_.WriteByte((byte)(value & 0xff));
            stream_.WriteByte((byte)(value >> 8));
        }

        /// <summary>
        /// Write an int in little endian byte order.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteLEInt(int value)
        {
            WriteLEShort(value);
            WriteLEShort(value >> 16);
        }

        /// <summary>
        /// Write a uint in little endian byte order.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteLEUint(uint value)
        {
            WriteLEUshort((ushort)(value & 0xffff));
            WriteLEUshort((ushort)(value >> 16));
        }

        /// <summary>
        /// Write a long in little endian byte order.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteLELong(long value)
        {
            WriteLEInt((int)value);
            WriteLEInt((int)(value >> 32));
        }

        /// <summary>
        /// Write a ulong in little endian byte order.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteLEUlong(ulong value)
        {
            WriteLEUint((uint)(value & 0xffffffff));
            WriteLEUint((uint)(value >> 32));
        }

        #endregion


        #region Instance Fields
        bool isOwner_;
        Stream stream_;
        #endregion
    }
}
