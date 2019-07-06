using ImportDataContracts;
using System.Collections.Generic;

namespace FileParsing
{
    public interface ITextFileParser
    {
        IEnumerable<AddressInfo> ParseFile(string filePath);
    }
}
