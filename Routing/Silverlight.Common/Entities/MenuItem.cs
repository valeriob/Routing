using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

using ReactiveUI;

namespace Silverlight.Common.Entities
{
   
    //TO-DO da integrare con i menu del portale
    public class MenuItem : ReactiveObject
    {
        public MenuItem() { }

        public MenuItem(String id, String description, String function)
        {
            Description = description;
            Url = function;
            Id = id;
        }

        public MenuItem(String id, String description, String url, String image, int position)
        {
            Id = id;
            Description = description;
            Url = url;
            Image = image;
            Position = position;
        }

        private int position;
        public int Position { get { return position; } set { position = value; this.RaisePropertyChanged(n => n.Position); } }

        private bool visibility;
        public bool Visibility
        {
            get { return visibility; }
            set { visibility = value; this.RaisePropertyChanged(n => n.Visibility); }
        }
        
        private ICommand command;
        public ICommand Command { get { return command; } set { command = value; this.RaisePropertyChanged(n => n.Command); } }

        private string description;
        public string Description { get { return description; } set { description = value; this.RaisePropertyChanged(n => n.Description); } }

        private string _image;
        public string Image
        {
            get
            {
                if (String.IsNullOrEmpty(_image))
                {
                    return "/Images/None.png";
                }
                else
                {
                    return _image;
                }
            }
            set
            {
                _image = value;
                this.RaisePropertyChanged(n => n.Image);
            }
        }

        private string url;
        public string Url { get { return url; } set { url = value; this.RaisePropertyChanged(n => n.Url); } }

        private string id;
        public string Id { get { return id; } set { id = value; this.RaisePropertyChanged(n => n.Id); } }

        protected ObservableCollection<MenuItem> _children;
        public ObservableCollection<MenuItem> Children 
        { 
            get { return _children ?? (_children = new ObservableCollection<MenuItem>()); }
            set { _children = value; this.RaisePropertyChanged(n => n.Children); } 
        }

    }

}
