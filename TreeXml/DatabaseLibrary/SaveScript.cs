using System.Collections.Generic;
using System.IO;
using System.Linq;
using DatabaseLibrary.Enums;
using TreeXmlLibrary;

namespace DatabaseLibrary
{
    public class SaveScript
    {
        public void Save(string argument, IList<Node> searchResult, Tree tree)
        {
            var factoryQuery = new FactoryQuery();
            using (var writer = new StreamWriter(argument))
            {
                if (searchResult == null)
                {
                    var script = factoryQuery.GetPrinter(tree.Root.GetChildByName(SingleItem.Database)).Print();
                    writer.Write(script);
                }
                else
                {
                    writer.Write(factoryQuery.CreateScriptItem(searchResult.First())?.GetUseDb());
                    foreach (var node in searchResult)
                    {
                        var script = factoryQuery.GetPrinter(node)?.Print();
                        writer.Write(script);
                    }
                }
            }
        }

    }
}