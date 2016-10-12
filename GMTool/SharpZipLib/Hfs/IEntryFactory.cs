using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Hfs
{
    /// <summary>
    /// Defines factory methods for creating new <see cref="HfsEntry"></see> values.
    /// </summary>
    public interface IEntryFactory
    {
        /// <summary>
        /// Create a <see cref="HfsEntry"/> for a file given its name
        /// </summary>
        /// <param name="fileName">The name of the file to create an entry for.</param>
        /// <returns>Returns a <see cref="HfsEntry">file entry</see> based on the <paramref name="fileName"/> passed.</returns>
        HfsEntry MakeFileEntry(string fileName);

        /// <summary>
        /// Create a <see cref="HfsEntry"/> for a file given its name
        /// </summary>
        /// <param name="fileName">The name of the file to create an entry for.</param>
        /// <param name="useFileSystem">If true get details from the file system if the file exists.</param>
        /// <returns>Returns a <see cref="HfsEntry">file entry</see> based on the <paramref name="fileName"/> passed.</returns>
        HfsEntry MakeFileEntry(string fileName, bool useFileSystem);

        /// <summary>
        /// Create a <see cref="HfsEntry"/> for a directory given its name
        /// </summary>
        /// <param name="directoryName">The name of the directory to create an entry for.</param>
        /// <returns>Returns a <see cref="HfsEntry">directory entry</see> based on the <paramref name="directoryName"/> passed.</returns>
        HfsEntry MakeDirectoryEntry(string directoryName);

        /// <summary>
        /// Create a <see cref="HfsEntry"/> for a directory given its name
        /// </summary>
        /// <param name="directoryName">The name of the directory to create an entry for.</param>
        /// <param name="useFileSystem">If true get details from the file system for this directory if it exists.</param>
        /// <returns>Returns a <see cref="HfsEntry">directory entry</see> based on the <paramref name="directoryName"/> passed.</returns>
        HfsEntry MakeDirectoryEntry(string directoryName, bool useFileSystem);

        /// <summary>
        /// Get/set the <see cref="INameTransform"></see> applicable.
        /// </summary>
        INameTransform NameTransform { get; set; }
    }
}
