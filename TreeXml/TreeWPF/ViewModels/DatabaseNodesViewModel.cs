using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using DatabaseLibrary;
using DatabaseLibrary.Enums;
using TreeWPF.Commands;
using TreeWPF.Enums;
using TreeWPF.Helpers;
using TreeWPF.Helpers.SaveOpen;
using TreeWPF.Interfaces;
using TreeXmlLibrary;
using MessageDialog = TreeWPF.Helpers.MessageDialog;

namespace TreeWPF.ViewModels
{
    public class DatabaseNodesViewModel : ViewModelBase
    {
        #region Fields

        private TreeState _treeState;

        private IOpenSaveDialog _openSaveDialog;

        private SearchNodeViewModel _searchNodeViewModel;

        private string _scriptText;

        private List<TreeXmlLibrary.Attribute> _properties;

        private string _searchText;
       
        #endregion

        #region Properties

        public ICommand GenerateScriptCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand OpenCommand { get; set; }

        public ICommand SetSelectedItemCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        public ICommand SearchCommand { get; set; }

        public ObservableCollection<NodeViewModel> Tree { get; set; }

        public NodeViewModel SelectedItem => SearchSelected(node => node.IsSelected);

        public TreeState TreeState
        {
            get { return _treeState; }
            set
            {
                _treeState = value;
                OnPropertyChanged(nameof(TreeState));
            }
        }

        public string ScriptText
        {
            get { return _scriptText; }
            set
            {
                _scriptText = value;
                OnPropertyChanged(nameof(ScriptText));
            }
        }

        public List<TreeXmlLibrary.Attribute> Properties
        {
            get { return _properties; }
            set
            {
                _properties = value;
                OnPropertyChanged(nameof(Properties));
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                _searchNodeViewModel.MatchingNodesEnumerator = null;
            }
        }

        #endregion

        #region ctor

        public DatabaseNodesViewModel(IOpenSaveDialog openSaveDialog)
        {
            #region Commands
            GenerateScriptCommand = new ActionCommand(OnGenerateScript);
            SaveCommand = new ActionCommand(OnSaveCommand);
            OpenCommand = new ActionCommand(OnOpenCommand);
            SetSelectedItemCommand = new ActionCommand(OnSelectedItemChanged);
            RefreshCommand = new ActionCommand(OnRefresh, OnRefreshCanExecute);
            SearchCommand = new ActionCommand(o =>
                {
                    _searchNodeViewModel.PerformSearch(SearchText);
                },
            o => !string.IsNullOrWhiteSpace(SearchText)
            && ConnectionStateData.ConnectionState == ConnectionState.Connected);
            #endregion
            TreeState = TreeState.Ready;
            _searchNodeViewModel = new SearchNodeViewModel();
            _openSaveDialog = openSaveDialog;
            Tree = new ObservableCollection<NodeViewModel>();
            ConnectionStateData.ConnectionDataChanged += ConnectionDataChanged;
            //_propertyChangedHandler = new PropertyChangedEventHandler(Item_PropertyChanged);
            //_collectionChangedhandler = new NotifyCollectionChangedEventHandler(Items_CollectionChanged);
            //Tree.CollectionChanged += _collectionChangedhandler;
        }

        #endregion

        private void OnGenerateScript(object o)
        {
            Task.Run(() =>
            {
                TreeState = TreeState.Busy;
                FactoryQuery factoryQuery = new FactoryQuery();
                try
                {
                    SelectedItem.Node.Children.Clear();
                    LoadNode(SelectedItem.Node);
                    var script = factoryQuery.CreateScriptItem(SelectedItem.Node)?.GenerateScript();
                    ScriptText = script;
                }
                catch (Exception)
                {
                    ScriptText = string.Empty;
                }
                finally
                {
                    TreeState = TreeState.Ready;
                }
            });
        }

        /// <summary>
        /// Метод для поиска узла
        /// </summary>
        /// <param name="o"></param>
        private void OnSearchCommand(object o)
        {
            var neededItem = SearchSelected(nodeViewModel =>
            {
                if (nodeViewModel.NodeName.StartsWith(SearchText))
                    return true;
                return false;
            });
            if (neededItem == null)
                MessageDialog.ShowMessage("Nothing found");
            else
                neededItem.IsSelected = true;
        }

        /// <summary>
        /// Метод Execute команды для кнопки обновления узла
        /// </summary>
        /// <param name="o"></param>
        private void OnRefresh(object o)
        {
            if (SelectedItem.IsExpanded)
            {
                SelectedItem.IsExpanded = false;
                SelectedItem.IsExpanded = true;
            }
            else
                SelectedItem.ClearChildren();
        }

        /// <summary>
        /// CanExecute для кнопки обновления
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool OnRefreshCanExecute(object o)
        {
            return SelectedItem != null;
        }

