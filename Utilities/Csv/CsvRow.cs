using System.Collections.Generic;

namespace Utilities.Csv
{
    public class CsvRow : List<string>
    {
        public string LineText { get; set; }
        public bool EmptyRow { get; set; }
    }
}