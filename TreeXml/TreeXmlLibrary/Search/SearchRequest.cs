using System;
using System.Collections.Generic;
using System.Linq;
using TreeXmlLibrary.Enums;

namespace TreeXmlLibrary.Search
{
    public class SearchRequest
    {
        public List<string> Names { get; }
        public List<string> Textes { get; }
        public List<Attribute> Attributes { get; }


        public SearchMode SearchMode { get; set; } = SearchMode.All;
        public SearchType SearchType { get; set; } = SearchType.And;

        public SearchRequest()
        {
            Names =  new List<string>();  
            Textes = new List<string>();
            Attributes = new List<Attribute>();
        }

        private Predicate<Node> ByAnyName()
        {
            // SearchState searchState = SearchState.ByName;
            return node => Names.Any(name => name == node.Name);
        }

        private Predicate<Node> ByAllNames(SearchState searchState)
        {
            bool flag = false;
            Predicate<Node> pred = node => Names.All(name =>
            {
                if (name == node.Name)
                {
                    flag = true;
                    return flag;
                }
                flag = false;
                return flag;
            });
           return node => Names.All(name => name == node.Name);
        }

        private Predicate<Node> ByAnyText()
        {
            return node => Textes.Any(text => text == node.Value);
        }

        private Predicate<Node> ByAllTextes(SearchState searchState)
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
                            return node.HasValue(attribute.Name);
                        return node.Attributes.Contains(attribute);
                    });
                return false;

            };
        }

        private Predicate<Node> ByAllAttributes(SearchState searchState)
        {
            Predicate<Node> node2 = node =>
            {
                if (node != null)
                    return Attributes.All(attribute =>
                    {
                        if (attribute.Value == null)
                            return node.HasValue(attribute.Name);
                        return node.Attributes.Contains(attribute);
                    });
                return false;

            };

            return node =>
            {
                if (node.Attributes != null)
                    return Attributes.All(attribute =>
                    {
                        if (attribute.Value == null)
                            return node.HasValue(attribute.Name);
                        return node.Attributes.Contains(attribute);
                    });
                return false;

            };
        }

        public bool PredicateCombiner(Node node)
        {
            SearchState searchState = SearchState.None | SearchState.ByName | SearchState.ByAttribute | SearchState.ByText;

            Predicate<Node> resultQuery = null;
            resultQuery = ByAnyName()(node) && (ByAnyText()(node) || ByAnyAttribute()(node));
            //if (ByAllNames(searchState)(node) && ByAllTextes(searchState)(node) && ByAllAttributes(searchState)(node))
            //    return true;
            if (ByAnyName()(node) && (ByAnyText()(node) || ByAnyAttribute()(node)))
                return true;
            //switch (SearchType)
            //{
            //    case SearchType.And:
            //        if (Names.Count != 0)
            //            resultQuery += ByAllNames(searchState);
            //        if (Textes.Count != 0)
            //            resultQuery += ByAllTextes(searchState);
            //        if (Attributes.Count != 0)
            //            resultQuery += ByAllAttributes(searchState);
            //        break;
            //    case SearchType.Or:
            //        break;
            //    case SearchType.Names:
            //        if (Names.Count != 0)
            //            resultQuery += ByAnyName();
            //        if (Textes.Count != 0)
            //            resultQuery += ByAnyText();
            //        if (Attributes.Count != 0)
            //            resultQuery += ByAnyAttribute();
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}
            return false;
        }

    }
}