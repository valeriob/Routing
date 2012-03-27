using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ReactiveUI;
using Routing.Silverlight.References;
using Routing.Silverlight.ServiceReferences;

namespace Routing.Silverlight
{
    public class Begin_Use_Case
    {
        public Begin_Use_Case()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; protected set; }
    }

    public class End_Use_Case
    {
        public End_Use_Case(Begin_Use_Case source)
        {
            Id = source.Id;
        }
        public Guid Id { get; protected set; }
    }



    public class Begin_Search_Destination :Begin_Use_Case {}

    public class End_Search_Destination :End_Use_Case
    {
        public DestinationDto Chosen { get; protected set; }

        public End_Search_Destination(Begin_Search_Destination source, DestinationDto chosen) : base(source) 
        { 
            Chosen = chosen;
        }
        public End_Search_Destination(Begin_Search_Destination source) : base(source) { }
    }


  
}
