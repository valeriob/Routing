using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus.Saga;


namespace Routing.Handlers
{
    public class MySaga2 : Saga<Saga_State>
        ,IAmStartedByMessages<StartingMessage>
    {
        StateMachine<Saga_State> Machine;

        public MySaga2()
        {
            Machine = new StateMachine<Saga_State>();

            Machine.Start_From(Saga_State.StatoA)
                .Stop_Into(Saga_State.StatoC)
                .Add_State(Saga_State.StatoB);

            Machine.Transition().From(Saga_State.StatoA).To(Saga_State.StatoB).OnMessage<StartingMessage>();

            Machine.Transition().From(Saga_State.StatoB).To(Saga_State.StatoC).When(s => s != null);

            //OnMessageReceived<StartingMessage>(m => { });
        }

        public void Handle(StartingMessage message)
        {
            Machine.Update_State(message);
        }
    }

}
