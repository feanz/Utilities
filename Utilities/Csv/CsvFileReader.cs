using System;
using System.IO;

namespace Utilities.Csv
{
    public class CsvFileReader : StreamReader
    {
        private readonly bool _ignoreEmpty;

        public CsvFileReader(Stream stream, bool ignoreEmpty = false)
            : base(stream)
        {
            _ignoreEmpty = ignoreEmpty;
        }

        public CsvFileReader(string filename, bool ignoreEmpty = false)
            : base(filename)
        {
            _ignoreEmpty = ignoreEmpty;
        }

        /// <summary>
        ///   Reads a row of data from a CSV file
        /// </summary>
        /// <param name="row"> </param>
        /// <returns> </returns>
        public bool ReadRow(CsvRow row)
        {
            row.LineText = ReadLine();
            if (row.LineText == string.Empty)
            {
                row.EmptyRow = true;
                return true;
            }
            if (string.IsNullOrEmpty(row.LineText))
                return false;

            int pos = 0;
            int rows = 0;

            while (pos < row.LineText.Length)
            {
                string value;

                // Special handling for quoted field
                if (row.LineText[pos] == '"')
                {
                    // Skip initial quote
                    pos++;

                    // Parse quoted value
                    int start = pos;
                    while (pos < row.LineText.Length)
                    {
                        // Test for quote character
                        if (row.LineText[pos] == '"')
                        {
                            // Found one
                            pos++;

                            // If two quotes together, keep one
                            // Otherwise, indicates end of value
                            if (pos >= row.LineText.Length || row.LineText[pos] != '"')
                            {
                                pos--;
                                break;
                            }
                        }
                        pos++;
                    }
                    value = row.LineText.Substring(start, pos - start);
                    value = value.Replace("\"\"", "\"");
                }
                else
                {
                    // Parse unquoted value
                    int start = pos;
                    while (pos < row.LineText.Length && row.LineText[pos] != ',')
                        pos++;
                    value = row.LineText.Substring(start, pos - start);
                }

                // Add field to items
                if (rows < row.Count)
                    row[rows] = value;
                else
                {
                    if (_ignoreEmpty)
                    {
                        if (!String.IsNullOrEmpty(value))
                            row.Add(value);
                    }
                    else
                        row.Add(value);
                }
                rows++;

                // Eat up to and including next comma
                while (pos < row.LineText.Length && row.LineText[pos] != ',')
                    pos++;
                if (pos < row.LineText.Length)
                    pos++;
            }
            // Delete any unused items
            while (row.Count > rows)
                row.RemoveAt(rows);

            // Return true if any columns read
            return (row.Count > 0);
        }
    }
}