﻿using System;
using System.IO;
using System.Text;

using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Hfs
{
    /// <summary>
    /// HfsNameTransform transforms names as per the Hfs file naming convention.
    /// </summary>
    /// <remarks>The use of absolute names is supported although its use is not valid 
    /// according to Hfs naming conventions, and should not be used if maximum compatability is desired.</remarks>
    public class HfsNameTransform : INameTransform
    {
        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="HfsNameTransform"></see>
        /// </summary>
        public HfsNameTransform()
        {
        }

        /// <summary>
        /// Initialize a new instance of <see cref="HfsNameTransform"></see>
        /// </summary>
        /// <param name="trimPrefix">The string to trim from the front of paths if found.</param>
        public HfsNameTransform(string trimPrefix)
        {
            TrimPrefix = trimPrefix;
        }
        #endregion

        /// <summary>
        /// Static constructor.
        /// </summary>
        static HfsNameTransform()
        {
            char[] invalidPathChars;
#if NET_1_0 || NET_1_1 || NETCF_1_0
			invalidPathChars = Path.InvalidPathChars;
#else
            invalidPathChars = Path.GetInvalidPathChars();
#endif
            int howMany = invalidPathChars.Length + 2;

            InvalidEntryCharsRelaxed = new char[howMany];
            Array.Copy(invalidPathChars, 0, InvalidEntryCharsRelaxed, 0, invalidPathChars.Length);
            InvalidEntryCharsRelaxed[howMany - 1] = '*';
            InvalidEntryCharsRelaxed[howMany - 2] = '?';

            howMany = invalidPathChars.Length + 4;
            InvalidEntryChars = new char[howMany];
            Array.Copy(invalidPathChars, 0, InvalidEntryChars, 0, invalidPathChars.Length);
            InvalidEntryChars[howMany - 1] = ':';
            InvalidEntryChars[howMany - 2] = '\\';
            InvalidEntryChars[howMany - 3] = '*';
            InvalidEntryChars[howMany - 4] = '?';
        }

        /// <summary>
        /// Transform a windows directory name according to the Hfs file naming conventions.
        /// </summary>
        /// <param name="name">The directory name to transform.</param>
        /// <returns>The transformed name.</returns>
        public string TransformDirectory(string name)
        {
            name = TransformFile(name);
            if (name.Length > 0)
            {
                if (!name.EndsWith("/"))
                {
                    name += "/";
                }
            }
            else
            {
                throw new HfsException("Cannot have an empty directory name");
            }
            return name;
        }

        /// <summary>
        /// Transform a windows file name according to the Hfs file naming conventions.
        /// </summary>
        /// <param name="name">The file name to transform.</param>
        /// <returns>The transformed name.</returns>
        public string TransformFile(string name)
        {
            if (name != null)
            {
                string lowerName = name.ToLower();
                if ((trimPrefix_ != null) && (lowerName.IndexOf(trimPrefix_) == 0))
                {
                    name = name.Substring(trimPrefix_.Length);
                }

                name = name.Replace(@"\", "/");
                name = WindowsPathUtils.DropPathRoot(name);

                // Drop any leading slashes.
                while ((name.Length > 0) && (name[0] == '/'))
                {
                    name = name.Remove(0, 1);
                }

                // Drop any trailing slashes.
                while ((name.Length > 0) && (name[name.Length - 1] == '/'))
                {
                    name = name.Remove(name.Length - 1, 1);
                }

                // Convert consecutive // characters to /
                int index = name.IndexOf("//");
                while (index >= 0)
                {
                    name = name.Remove(index, 1);
                    index = name.IndexOf("//");
                }

                name = MakeValidName(name, '_');
            }
            else
            {
                name = string.Empty;
            }
            return name;
        }

        /// <summary>
        /// Get/set the path prefix to be trimmed from paths if present.
        /// </summary>
        /// <remarks>The prefix is trimmed before any conversion from
        /// a windows path is done.</remarks>
        public string TrimPrefix
        {
            get { return trimPrefix_; }
            set
            {
                trimPrefix_ = value;
                if (trimPrefix_ != null)
                {
                    trimPrefix_ = trimPrefix_.ToLower();
                }
            }
        }

        /// <summary>
        /// Force a name to be valid by replacing invalid characters with a fixed value
        /// </summary>
        /// <param name="name">The name to force valid</param>
        /// <param name="replacement">The replacement character to use.</param>
        /// <returns>Returns a valid name</returns>
        static string MakeValidName(string name, char replacement)
        {
            int index = name.IndexOfAny(InvalidEntryChars);
            if (index >= 0)
            {
                StringBuilder builder = new StringBuilder(name);

                while (index >= 0)
                {
                    builder[index] = replacement;

                    if (index >= name.Length)
                    {
                        index = -1;
                    }
                    else
                    {
                        index = name.IndexOfAny(InvalidEntryChars, index + 1);
                    }
                }
                name = builder.ToString();
            }

            if (name.Length > 0xffff)
            {
                throw new PathTooLongException();
            }

            return name;
        }

        /// <summary>
        /// Test a name to see if it is a valid name for a Hfs entry.
        /// </summary>
        /// <param name="name">The name to test.</param>
        /// <param name="relaxed">If true checking is relaxed about windows file names and absolute paths.</param>
        /// <returns>Returns true if the name is a valid Hfs name; false otherwise.</returns>
        /// <remarks>Hfs path names are actually in Unix format, and should only contain relative paths.
        /// This means that any path stored should not contain a drive or
        /// device letter, or a leading slash.  All slashes should forward slashes '/'.
        /// An empty name is valid for a file where the input comes from standard input.
        /// A null name is not considered valid.
        /// </remarks>
        public static bool IsValidName(string name, bool relaxed)
        {
            bool result = (name != null);

            if (result)
            {
                if (relaxed)
                {
                    result = name.IndexOfAny(InvalidEntryCharsRelaxed) < 0;
                }
                else
                {
                    result =
                        (name.IndexOfAny(InvalidEntryChars) < 0) &&
                        (name.IndexOf('/') != 0);
                }
            }

            return result;
        }

        /// <summary>
        /// Test a name to see if it is a valid name for a Hfs entry.
        /// </summary>
        /// <param name="name">The name to test.</param>
        /// <returns>Returns true if the name is a valid Hfs name; false otherwise.</returns>
        /// <remarks>Hfs path names are actually in unix format,
        /// and should only contain relative paths if a path is present.
        /// This means that the path stored should not contain a drive or
        /// device letter, or a leading slash.  All slashes should forward slashes '/'.
        /// An empty name is valid where the input comes from standard input.
        /// A null name is not considered valid.
        /// </remarks>
        public static bool IsValidName(string name)
        {
            bool result =
                (name != null) &&
                (name.IndexOfAny(InvalidEntryChars) < 0) &&
                (name.IndexOf('/') != 0)
                ;
            return result;
        }

        #region Instance Fields
        string trimPrefix_;
        #endregion

        #region Class Fields
        static readonly char[] InvalidEntryChars;
        static readonly char[] InvalidEntryCharsRelaxed;
        #endregion
    }
}
