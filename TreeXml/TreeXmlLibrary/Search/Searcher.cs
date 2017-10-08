using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using TreeXmlLibrary.Enums;

namespace TreeXmlLibrary.Search
{
    public class Searcher
    {
        #region Properties

        public List<string> Names { get; }
        public List<string> Textes { get; }
        public List<Attribute> Attributes { get; }

        public string Runtime { get; set; }
        public int Step { get; set; }


        public AlgoType AlgoType { private get; set; }
        public SearchMode SearchMode { private get; set; } = SearchMode.All;
        public SearchType SearchType { private get; set; } = SearchType.And;

        #endregion

        public Searcher()
        {
            Attributes = new List<Attribute>();
            Names = new List<string>();
            Textes = new List<string>();
        }

        public List<Node> Search(Node rootNode)
        {
            List<Node> result = null;
            SearchInvoker searchInvoker = new SearchInvoker();
            var search = searchInvoker.SearchCommand(AlgoType);
            int step;
            var stopWatch = Stopwatch.StartNew();
            result = search.ExecuteSearch(rootNode, PredicateCombiner(), out step, SearchMode);
            stopWatch.Stop();
            Runtime = stopWatch.Elapsed.TotalMilliseconds.ToString();
            Step = step;
            return result;
        }
       
        #region ByPredicates

        private Predicate<Node> ByAnyName()
        {
            return node => Names.Any(name => name == node.Name);
        }

        private Predicate<Node> ByAllNames()
        {
            return node => Names.All(name => name == node.Name);
        }

        private Predicate<Node> ByAnyText()
        {
            return node => Textes.Any(text => text == node.Value);
        }

        private Predicate<Node> ByAllTextes()
        {
            return node => Textes.All(text => text == node.Value);
        }

        private Predicate<Node> ByAnyAttribute()
        {
            return node =>
            {
                if (node.Attributes != null)
                    return Attributes.Any(attribute =>
                    {
                        if (attribute.Value == null)
                            return node.GetAttributeValue(attribute.Name) != null;
                        return node.Attributes.Contains(attribute);
                    });
                return false;

            };
        }

        private Predicate<Node> ByAllAttributes()
        {
            return node =>
            {
                if (node.Attributes != null)
                    return Attributes.All(attribute =>
                    {
                        if (attribute.Value == null)
                            return node.GetAttributeValue(attribute.Name) != null;
                        return node.Attributes.Contains(attribute);
                    });
                return false;

            };
        }

        #endregion

        private Predicate<Node> PredicateCombiner()
        {
            SearchState searchState = (SearchState)0;
            if (Names.Count != 0)
                searchState |= SearchState.ByName;
            if (Textes.Count != 0)
                searchState |= SearchState.ByText;
            if (Attributes.Count != 0)
                searchState |= SearchState.ByAttribute;
            return SearchChooser(searchState);
        }

        private Predicate<Node> SearchChooser(SearchState searchState)
        {
            switch (searchState)
            {
                case SearchState.ByName:
                    return SearchType == SearchType.And ? ByAllNames() : ByAnyName();
                case SearchState.ByText:
                    return SearchType == SearchType.And ? ByAllTextes() : ByAnyText();
                case SearchState.ByAttribute:
                    return SearchType == SearchType.And ? ByAllAttributes() : ByAnyAttribute();
                case (SearchState.ByName | SearchState.ByText):
                    if (SearchType == SearchType.And)
                        return node => ByAllNames()(node) && ByAllTextes()(node);
                   else
                        return node => ByAnyName()(node) && ByAnyText()(node);
                case (SearchState.ByName | SearchState.ByAttribute):
                    if (SearchType == SearchType.And)
                        return node => ByAllNames()(node) && ByAllAttributes()(node);
                    else
                        return node => ByAnyName()(node) && ByAnyAttribute()(node);
                case (SearchState.ByText | SearchState.ByAttribute):
                    if (SearchType == SearchType.And)
                        return node => ByAllTextes()(node) && ByAllAttributes()(node);
                    else
                        return node => ByAnyText()(node) || ByAnyAttribute()(node);
                case (SearchState.ByName | SearchState.ByText | SearchState.ByAttribute):
                    if (SearchType == SearchType.And)
                        return node => ByAllNames()(node) && ByAllAttributes()(node) && ByAllTextes()(node);
                   else
                        return node => ByAnyName()(node) && (ByAnyAttribute()(node) || ByAnyText()(node));
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchState), searchState, null);
            }
        }


    }
}
