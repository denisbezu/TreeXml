using TreeXmlLibrary.Enums;
using TreeXmlLibrary.interfaces;

namespace TreeXmlLibrary.Search
{
    public class SearchInvoker
    {
        public ISearch SearchCommand(AlgoType algoType)
        {
            ISearch search = null;
            if(algoType == AlgoType.Level)
                search = new LevelSearch();
            else if(algoType == AlgoType.Width)
                search = new WidthSearch();
            return search;
        }
    }
}