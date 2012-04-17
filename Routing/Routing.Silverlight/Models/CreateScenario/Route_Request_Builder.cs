using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Maps.MapControl;

namespace Routing.Silverlight.Models.CreateScenario
{
    public class Route_Request_Builder
    {
        IEnumerable<DistanzaStimata> Distanze;
        int Max = 20;
        public Route_Request_Builder(IEnumerable<DistanzaStimata> distanze)
        {
            Distanze = distanze;
            Build();
        }
        public IEnumerable<Request> Get_Requests() { return Requests; }
        List<Request> Requests;
        protected void Build()
        {
            Requests = new List<Request>();
            var rnd = new Random();

            foreach (var tratta in Distanze)
            {
                Request request;
                var requests = Requests.Where(r => r.Can_Append(tratta)).ToList();
                if (!requests.Any())
                {
                    request = Build_Request(tratta);
                    Requests.Add(request);
                }
                else
                    //request = requests[rnd.Next(requests.Count)];
                    request = requests.First();

                request.Append(tratta);
            }

        }

        protected Request Build_Request(DistanzaStimata trattaIniziale)
        {
            var request = new RequestDuplicated(Max);
            return request;
        }



    }

    public class Request
    {
        protected int Max;
        public Request(int max)
        {
            Max = max;
        }

        public DistanzaStimata Head { get; protected set; }
        public DistanzaStimata Tail { get; protected set; }

        protected List<DistanzaStimata> Routes = new List<DistanzaStimata>();
        public IEnumerable<DistanzaStimata> Get_Routes() { return Routes; }
        public IEnumerable<Location> Get_Locations()
        {
            return Routes.Select(r => r.From).Concat(Routes.Select(r => r.To)).Distinct();
        }
        public void Append_Head(DistanzaStimata tratta)
        {
            Head = tratta;
            if (Tail == null)
                Tail = Head;
            Routes.Insert(0, tratta);
        }

        protected void Append_Tail(DistanzaStimata tratta)
        {
            Tail = tratta;
            if (Head == null)
                Head = Tail;
            Routes.Add(tratta);
        }

        protected bool Can_Append_Head(DistanzaStimata tratta)
        {
            return Routes.Count <= Max && (Head == null || Head.From == tratta.To);
            //return (Head == null || Head.From == tratta.To);
        }

        protected bool Can_Append_Tail(DistanzaStimata tratta)
        {
            return Routes.Count <= Max && (Tail == null || Tail.To == tratta.From);
            //return (Tail == null || Tail.To == tratta.From);
        }


        public virtual void Append(DistanzaStimata tratta)
        {
            if (Can_Append_Head(tratta))
                Append_Head(tratta);
            else
                if (Can_Append_Tail(tratta))
                    Append_Tail(tratta);
        }

        public virtual bool Can_Append(DistanzaStimata tratta)
        {
            return Can_Append_Head(tratta) || Can_Append_Tail(tratta);
        }

        public bool Completed { get; protected set; }
        public void Done()
        {
            Completed = true;
        }
        //public IEnumerable<DistanzaStimata> Non_Completate()
        //{
        //    return this.Routes.Where(r=> r.Completed)
        //}
    }

    public class RequestDuplicated : Request
    {
        public RequestDuplicated(int max)
            : base(max)
        {

        }


        public override void Append(DistanzaStimata tratta)
        {
            if (Can_Append_Head(tratta))
                Append_Head(tratta);
            else
            {
                if (Can_Append_Tail(tratta))
                    Append_Tail(tratta);
                else
                {
                    var fake = new DistanzaStimata { From = tratta.To, To = Head.From };
                    Append_Head(fake);
                    Append_Head(tratta);
                }
            }
        }

        public override bool Can_Append(DistanzaStimata tratta)
        {
            var points = Routes.Select(r => r.From).Concat(Routes.Select(r => r.To));
            return Max >= Routes.Count && (Can_Append_Head(tratta) | Can_Append_Tail(tratta) |
                !(points.Contains(tratta.From) || points.Contains(tratta.To)));
        }


    }

}
