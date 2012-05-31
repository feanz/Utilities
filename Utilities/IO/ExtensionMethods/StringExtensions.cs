using System.IO;

namespace Utilities.IO.ExtensionMethods
{
    /// <summary>
    ///   Extension methods for strings
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///   Creates or opens a file for writing and writes text to it. Very simple quick file write.
        /// </summary>
        /// <param name="absolutePath"> The complete file path to write to. </param>
        /// <param name="fileText"> A String containing text to be written to the file. </param>
        public static void CreateToFile(this string fileText, string absolutePath)
        {
            using (var sw = File.CreateText(absolutePath))
                sw.Write(fileText);
        }

        /// <summary>
        ///   Read a text file and obtain it's contents.
        /// </summary>
        /// <param name="absolutePath"> The complete file path to write to. </param>
        /// <returns> String containing the content of the file. </returns>
        public static string GetFileText(this string absolutePath)
        {
            using (var sr = new StreamReader(absolutePath))
                return sr.ReadToEnd();
        }

        /// <summary>
        ///   Removes illegal characters from a directory
        /// </summary>
        /// <param name="directoryName"> Directory name </param>
        /// <param name="replacementChar"> Replacement character </param>
        /// <returns> DirectoryName with all illegal characters replaced with ReplacementChar </returns>
        public static string RemoveIllegalDirectoryNameCharacters(this string directoryName, char replacementChar = '_')
        {
            if (!string.IsNullOrEmpty(directoryName))
                return directoryName;
            foreach (var Char in Path.GetInvalidPathChars())
                directoryName = directoryName.Replace(Char, replacementChar);
            return directoryName;
        }

        /// <summary>
        ///   Removes illegal characters from a file
        /// </summary>
        /// <param name="fileName"> File name </param>
        /// <param name="replacemetChar"> Replacement character </param>
        /// <returns> FileName with all illegal characters replaced with ReplacementChar </returns>
        public static string RemoveIllegalFileNameCharacters(this string fileName, char replacemetChar = '_')
        {
            if (string.IsNullOrEmpty(fileName))
                return fileName;
            foreach (var c in Path.GetInvalidFileNameChars())
                fileName = fileName.Replace(c, replacemetChar);
            return fileName;
        }

        /// <summary>
        ///   Update text within a file by replacing a substring within the file.
        /// </summary>
        /// <param name="absolutePath"> The complete file path to write to. </param>
        /// <param name="lookFor"> A String to be replaced. </param>
        /// <param name="replaceWith"> A String to replace all occurrences of lookFor. </param>
        public static void UpdateFileText(this string absolutePath, string lookFor, string replaceWith)
        {
            var newText = GetFileText(absolutePath).Replace(lookFor, replaceWith);
            WriteToFile(absolutePath, newText);
        }

        /// <summary>
        ///   Writes out a string to a file. Very simple quick file write.
        /// </summary>
        /// <param name="absolutePath"> The complete file path to write to. </param>
        /// <param name="fileText"> A String containing text to be written to the file. </param>
        public static void WriteToFile(this string absolutePath, string fileText)
        {
            using (var sw = new StreamWriter(absolutePath, false))
                sw.Write(fileText);
        }
    }
}