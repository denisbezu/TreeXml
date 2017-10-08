using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using DatabaseLibrary;
using DatabaseLibrary.Enums;
using TreeWPF.Commands;
using TreeWPF.Helpers;
using TreeWPF.Helpers.SaveOpen;
using TreeXmlLibrary;

namespace TreeWPF.ViewModels
{
    public class NodeViewModel : ViewModelBase
    {
        #region Fields

        private const string _defaultNodeName = "";

        private readonly Action<object> _selectedChanged;

        private bool _isExpanded;

        private bool _isSelected;

        private string _nodeName;

        private Node _node;

        private string _type;

        private ObservableCollection<NodeViewModel> _children;

        #endregion

        #region Properties

        public ICommand RefreshCommand { get; set; }

        public ICommand ContextOpeningCommand { get; set; }

        /// <summary>
        /// Имя узла в дереве
        /// </summary>
        public string NodeName
        {
            get { return _nodeName; }
            set
            {
                _nodeName = value;
                OnPropertyChanged(nameof(NodeName));
            }
        }

        /// <summary>
        /// Конкретный узел
        /// </summary>
        public Node Node
        {
            get { return _node; }
            set
            {
                _node = value;
                OnPropertyChanged(nameof(Node));
            }
        }

        /// <summary>
        /// Тип узла (для того, чтобы определить потом картинку)
        /// </summary>
        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        /// <summary>
        /// Коллекция дочерних элементов
        /// </summary>
        public ObservableCollection<NodeViewModel> Children
        {
            get { return _children; }
            set
            {
                _children = value;
                OnPropertyChanged(nameof(Children));
            }
        }

        /// <summary>
        /// Выбран или нет элемент
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        /// <summary>
        /// Был ли раскрыт элемент
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                if (value)
                {
                    this.Children = new ObservableCollection<NodeViewModel>();
                    Node.Children.Clear();
                    Expand();
                }
                else
                {
                    this.ClearChildren();
                    _selectedChanged.Invoke(true);
                }
                OnPropertyChanged(nameof(IsExpanded));
            }
        }
        #endregion

        #region Ctor

        private NodeViewModel()
        {
            ContextOpeningCommand = new ActionCommand(OnContextMenuOpen);
            RefreshCommand = new ActionCommand(OnRefreshCommand);
        }

        public NodeViewModel(NodeViewModelMomento momento) : this()
        {
            _selectedChanged = momento.OnSelectedAction;
            _isExpanded = momento.IsExpanded;
            _isSelected = momento.IsSelected;
            _nodeName = momento.NodeName;
            _node = momento.Node;
            _type = momento.Type;
            Children = new ObservableCollection<NodeViewModel>();
        }

        public NodeViewModel(Node node) : this()
        {
            Node = node;
            Type = node.Name;
            SetNodeName(node);
            ClearChildren();
        }

        public NodeViewModel(Node node, Action<object> execute) : this(node)
        {
            _selectedChanged = execute;
        }

        private void OnContextMenuOpen(object o)
        {
            IsSelected = true;
        }

        private void OnRefreshCommand(object o)
        {
            if (IsExpanded)
            {
                IsExpanded = false;
                IsExpanded = true;
            }
            else
                ClearChildren();
        }
       
        #endregion

        /// <summary>
        /// Метод для установки имени узла
        /// </summary>
        /// <param name="node"></param>
        private void SetNodeName(Node node)
        {
            if (SingleItem.IsSingleItem(node.Name))
                NodeName = node.GetAttributeValue(ObjectAttribute.Name);
            if (GroupItem.IsGroupItem(node.Name))
                NodeName = node.Name;
            if (NodeName == null)
                NodeName = _defaultNodeName;
        }

        /// <summary>
        /// Возвращает значение возможности раскрытия узла
        /// </summary>
        /// <returns></returns>
        public bool CanExpand()
        {
            var canExpand = new[]
            {
                SingleItem.Database,
                SingleItem.Procedure,
                SingleItem.Schema,
                SingleItem.Table,
                SingleItem.View,
                SingleItem.Server
            }.Any(it => it == Node.Name) || GroupItem.IsGroupItem(Node.Name);
            return canExpand;
        }

        /// <summary>
        /// Очистка всех дочерних узлов и добавлеение пустого
        /// </summary>
        public void ClearChildren()
        {
            Children = new ObservableCollection<NodeViewModel>();
            Node.Children.Clear();
            if (CanExpand())
                Children.Add(new NodeViewModel(new Node("Dummy child")));
        }

        /// <summary>
        /// Раскрытие узла
        /// </summary>
        private void Expand()
        {
            Task.Run(() =>
            {
                //Thread.Sleep(1000);
                LazyLoader lazyLoader = new LazyLoader();
                lazyLoader.LoadNode(Node, ConnectionStateData.ConnectionData);
                App.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var nodeChild in Node.Children)
                    {
                        this.Children.Add(new NodeViewModel(nodeChild, _selectedChanged));
                    }
                });
                _selectedChanged(true);
            });
        }

    }
}