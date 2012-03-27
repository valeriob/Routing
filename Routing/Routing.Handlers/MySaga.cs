using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus.Saga;


namespace Routing.Handlers
{
    public class MySaga : StateMachine<Saga_State>
        ,IAmStartedByMessages<StartingMessage>
    {
        public MySaga()
        {
            Start_From(Saga_State.StatoA)
                .Stop_Into(Saga_State.StatoC);
                //.Add_State(Saga_State.StatoB);

            Transition().From(Saga_State.StatoA).To(Saga_State.StatoB).OnMessage<StartingMessage>()
                .OnExecute(s => { });

            Transition().From(Saga_State.StatoB).To(Saga_State.StatoC).When(s => s != null)
                .OnExecute(s => { });


            Validate();

            Start();
        }

        public void Handle(StartingMessage message)
        {
            Update_State(message);
        }

    }

    public class Saga_State : IState
    {
        public static readonly string StatoA = "Stato_A";
        public static readonly string StatoB = "Stato_B";
        public static readonly string StatoC = "Stato_C";

        public string Current_State { get; set; }
        public string Starting_State { get; set; }
        public string Stopping_State { get; set; }
    }

    public class StartingMessage
    {
    
    }

}
