using System;
using System.IO;
using System.Security.Cryptography;
using ICSharpCode.SharpZipLib.Hfs;

namespace ICSharpCode.SharpZipLib.Encryption
{
    /// <summary>
    /// Encrypts and decrypts HFS Xor Cipher
    /// </summary>
    internal class HFSXorStream : Stream
    {
        private Stream  _stream;
        private long    _base;

        private byte[]  _key; // for keyed xor

        private bool _close;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stream">The stream on which to perform the cryptographic transformation.</param>
        /// <param name="base_position">Base stream position</param>
        /// <param name="key">Xor key</param>
        /// <param name="close_base">close base stream</param>
        public HFSXorStream(Stream stream, long base_position, byte[] key, bool close_base)
        {

            _stream = stream;
            _base = base_position;
            _key = key;

            _close = close_base;
        }

        /// <summary>
        /// close
        /// </summary>
        public override void Close()
        {
            if(_close)
                _stream.Close();
        }

        /// <summary>
        /// Reads a sequence of bytes from the current CryptoStream into buffer,
        /// and advances the position within the stream by the number of bytes read.
        /// </summary>
        public override int Read(byte[] outBuffer, int offset, int count)
        {
            long base_offset = _base + offset + _stream.Position;

            _stream.Read(outBuffer, offset, count);

            HfsXorCipher.XorBlockWithKey(outBuffer, _key, (int)base_offset);

            return count;
        }

        /// <summary>
        /// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream. </param>
        /// <param name="offset">The byte offset in buffer at which to begin copying bytes to the current stream. </param>
        /// <param name="count">The number of bytes to be written to the current stream. </param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            long base_offset = offset + _stream.Position;

            HfsXorCipher.XorBlockWithKey(buffer, _key, (int)base_offset);

            _stream.Write(buffer, offset, count);
        }


        /// <summary>
        /// Gets value indicating stream can be read from
        /// </summary>
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating if seeking is supported for this stream
        /// This property always returns false
        /// </summary>
        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Get value indicating if this stream supports writing
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                return _stream.CanWrite;
            }
        }

        /// <summary>
        /// Get current length of stream
        /// </summary>
        public override long Length
        {
            get
            {
                return _stream.Length;
            }
        }

        /// <summary>
        /// Gets the current position within the stream.
        /// </summary>
        /// <exception cref="NotSupportedException">Any attempt to set position</exception>
        public override long Position
        {
            get
            {
                return _stream.Position;
            }
            set
            {
                throw new NotSupportedException("Position property not supported");
            }
        }

        /// <summary>
        /// Sets the current position of this stream to the given value. Not supported by this class!
        /// </summary>
        /// <param name="offset">The offset relative to the <paramref name="origin"/> to seek.</param>
        /// <param name="origin">The <see cref="SeekOrigin"/> to seek from.</param>
        /// <returns>The new position in the stream.</returns>
        /// <exception cref="NotSupportedException">Any access</exception>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        /// <summary>
        /// Sets the length of this stream to the given value. Not supported by this class!
        /// </summary>
        /// <param name="value">The new stream length.</param>
        /// <exception cref="NotSupportedException">Any access</exception>
        public override void SetLength(long value)
        {
            throw new NotSupportedException("HfsXorStream SetLength not supported");
        }

        /// <summary>
        /// Read a byte from stream advancing position by one
        /// </summary>
        /// <returns>The byte read cast to an int.  THe value is -1 if at the end of the stream.</returns>
        /// <exception cref="NotSupportedException">Any access</exception>
        public override int ReadByte()
        {
            throw new NotSupportedException("HfsXorStream ReadByte not supported");
        }

        /// <summary>
        /// Asynchronous reads are not supported a NotSupportedException is always thrown
        /// </summary>
        /// <param name="buffer">The buffer to read into.</param>
        /// <param name="offset">The offset to start storing data at.</param>
        /// <param name="count">The number of bytes to read</param>
        /// <param name="callback">The async callback to use.</param>
        /// <param name="state">The state to use.</param>
        /// <returns>Returns an <see cref="IAsyncResult"/></returns>
        /// <exception cref="NotSupportedException">Any access</exception>
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException("HfsXorStream BeginRead not currently supported");
        }

        /// <summary>
        /// Asynchronous writes arent supported, a NotSupportedException is always thrown
        /// </summary>
        /// <param name="buffer">The buffer to write.</param>
        /// <param name="offset">The offset to begin writing at.</param>
        /// <param name="count">The number of bytes to write.</param>
        /// <param name="callback">The <see cref="AsyncCallback"/> to use.</param>
        /// <param name="state">The state object.</param>
        /// <returns>Returns an IAsyncResult.</returns>
        /// <exception cref="NotSupportedException">Any access</exception>
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException("BeginWrite is not supported");
        }

        /// <summary>
        /// Flushes the stream
        /// </summary>
        public override void Flush()
        {
            _stream.Flush();
        }

    }
}