        /// <summary>
        /// Изменение выделенного элемента
        /// </summary>
        /// <param name="o"></param>
        private void OnSelectedItemChanged(object o)
        {
            Task.Run(() => UpdateScript());
            UpdateProperties();
        }

        /// <summary>
        /// Изменение соединения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectionDataChanged(object sender, EventArgs e)
        {
            if (ConnectionStateData.ConnectionState != ConnectionState.Disconnected)
            {
                if (Tree != null && Tree.Count != 0 && ConnectionStateData.ConnectionData.ServerName.Equals(Tree.First().NodeName))
                    return;
                Tree.Clear();
                Node server = new Node(SingleItem.Server);
                server.AddAttribute(ObjectAttribute.Name, ConnectionStateData.ServerName);
                NodeViewModel serverViewModel = new NodeViewModel(server, OnSelectedItemChanged);
                _searchNodeViewModel.RootNode = serverViewModel;
                Tree.Add(serverViewModel);
            }
            else
                Tree.Clear();
        }

        /// <summary>
        /// Поиск выделенного элемента
        /// </summary>
        /// <returns></returns>
        private NodeViewModel SearchSelected(Predicate<NodeViewModel> condition)
        {
            try
            {
                var nodesStack = new Stack<NodeViewModel>();
                nodesStack.Push(Tree.First());
                while (nodesStack.Count != 0)
                {
                    var currentNode = nodesStack.Pop();
                    if (currentNode == null)
                        continue;
                    if (condition(currentNode))
                    {
                        return currentNode;
                    }
                    foreach (var children in currentNode.Children)
                        nodesStack.Push(children);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// Обновления скрипта
        /// </summary>
        private void UpdateScript()//
        {
            FactoryQuery factoryQuery = new FactoryQuery();
            try
            {
                ScriptText = "Loading...";
                var script = factoryQuery.CreateScriptItem(SelectedItem.Node)?.GenerateScript();
                ScriptText = script;
            }
            catch (Exception)
            {
                ScriptText = string.Empty;
            }
        }

        /// <summary>
        /// Обновление свойств после измения выделенного элемента
        /// </summary>
        private void UpdateProperties()
        {
            Properties = SelectedItem?.Node?.Attributes;//
        }

        private void OnSaveCommand(object obj)
        {
            NodesSaver nodesSaver = new NodesSaver();
            _openSaveDialog.SaveFile(file => nodesSaver.SaveToFile(Tree, file));
        }

        private void OnOpenCommand(object obj)
        {
            _openSaveDialog.OpenFile(filename =>
            {
                OpenNodes openNodes = new OpenNodes();
                var tree = openNodes.OpenFile(filename);
                var rootNodeViewModel = openNodes.MakeTreeViewModel(OnSelectedItemChanged, tree);
                Tree.Clear();
                Tree.Add(rootNodeViewModel);
                _searchNodeViewModel.RootNode = rootNodeViewModel;
            });
        }
        
        private void LoadNode(Node node, bool startFromChild = false)//
        {
            foreach (var child in node)
            {
                if (child == null)
                    continue;
                if (startFromChild)
                {
                    startFromChild = false;
                    continue;
                }
                LazyLoader lazyLoader = new LazyLoader();
                lazyLoader.LoadNode(child, ConnectionStateData.ConnectionData);
            }
        }

        #region EventChanged

        //PropertyChangedEventHandler _propertyChangedHandler;
        //NotifyCollectionChangedEventHandler _collectionChangedhandler;

        //private void SubscribePropertyChanged(NodeViewModel item)
        //{
        //    item.PropertyChanged += _propertyChangedHandler;
        //    item.Children.CollectionChanged += _collectionChangedhandler;
        //    foreach (var subitem in item.Children)
        //    {
        //        if (subitem != null)
        //            SubscribePropertyChanged(subitem);
        //    }
        //}

        //private void UnsubscribePropertyChanged(NodeViewModel item)
        //{
        //    foreach (var subitem in item.Children)
        //    {
        //        UnsubscribePropertyChanged(subitem);
        //    }
        //    item.Children.CollectionChanged -= _collectionChangedhandler;
        //    item.PropertyChanged -= _propertyChangedHandler;
        //}

        //private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.OldItems != null)
        //    {
        //        foreach (NodeViewModel item in e.OldItems)
        //        {
        //            UnsubscribePropertyChanged(item);
        //        }
        //    }

        //    if (e.NewItems != null)
        //    {
        //        foreach (NodeViewModel item in e.NewItems)
        //        {
        //            SubscribePropertyChanged(item);
        //        }
        //    }
        //}

        //private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "IsSelected")
        //    {
        //        OnSelectedItemChanged(null);
        //    }
        //}

        #endregion
    }
}
