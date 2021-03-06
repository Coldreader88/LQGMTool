﻿// mostly a direct copy of Hfs
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Globalization;
using System.IO.Compression;

#if !NETCF_1_0
using System.Security.Cryptography;
using ICSharpCode.SharpZipLib.Encryption;
#endif

using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.GZip;
using System.Diagnostics;

namespace ICSharpCode.SharpZipLib.Hfs
{
    #region Keys Required Event Args
    /// <summary>
    /// Arguments used with KeysRequiredEvent
    /// </summary>
    public class KeysRequiredEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Initialise a new instance of <see cref="KeysRequiredEventArgs"></see>
        /// </summary>
        /// <param name="name">The name of the file for which keys are required.</param>
        public KeysRequiredEventArgs(string name)
        {
            fileName = name;
        }

        /// <summary>
        /// Initialise a new instance of <see cref="KeysRequiredEventArgs"></see>
        /// </summary>
        /// <param name="name">The name of the file for which keys are required.</param>
        /// <param name="keyValue">The current key value.</param>
        public KeysRequiredEventArgs(string name, byte[] keyValue)
        {
            fileName = name;
            key = keyValue;
        }

        #endregion
        #region Properties
        /// <summary>
        /// Gets the name of the file for which keys are required.
        /// </summary>
        public string FileName
        {
            get { return fileName; }
        }

        /// <summary>
        /// Gets or sets the key value
        /// </summary>
        public byte[] Key
        {
            get { return key; }
            set { key = value; }
        }
        #endregion

