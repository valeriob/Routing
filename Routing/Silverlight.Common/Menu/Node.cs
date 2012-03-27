using System;
using System.Net;
using System.Linq;
using System.Collections.ObjectModel;

namespace Silverlight.Common.Menu
{
    public abstract class AbstractNode
    {
        public AbstractNode Parent { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class Node : AbstractNode
    {
        public ObservableCollection<AbstractNode> Children { get; set; }

        public Node()
        {
            Children = new ObservableCollection<AbstractNode>();
            //Children.CollectionChanged += (sender, e) => 
            //{ 
            //    if(e.item
            //};
        }
    }

    public class Leaf : AbstractNode
    {
        //public string Url { get; set; }
    }
}
