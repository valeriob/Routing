using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using NServiceBus.Saga;

namespace Routing.Handlers
{
    public class StateMachine<TState> : Saga<TState> where TState : IState
    {
        //private List<string> _States;
        //public IEnumerable<string> States { get { return _States; } }

        private List<Transition<TState>> _Transitions;
        public IEnumerable<Transition<TState>> Transitions { get { return _Transitions; } }

        //private List<Subscription<TState>> _Handlers;
        //public IEnumerable<Subscription<TState>> Handlers { get { return _Handlers; } }

        public StateMachine()
        {
            //_States = new List<string>();
            _Transitions = new List<Transition<TState>>();
        }

        public void Start()
        {
            if(Data.Current_State == null)
                Data.Current_State = Data.Starting_State;
        }

        public void Update_State<T>(T message)
        {
            foreach (var transition in Transitions)
                if (transition.Matches(Data, message))
                {
                    ExecuteTransition(transition);
                    break;
                }
        }

        protected void ExecuteTransition(Transition<TState> transition)
        {
            transition.Execute(Data);
            Data.Current_State = transition.ToState;
        }


        //public StateMachine<TState> Add_State(string state)
        //{
        //    _States.Add(state);
        //    return this;
        //}

        public StateMachine<TState> Start_From(string startingState, Action<TState> init = null)
        {
            Data.Starting_State = startingState;
            return this;
        }

        public StateMachine<TState> Stop_Into(string startingState)
        {
            Data.Starting_State = startingState;
            return this;
        }

        public Transition<TState> Transition()
        {
            var transition =  new Transition<TState>();
            _Transitions.Add(transition);
            return transition;
        }

        public void Validate()
        {
            // Starting To Stopping
            // double arcs
        }
    }
    
    public class Subscription<TState>
    {
        public TState State { get; protected set; }
        public object Message { get; protected set; }
        public dynamic Action { get; protected set; }

        public Subscription(TState state, dynamic action)
        {
            State = state;
            Action = action;
        }

        public void Execute<T>(T message)
        {
            Action.Invoke(Message);
        }

        public bool Matches<TMessage>(TMessage message)
        {
            return typeof(TMessage) == Message.GetType();
        }
    }

    public class Transition<T> where T : IState
    {
        static Func<T, bool> Default = a => true;

        public string FromState { get; protected set; }
        public string ToState { get; protected set; }
        public Action<T> ToBeExecuted { get; protected set; }
        public Func<T, bool> Condition { get; protected set; }

        public Type MessageType { get; set; }
        public dynamic Message_Condition { get; set; }

        protected Transition(Transition<T> source)
        {
            FromState = source.FromState;
            ToState = source.ToState;
            Condition = source.Condition;
            ToBeExecuted = source.ToBeExecuted;
        }
        public Transition()
        {
        }
       

        public Transition<T> From(string fromState)
        {
            FromState = fromState;
            return this;
        }

        public Transition<T> To(string toState)
        {
            ToState = ToState;
            return this;
        }

        public Transition<T> When(Func<T, bool> condition)
        {
            Condition = condition ?? (f => true);
            return this;
        }

        public Transition<T> OnMessage<TMessage>(Func<TMessage, bool> condition = null)
        {
            MessageType = typeof(TMessage);
            Message_Condition = condition ?? (f=> true);
            return this;
        }

        public Transition<T> OnExecute(Action<T> action)
        {
            ToBeExecuted = action;
            return this;
        }

        public bool Matches(T state, object message)
        {
            return State_Matches(state) || Message_Matches(message);
        }

        public bool State_Matches(T state)
        {
            return FromState == state.Current_State && Condition(state);
        }

        public bool Message_Matches(object message)
        {
            return (MessageType == null || MessageType == message.GetType() && Message_Condition());
        }

        public void Execute(T state)
        {
            ToBeExecuted(state);
        }
    }

    /*
    public class State_Transition<TState> :Transition<TState>
    {
        static Func<TState, bool> Default = a => true;
        public Func<TState, bool> Condition { get; protected set; }

        public Type MessageType { get; set; }
        public dynamic Predicate { get; set; }

        public State_Transition()
        {
            Condition = Default;
        }

        public State_Transition<TState> When(Func<TState, bool> condition)
        {
            Condition = condition;
            return this;
        }

        public State_Transition<TState> On<T>(Func<T, bool> predicate)
        {
            MessageType = typeof(T);
            Predicate = predicate;
            return this;
        }
    }

    public class Message_Transition<TState> : Transition<TState>
    {
        public Type MessageType { get; set; }
        public dynamic Predicate { get; set; }

        public Message_Transition<TState> When<T>(Func<T,bool> predicate)
        {
            //predicate.Invoke()

            return this;
        }
    }
    */

    public interface IState
    {
        string Current_State { get; set; }
        string Starting_State { get; set; }
        string Stopping_State { get; set; }
    }
}
