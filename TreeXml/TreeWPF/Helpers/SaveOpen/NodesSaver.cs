using System.Collections.ObjectModel;
using System.Linq;
using TreeWPF.ViewModels;
using TreeXmlLibrary;

namespace TreeWPF.Helpers.SaveOpen
{
    public class NodesSaver
    {
        #region Fields

        private ObservableCollection<NodeViewModel> _treeCollection;

        private Tree _tree;
        #endregion
        
        public void SaveToFile(ObservableCollection<NodeViewModel> tree, string filename)
        {
            _treeCollection = tree;
            Node root = NodeFormer(_treeCollection.First());
            _tree = new Tree(root);
            SaveNodes(_treeCollection.First().Children, root);
            SaveXml saveXml = new SaveXml();
            saveXml.Save(root, filename);
        }

        private void SaveNodes(ObservableCollection<NodeViewModel> collection, Node parent)
        {
            foreach (var nodeViewModel in collection)
            {
                var currentNode = NodeFormer(nodeViewModel);
                if (nodeViewModel.NodeName != "")
                    parent.AddChild(currentNode);
                if (nodeViewModel.Children.Count != 0)
                    SaveNodes(nodeViewModel.Children, currentNode);
            }
        }


        private Node NodeFormer(NodeViewModel viewModel)
        {
            Node node = new Node();
            node.Name = viewModel.Type;
            node.AddAttribute(nameof(viewModel.NodeName), viewModel.NodeName);
            node.AddAttribute(nameof(viewModel.IsExpanded), viewModel.IsExpanded.ToString());
            node.AddAttribute(nameof(viewModel.IsSelected), viewModel.IsSelected.ToString());
            node.AddAttribute("Node.MainName", viewModel.Node.Name);
            if (viewModel.Node.Attributes != null)
                foreach (var nodeAttribute in viewModel.Node.Attributes)
                {
                    node.AddAttribute("Node." + nodeAttribute.Name, nodeAttribute.Value);
                }


            return node;
        }

        


    }
}