        #region Instance Fields
        string fileName;
        byte[] key;
        #endregion
    }
    #endregion

    #region Test Definitions
    /// <summary>
    /// The strategy to apply to testing.
    /// </summary>
    public enum TestStrategy
    {
        /// <summary>
        /// Find the first error only.
        /// </summary>
        FindFirstError,
        /// <summary>
        /// Find all possible errors.
        /// </summary>
        FindAllErrors,
    }

    /// <summary>
    /// The operation in progress reported by a <see cref="HfsTestResultHandler"/> during testing.
    /// </summary>
    /// <seealso cref="HfsFile.TestArchive(bool)">TestArchive</seealso>
    public enum TestOperation
    {
        /// <summary>
        /// Setting up testing.
        /// </summary>
        Initialising,

        /// <summary>
        /// Testing an individual entries header
        /// </summary>
        EntryHeader,

        /// <summary>
        /// Testing an individual entries data
        /// </summary>
        EntryData,

        /// <summary>
        /// Testing an individual entry has completed.
        /// </summary>
        EntryComplete,

        /// <summary>
        /// Running miscellaneous tests
        /// </summary>
        MiscellaneousTests,

        /// <summary>
        /// Testing is complete
        /// </summary>
        Complete,
    }

    /// <summary>
    /// Status returned returned by <see cref="HfsTestResultHandler"/> during testing.
    /// </summary>
    /// <seealso cref="HfsFile.TestArchive(bool)">TestArchive</seealso>
    public class TestStatus
    {
        #region Constructors
        /// <summary>
        /// Initialise a new instance of <see cref="TestStatus"/>
        /// </summary>
        /// <param name="file">The <see cref="HfsFile"/> this status applies to.</param>
        public TestStatus(HfsFile file)
        {
            file_ = file;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Get the current <see cref="TestOperation"/> in progress.
        /// </summary>
        public TestOperation Operation
        {
            get { return operation_; }
        }

        /// <summary>
        /// Get the <see cref="HfsFile"/> this status is applicable to.
        /// </summary>
        public HfsFile File
        {
            get { return file_; }
        }

        /// <summary>
        /// Get the current/last entry tested.
        /// </summary>
        public HfsEntry Entry
        {
            get { return entry_; }
        }

        /// <summary>
        /// Get the number of errors detected so far.
        /// </summary>
        public int ErrorCount
        {
            get { return errorCount_; }
        }

        /// <summary>
        /// Get the number of bytes tested so far for the current entry.
        /// </summary>
        public long BytesTested
        {
            get { return bytesTested_; }
        }

        /// <summary>
        /// Get a value indicating wether the last entry test was valid.
        /// </summary>
        public bool EntryValid
        {
            get { return entryValid_; }
        }
        #endregion

        #region Internal API
        internal void AddError()
        {
            errorCount_++;
            entryValid_ = false;
        }

        internal void SetOperation(TestOperation operation)
        {
            operation_ = operation;
        }

        internal void SetEntry(HfsEntry entry)
        {
            entry_ = entry;
            entryValid_ = true;
            bytesTested_ = 0;
        }

        internal void SetBytesTested(long value)
        {
            bytesTested_ = value;
        }
        #endregion

        #region Instance Fields
        HfsFile file_;
        HfsEntry entry_;
        bool entryValid_;
        int errorCount_;
        long bytesTested_;
        TestOperation operation_;
        #endregion
    }

    /// <summary>
    /// Delegate invoked during <see cref="HfsFile.TestArchive(bool, TestStrategy, HfsTestResultHandler)">testing</see> if supplied indicating current progress and status.
    /// </summary>
    /// <remarks>If the message is non-null an error has occured.  If the message is null
    /// the operation as found in <see cref="TestStatus">status</see> has started.</remarks>
    public delegate void HfsTestResultHandler(TestStatus status, string message);
    #endregion

    #region Update Definitions
    /// <summary>
    /// The possible ways of <see cref="HfsFile.CommitUpdate()">applying updates</see> to an archive.
    /// </summary>
    public enum FileUpdateMode
    {
        /// <summary>
        /// Perform all updates on temporary files ensuring that the original file is saved.
        /// </summary>
        Safe,
        /// <summary>
        /// Update the archive directly, which is faster but less safe.
        /// </summary>
        Direct,
    }
    #endregion

    #region HfsFile Class
    /// <summary>
    /// This class represents a Hfs archive.  You can ask for the contained
    /// entries, or get an input stream for a file entry.  The entry is
    /// automatically decompressed.
    /// 
    /// You can also update the archive adding or deleting entries.
    /// 
    /// This class is thread safe for input:  You can open input streams for arbitrary
    /// entries in different threads.
    /// <br/>
    /// <br/>Author of the original java version : Jochen Hoenicke
    /// </summary>
    /// <example>
    /// <code>
    /// using System;
    /// using System.Text;
    /// using System.Collections;
    /// using System.IO;
    /// 
    /// using ICSharpCode.SharpZipLib.Hfs;
    /// 
    /// class MainClass
    /// {
    /// 	static public void Main(string[] args)
    /// 	{
    /// 		using (HfsFile zFile = new HfsFile(args[0])) {
    /// 			Console.WriteLine("Listing of : " + zFile.Name);
    /// 			Console.WriteLine("");
    /// 			Console.WriteLine("Raw Size    Size      Date     Time     Name");
    /// 			Console.WriteLine("--------  --------  --------  ------  ---------");
    /// 			foreach (HfsEntry e in zFile) {
    /// 				if ( e.IsFile ) {
    /// 					DateTime d = e.DateTime;
    /// 					Console.WriteLine("{0, -10}{1, -10}{2}  {3}   {4}", e.Size, e.CompressedSize,
    /// 						d.ToString("dd-MM-yy"), d.ToString("HH:mm"),
    /// 						e.Name);
    /// 				}
    /// 			}
    /// 		}
    /// 	}
    /// }
    /// </code>
    /// </example>
    public class HfsFile : IEnumerable, IDisposable
    {
        #region Constructors
        /// <summary>
        /// Opens a Hfs file with the given name for reading.
        /// </summary>
        /// <param name="name">The name of the file to open.</param>
        /// <exception cref="ArgumentNullException">The argument supplied is null.</exception>
        /// <exception cref="IOException">
        /// An i/o error occurs
        /// </exception>
        /// <exception cref="HfsException">
        /// The file doesn't contain a valid Hfs archive.
        /// </exception>
        public HfsFile(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            name_ = name;

            baseStream_ = File.Open(name, FileMode.Open, FileAccess.Read, FileShare.Read);
            isStreamOwner = true;

            try
            {
                ReadEntries();
            }
            catch
            {
                DisposeInternal(true);
                throw;
            }
        }

        /// <summary>
        /// Opens a Hfs file reading the given <see cref="FileStream"/>.
        /// </summary>
        /// <param name="file">The <see cref="FileStream"/> to read archive data from.</param>
        /// <exception cref="ArgumentNullException">The supplied argument is null.</exception>
        /// <exception cref="IOException">
        /// An i/o error occurs.
        /// </exception>
        /// <exception cref="HfsException">
        /// The file doesn't contain a valid Hfs archive.
        /// </exception>
        public HfsFile(FileStream file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            if (!file.CanSeek)
            {
                throw new ArgumentException("Stream is not seekable", "file");
            }

            baseStream_ = file;
            name_ = file.Name;
            isStreamOwner = true;

            try
            {
                ReadEntries();
            }
            catch
            {
                DisposeInternal(true);
                throw;
            }
        }

        /// <summary>
        /// Opens a Hfs file reading the given <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read archive data from.</param>
        /// <exception cref="IOException">
        /// An i/o error occurs
        /// </exception>
        /// <exception cref="HfsException">
        /// The stream doesn't contain a valid Hfs archive.<br/>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <see cref="Stream">stream</see> doesnt support seeking.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The <see cref="Stream">stream</see> argument is null.
        /// </exception>
        public HfsFile(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (!stream.CanSeek)
            {
                throw new ArgumentException("Stream is not seekable", "stream");
            }

            baseStream_ = stream;
            isStreamOwner = true;

            if (baseStream_.Length > 0)
            {
                try
                {
                    ReadEntries();
                }
                catch
                {
                    DisposeInternal(true);
                    throw;
                }
            }
            else
            {
                entries_ = new HfsEntry[0];
                isNewArchive_ = true;
            }
        }

        /// <summary>
        /// Initialises a default <see cref="HfsFile"/> instance with no entries and no file storage.
        /// </summary>
        internal HfsFile()
        {
            entries_ = new HfsEntry[0];
            isNewArchive_ = true;
        }

        #endregion

        #region Destructors and Closing
        /// <summary>
        /// Finalize this instance.
        /// </summary>
        ~HfsFile()
        {
            Dispose(false);
        }

        /// <summary>
        /// Closes the HfsFile.  If the stream is <see cref="IsStreamOwner">owned</see> then this also closes the underlying input stream.
        /// Once closed, no further instance methods should be called.
        /// </summary>
        /// <exception cref="System.IO.IOException">
        /// An i/o error occurs.
        /// </exception>
        public void Close()
        {
            DisposeInternal(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Creators
        /// <summary>
        /// Create a new <see cref="HfsFile"/> whose data will be stored in a file.
        /// </summary>
        /// <param name="fileName">The name of the archive to create.</param>
        /// <returns>Returns the newly created <see cref="HfsFile"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="fileName"></paramref> is null</exception>
        public static HfsFile Create(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            FileStream fs = File.Create(fileName);

            HfsFile result = new HfsFile();
            result.name_ = fileName;
            result.baseStream_ = fs;
            result.isStreamOwner = true;
            return result;
        }

        /// <summary>
        /// Create a new <see cref="HfsFile"/> whose data will be stored on a stream.
        /// </summary>
        /// <param name="outStream">The stream providing data storage.</param>
        /// <returns>Returns the newly created <see cref="HfsFile"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="outStream"> is null</paramref></exception>
        /// <exception cref="ArgumentException"><paramref name="outStream"> doesnt support writing.</paramref></exception>
        public static HfsFile Create(Stream outStream)
        {
            if (outStream == null)
            {
                throw new ArgumentNullException("outStream");
            }

            if (!outStream.CanWrite)
            {
                throw new ArgumentException("Stream is not writeable", "outStream");
            }

            if (!outStream.CanSeek)
            {
                throw new ArgumentException("Stream is not seekable", "outStream");
            }

            HfsFile result = new HfsFile();
            result.baseStream_ = outStream;
            return result;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get/set a flag indicating if the underlying stream is owned by the HfsFile instance.
        /// If the flag is true then the stream will be closed when <see cref="Close">Close</see> is called.
        /// </summary>
        /// <remarks>
        /// The default value is true in all cases.
        /// </remarks>
        public bool IsStreamOwner
        {
            get { return isStreamOwner; }
            set { isStreamOwner = value; }
        }

        /// <summary>
        /// Get a value indicating wether
        /// this archive is embedded in another file or not.
        /// </summary>
        public bool IsEmbeddedArchive
        {
            // Not strictly correct in all circumstances currently
            get { return offsetOfFirstEntry > 0; }
        }

        /// <summary>
        /// Get a value indicating that this archive is a new one.
        /// </summary>
        public bool IsNewArchive
        {
            get { return isNewArchive_; }
        }

        /// <summary>
        /// Gets the comment for the Hfs file.
        /// </summary>
        public string HfsFileComment
        {
            get { return comment_; }
        }

        /// <summary>
        /// Gets the name of this Hfs file.
        /// </summary>
        public string Name
        {
            get { return name_; }
        }

        /// <summary>
        /// Gets obfuscation key.
        /// </summary>
        public int ObfuscationKey
        {
            get { return (int)obfuscationkey_; }
            set { obfuscationkey_ = (uint)value; bytekey_ = BitConverter.GetBytes(obfuscationkey_);  }
        }

        /// <summary>
        /// Gets the number of entries in this Hfs file.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The Hfs file has been closed.
        /// </exception>
        [Obsolete("Use the Count property instead")]
        public int Size
        {
            get
            {
                return entries_.Length;
            }
        }

        /// <summary>
        /// Get the number of entries contained in this <see cref="HfsFile"/>.
        /// </summary>
        public long Count
        {
            get
            {
                return entries_.Length;
            }
        }

        /// <summary>
        /// Indexer property for HfsEntries
        /// </summary>
        [System.Runtime.CompilerServices.IndexerNameAttribute("EntryByIndex")]
        public HfsEntry this[int index]
        {
            get
            {
                return (HfsEntry)entries_[index].Clone();
            }
        }

        #endregion

        #region Input Handling
        /// <summary>
        /// Gets an enumerator for the Hfs entries in this Hfs file.
        /// </summary>
        /// <returns>Returns an <see cref="IEnumerator"/> for this archive.</returns>
        /// <exception cref="ObjectDisposedException">
        /// The Hfs file has been closed.
        /// </exception>
        public IEnumerator GetEnumerator()
        {
            if (isDisposed_)
            {
                throw new ObjectDisposedException("HfsFile");
            }

            return new HfsEntryEnumerator(entries_);
        }

        /// <summary>
        /// Return the index of the entry with a matching name
        /// </summary>
        /// <param name="name">Entry name to find</param>
        /// <param name="ignoreCase">If true the comparison is case insensitive</param>
        /// <returns>The index position of the matching entry or -1 if not found</returns>
        /// <exception cref="ObjectDisposedException">
        /// The Hfs file has been closed.
        /// </exception>
        public int FindEntry(string name, bool ignoreCase)
        {
            if (isDisposed_)
            {
                throw new ObjectDisposedException("HfsFile");
            }

            // TODO: This will be slow as the next ice age for huge archives!
            for (int i = 0; i < entries_.Length; i++)
            {
                if (string.Compare(name, entries_[i].Name, ignoreCase, CultureInfo.InvariantCulture) == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Searches for a Hfs entry in this archive with the given name.
        /// String comparisons are case insensitive
        /// </summary>
        /// <param name="name">
        /// The name to find. May contain directory components separated by slashes ('/').
        /// </param>
        /// <returns>
        /// A clone of the Hfs entry, or null if no entry with that name exists.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The Hfs file has been closed.
        /// </exception>
        public HfsEntry GetEntry(string name)
        {
            if (isDisposed_)
            {
                throw new ObjectDisposedException("HfsFile");
            }

            int index = FindEntry(name, true);
            return (index >= 0) ? (HfsEntry)entries_[index].Clone() : null;
        }

        /// <summary>
        /// Gets an input stream for reading the given Hfs entry data in an uncompressed form.
        /// Normally the <see cref="HfsEntry"/> should be an entry returned by GetEntry().
        /// </summary>
        /// <param name="entry">The <see cref="HfsEntry"/> to obtain a data <see cref="Stream"/> for</param>
        /// <returns>An input <see cref="Stream"/> containing data for this <see cref="HfsEntry"/></returns>
        /// <exception cref="ObjectDisposedException">
        /// The HfsFile has already been closed
        /// </exception>
        /// <exception cref="ICSharpCode.SharpZipLib.Hfs.HfsException">
        /// The compression method for the entry is unknown
        /// </exception>
        /// <exception cref="IndexOutOfRangeException">
        /// The entry is not found in the HfsFile
        /// </exception>
        public Stream GetInputStream(HfsEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }

            if (isDisposed_)
            {
                throw new ObjectDisposedException("HfsFile");
            }

            long index = entry.HfsFileIndex;
            if ((index < 0) || (index >= entries_.Length) || (entries_[index].Name != entry.Name))
            {
                index = FindEntry(entry.Name, true);
                if (index < 0)
                {
                    throw new HfsException("Entry cannot be found");
                }
            }
            return GetInputStream(index);
        }

        /// <summary>
        /// Creates an input stream reading a Hfs entry
        /// </summary>
        /// <param name="entryIndex">The index of the entry to obtain an input stream for.</param>
        /// <returns>
        /// An input <see cref="Stream"/> containing data for this <paramref name="entryIndex"/>
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The HfsFile has already been closed
        /// </exception>
        /// <exception cref="ICSharpCode.SharpZipLib.Hfs.HfsException">
        /// The compression method for the entry is unknown
        /// </exception>
        /// <exception cref="IndexOutOfRangeException">
        /// The entry is not found in the HfsFile
        /// </exception>
        public Stream GetInputStream(long entryIndex)
        {
            if (isDisposed_)
            {
                throw new ObjectDisposedException("HfsFile");
            }

            HfsEntry entry = entries_[entryIndex];
            long start = LocateEntry(entry);
            CompressionMethod method = entries_[entryIndex].CompressionMethod;
            Stream result = new PartialInputStream(this, start, entries_[entryIndex].CompressedSize);

            switch (method)
            {
                case CompressionMethod.Stored:
                    {
                        Stream base_result = result = new HFSXorStream(result, start, HfsXorCipher.XorTruths, true);

                        if (obfuscationkey_ > 0)
                        {
                            result = new HFSXorStream(result, start, bytekey_, true);
                        }

                        if (entry.Name.Substring(entry.Name.Length - 5) == ".comp")
                        {
                            UInt32 decomp;

                            // technically we don't decompress this, but we will to make it easier (.comp is transparently handled in this lib)
                            byte[] compHeader = new byte[8];
                            result.Read(compHeader, 0, compHeader.Length);

                            if (!HfsXorCipher.ValidateCompSig(compHeader, out decomp))
                            {
                                if (obfuscationkey_ == 0)
                                {
                                    throw new Exception("No obfs key, bad signature");
                                }

                                base_result.Seek(-8, SeekOrigin.Current);
                                base_result.Read(compHeader, 0, compHeader.Length);

                                key = HfsXorCipher.BruteforceInnerKey(compHeader, (int)start);
                                HfsXorCipher.XorBlockWithKey(compHeader, key, (int)start);

                                if (!HfsXorCipher.ValidateCompSig(compHeader, out decomp))
                                {
                                    throw new Exception("Bad compression signature");
                                }

                                Console.WriteLine("Had to brute-force inner XOR key");
                                
                                bytekey_ = key;
                                obfuscationkey_ = BitConverter.ToUInt32(bytekey_, 0);

                                result = new HFSXorStream(base_result, start, key, true);
                            }

                            entry.Size = decomp;

                            // to ease copying the stream straight to the zip we wrap it with the correct length
                            result = new WrapperStream(new InflaterInputStream(result), entry.Size);
                        }
                    }
                    break;

                default:
                    throw new HfsException("Unsupported compression method " + method);
            }

            return result;
        }

        #endregion

        #region Archive Testing
        /// <summary>
        /// Test an archive for integrity/validity
        /// </summary>
        /// <param name="testData">Perform low level data Crc check</param>
        /// <returns>true if all tests pass, false otherwise</returns>
        /// <remarks>Testing will terminate on the first error found.</remarks>
        public bool TestArchive(bool testData)
        {
            return TestArchive(testData, TestStrategy.FindFirstError, null);
        }

        /// <summary>
        /// Test an archive for integrity/validity
        /// </summary>
        /// <param name="testData">Perform low level data Crc check</param>
        /// <param name="strategy">The <see cref="TestStrategy"></see> to apply.</param>
        /// <param name="resultHandler">The <see cref="HfsTestResultHandler"></see> handler to call during testing.</param>
        /// <returns>true if all tests pass, false otherwise</returns>
        /// <exception cref="ObjectDisposedException">The object has already been closed.</exception>
        public bool TestArchive(bool testData, TestStrategy strategy, HfsTestResultHandler resultHandler)
        {
            if (isDisposed_)
            {
                throw new ObjectDisposedException("HfsFile");
            }

            TestStatus status = new TestStatus(this);

            if (resultHandler != null)
            {
                resultHandler(status, null);
            }

            HeaderTest test = testData ? (HeaderTest.Header | HeaderTest.Extract) : HeaderTest.Header;

            bool testing = true;

            try
            {
                int entryIndex = 0;

                while (testing && (entryIndex < Count))
                {
                    if (resultHandler != null)
                    {
                        status.SetEntry(this[entryIndex]);
                        status.SetOperation(TestOperation.EntryHeader);
                        resultHandler(status, null);
                    }

                    try
                    {
                        TestLocalHeader(this[entryIndex], test);
                    }
                    catch (HfsException ex)
                    {
                        status.AddError();

                        if (resultHandler != null)
                        {
                            resultHandler(status,
                                string.Format("Exception during test - '{0}'", ex.Message));
                        }

                        if (strategy == TestStrategy.FindFirstError)
                        {
                            testing = false;
                        }
                    }

                    if (testing && testData && this[entryIndex].IsFile)
                    {
                        if (resultHandler != null)
                        {
                            status.SetOperation(TestOperation.EntryData);
                            resultHandler(status, null);
                        }

                        Crc32 crc = new Crc32();

                        using (Stream entryStream = this.GetInputStream(this[entryIndex]))
                        {

                            byte[] buffer = new byte[4096];
                            long totalBytes = 0;
                            int bytesRead;
                            while ((bytesRead = entryStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                crc.Update(buffer, 0, bytesRead);

                                if (resultHandler != null)
                                {
                                    totalBytes += bytesRead;
                                    status.SetBytesTested(totalBytes);
                                    resultHandler(status, null);
                                }
                            }
                        }

                        if (this[entryIndex].Crc != crc.Value)
                        {
                            status.AddError();

                            if (resultHandler != null)
                            {
                                resultHandler(status, "CRC mismatch");
                            }

                            if (strategy == TestStrategy.FindFirstError)
                            {
                                testing = false;
                            }
                        }

                    }

                    if (resultHandler != null)
                    {
                        status.SetOperation(TestOperation.EntryComplete);
                        resultHandler(status, null);
                    }

                    entryIndex += 1;
                }

                if (resultHandler != null)
                {
                    status.SetOperation(TestOperation.MiscellaneousTests);
                    resultHandler(status, null);
                }

                // TODO: the 'Corrina Johns' test where local headers are missing from
                // the central directory.  They are therefore invisible to many archivers.
            }
            catch (Exception ex)
            {
                status.AddError();

                if (resultHandler != null)
                {
                    resultHandler(status, string.Format("Exception during test - '{0}'", ex.Message));
                }
            }

            if (resultHandler != null)
            {
                status.SetOperation(TestOperation.Complete);
                status.SetEntry(null);
                resultHandler(status, null);
            }

            return (status.ErrorCount == 0);
        }

        [Flags]
        enum HeaderTest
        {
            Extract = 0x01,     // Check that this header represents an entry whose data can be extracted
            Header = 0x02,     // Check that this header contents are valid
        }

        /// <summary>
        /// Test a local header against that provided from the central directory
        /// </summary>
        /// <param name="entry">
        /// The entry to test against
        /// </param>
        /// <param name="tests">The type of <see cref="HeaderTest">tests</see> to carry out.</param>
        /// <returns>The offset of the entries data in the file</returns>
        long TestLocalHeader(HfsEntry entry, HeaderTest tests)
        {
            lock (baseStream_)
            {
                bool testHeader = (tests & HeaderTest.Header) != 0;
                bool testData = (tests & HeaderTest.Extract) != 0;

                baseStream_.Seek(offsetOfFirstEntry + entry.Offset, SeekOrigin.Begin);
                if ((int)ReadLEUint() != HfsConstants.LocalHeaderSignature)
                {
                    throw new HfsException(string.Format("Wrong local header signature @{0:X}", offsetOfFirstEntry + entry.Offset));
                }

                short extractVersion = (short)ReadLEUshort();
                short localFlags = (short)ReadLEUshort();
                short compressionMethod = (short)ReadLEUshort();
                short fileTime = (short)ReadLEUshort();
                short fileDate = (short)ReadLEUshort();
                uint crcValue = ReadLEUint();
                long compressedSize = ReadLEUint();
                long size = ReadLEUint();
                int storedNameLength = ReadLEUshort();
                int extraDataLength = ReadLEUshort();

                long base_buffpos = baseStream_.Position;

                byte[] nameData = new byte[storedNameLength];
                StreamUtils.ReadFully(baseStream_, nameData);

                byte[] extraData = new byte[extraDataLength];
                StreamUtils.ReadFully(baseStream_, extraData);

                if (testData)
                {
                    if (entry.IsFile)
                    {
                        if (!entry.IsCompressionMethodSupported())
                        {
                            throw new HfsException("Compression method not supported");
                        }

                        if ((extractVersion > HfsConstants.VersionMadeBy))
                        {
                            throw new HfsException(string.Format("Version required to extract this entry not supported ({0})", extractVersion));
                        }

                    }
                }

                if (testHeader)
                {
                    if ((extractVersion <= 63) &&	// Ignore later versions as we dont know about them..
                        (extractVersion != 10) &&
                        (extractVersion != 11) &&
                        (extractVersion != 20) &&
                        (extractVersion != 21) &&
                        (extractVersion != 25) &&
                        (extractVersion != 27) &&
                        (extractVersion != 45) &&
                        (extractVersion != 46) &&
                        (extractVersion != 50) &&
                        (extractVersion != 51) &&
                        (extractVersion != 52) &&
                        (extractVersion != 61) &&
                        (extractVersion != 62) &&
                        (extractVersion != 63)
                        )
                    {
                        throw new HfsException(string.Format("Version required to extract this entry is invalid ({0})", extractVersion));
                    }


                    // Central header flags match local entry flags.
                    if (localFlags != entry.Flags)
                    {
                        throw new HfsException("Central header/local header flags mismatch");
                    }

                    // Central header compression method matches local entry
                    if (entry.CompressionMethod != (CompressionMethod)compressionMethod)
                    {
                        throw new HfsException("Central header/local header compression method mismatch");
                    }

                    if (entry.Version != extractVersion)
                    {
                        throw new HfsException("Extract version mismatch");
                    }

                    // Crc valid for empty entry.
                    // This will also apply to streamed entries where size isnt known and the header cant be patched
                    if ((size == 0) && (compressedSize == 0))
                    {
                        if (crcValue != 0)
                        {
                            throw new HfsException("Invalid CRC for empty entry");
                        }
                    }

                    // TODO: make test more correct...  can't compare lengths as was done originally as this can fail for MBCS strings
                    // Assuming a code page at this point is not valid?  Best is to store the name length in the HfsEntry probably
                    if (entry.Name.Length > storedNameLength)
                    {
                        throw new HfsException("File name length mismatch");
                    }

                    // Name data has already been read convert it and compare.
                    string localName = HfsConstants.ConvertToStringObfs(nameData, storedNameLength, (int)base_buffpos);

                    // Central directory and local entry name match
                    if (localName != entry.Name)
                    {
                        throw new HfsException("Central header and local header file name mismatch");
                    }

                    // Directories have zero actual size but can have compressed size
                    if (entry.IsDirectory)
                    {
                        if (size > 0)
                        {
                            throw new HfsException("Directory cannot have size");
                        }
                    }

                    if (!HfsNameTransform.IsValidName(localName, true))
                    {
                        throw new HfsException("Name is invalid");
                    }
                }

                // Tests that apply to both data and header.

                // Size can be verified only if it is known in the local header.
                // it will always be known in the central header.
                if (((size > 0) || (compressedSize > 0)))
                {

                    if (size != entry.Size)
                    {
                        throw new HfsException(
                            string.Format("Size mismatch between central header({0}) and local header({1})",
                                entry.Size, size));
                    }

                    if (compressedSize != entry.CompressedSize &&
                        compressedSize != 0xFFFFFFFF && compressedSize != -1)
                    {
                        throw new HfsException(
                            string.Format("Compressed size mismatch between central header({0}) and local header({1})",
                            entry.CompressedSize, compressedSize));
                    }
                }

                int extraLength = storedNameLength + extraDataLength;
                return offsetOfFirstEntry + entry.Offset + HfsConstants.LocalHeaderBaseSize + extraLength;
            }
        }

        #endregion

        #region Updating

        const int DefaultBufferSize = 4096;

        /// <summary>
        /// The kind of update to apply.
        /// </summary>
        enum UpdateCommand
        {
            Copy,       // Copy original file contents.
            Modify,     // Change encryption, compression, attributes, name, time etc, of an existing file.
            Add,        // Add a new file to the archive.
        }

        #region Properties
        /// <summary>
        /// Get / set the <see cref="INameTransform"/> to apply to names when updating.
        /// </summary>
        public INameTransform NameTransform
        {
            get
            {
                return updateEntryFactory_.NameTransform;
            }

            set
            {
                updateEntryFactory_.NameTransform = value;
            }
        }

        /// <summary>
        /// Get/set the <see cref="IEntryFactory"/> used to generate <see cref="HfsEntry"/> values
        /// during updates.
        /// </summary>
        public IEntryFactory EntryFactory
        {
            get
            {
                return updateEntryFactory_;
            }

            set
            {
                if (value == null)
                {
                    updateEntryFactory_ = new HfsEntryFactory();
                }
                else
                {
                    updateEntryFactory_ = value;
                }
            }
        }

        /// <summary>
        /// Get /set the buffer size to be used when updating this Hfs file.
        /// </summary>
        public int BufferSize
        {
            get { return bufferSize_; }
            set
            {
                if (value < 1024)
                {
#if NETCF_1_0					
					throw new ArgumentOutOfRangeException("value");
#else
                    throw new ArgumentOutOfRangeException("value", "cannot be below 1024");
#endif
                }

                if (bufferSize_ != value)
                {
                    bufferSize_ = value;
                    copyBuffer_ = null;
                }
            }
        }

        /// <summary>
        /// Get a value indicating an update has <see cref="BeginUpdate()">been started</see>.
        /// </summary>
        public bool IsUpdating
        {
            get { return updates_ != null; }
        }

        #endregion

        #region Immediate updating
        //		TBD: Direct form of updating
        // 
        //		public void Update(IEntryMatcher deleteMatcher)
        //		{
        //		}
        //
        //		public void Update(IScanner addScanner)
        //		{
        //		}
        #endregion

        #region Deferred Updating
        /// <summary>
        /// Begin updating this <see cref="HfsFile"/> archive.
        /// </summary>
        /// <param name="archiveStorage">The <see cref="IArchiveStorage">archive storage</see> for use during the update.</param>
        /// <param name="dataSource">The <see cref="IDynamicDataSource">data source</see> to utilise during updating.</param>
        /// <exception cref="ObjectDisposedException">HfsFile has been closed.</exception>
        /// <exception cref="ArgumentNullException">One of the arguments provided is null</exception>
        /// <exception cref="ObjectDisposedException">HfsFile has been closed.</exception>
        public void BeginUpdate(IArchiveStorage archiveStorage, IDynamicDataSource dataSource)
        {
            if (archiveStorage == null)
            {
                throw new ArgumentNullException("archiveStorage");
            }

            if (dataSource == null)
            {
                throw new ArgumentNullException("dataSource");
            }

            if (isDisposed_)
            {
                throw new ObjectDisposedException("HfsFile");
            }

            if (IsEmbeddedArchive)
            {
                throw new HfsException("Cannot update embedded/SFX archives");
            }

            archiveStorage_ = archiveStorage;
            updateDataSource_ = dataSource;

            // NOTE: the baseStream_ may not currently support writing or seeking.

            updateIndex_ = new Hashtable();

            updates_ = new ArrayList(entries_.Length);
            foreach (HfsEntry entry in entries_)
            {
                int index = updates_.Add(new HfsUpdate(entry));
                updateIndex_.Add(entry.Name, index);
            }

            // We must sort by offset before using offset's calculated sizes
            updates_.Sort(new UpdateComparer());

            int idx = 0;
            foreach (HfsUpdate update in updates_)
            {
                //If last entry, there is no next entry offset to use
                if (idx == updates_.Count - 1)
                    break;

                update.OffsetBasedSize = ((HfsUpdate)updates_[idx + 1]).Entry.Offset - update.Entry.Offset;
                idx++;
            }
            updateCount_ = updates_.Count;

            contentsEdited_ = false;
            commentEdited_ = false;
            newComment_ = null;
        }

        /// <summary>
        /// Begin updating to this <see cref="HfsFile"/> archive.
        /// </summary>
        /// <param name="archiveStorage">The storage to use during the update.</param>
        public void BeginUpdate(IArchiveStorage archiveStorage)
        {
            BeginUpdate(archiveStorage, new DynamicDiskDataSource());
        }

        /// <summary>
        /// Begin updating this <see cref="HfsFile"/> archive.
        /// </summary>
        /// <seealso cref="BeginUpdate(IArchiveStorage)"/>
        /// <seealso cref="CommitUpdate"></seealso>
        /// <seealso cref="AbortUpdate"></seealso>
        public void BeginUpdate()
        {
            if (Name == null)
            {
                BeginUpdate(new MemoryArchiveStorage(), new DynamicDiskDataSource());
            }
            else
            {
                BeginUpdate(new DiskArchiveStorage(this), new DynamicDiskDataSource());
            }
        }

        /// <summary>
        /// Commit current updates, updating this archive.
        /// </summary>
        /// <seealso cref="BeginUpdate()"></seealso>
        /// <seealso cref="AbortUpdate"></seealso>
        /// <exception cref="ObjectDisposedException">HfsFile has been closed.</exception>
        public void CommitUpdate()
        {
            if (isDisposed_)
            {
                throw new ObjectDisposedException("HfsFile");
            }

            CheckUpdating();

            try
            {
                updateIndex_.Clear();
                updateIndex_ = null;

                if (contentsEdited_)
                {
                    RunUpdates();
                }
                else if (commentEdited_)
                {
                    UpdateCommentOnly();
                }
                else
                {
                    // Create an empty archive if none existed originally.
                    if (entries_.Length == 0)
                    {
                        byte[] theComment = (newComment_ != null) ? newComment_.RawComment : HfsConstants.ConvertToArray(comment_);
                        using (HfsHelperStream zhs = new HfsHelperStream(baseStream_))
                        {
                            zhs.WriteEndOfCentralDirectory(0, 0, 0, theComment, 0);
                        }
                    }
                }

            }
            finally
            {
                PostUpdateCleanup();
            }
        }

        /// <summary>
        /// Abort updating leaving the archive unchanged.
        /// </summary>
        /// <seealso cref="BeginUpdate()"></seealso>
        /// <seealso cref="CommitUpdate"></seealso>
        public void AbortUpdate()
        {
            PostUpdateCleanup();
        }

        /// <summary>
        /// Set the file comment to be recorded when the current update is <see cref="CommitUpdate">commited</see>.
        /// </summary>
        /// <param name="comment">The comment to record.</param>
        /// <exception cref="ObjectDisposedException">HfsFile has been closed.</exception>
        public void SetComment(string comment)
        {
            if (isDisposed_)
            {
                throw new ObjectDisposedException("HfsFile");
            }

            CheckUpdating();

            newComment_ = new HfsString(comment);

            if (newComment_.RawLength > 0xffff)
            {
                newComment_ = null;
                throw new HfsException("Comment length exceeds maximum - 65535");
            }

            // We dont take account of the original and current comment appearing to be the same
            // as encoding may be different.
            commentEdited_ = true;
        }

        #endregion

        #region Adding Entries

        void AddUpdate(HfsUpdate update)
        {
            contentsEdited_ = true;

            int index = FindExistingUpdate(update.Entry.Name);

            if (index >= 0)
            {
                if (updates_[index] == null)
                {
                    updateCount_ += 1;
                }

                // Direct replacement is faster than delete and add.
                updates_[index] = update;
            }
            else
            {
                index = updates_.Add(update);
                updateCount_ += 1;
                updateIndex_.Add(update.Entry.Name, index);
            }
        }

        /// <summary>
        /// Add a new entry to the archive.
        /// </summary>
        /// <param name="fileName">The name of the file to add.</param>
        /// <param name="compressionMethod">The compression method to use.</param>
        /// <param name="useUnicodeText">Ensure Unicode text is used for name and comment for this entry.</param>
        /// <exception cref="ArgumentNullException">Argument supplied is null.</exception>
        /// <exception cref="ObjectDisposedException">HfsFile has been closed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Compression method is not supported.</exception>
        public void Add(string fileName, CompressionMethod compressionMethod, bool useUnicodeText)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            if (isDisposed_)
            {
                throw new ObjectDisposedException("HfsFile");
            }

            if (!HfsEntry.IsCompressionMethodSupported(compressionMethod))
            {
                throw new ArgumentOutOfRangeException("compressionMethod");
            }

            CheckUpdating();
            contentsEdited_ = true;

            HfsEntry entry = EntryFactory.MakeFileEntry(fileName);
            entry.CompressionMethod = compressionMethod;

            AddUpdate(new HfsUpdate(fileName, entry));
        }

        /// <summary>
        /// Add a new entry to the archive.
        /// </summary>
        /// <param name="fileName">The name of the file to add.</param>
        /// <param name="compressionMethod">The compression method to use.</param>
        /// <exception cref="ArgumentNullException">HfsFile has been closed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The compression method is not supported.</exception>
        public void Add(string fileName, CompressionMethod compressionMethod)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            if (!HfsEntry.IsCompressionMethodSupported(compressionMethod))
            {
                throw new ArgumentOutOfRangeException("compressionMethod");
            }

            CheckUpdating();
            contentsEdited_ = true;

            HfsEntry entry = EntryFactory.MakeFileEntry(fileName);
            entry.CompressionMethod = compressionMethod;
            AddUpdate(new HfsUpdate(fileName, entry));
        }

        /// <summary>
        /// Add a file to the archive.
        /// </summary>
        /// <param name="fileName">The name of the file to add.</param>
        /// <exception cref="ArgumentNullException">Argument supplied is null.</exception>
        public void Add(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            CheckUpdating();
            AddUpdate(new HfsUpdate(fileName, EntryFactory.MakeFileEntry(fileName)));
        }

        /// <summary>
        /// Add a file to the archive.
        /// </summary>
        /// <param name="fileName">The name of the file to add.</param>
        /// <param name="entryName">The name to use for the <see cref="HfsEntry"/> on the Hfs file created.</param>
        /// <exception cref="ArgumentNullException">Argument supplied is null.</exception>
        public void Add(string fileName, string entryName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            if (entryName == null)
            {
                throw new ArgumentNullException("entryName");
            }

            CheckUpdating();
            AddUpdate(new HfsUpdate(fileName, EntryFactory.MakeFileEntry(entryName)));
        }


        /// <summary>
        /// Add a file entry with data.
        /// </summary>
        /// <param name="dataSource">The source of the data for this entry.</param>
        /// <param name="entryName">The name to give to the entry.</param>
        public void Add(IStaticDataSource dataSource, string entryName)
        {
            if (dataSource == null)
            {
                throw new ArgumentNullException("dataSource");
            }

            if (entryName == null)
            {
                throw new ArgumentNullException("entryName");
            }

            CheckUpdating();
            AddUpdate(new HfsUpdate(dataSource, EntryFactory.MakeFileEntry(entryName, false)));
        }

        /// <summary>
        /// Add a file entry with data.
        /// </summary>
        /// <param name="dataSource">The source of the data for this entry.</param>
        /// <param name="entryName">The name to give to the entry.</param>
        /// <param name="compressionMethod">The compression method to use.</param>
        public void Add(IStaticDataSource dataSource, string entryName, CompressionMethod compressionMethod)
        {
            if (dataSource == null)
            {
                throw new ArgumentNullException("dataSource");
            }

            if (entryName == null)
            {
                throw new ArgumentNullException("entryName");
            }

            CheckUpdating();

            HfsEntry entry = EntryFactory.MakeFileEntry(entryName, false);
            entry.CompressionMethod = compressionMethod;

            AddUpdate(new HfsUpdate(dataSource, entry));
        }

        /// <summary>
        /// Add a file entry with data.
        /// </summary>
        /// <param name="dataSource">The source of the data for this entry.</param>
        /// <param name="entryName">The name to give to the entry.</param>
        /// <param name="compressionMethod">The compression method to use.</param>
        /// <param name="useUnicodeText">Ensure Unicode text is used for name and comments for this entry.</param>
        public void Add(IStaticDataSource dataSource, string entryName, CompressionMethod compressionMethod, bool useUnicodeText)
        {
            if (dataSource == null)
            {
                throw new ArgumentNullException("dataSource");
            }

            if (entryName == null)
            {
                throw new ArgumentNullException("entryName");
            }

            CheckUpdating();

            HfsEntry entry = EntryFactory.MakeFileEntry(entryName, false);
            entry.CompressionMethod = compressionMethod;

            AddUpdate(new HfsUpdate(dataSource, entry));
        }

        /// <summary>
        /// Add a <see cref="HfsEntry"/> that contains no data.
        /// </summary>
        /// <param name="entry">The entry to add.</param>
        /// <remarks>This can be used to add directories, volume labels, or empty file entries.</remarks>
        public void Add(HfsEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }

            CheckUpdating();

            if ((entry.Size != 0) || (entry.CompressedSize != 0))
            {
                throw new HfsException("Entry cannot have any data");
            }

            AddUpdate(new HfsUpdate(UpdateCommand.Add, entry));
        }

        /// <summary>
        /// Add a directory entry to the archive.
        /// </summary>
        /// <param name="directoryName">The directory to add.</param>
        public void AddDirectory(string directoryName)
        {
            if (directoryName == null)
            {
                throw new ArgumentNullException("directoryName");
            }

            CheckUpdating();

            HfsEntry dirEntry = EntryFactory.MakeDirectoryEntry(directoryName);
            AddUpdate(new HfsUpdate(UpdateCommand.Add, dirEntry));
        }

        #endregion

        #region Modifying Entries
        /* Modify not yet ready for public consumption.
   Direct modification of an entry should not overwrite original data before its read.
   Safe mode is trivial in this sense.
		public void Modify(HfsEntry original, HfsEntry updated)
		{
			if ( original == null ) {
				throw new ArgumentNullException("original");
			}

			if ( updated == null ) {
				throw new ArgumentNullException("updated");
			}

			CheckUpdating();
			contentsEdited_ = true;
			updates_.Add(new HfsUpdate(original, updated));
		}
*/
        #endregion

        #region Deleting Entries
        /// <summary>
        /// Delete an entry by name
        /// </summary>
        /// <param name="fileName">The filename to delete</param>
        /// <returns>True if the entry was found and deleted; false otherwise.</returns>
        public bool Delete(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            CheckUpdating();

            bool result = false;
            int index = FindExistingUpdate(fileName);
            if ((index >= 0) && (updates_[index] != null))
            {
                result = true;
                contentsEdited_ = true;
                updates_[index] = null;
                updateCount_ -= 1;
            }
            else
            {
                throw new HfsException("Cannot find entry to delete");
            }
            return result;
        }

        /// <summary>
        /// Delete a <see cref="HfsEntry"/> from the archive.
        /// </summary>
        /// <param name="entry">The entry to delete.</param>
        public void Delete(HfsEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }

            CheckUpdating();

            int index = FindExistingUpdate(entry);
            if (index >= 0)
            {
                contentsEdited_ = true;
                updates_[index] = null;
                updateCount_ -= 1;
            }
            else
            {
                throw new HfsException("Cannot find entry to delete");
            }
        }

        #endregion

        #region Update Support

        #region Writing Values/Headers
        void WriteLEShort(int value)
        {
            baseStream_.WriteByte((byte)(value & 0xff));
            baseStream_.WriteByte((byte)((value >> 8) & 0xff));
        }

        /// <summary>
        /// Write an unsigned short in little endian byte order.
        /// </summary>
        void WriteLEUshort(ushort value)
        {
            baseStream_.WriteByte((byte)(value & 0xff));
            baseStream_.WriteByte((byte)(value >> 8));
        }

        /// <summary>
        /// Write an int in little endian byte order.
        /// </summary>
        void WriteLEInt(int value)
        {
            WriteLEShort(value & 0xffff);
            WriteLEShort(value >> 16);
        }

        /// <summary>
        /// Write an unsigned int in little endian byte order.
        /// </summary>
        void WriteLEUint(uint value)
        {
            WriteLEUshort((ushort)(value & 0xffff));
            WriteLEUshort((ushort)(value >> 16));
        }

        /// <summary>
        /// Write a long in little endian byte order.
        /// </summary>
        void WriteLeLong(long value)
        {
            WriteLEInt((int)(value & 0xffffffff));
            WriteLEInt((int)(value >> 32));
        }

        void WriteLEUlong(ulong value)
        {
            WriteLEUint((uint)(value & 0xffffffff));
            WriteLEUint((uint)(value >> 32));
        }

        void WriteLocalEntryHeader(HfsUpdate update)
        {
            HfsEntry entry = update.OutEntry;

            // TODO: Local offset will require adjusting for multi-disk Hfs files.
            entry.Offset = baseStream_.Position;

            // TODO: Need to clear any entry flags that dont make sense or throw an exception here.
            if (update.Command != UpdateCommand.Copy)
            {

            }

            // Write the local file header
            WriteLEInt(HfsConstants.LocalHeaderSignature);

            WriteLEShort(entry.Version);
            WriteLEShort(entry.Flags);

            WriteLEShort((byte)entry.CompressionMethod);
            WriteLEInt((int)entry.DosTime);

            if (!entry.HasCrc)
            {
                // Note patch address for updating CRC later.
                update.CrcPatchOffset = baseStream_.Position;
                WriteLEInt((int)0);
            }
            else
            {
                WriteLEInt(unchecked((int)entry.Crc));
            }


            if ((entry.CompressedSize < 0) || (entry.Size < 0))
            {
                update.SizePatchOffset = baseStream_.Position;
            }

            WriteLEInt((int)entry.CompressedSize);
            WriteLEInt((int)entry.Size);

            byte[] name = HfsConstants.ConvertToArray(entry.Flags, entry.Name);

            if (name.Length > 0xFFFF)
            {
                throw new HfsException("Entry name too long.");
            }

            entry.ExtraData = new byte[0];

            WriteLEShort(name.Length);
            WriteLEShort(entry.ExtraData.Length);

            HfsXorCipher.XorBlockWithKey(name, HfsXorCipher.XorTruths, (int)baseStream_.Position);

            if (name.Length > 0)
            {
                baseStream_.Write(name, 0, name.Length);
            }

            if (entry.ExtraData.Length > 0)
            {
                baseStream_.Write(entry.ExtraData, 0, entry.ExtraData.Length);
            }
        }

        int WriteCentralDirectoryHeader(HfsEntry entry)
        {
            if (entry.CompressedSize < 0)
            {
                throw new HfsException("Attempt to write central directory entry with unknown csize");
            }

            if (entry.Size < 0)
            {
                throw new HfsException("Attempt to write central directory entry with unknown size");
            }

            if (entry.Crc < 0)
            {
                throw new HfsException("Attempt to write central directory entry with unknown crc");
            }

            // Write the central file header
            WriteLEInt(HfsConstants.CentralHeaderSignature);

            // Version made by
            WriteLEShort(HfsConstants.VersionMadeBy);

            // Version required to extract
            WriteLEShort(entry.Version);

            WriteLEShort(entry.Flags);

            unchecked
            {
                WriteLEShort((byte)entry.CompressionMethod);
                WriteLEInt((int)entry.DosTime);
                WriteLEInt((int)entry.Crc);
            }


            WriteLEInt((int)(entry.CompressedSize & 0xffffffff));
            WriteLEInt((int)entry.Size);


            byte[] name = HfsConstants.ConvertToArray(entry.Flags, entry.Name);

            if (name.Length > 0xFFFF)
            {
                throw new HfsException("Entry name is too long.");
            }

            WriteLEShort(name.Length);

            byte[] centralExtraData = new byte[0];

            WriteLEShort(centralExtraData.Length);
            WriteLEShort(entry.Comment != null ? entry.Comment.Length : 0);

            WriteLEShort(0);	// disk number
            WriteLEShort(0);	// internal file attributes

            // External file attributes...
            if (entry.ExternalFileAttributes != -1)
            {
                WriteLEInt(entry.ExternalFileAttributes);
            }
            else
            {
                if (entry.IsDirectory)
                {
                    WriteLEUint(16);
                }
                else
                {
                    WriteLEUint(0);
                }
            }

            if (entry.Offset >= 0xffffffff)
            {
                WriteLEUint(0xffffffff);
            }
            else
            {
                WriteLEUint((uint)(int)entry.Offset);
            }


            HfsXorCipher.XorBlockWithKey(name, HfsXorCipher.XorTruths, (int)baseStream_.Position);

            if (name.Length > 0)
            {
                baseStream_.Write(name, 0, name.Length);
            }

            if (centralExtraData.Length > 0)
            {
                baseStream_.Write(centralExtraData, 0, centralExtraData.Length);
            }

            byte[] rawComment = (entry.Comment != null) ? Encoding.ASCII.GetBytes(entry.Comment) : new byte[0];

            if (rawComment.Length > 0)
            {
                baseStream_.Write(rawComment, 0, rawComment.Length);
            }

            return HfsConstants.CentralHeaderBaseSize + name.Length + centralExtraData.Length + rawComment.Length;
        }
        #endregion

        void PostUpdateCleanup()
        {
            updateDataSource_ = null;
            updates_ = null;
            updateIndex_ = null;

            if (archiveStorage_ != null)
            {
                archiveStorage_.Dispose();
                archiveStorage_ = null;
            }
        }

        string GetTransformedFileName(string name)
        {
            INameTransform transform = NameTransform;
            return (transform != null) ?
                transform.TransformFile(name) :
                name;
        }

        string GetTransformedDirectoryName(string name)
        {
            INameTransform transform = NameTransform;
            return (transform != null) ?
                transform.TransformDirectory(name) :
                name;
        }

        /// <summary>
        /// Get a raw memory buffer.
        /// </summary>
        /// <returns>Returns a raw memory buffer.</returns>
        byte[] GetBuffer()
        {
            if (copyBuffer_ == null)
            {
                copyBuffer_ = new byte[bufferSize_];
            }
            return copyBuffer_;
        }

        void CopyDescriptorBytes(HfsUpdate update, Stream dest, Stream source)
        {
            int bytesToCopy = GetDescriptorSize(update);

            if (bytesToCopy > 0)
            {
                byte[] buffer = GetBuffer();

                while (bytesToCopy > 0)
                {
                    int readSize = Math.Min(buffer.Length, bytesToCopy);

                    int bytesRead = source.Read(buffer, 0, readSize);
                    if (bytesRead > 0)
                    {
                        dest.Write(buffer, 0, bytesRead);
                        bytesToCopy -= bytesRead;
                    }
                    else
                    {
                        throw new HfsException("Unxpected end of stream");
                    }
                }
            }
        }

        void CopyBytes(HfsUpdate update, Stream destination, Stream source,
            long bytesToCopy, bool updateCrc)
        {
            if (destination == source)
            {
                throw new InvalidOperationException("Destination and source are the same");
            }

            // NOTE: Compressed size is updated elsewhere.
            Crc32 crc = new Crc32();
            byte[] buffer = GetBuffer();

            long targetBytes = bytesToCopy;
            long totalBytesRead = 0;

            int bytesRead;
            do
            {
                int readSize = buffer.Length;

                if (bytesToCopy < readSize)
                {
                    readSize = (int)bytesToCopy;
                }

                bytesRead = source.Read(buffer, 0, readSize);
                if (bytesRead > 0)
                {
                    if (updateCrc)
                    {
                        crc.Update(buffer, 0, bytesRead);
                    }
                    destination.Write(buffer, 0, bytesRead);
                    bytesToCopy -= bytesRead;
                    totalBytesRead += bytesRead;
                }
            }
            while ((bytesRead > 0) && (bytesToCopy > 0));

            if (totalBytesRead != targetBytes)
            {
                throw new HfsException(string.Format("Failed to copy bytes expected {0} read {1}", targetBytes, totalBytesRead));
            }

            if (updateCrc)
            {
                update.OutEntry.Crc = crc.Value;
            }
        }

        /// <summary>
        /// Get the size of the source descriptor for a <see cref="HfsUpdate"/>.
        /// </summary>
        /// <param name="update">The update to get the size for.</param>
        /// <returns>The descriptor size, zero if there isnt one.</returns>
        int GetDescriptorSize(HfsUpdate update)
        {
            int result = 0;
            return result;
        }

        void CopyDescriptorBytesDirect(HfsUpdate update, Stream stream, ref long destinationPosition, long sourcePosition)
        {
            int bytesToCopy = GetDescriptorSize(update);

            while (bytesToCopy > 0)
            {
                int readSize = (int)bytesToCopy;
                byte[] buffer = GetBuffer();

                stream.Position = sourcePosition;
                int bytesRead = stream.Read(buffer, 0, readSize);
                if (bytesRead > 0)
                {
                    stream.Position = destinationPosition;
                    stream.Write(buffer, 0, bytesRead);
                    bytesToCopy -= bytesRead;
                    destinationPosition += bytesRead;
                    sourcePosition += bytesRead;
                }
                else
                {
                    throw new HfsException("Unxpected end of stream");
                }
            }
        }

        void CopyEntryDataDirect(HfsUpdate update, Stream stream, bool updateCrc, ref long destinationPosition, ref long sourcePosition)
        {
            long bytesToCopy = update.Entry.CompressedSize;

            // NOTE: Compressed size is updated elsewhere.
            Crc32 crc = new Crc32();
            byte[] buffer = GetBuffer();

            long targetBytes = bytesToCopy;
            long totalBytesRead = 0;

            int bytesRead;
            do
            {
                int readSize = buffer.Length;

                if (bytesToCopy < readSize)
                {
                    readSize = (int)bytesToCopy;
                }

                stream.Position = sourcePosition;
                bytesRead = stream.Read(buffer, 0, readSize);
                if (bytesRead > 0)
                {
                    if (updateCrc)
                    {
                        crc.Update(buffer, 0, bytesRead);
                    }
                    stream.Position = destinationPosition;
                    stream.Write(buffer, 0, bytesRead);

                    destinationPosition += bytesRead;
                    sourcePosition += bytesRead;
                    bytesToCopy -= bytesRead;
                    totalBytesRead += bytesRead;
                }
            }
            while ((bytesRead > 0) && (bytesToCopy > 0));

            if (totalBytesRead != targetBytes)
            {
                throw new HfsException(string.Format("Failed to copy bytes expected {0} read {1}", targetBytes, totalBytesRead));
            }

            if (updateCrc)
            {
                update.OutEntry.Crc = crc.Value;
            }
        }

        int FindExistingUpdate(HfsEntry entry)
        {
            int result = -1;
            string convertedName = GetTransformedFileName(entry.Name);

            if (updateIndex_.ContainsKey(convertedName))
            {
                result = (int)updateIndex_[convertedName];
            }
            /*
                        // This is slow like the coming of the next ice age but takes less storage and may be useful
                        // for CF?
                        for (int index = 0; index < updates_.Count; ++index)
                        {
                            HfsUpdate zu = ( HfsUpdate )updates_[index];
                            if ( (zu.Entry.HfsFileIndex == entry.HfsFileIndex) &&
                                (string.Compare(convertedName, zu.Entry.Name, true, CultureInfo.InvariantCulture) == 0) ) {
                                result = index;
                                break;
                            }
                        }
             */
            return result;
        }

        int FindExistingUpdate(string fileName)
        {
            int result = -1;

            string convertedName = GetTransformedFileName(fileName);

            if (updateIndex_.ContainsKey(convertedName))
            {
                result = (int)updateIndex_[convertedName];
            }

            /*
                        // This is slow like the coming of the next ice age but takes less storage and may be useful
                        // for CF?
                        for ( int index = 0; index < updates_.Count; ++index ) {
                            if ( string.Compare(convertedName, (( HfsUpdate )updates_[index]).Entry.Name,
                                true, CultureInfo.InvariantCulture) == 0 ) {
                                result = index;
                                break;
                            }
                        }
             */

            return result;
        }

        /// <summary>
        /// Get an output stream for the specified <see cref="HfsEntry"/>
        /// </summary>
        /// <param name="entry">The entry to get an output stream for.</param>
        /// <returns>The output stream obtained for the entry.</returns>
        Stream GetOutputStream(HfsEntry entry)
        {
            Stream result = baseStream_;
            long start = result.Position;

            switch (entry.CompressionMethod)
            {
                case CompressionMethod.Stored:
                    {
                        Stream base_result = result;

                        if (entry.Name.Substring(entry.Name.Length - 5) == ".comp")
                        {
                            start += 8;
                        }

                        result = new HFSXorStream(result, start, HfsXorCipher.XorTruths, false);

                        // post process instead
                        //if (obfuscationkey_ > 0)
                        //{
                        //   result = new HFSXorStream(result, start, bytekey_, true);
                        //}

                        if (entry.Name.Substring(entry.Name.Length - 5) == ".comp")
                        {
                            using (MemoryStream ms = new MemoryStream())
                            using (BinaryWriter writer = new BinaryWriter(ms))
                            {
                                writer.Write((UInt32)HfsConstants.CompSignature);
                                writer.Write((UInt32)entry.Size);

                                ms.Seek(0, SeekOrigin.Begin);

                                byte[] buff = new byte[ms.Length];
                                ms.Read(buff, 0, buff.Length);

                                HfsXorCipher.XorBlockWithKey(buff, HfsXorCipher.XorTruths, (int)start - 8);

                                //if (obfuscationkey_ > 0)
                                //{
                                //    HfsXorCipher.XorBlockWithKey(buff, bytekey_, (int)start - 8);
                                //}

                                start += buff.Length;

                                base_result.Write(buff, 0, buff.Length);
                            }

                            DeflaterOutputStream dos = new DeflaterOutputStream(result, new Deflater(9, false));
                            result = dos;
                            dos.IsStreamOwner = false;
                        }


                    }
                    break;

                default:
                    throw new HfsException("Unknown compression method " + entry.CompressionMethod);
            }
            return result;
        }

        void AddEntry(HfsFile workFile, HfsUpdate update)
        {
            Stream source = null;

            if (update.Entry.IsFile)
            {
                source = update.GetSource();

                if (source == null)
                {
                    source = updateDataSource_.GetSource(update.Entry, update.Filename);
                }
            }

            if (source != null)
            {
                using (source)
                {
                    long sourceStreamLength = source.Length;
                    if (update.OutEntry.Size < 0)
                    {
                        update.OutEntry.Size = sourceStreamLength;
                    }
                    else
                    {
                        // Check for errant entries.
                        if (update.OutEntry.Size != sourceStreamLength)
                        {
                            throw new HfsException("Entry size/stream size mismatch");
                        }
                    }

                    workFile.WriteLocalEntryHeader(update);

                    long dataStart = workFile.baseStream_.Position;
                    update.OutEntry.DataOffset = dataStart;

                    using (Stream output = workFile.GetOutputStream(update.OutEntry))
                    {
                        CopyBytes(update, output, source, sourceStreamLength, true);
                    }

                    long dataEnd = workFile.baseStream_.Position;

                    update.OutEntry.CompressedSize = update.OutEntry.Size = dataEnd - dataStart;
                    
                }
            }
            else
            {
                workFile.WriteLocalEntryHeader(update);
                update.OutEntry.CompressedSize = 0;
            }

        }

        void ModifyEntry(HfsFile workFile, HfsUpdate update)
        {
            workFile.WriteLocalEntryHeader(update);
            long dataStart = workFile.baseStream_.Position;

            // TODO: This is slow if the changes don't effect the data!!
            if (update.Entry.IsFile && (update.Filename != null))
            {
                using (Stream output = workFile.GetOutputStream(update.OutEntry))
                {
                    using (Stream source = this.GetInputStream(update.Entry))
                    {
                        CopyBytes(update, output, source, source.Length, true);
                    }
                }
            }

            long dataEnd = workFile.baseStream_.Position;
            update.Entry.CompressedSize = dataEnd - dataStart;
        }

        void CopyEntryDirect(HfsFile workFile, HfsUpdate update, ref long destinationPosition)
        {
            bool skipOver = false;
            if (update.Entry.Offset == destinationPosition)
            {
                skipOver = true;
            }

            if (!skipOver)
            {
                baseStream_.Position = destinationPosition;
                workFile.WriteLocalEntryHeader(update);
                destinationPosition = baseStream_.Position;
            }

            long sourcePosition = 0;

            const int NameLengthOffset = 26;

            // TODO: Add base for SFX friendly handling
            long entryDataOffset = update.Entry.Offset + NameLengthOffset;

            baseStream_.Seek(entryDataOffset, SeekOrigin.Begin);

            // Clumsy way of handling retrieving the original name and extra data length for now.
            // TODO: Stop re-reading name and data length in CopyEntryDirect.
            uint nameLength = ReadLEUshort();
            uint extraLength = ReadLEUshort();

            sourcePosition = baseStream_.Position + nameLength + extraLength;

            if (skipOver)
            {
                if (update.OffsetBasedSize != -1)
                    destinationPosition += update.OffsetBasedSize;
                else
                    // TODO: Find out why this calculation comes up 4 bytes short on some entries in ODT (Office Document Text) archives.
                    // WinHfs produces a warning on these entries:
                    // "caution: value of lrec.csize (compressed size) changed from ..."
                    destinationPosition +=
                        (sourcePosition - entryDataOffset) + NameLengthOffset +	// Header size
                        update.Entry.CompressedSize + GetDescriptorSize(update);
            }
            else
            {
                if (update.Entry.CompressedSize > 0)
                {
                    CopyEntryDataDirect(update, baseStream_, false, ref destinationPosition, ref sourcePosition);
                }
                CopyDescriptorBytesDirect(update, baseStream_, ref destinationPosition, sourcePosition);
            }
        }

        void CopyEntry(HfsFile workFile, HfsUpdate update)
        {
            workFile.WriteLocalEntryHeader(update);

            if (update.Entry.CompressedSize > 0)
            {
                const int NameLengthOffset = 26;

                long entryDataOffset = update.Entry.Offset + NameLengthOffset;

                // TODO: This wont work for SFX files!
                baseStream_.Seek(entryDataOffset, SeekOrigin.Begin);

                uint nameLength = ReadLEUshort();
                uint extraLength = ReadLEUshort();

                baseStream_.Seek(nameLength + extraLength, SeekOrigin.Current);

                CopyBytes(update, workFile.baseStream_, baseStream_, update.Entry.CompressedSize, false);
            }
            CopyDescriptorBytes(update, workFile.baseStream_, baseStream_);
        }

        void Reopen(Stream source)
        {
            if (source == null)
            {
                throw new HfsException("Failed to reopen archive - no source");
            }

            isNewArchive_ = false;
            baseStream_ = source;
            ReadEntries();
        }

        void Reopen()
        {
            if (Name == null)
            {
                throw new InvalidOperationException("Name is not known cannot Reopen");
            }

            Reopen(File.Open(Name, FileMode.Open, FileAccess.Read, FileShare.Read));
        }

        void UpdateCommentOnly()
        {
            long baseLength = baseStream_.Length;

            HfsHelperStream updateFile = null;

            if (archiveStorage_.UpdateMode == FileUpdateMode.Safe)
            {
                Stream copyStream = archiveStorage_.MakeTemporaryCopy(baseStream_);
                updateFile = new HfsHelperStream(copyStream);
                updateFile.IsStreamOwner = true;

                baseStream_.Close();
                baseStream_ = null;
            }
            else
            {
                if (archiveStorage_.UpdateMode == FileUpdateMode.Direct)
                {
                    // TODO: archiveStorage wasnt originally intended for this use.
                    // Need to revisit this to tidy up handling as archive storage currently doesnt 
                    // handle the original stream well.
                    // The problem is when using an existing Hfs archive with an in memory archive storage.
                    // The open stream wont support writing but the memory storage should open the same file not an in memory one.

                    // Need to tidy up the archive storage interface and contract basically.
                    baseStream_ = archiveStorage_.OpenForDirectUpdate(baseStream_);
                    updateFile = new HfsHelperStream(baseStream_);
                }
                else
                {
                    baseStream_.Close();
                    baseStream_ = null;
                    updateFile = new HfsHelperStream(Name);
                }
            }

            using (updateFile)
            {
                long locatedCentralDirOffset =
                    updateFile.LocateBlockWithSignature(HfsConstants.EndOfCentralDirectorySignature,
                                                        baseLength, HfsConstants.EndOfCentralRecordBaseSize, 0xffff);
                if (locatedCentralDirOffset < 0)
                {
                    throw new HfsException("Cannot find central directory");
                }

                const int CentralHeaderCommentSizeOffset = 16;
                updateFile.Position += CentralHeaderCommentSizeOffset;

                byte[] rawComment = newComment_.RawComment;

                updateFile.WriteLEShort(rawComment.Length);
                updateFile.Write(rawComment, 0, rawComment.Length);
                updateFile.SetLength(updateFile.Position);
            }

            if (archiveStorage_.UpdateMode == FileUpdateMode.Safe)
            {
                Reopen(archiveStorage_.ConvertTemporaryToFinal());
            }
            else
            {
                ReadEntries();
            }
        }

        /// <summary>
        /// Class used to sort updates.
        /// </summary>
        class UpdateComparer : IComparer
        {
            /// <summary>
            /// Compares two objects and returns a value indicating whether one is 
            /// less than, equal to or greater than the other.
            /// </summary>
            /// <param name="x">First object to compare</param>
            /// <param name="y">Second object to compare.</param>
            /// <returns>Compare result.</returns>
            public int Compare(
                object x,
                object y)
            {
                HfsUpdate zx = x as HfsUpdate;
                HfsUpdate zy = y as HfsUpdate;

                int result;

                if (zx == null)
                {
                    if (zy == null)
                    {
                        result = 0;
                    }
                    else
                    {
                        result = -1;
                    }
                }
                else if (zy == null)
                {
                    result = 1;
                }
                else
                {
                    int xCmdValue = ((zx.Command == UpdateCommand.Copy) || (zx.Command == UpdateCommand.Modify)) ? 0 : 1;
                    int yCmdValue = ((zy.Command == UpdateCommand.Copy) || (zy.Command == UpdateCommand.Modify)) ? 0 : 1;

                    result = xCmdValue - yCmdValue;
                    if (result == 0)
                    {
                        long offsetDiff = zx.Entry.Offset - zy.Entry.Offset;
                        if (offsetDiff < 0)
                        {
                            result = -1;
                        }
                        else if (offsetDiff == 0)
                        {
                            result = 0;
                        }
                        else
                        {
                            result = 1;
                        }
                    }
                }
                return result;
            }
        }

        void RunUpdates()
        {
            long sizeEntries = 0;
            long endOfStream = 0;
            bool directUpdate = false;
            long destinationPosition = 0; // NOT SFX friendly

            HfsFile workFile;

            if (IsNewArchive)
            {
                workFile = this;
                workFile.baseStream_.Position = 0;
                directUpdate = true;
            }
            else if (archiveStorage_.UpdateMode == FileUpdateMode.Direct)
            {
                workFile = this;
                workFile.baseStream_.Position = 0;
                directUpdate = true;

                // Sort the updates by offset within copies/modifies, then adds.
                // This ensures that data required by copies will not be overwritten.
                updates_.Sort(new UpdateComparer());
            }
            else
            {
                workFile = HfsFile.Create(archiveStorage_.GetTemporaryOutput());

                if (key != null)
                {
                    workFile.key = (byte[])key.Clone();
                }

                if (obfuscationkey_ != 0)
                {
                    workFile.obfuscationkey_ = obfuscationkey_;
                    workFile.bytekey_ = bytekey_;
                }
            }

            try
            {
                foreach (HfsUpdate update in updates_)
                {
                    if (update != null)
                    {
                        switch (update.Command)
                        {
                            case UpdateCommand.Copy:
                                if (directUpdate)
                                {
                                    CopyEntryDirect(workFile, update, ref destinationPosition);
                                }
                                else
                                {
                                    CopyEntry(workFile, update);
                                }
                                break;

                            case UpdateCommand.Modify:
                                // TODO: Direct modifying of an entry will take some legwork.
                                ModifyEntry(workFile, update);
                                break;

                            case UpdateCommand.Add:
                                if (!IsNewArchive && directUpdate)
                                {
                                    workFile.baseStream_.Position = destinationPosition;
                                }

                                AddEntry(workFile, update);

                                if (directUpdate)
                                {
                                    destinationPosition = workFile.baseStream_.Position;
                                }
                                break;
                        }
                    }
                }

                if (!IsNewArchive && directUpdate)
                {
                    workFile.baseStream_.Position = destinationPosition;
                }

                long centralDirOffset = workFile.baseStream_.Position;

                foreach (HfsUpdate update in updates_)
                {
                    if (update != null)
                    {
                        sizeEntries += workFile.WriteCentralDirectoryHeader(update.OutEntry);
                    }
                }

                byte[] theComment = (newComment_ != null) ? newComment_.RawComment : HfsConstants.ConvertToArray(comment_);
                uint obfuscationKey = workFile.obfuscationkey_;

                if (obfuscationKey > 0)
                {
                    obfuscationKey = workFile.obfuscationkey_ = (UInt32)(centralDirOffset * sizeEntries);
                    Array.Copy(BitConverter.GetBytes(obfuscationKey), workFile.bytekey_, 4);
                }

                using (HfsHelperStream zhs = new HfsHelperStream(workFile.baseStream_))
                {
                    zhs.WriteEndOfCentralDirectory(updateCount_, sizeEntries, centralDirOffset, theComment, obfuscationKey);
                }

                endOfStream = workFile.baseStream_.Position;

                // And now patch entries...
                foreach (HfsUpdate update in updates_)
                {
                    if (update != null)
                    {
                        // If the size of the entry is zero leave the crc as 0 as well.
                        // The calculated crc will be all bits on...
                        if ((update.CrcPatchOffset > 0) && (update.OutEntry.CompressedSize > 0))
                        {
                            workFile.baseStream_.Position = update.CrcPatchOffset;
                            workFile.WriteLEInt((int)update.OutEntry.Crc);
                        }

                        if (update.SizePatchOffset > 0)
                        {
                            workFile.baseStream_.Position = update.SizePatchOffset;

                            workFile.WriteLEInt((int)update.OutEntry.CompressedSize);
                            workFile.WriteLEInt((int)update.OutEntry.Size);
                        }

                        // HFS xor post-process
                        if (obfuscationKey > 0 && update.Command == UpdateCommand.Add)
                        {
                            long dataStart = update.OutEntry.DataOffset;

                            byte[] buffer = new byte[4096];
                            long bytesToCopy = update.OutEntry.CompressedSize;
                            int bytesRead;

                            do
                            {
                                workFile.baseStream_.Position = dataStart;

                                int readSize = buffer.Length;

                                if (bytesToCopy < readSize)
                                {
                                    readSize = (int)bytesToCopy;
                                }

                                bytesRead = workFile.baseStream_.Read(buffer, 0, readSize);
                                if (bytesRead > 0)
                                {
                                    workFile.baseStream_.Position = dataStart;

                                    HfsXorCipher.XorBlockWithKey(buffer, bytekey_, (int)dataStart);

                                    workFile.baseStream_.Write(buffer, 0, bytesRead);
                                }

                                dataStart += bytesRead;
                                bytesToCopy -= bytesRead;
                            }
                            while ((bytesRead > 0) && (bytesToCopy > 0));
                        }

                    }
                }
            }
            catch
            {
                workFile.Close();
                if (!directUpdate && (workFile.Name != null))
                {
                    File.Delete(workFile.Name);
                }
                throw;
            }

            if (directUpdate)
            {
                workFile.baseStream_.SetLength(endOfStream);
                workFile.baseStream_.Flush();
                isNewArchive_ = false;
                ReadEntries();
            }
            else
            {
                baseStream_.Close();
                Reopen(archiveStorage_.ConvertTemporaryToFinal());
            }
        }

        void CheckUpdating()
        {
            if (updates_ == null)
            {
                throw new InvalidOperationException("BeginUpdate has not been called");
            }
        }

        #endregion

        #region HfsUpdate class
        /// <summary>
        /// Represents a pending update to a Hfs file.
        /// </summary>
        class HfsUpdate
        {
            #region Constructors
            public HfsUpdate(string fileName, HfsEntry entry)
            {
                command_ = UpdateCommand.Add;
                entry_ = entry;
                filename_ = fileName;
            }

            [Obsolete]
            public HfsUpdate(string fileName, string entryName, CompressionMethod compressionMethod)
            {
                command_ = UpdateCommand.Add;
                entry_ = new HfsEntry(entryName);
                entry_.CompressionMethod = compressionMethod;
                filename_ = fileName;
            }

            [Obsolete]
            public HfsUpdate(string fileName, string entryName)
                : this(fileName, entryName, CompressionMethod.Stored)
            {
                // Do nothing.
            }

            [Obsolete]
            public HfsUpdate(IStaticDataSource dataSource, string entryName, CompressionMethod compressionMethod)
            {
                command_ = UpdateCommand.Add;
                entry_ = new HfsEntry(entryName);
                entry_.CompressionMethod = compressionMethod;
                dataSource_ = dataSource;
            }

            public HfsUpdate(IStaticDataSource dataSource, HfsEntry entry)
            {
                command_ = UpdateCommand.Add;
                entry_ = entry;
                dataSource_ = dataSource;
            }

            public HfsUpdate(HfsEntry original, HfsEntry updated)
            {
                throw new HfsException("Modify not currently supported");
                /*
                    command_ = UpdateCommand.Modify;
                    entry_ = ( HfsEntry )original.Clone();
                    outEntry_ = ( HfsEntry )updated.Clone();
                */
            }

            public HfsUpdate(UpdateCommand command, HfsEntry entry)
            {
                command_ = command;
                entry_ = (HfsEntry)entry.Clone();
            }


            /// <summary>
            /// Copy an existing entry.
            /// </summary>
            /// <param name="entry">The existing entry to copy.</param>
            public HfsUpdate(HfsEntry entry)
                : this(UpdateCommand.Copy, entry)
            {
                // Do nothing.
            }
            #endregion

            /// <summary>
            /// Get the <see cref="HfsEntry"/> for this update.
            /// </summary>
            /// <remarks>This is the source or original entry.</remarks>
            public HfsEntry Entry
            {
                get { return entry_; }
            }

            /// <summary>
            /// Get the <see cref="HfsEntry"/> that will be written to the updated/new file.
            /// </summary>
            public HfsEntry OutEntry
            {
                get
                {
                    if (outEntry_ == null)
                    {
                        outEntry_ = (HfsEntry)entry_.Clone();
                    }

                    return outEntry_;
                }
            }

            /// <summary>
            /// Get the command for this update.
            /// </summary>
            public UpdateCommand Command
            {
                get { return command_; }
            }

            /// <summary>
            /// Get the filename if any for this update.  Null if none exists.
            /// </summary>
            public string Filename
            {
                get { return filename_; }
            }

            /// <summary>
            /// Get/set the location of the size patch for this update.
            /// </summary>
            public long SizePatchOffset
            {
                get { return sizePatchOffset_; }
                set { sizePatchOffset_ = value; }
            }

            /// <summary>
            /// Get /set the location of the crc patch for this update.
            /// </summary>
            public long CrcPatchOffset
            {
                get { return crcPatchOffset_; }
                set { crcPatchOffset_ = value; }
            }

            /// <summary>
            /// Get/set the size calculated by offset.
            /// Specifically, the difference between this and next entry's starting offset.
            /// </summary>
            public long OffsetBasedSize
            {
                get { return _offsetBasedSize; }
                set { _offsetBasedSize = value; }
            }

            public Stream GetSource()
            {
                Stream result = null;
                if (dataSource_ != null)
                {
                    result = dataSource_.GetSource();
                }

                return result;
            }

            #region Instance Fields
            HfsEntry entry_;
            HfsEntry outEntry_;
            UpdateCommand command_;
            IStaticDataSource dataSource_;
            string filename_;
            long sizePatchOffset_ = -1;
            long crcPatchOffset_ = -1;
            long _offsetBasedSize = -1;
            #endregion
        }

        #endregion
        #endregion

        #region Disposing

        #region IDisposable Members
        void IDisposable.Dispose()
        {
            Close();
        }
        #endregion

        void DisposeInternal(bool disposing)
        {
            if (!isDisposed_)
            {
                isDisposed_ = true;
                entries_ = new HfsEntry[0];

                if (IsStreamOwner && (baseStream_ != null))
                {
                    lock (baseStream_)
                    {
                        baseStream_.Close();
                    }
                }

                PostUpdateCleanup();
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the this instance and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            DisposeInternal(disposing);
        }

        #endregion

        #region Internal routines
        #region Reading
        /// <summary>
        /// Read an unsigned short in little endian byte order.
        /// </summary>
        /// <returns>Returns the value read.</returns>
        /// <exception cref="EndOfStreamException">
        /// The stream ends prematurely
        /// </exception>
        ushort ReadLEUshort()
        {
            int data1 = baseStream_.ReadByte();

            if (data1 < 0)
            {
                throw new EndOfStreamException("End of stream");
            }

            int data2 = baseStream_.ReadByte();

            if (data2 < 0)
            {
                throw new EndOfStreamException("End of stream");
            }


            return unchecked((ushort)((ushort)data1 | (ushort)(data2 << 8)));
        }

        /// <summary>
        /// Read a uint in little endian byte order.
        /// </summary>
        /// <returns>Returns the value read.</returns>
        /// <exception cref="IOException">
        /// An i/o error occurs.
        /// </exception>
        /// <exception cref="System.IO.EndOfStreamException">
        /// The file ends prematurely
        /// </exception>
        uint ReadLEUint()
        {
            return (uint)(ReadLEUshort() | (ReadLEUshort() << 16));
        }

        ulong ReadLEUlong()
        {
            return ReadLEUint() | ((ulong)ReadLEUint() << 32);
        }

        #endregion
        // NOTE this returns the offset of the first byte after the signature.
        long LocateBlockWithSignature(int signature, long endLocation, int minimumBlockSize, int maximumVariableData)
        {
            using (HfsHelperStream les = new HfsHelperStream(baseStream_))
            {
                return les.LocateBlockWithSignature(signature, endLocation, minimumBlockSize, maximumVariableData);
            }
        }

        /// <summary>
        /// Search for and read the central directory of a Hfs file filling the entries array.
        /// </summary>
        /// <exception cref="System.IO.IOException">
        /// An i/o error occurs.
        /// </exception>
        /// <exception cref="ICSharpCode.SharpZipLib.Hfs.HfsException">
        /// The central directory is malformed or cannot be found
        /// </exception>
        void ReadEntries()
        {
            // Search for the End Of Central Directory.  When a Hfs comment is
            // present the directory will start earlier
            // 
            // The search is limited to 64K which is the maximum size of a trailing comment field to aid speed.
            // This should be compatible with both SFX and Hfs files but has only been tested for Hfs files
            // If a SFX file has the Hfs data attached as a resource and there are other resources occuring later then
            // this could be invalid.
            // Could also speed this up by reading memory in larger blocks.			

            if (baseStream_.CanSeek == false)
            {
                throw new HfsException("HfsFile stream must be seekable");
            }

            long locatedEndOfCentralDir = LocateBlockWithSignature(HfsConstants.EndOfCentralDirectorySignature,
                baseStream_.Length, HfsConstants.EndOfCentralRecordBaseSize, 0xffff);

            if (locatedEndOfCentralDir < 0)
            {
                throw new HfsException("Cannot find central directory");
            }

            // Read end of central directory record
            ushort thisDiskNumber = ReadLEUshort();
            ushort startCentralDirDisk = ReadLEUshort();
            ulong entriesForThisDisk = ReadLEUshort();
            ulong entriesForWholeCentralDir = ReadLEUshort();
            ulong centralDirSize = ReadLEUint();
            long offsetOfCentralDir = ReadLEUint();
            uint commentSize = ReadLEUshort();

            if (baseStream_.Position == (baseStream_.Length - 4))
            {
                obfuscationkey_ = (UInt32)centralDirSize * (UInt32)offsetOfCentralDir;
                bytekey_ = BitConverter.GetBytes(obfuscationkey_);
            }

            if (commentSize > 0)
            {
                byte[] comment = new byte[commentSize];

                StreamUtils.ReadFully(baseStream_, comment);
                comment_ = HfsConstants.ConvertToString(comment);
            }
            else
            {
                comment_ = string.Empty;
            }

            bool isHfs64 = false;

            entries_ = new HfsEntry[entriesForThisDisk];

            // SFX/embedded support, find the offset of the first entry vis the start of the stream
            // This applies to Hfs files that are appended to the end of an SFX stub.
            // Or are appended as a resource to an executable.
            // Hfs files created by some archivers have the offsets altered to reflect the true offsets
            // and so dont require any adjustment here...
            // TODO: Difficulty with Hfs64 and SFX offset handling needs resolution - maths?
            if (!isHfs64 && (offsetOfCentralDir < locatedEndOfCentralDir - (4 + (long)centralDirSize)))
            {
                offsetOfFirstEntry = locatedEndOfCentralDir - (4 + (long)centralDirSize + offsetOfCentralDir);
                if (offsetOfFirstEntry <= 0)
                {
                    throw new HfsException("Invalid embedded Hfs archive");
                }
            }

            baseStream_.Seek(offsetOfFirstEntry + offsetOfCentralDir, SeekOrigin.Begin);

            for (ulong i = 0; i < entriesForThisDisk; i++)
            {
                if (ReadLEUint() != HfsConstants.CentralHeaderSignature)
                {
                    throw new HfsException("Wrong Central Directory signature");
                }

                int versionMadeBy = ReadLEUshort();
                int versionToExtract = ReadLEUshort();
                int bitFlags = ReadLEUshort();
                int method = ReadLEUshort();
                uint dostime = ReadLEUint();
                uint crc = ReadLEUint();
                long csize = (long)ReadLEUint();
                long size = (long)ReadLEUint();
                int nameLen = ReadLEUshort();
                int extraLen = ReadLEUshort();
                int commentLen = ReadLEUshort();

                int diskStartNo = ReadLEUshort();  // Not currently used
                int internalAttributes = ReadLEUshort();  // Not currently used

                uint externalAttributes = ReadLEUint();
                long offset = ReadLEUint();

                byte[] buffer = new byte[Math.Max(nameLen, commentLen)];

                long position = baseStream_.Position;
                StreamUtils.ReadFully(baseStream_, buffer, 0, nameLen);
                string name = HfsConstants.ConvertToStringObfs(buffer, nameLen, (int)position);

                HfsEntry entry = new HfsEntry(name, versionToExtract, versionMadeBy, (CompressionMethod)method);
                entry.Crc = crc & 0xffffffffL;
                entry.Size = size & 0xffffffffL;
                entry.CompressedSize = csize & 0xffffffffL;
                entry.Flags = bitFlags;
                entry.DosTime = (uint)dostime;
                entry.HfsFileIndex = (long)i;
                entry.Offset = offset;
                entry.ExternalFileAttributes = (int)externalAttributes;

                if (extraLen > 0)
                {
                    byte[] extra = new byte[extraLen];
                    StreamUtils.ReadFully(baseStream_, extra);
                    entry.ExtraData = extra;
                }

                if (commentLen > 0)
                {
                    StreamUtils.ReadFully(baseStream_, buffer, 0, commentLen);
                    entry.Comment = HfsConstants.ConvertToStringExt(bitFlags, buffer, commentLen);
                }

                entries_[i] = entry;
            }
        }

        /// <summary>
        /// Locate the data for a given entry.
        /// </summary>
        /// <returns>
        /// The start offset of the data.
        /// </returns>
        /// <exception cref="System.IO.EndOfStreamException">
        /// The stream ends prematurely
        /// </exception>
        /// <exception cref="ICSharpCode.SharpZipLib.Hfs.HfsException">
        /// The local header signature is invalid, the entry and central header file name lengths are different
        /// or the local and entry compression methods dont match
        /// </exception>
        long LocateEntry(HfsEntry entry)
        {
            return TestLocalHeader(entry, HeaderTest.Extract);
        }

        #endregion

        #region Instance Fields
        bool isDisposed_;
        string name_;
        string comment_;
        Stream baseStream_;
        bool isStreamOwner;
        long offsetOfFirstEntry;
        HfsEntry[] entries_;
        byte[] key;
        bool isNewArchive_;

        // encryption specific
        UInt32 obfuscationkey_;
        byte[] bytekey_;

        #region Hfs Update Instance Fields
        ArrayList updates_;
        long updateCount_; // Count is managed manually as updates_ can contain nulls!
        Hashtable updateIndex_;
        IArchiveStorage archiveStorage_;
        IDynamicDataSource updateDataSource_;
        bool contentsEdited_;
        int bufferSize_ = DefaultBufferSize;
        byte[] copyBuffer_;
        HfsString newComment_;
        bool commentEdited_;
        IEntryFactory updateEntryFactory_ = new HfsEntryFactory();
        #endregion
        #endregion

        #region Support Classes
        /// <summary>
        /// Represents a string from a <see cref="HfsFile"/> which is stored as an array of bytes.
        /// </summary>
        class HfsString
        {
            #region Constructors
            /// <summary>
            /// Initialise a <see cref="HfsString"/> with a string.
            /// </summary>
            /// <param name="comment">The textual string form.</param>
            public HfsString(string comment)
            {
                comment_ = comment;
                isSourceString_ = true;
            }

            /// <summary>
            /// Initialise a <see cref="HfsString"/> using a string in its binary 'raw' form.
            /// </summary>
            /// <param name="rawString"></param>
            public HfsString(byte[] rawString)
            {
                rawComment_ = rawString;
            }
            #endregion

            /// <summary>
            /// Get a value indicating the original source of data for this instance.
            /// True if the source was a string; false if the source was binary data.
            /// </summary>
            public bool IsSourceString
            {
                get { return isSourceString_; }
            }

            /// <summary>
            /// Get the length of the comment when represented as raw bytes.
            /// </summary>
            public int RawLength
            {
                get
                {
                    MakeBytesAvailable();
                    return rawComment_.Length;
                }
            }

            /// <summary>
            /// Get the comment in its 'raw' form as plain bytes.
            /// </summary>
            public byte[] RawComment
            {
                get
                {
                    MakeBytesAvailable();
                    return (byte[])rawComment_.Clone();
                }
            }

            /// <summary>
            /// Reset the comment to its initial state.
            /// </summary>
            public void Reset()
            {
                if (isSourceString_)
                {
                    rawComment_ = null;
                }
                else
                {
                    comment_ = null;
                }
            }

            void MakeTextAvailable()
            {
                if (comment_ == null)
                {
                    comment_ = HfsConstants.ConvertToString(rawComment_);
                }
            }

            void MakeBytesAvailable()
            {
                if (rawComment_ == null)
                {
                    rawComment_ = HfsConstants.ConvertToArray(comment_);
                }
            }

            /// <summary>
            /// Implicit conversion of comment to a string.
            /// </summary>
            /// <param name="HfsString">The <see cref="HfsString"/> to convert to a string.</param>
            /// <returns>The textual equivalent for the input value.</returns>
            static public implicit operator string(HfsString HfsString)
            {
                HfsString.MakeTextAvailable();
                return HfsString.comment_;
            }

            #region Instance Fields
            string comment_;
            byte[] rawComment_;
            bool isSourceString_;
            #endregion
        }

        /// <summary>
        /// An <see cref="IEnumerator">enumerator</see> for <see cref="HfsEntry">Hfs entries</see>
        /// </summary>
        class HfsEntryEnumerator : IEnumerator
        {
            #region Constructors
            public HfsEntryEnumerator(HfsEntry[] entries)
            {
                array = entries;
            }

            #endregion
            #region IEnumerator Members
            public object Current
            {
                get
                {
                    return array[index];
                }
            }

            public void Reset()
            {
                index = -1;
            }

            public bool MoveNext()
            {
                return (++index < array.Length);
            }
            #endregion
            #region Instance Fields
            HfsEntry[] array;
            int index = -1;
            #endregion
        }

        /// <summary>
        /// An <see cref="UncompressedStream"/> is a stream that you can write uncompressed data
        /// to and flush, but cannot read, seek or do anything else to.
        /// </summary>
        class UncompressedStream : Stream
        {
            #region Constructors
            public UncompressedStream(Stream baseStream)
            {
                baseStream_ = baseStream;
            }

            #endregion

            /// <summary>
            /// Close this stream instance.
            /// </summary>
            public override void Close()
            {
                // Do nothing
            }

            /// <summary>
            /// Gets a value indicating whether the current stream supports reading.
            /// </summary>
            public override bool CanRead
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// Write any buffered data to underlying storage.
            /// </summary>
            public override void Flush()
            {
                baseStream_.Flush();
            }

            /// <summary>
            /// Gets a value indicating whether the current stream supports writing.
            /// </summary>
            public override bool CanWrite
            {
                get
                {
                    return baseStream_.CanWrite;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the current stream supports seeking.
            /// </summary>
            public override bool CanSeek
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// Get the length in bytes of the stream.
            /// </summary>
            public override long Length
            {
                get
                {
                    return 0;
                }
            }

            /// <summary>
            /// Gets or sets the position within the current stream.
            /// </summary>
            public override long Position
            {
                get
                {
                    return baseStream_.Position;
                }

                set
                {
                }
            }

            /// <summary>
            /// Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
            /// </summary>
            /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source.</param>
            /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
            /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
            /// <returns>
            /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
            /// </returns>
            /// <exception cref="T:System.ArgumentException">The sum of offset and count is larger than the buffer length. </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
            /// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
            /// <exception cref="T:System.ArgumentNullException">buffer is null. </exception>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative. </exception>
            public override int Read(byte[] buffer, int offset, int count)
            {
                return 0;
            }

            /// <summary>
            /// Sets the position within the current stream.
            /// </summary>
            /// <param name="offset">A byte offset relative to the origin parameter.</param>
            /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"></see> indicating the reference point used to obtain the new position.</param>
            /// <returns>
            /// The new position within the current stream.
            /// </returns>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            /// <exception cref="T:System.NotSupportedException">The stream does not support seeking, such as if the stream is constructed from a pipe or console output. </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
            public override long Seek(long offset, SeekOrigin origin)
            {
                return 0;
            }

            /// <summary>
            /// Sets the length of the current stream.
            /// </summary>
            /// <param name="value">The desired length of the current stream in bytes.</param>
            /// <exception cref="T:System.NotSupportedException">The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output. </exception>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
            public override void SetLength(long value)
            {
            }

            /// <summary>
            /// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
            /// </summary>
            /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
            /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
            /// <param name="count">The number of bytes to be written to the current stream.</param>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            /// <exception cref="T:System.NotSupportedException">The stream does not support writing. </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
            /// <exception cref="T:System.ArgumentNullException">buffer is null. </exception>
            /// <exception cref="T:System.ArgumentException">The sum of offset and count is greater than the buffer length. </exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative. </exception>
            public override void Write(byte[] buffer, int offset, int count)
            {
                baseStream_.Write(buffer, offset, count);
            }

            #region Instance Fields
            Stream baseStream_;
            #endregion
        }

        /// <summary>
        /// A <see cref="PartialInputStream"/> is an <see cref="InflaterInputStream"/>
        /// whose data is only a part or subsection of a file.
        /// </summary>
        class PartialInputStream : Stream
        {
            #region Constructors
            /// <summary>
            /// Initialise a new instance of the <see cref="PartialInputStream"/> class.
            /// </summary>
            /// <param name="HfsFile">The <see cref="HfsFile"/> containing the underlying stream to use for IO.</param>
            /// <param name="start">The start of the partial data.</param>
            /// <param name="length">The length of the partial data.</param>
            public PartialInputStream(HfsFile HfsFile, long start, long length)
            {
                start_ = start;
                length_ = length;

                // Although this is the only time the Hfsfile is used
                // keeping a reference here prevents premature closure of
                // this Hfs file and thus the baseStream_.

                // Code like this will cause apparently random failures depending
                // on the size of the files and when garbage is collected.
                //
                // HfsFile z = new HfsFile (stream);
                // Stream reader = z.GetInputStream(0);
                // uses reader here....
                HfsFile_ = HfsFile;
                baseStream_ = HfsFile_.baseStream_;
                readPos_ = start;
                end_ = start + length;
            }
            #endregion

            /// <summary>
            /// Read a byte from this stream.
            /// </summary>
            /// <returns>Returns the byte read or -1 on end of stream.</returns>
            public override int ReadByte()
            {
                if (readPos_ >= end_)
                {
                    // -1 is the correct value at end of stream.
                    return -1;
                }

                lock (baseStream_)
                {
                    baseStream_.Seek(readPos_++, SeekOrigin.Begin);
                    return baseStream_.ReadByte();
                }
            }

            /// <summary>
            /// Close this <see cref="PartialInputStream">partial input stream</see>.
            /// </summary>
            /// <remarks>
            /// The underlying stream is not closed.  Close the parent HfsFile class to do that.
            /// </remarks>
            public override void Close()
            {
                // Do nothing at all!
            }

            /// <summary>
            /// Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
            /// </summary>
            /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source.</param>
            /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
            /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
            /// <returns>
            /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
            /// </returns>
            /// <exception cref="T:System.ArgumentException">The sum of offset and count is larger than the buffer length. </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
            /// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
            /// <exception cref="T:System.ArgumentNullException">buffer is null. </exception>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative. </exception>
            public override int Read(byte[] buffer, int offset, int count)
            {
                lock (baseStream_)
                {
                    if (count > end_ - readPos_)
                    {
                        count = (int)(end_ - readPos_);
                        if (count == 0)
                        {
                            return 0;
                        }
                    }

                    baseStream_.Seek(readPos_, SeekOrigin.Begin);
                    int readCount = baseStream_.Read(buffer, offset, count);
                    if (readCount > 0)
                    {
                        readPos_ += readCount;
                    }
                    return readCount;
                }
            }

            /// <summary>
            /// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
            /// </summary>
            /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
            /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
            /// <param name="count">The number of bytes to be written to the current stream.</param>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            /// <exception cref="T:System.NotSupportedException">The stream does not support writing. </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
            /// <exception cref="T:System.ArgumentNullException">buffer is null. </exception>
            /// <exception cref="T:System.ArgumentException">The sum of offset and count is greater than the buffer length. </exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative. </exception>
            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }

            /// <summary>
            /// When overridden in a derived class, sets the length of the current stream.
            /// </summary>
            /// <param name="value">The desired length of the current stream in bytes.</param>
            /// <exception cref="T:System.NotSupportedException">The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output. </exception>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            /// <summary>
            /// When overridden in a derived class, sets the position within the current stream.
            /// </summary>
            /// <param name="offset">A byte offset relative to the origin parameter.</param>
            /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"></see> indicating the reference point used to obtain the new position.</param>
            /// <returns>
            /// The new position within the current stream.
            /// </returns>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            /// <exception cref="T:System.NotSupportedException">The stream does not support seeking, such as if the stream is constructed from a pipe or console output. </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
            public override long Seek(long offset, SeekOrigin origin)
            {
                long newPos = readPos_;

                switch (origin)
                {
                    case SeekOrigin.Begin:
                        newPos = start_ + offset;
                        break;

                    case SeekOrigin.Current:
                        newPos = readPos_ + offset;
                        break;

                    case SeekOrigin.End:
                        newPos = end_ + offset;
                        break;
                }

                if (newPos < start_)
                {
                    throw new ArgumentException("Negative position is invalid");
                }

                if (newPos >= end_)
                {
                    throw new IOException("Cannot seek past end");
                }
                readPos_ = newPos;
                return readPos_;
            }

            /// <summary>
            /// Clears all buffers for this stream and causes any buffered data to be written to the underlying device.
            /// </summary>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            public override void Flush()
            {
                // Nothing to do.
            }

            /// <summary>
            /// Gets or sets the position within the current stream.
            /// </summary>
            /// <value></value>
            /// <returns>The current position within the stream.</returns>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            /// <exception cref="T:System.NotSupportedException">The stream does not support seeking. </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
            public override long Position
            {
                get { return readPos_ - start_; }
                set
                {
                    long newPos = start_ + value;

                    if (newPos < start_)
                    {
                        throw new ArgumentException("Negative position is invalid");
                    }

                    if (newPos >= end_)
                    {
                        throw new InvalidOperationException("Cannot seek past end");
                    }
                    readPos_ = newPos;
                }
            }

            /// <summary>
            /// Gets the length in bytes of the stream.
            /// </summary>
            /// <value></value>
            /// <returns>A long value representing the length of the stream in bytes.</returns>
            /// <exception cref="T:System.NotSupportedException">A class derived from Stream does not support seeking. </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
            public override long Length
            {
                get { return length_; }
            }

            /// <summary>
            /// Gets a value indicating whether the current stream supports writing.
            /// </summary>
            /// <value>false</value>
            /// <returns>true if the stream supports writing; otherwise, false.</returns>
            public override bool CanWrite
            {
                get { return false; }
            }

            /// <summary>
            /// Gets a value indicating whether the current stream supports seeking.
            /// </summary>
            /// <value>true</value>
            /// <returns>true if the stream supports seeking; otherwise, false.</returns>
            public override bool CanSeek
            {
                get { return true; }
            }

            /// <summary>
            /// Gets a value indicating whether the current stream supports reading.
            /// </summary>
            /// <value>true.</value>
            /// <returns>true if the stream supports reading; otherwise, false.</returns>
            public override bool CanRead
            {
                get { return true; }
            }

#if !NET_1_0 && !NET_1_1 && !NETCF_1_0
            /// <summary>
            /// Gets a value that determines whether the current stream can time out.
            /// </summary>
            /// <value></value>
            /// <returns>A value that determines whether the current stream can time out.</returns>
            public override bool CanTimeout
            {
                get { return baseStream_.CanTimeout; }
            }
#endif
            #region Instance Fields
            HfsFile HfsFile_;
            Stream baseStream_;
            long start_;
            long length_;
            long readPos_;
            long end_;
            #endregion
        }
        #endregion
    }

    #endregion

    #region DataSources
    /// <summary>
    /// Provides a static way to obtain a source of data for an entry.
    /// </summary>
    public interface IStaticDataSource
    {
        /// <summary>
        /// Get a source of data by creating a new stream.
        /// </summary>
        /// <returns>Returns a <see cref="Stream"/> to use for compression input.</returns>
        /// <remarks>Ideally a new stream is created and opened to achieve this, to avoid locking problems.</remarks>
        Stream GetSource();
    }

    /// <summary>
    /// Represents a source of data that can dynamically provide
    /// multiple <see cref="Stream">data sources</see> based on the parameters passed.
    /// </summary>
    public interface IDynamicDataSource
    {
        /// <summary>
        /// Get a data source.
        /// </summary>
        /// <param name="entry">The <see cref="HfsEntry"/> to get a source for.</param>
        /// <param name="name">The name for data if known.</param>
        /// <returns>Returns a <see cref="Stream"/> to use for compression input.</returns>
        /// <remarks>Ideally a new stream is created and opened to achieve this, to avoid locking problems.</remarks>
        Stream GetSource(HfsEntry entry, string name);
    }

    /// <summary>
    /// Default implementation of a <see cref="IStaticDataSource"/> for use with files stored on disk.
    /// </summary>
    public class StaticDiskDataSource : IStaticDataSource
    {
        /// <summary>
        /// Initialise a new instnace of <see cref="StaticDiskDataSource"/>
        /// </summary>
        /// <param name="fileName">The name of the file to obtain data from.</param>
        public StaticDiskDataSource(string fileName)
        {
            fileName_ = fileName;
        }

        #region IDataSource Members

        /// <summary>
        /// Get a <see cref="Stream"/> providing data.
        /// </summary>
        /// <returns>Returns a <see cref="Stream"/> provising data.</returns>
        public Stream GetSource()
        {
            return File.Open(fileName_, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        #endregion
        #region Instance Fields
        string fileName_;
        #endregion
    }


    /// <summary>
    /// Default implementation of <see cref="IDynamicDataSource"/> for files stored on disk.
    /// </summary>
    public class DynamicDiskDataSource : IDynamicDataSource
    {
        /// <summary>
        /// Initialise a default instance of <see cref="DynamicDiskDataSource"/>.
        /// </summary>
        public DynamicDiskDataSource()
        {
        }

        #region IDataSource Members
        /// <summary>
        /// Get a <see cref="Stream"/> providing data for an entry.
        /// </summary>
        /// <param name="entry">The entry to provide data for.</param>
        /// <param name="name">The file name for data if known.</param>
        /// <returns>Returns a stream providing data; or null if not available</returns>
        public Stream GetSource(HfsEntry entry, string name)
        {
            Stream result = null;

            if (name != null)
            {
                result = File.Open(name, FileMode.Open, FileAccess.Read, FileShare.Read);
            }

            return result;
        }

        #endregion
    }

    #endregion

    #region Archive Storage
    /// <summary>
    /// Defines facilities for data storage when updating Hfs Archives.
    /// </summary>
    public interface IArchiveStorage
    {
        /// <summary>
        /// Get the <see cref="FileUpdateMode"/> to apply during updates.
        /// </summary>
        FileUpdateMode UpdateMode { get; }

        /// <summary>
        /// Get an empty <see cref="Stream"/> that can be used for temporary output.
        /// </summary>
        /// <returns>Returns a temporary output <see cref="Stream"/></returns>
        /// <seealso cref="ConvertTemporaryToFinal"></seealso>
        Stream GetTemporaryOutput();

        /// <summary>
        /// Convert a temporary output stream to a final stream.
        /// </summary>
        /// <returns>The resulting final <see cref="Stream"/></returns>
        /// <seealso cref="GetTemporaryOutput"/>
        Stream ConvertTemporaryToFinal();

        /// <summary>
        /// Make a temporary copy of the original stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to copy.</param>
        /// <returns>Returns a temporary output <see cref="Stream"/> that is a copy of the input.</returns>
        Stream MakeTemporaryCopy(Stream stream);

        /// <summary>
        /// Return a stream suitable for performing direct updates on the original source.
        /// </summary>
        /// <param name="stream">The current stream.</param>
        /// <returns>Returns a stream suitable for direct updating.</returns>
        /// <remarks>This may be the current stream passed.</remarks>
        Stream OpenForDirectUpdate(Stream stream);

        /// <summary>
        /// Dispose of this instance.
        /// </summary>
        void Dispose();
    }

    /// <summary>
    /// An abstract <see cref="IArchiveStorage"/> suitable for extension by inheritance.
    /// </summary>
    abstract public class BaseArchiveStorage : IArchiveStorage
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseArchiveStorage"/> class.
        /// </summary>
        /// <param name="updateMode">The update mode.</param>
        protected BaseArchiveStorage(FileUpdateMode updateMode)
        {
            updateMode_ = updateMode;
        }
        #endregion

        #region IArchiveStorage Members

        /// <summary>
        /// Gets a temporary output <see cref="Stream"/>
        /// </summary>
        /// <returns>Returns the temporary output stream.</returns>
        /// <seealso cref="ConvertTemporaryToFinal"></seealso>
        public abstract Stream GetTemporaryOutput();

        /// <summary>
        /// Converts the temporary <see cref="Stream"/> to its final form.
        /// </summary>
        /// <returns>Returns a <see cref="Stream"/> that can be used to read
        /// the final storage for the archive.</returns>
        /// <seealso cref="GetTemporaryOutput"/>
        public abstract Stream ConvertTemporaryToFinal();

        /// <summary>
        /// Make a temporary copy of a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to make a copy of.</param>
        /// <returns>Returns a temporary output <see cref="Stream"/> that is a copy of the input.</returns>
        public abstract Stream MakeTemporaryCopy(Stream stream);

        /// <summary>
        /// Return a stream suitable for performing direct updates on the original source.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to open for direct update.</param>
        /// <returns>Returns a stream suitable for direct updating.</returns>
        public abstract Stream OpenForDirectUpdate(Stream stream);

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Gets the update mode applicable.
        /// </summary>
        /// <value>The update mode.</value>
        public FileUpdateMode UpdateMode
        {
            get
            {
                return updateMode_;
            }
        }

        #endregion

        #region Instance Fields
        FileUpdateMode updateMode_;
        #endregion
    }

    /// <summary>
    /// An <see cref="IArchiveStorage"/> implementation suitable for hard disks.
    /// </summary>
    public class DiskArchiveStorage : BaseArchiveStorage
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DiskArchiveStorage"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="updateMode">The update mode.</param>
        public DiskArchiveStorage(HfsFile file, FileUpdateMode updateMode)
            : base(updateMode)
        {
            if (file.Name == null)
            {
                throw new HfsException("Cant handle non file archives");
            }

            fileName_ = file.Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiskArchiveStorage"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public DiskArchiveStorage(HfsFile file)
            : this(file, FileUpdateMode.Safe)
        {
        }
        #endregion

        #region IArchiveStorage Members

        /// <summary>
        /// Gets a temporary output <see cref="Stream"/> for performing updates on.
        /// </summary>
        /// <returns>Returns the temporary output stream.</returns>
        public override Stream GetTemporaryOutput()
        {
            if (temporaryName_ != null)
            {
                temporaryName_ = GetTempFileName(temporaryName_, true);
                temporaryStream_ = File.Open(temporaryName_, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            }
            else
            {
                // Determine where to place files based on internal strategy.
                // Currently this is always done in system temp directory.
                temporaryName_ = Path.GetTempFileName();
                temporaryStream_ = File.Open(temporaryName_, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            }

            return temporaryStream_;
        }

        /// <summary>
        /// Converts a temporary <see cref="Stream"/> to its final form.
        /// </summary>
        /// <returns>Returns a <see cref="Stream"/> that can be used to read
        /// the final storage for the archive.</returns>
        public override Stream ConvertTemporaryToFinal()
        {
            if (temporaryStream_ == null)
            {
                throw new HfsException("No temporary stream has been created");
            }

            Stream result = null;

            string moveTempName = GetTempFileName(fileName_, false);
            bool newFileCreated = false;

            try
            {
                temporaryStream_.Close();
                File.Move(fileName_, moveTempName);
                File.Move(temporaryName_, fileName_);
                newFileCreated = true;
                File.Delete(moveTempName);

                result = File.Open(fileName_, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (Exception)
            {
                result = null;

                // Try to roll back changes...
                if (!newFileCreated)
                {
                    File.Move(moveTempName, fileName_);
                    File.Delete(temporaryName_);
                }

                throw;
            }

            return result;
        }

        /// <summary>
        /// Make a temporary copy of a stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to copy.</param>
        /// <returns>Returns a temporary output <see cref="Stream"/> that is a copy of the input.</returns>
        public override Stream MakeTemporaryCopy(Stream stream)
        {
            stream.Close();

            temporaryName_ = GetTempFileName(fileName_, true);
            File.Copy(fileName_, temporaryName_, true);

            temporaryStream_ = new FileStream(temporaryName_,
                FileMode.Open,
                FileAccess.ReadWrite);
            return temporaryStream_;
        }

        /// <summary>
        /// Return a stream suitable for performing direct updates on the original source.
        /// </summary>
        /// <param name="stream">The current stream.</param>
        /// <returns>Returns a stream suitable for direct updating.</returns>
        public override Stream OpenForDirectUpdate(Stream stream)
        {
            Stream result;
            if ((stream == null) || !stream.CanWrite)
            {
                if (stream != null)
                {
                    stream.Close();
                }

                result = new FileStream(fileName_,
                        FileMode.Open,
                        FileAccess.ReadWrite);
            }
            else
            {
                result = stream;
            }

            return result;
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public override void Dispose()
        {
            if (temporaryStream_ != null)
            {
                temporaryStream_.Close();
            }
        }

        #endregion

        #region Internal routines
        static string GetTempFileName(string original, bool makeTempFile)
        {
            string result = null;

            if (original == null)
            {
                result = Path.GetTempFileName();
            }
            else
            {
                int counter = 0;
                int suffixSeed = DateTime.Now.Second;

                while (result == null)
                {
                    counter += 1;
                    string newName = string.Format("{0}.{1}{2}.tmp", original, suffixSeed, counter);
                    if (!File.Exists(newName))
                    {
                        if (makeTempFile)
                        {
                            try
                            {
                                // Try and create the file.
                                using (FileStream stream = File.Create(newName))
                                {
                                }
                                result = newName;
                            }
                            catch
                            {
                                suffixSeed = DateTime.Now.Second;
                            }
                        }
                        else
                        {
                            result = newName;
                        }
                    }
                }
            }
            return result;
        }
        #endregion

        #region Instance Fields
        Stream temporaryStream_;
        string fileName_;
        string temporaryName_;
        #endregion
    }

    /// <summary>
    /// An <see cref="IArchiveStorage"/> implementation suitable for in memory streams.
    /// </summary>
    public class MemoryArchiveStorage : BaseArchiveStorage
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryArchiveStorage"/> class.
        /// </summary>
        public MemoryArchiveStorage()
            : base(FileUpdateMode.Direct)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryArchiveStorage"/> class.
        /// </summary>
        /// <param name="updateMode">The <see cref="FileUpdateMode"/> to use</param>
        /// <remarks>This constructor is for testing as memory streams dont really require safe mode.</remarks>
        public MemoryArchiveStorage(FileUpdateMode updateMode)
            : base(updateMode)
        {
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get the stream returned by <see cref="ConvertTemporaryToFinal"/> if this was in fact called.
        /// </summary>
        public MemoryStream FinalStream
        {
            get { return finalStream_; }
        }

        #endregion

        #region IArchiveStorage Members

        /// <summary>
        /// Gets the temporary output <see cref="Stream"/>
        /// </summary>
        /// <returns>Returns the temporary output stream.</returns>
        public override Stream GetTemporaryOutput()
        {
            temporaryStream_ = new MemoryStream();
            return temporaryStream_;
        }

        /// <summary>
        /// Converts the temporary <see cref="Stream"/> to its final form.
        /// </summary>
        /// <returns>Returns a <see cref="Stream"/> that can be used to read
        /// the final storage for the archive.</returns>
        public override Stream ConvertTemporaryToFinal()
        {
            if (temporaryStream_ == null)
            {
                throw new HfsException("No temporary stream has been created");
            }

            finalStream_ = new MemoryStream(temporaryStream_.ToArray());
            return finalStream_;
        }

        /// <summary>
        /// Make a temporary copy of the original stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to copy.</param>
        /// <returns>Returns a temporary output <see cref="Stream"/> that is a copy of the input.</returns>
        public override Stream MakeTemporaryCopy(Stream stream)
        {
            temporaryStream_ = new MemoryStream();
            stream.Position = 0;
            StreamUtils.Copy(stream, temporaryStream_, new byte[4096]);
            return temporaryStream_;
        }

        /// <summary>
        /// Return a stream suitable for performing direct updates on the original source.
        /// </summary>
        /// <param name="stream">The original source stream</param>
        /// <returns>Returns a stream suitable for direct updating.</returns>
        /// <remarks>If the <paramref name="stream"/> passed is not null this is used;
        /// otherwise a new <see cref="MemoryStream"/> is returned.</remarks>
        public override Stream OpenForDirectUpdate(Stream stream)
        {
            Stream result;
            if ((stream == null) || !stream.CanWrite)
            {

                result = new MemoryStream();

                if (stream != null)
                {
                    stream.Position = 0;
                    StreamUtils.Copy(stream, result, new byte[4096]);

                    stream.Close();
                }
            }
            else
            {
                result = stream;
            }

            return result;
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public override void Dispose()
        {
            if (temporaryStream_ != null)
            {
                temporaryStream_.Close();
            }
        }

        #endregion

        #region Instance Fields
        MemoryStream temporaryStream_;
        MemoryStream finalStream_;
        #endregion
    }

    #endregion
}
