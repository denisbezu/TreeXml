using System.Collections.Generic;
using System.Windows;
using TreeWPF.ViewModels;

namespace TreeWPF.Helpers
{
    public class SearchNodeViewModel
    {
        public NodeViewModel RootNode { get; set; }

        public IEnumerator<NodeViewModel> MatchingNodesEnumerator { get; set; }

        public void PerformSearch(string searchText)
        {
            searchText = searchText.ToLower();
            if (MatchingNodesEnumerator == null || !MatchingNodesEnumerator.MoveNext())
                VerifyMatchingNodesEnumerator(searchText);

            var nodeViewModel = MatchingNodesEnumerator.Current;

            if (nodeViewModel == null)
                return;

            nodeViewModel.IsSelected = true;
        }

        private void VerifyMatchingNodesEnumerator(string searchText)
        {
            var matches = this.FindMatches(searchText, RootNode);
            MatchingNodesEnumerator = matches.GetEnumerator();

            if (!MatchingNodesEnumerator.MoveNext())
            {
                MessageDialog.ShowMessage("Nothing found, expand more nodes and try again");
            }
        }

        IEnumerable<NodeViewModel> FindMatches(string searchText, NodeViewModel nodeViewModel)
        {
            if (nodeViewModel.NodeName.ToLower().StartsWith(searchText))
                yield return nodeViewModel;

            foreach (NodeViewModel child in nodeViewModel.Children)
                foreach (NodeViewModel match in this.FindMatches(searchText, child))
                    yield return match;
        }
    }
}