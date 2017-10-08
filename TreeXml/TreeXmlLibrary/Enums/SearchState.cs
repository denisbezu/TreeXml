using System;

namespace TreeXmlLibrary.Enums
{
    [Flags]
    public enum SearchState
    {
        None = 1,
        ByName = 2,
        ByText = 4,
        ByAttribute = 8,
    }
}