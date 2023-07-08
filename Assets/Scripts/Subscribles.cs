using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
    public class SubscribableAction
    {
        private readonly List<Action> _subscribers = new();

        public void Subscribe(Action action)
        {
            _subscribers.Add(action);
        }

        public void Invoke()
        {
            foreach (var subscriber in _subscribers)
                subscriber.Invoke();
        }
    }
    
    public  class SubscribableAction<T>
    {
        private readonly List<Action<T>> _subscribers = new();

        public void Subscribe(Action<T> action)
        {
            _subscribers.Add(action);
        }

        public void Invoke(T value)
        {
            foreach (var subscriber in _subscribers)
                subscriber.Invoke(value);
        }
    }

    
    public  class SubscribableAction<T0,T1>
    {
        
        private readonly List<Action<T0,T1>> _subscribers = new();

        public void Subscribe(Action<T0,T1> action)
        {
            _subscribers.Add(action);
        }

        public void Invoke(T0 value1,T1 value2)
        {
            foreach (var subscriber in _subscribers)
                subscriber.Invoke(value1,value2);
        }
    }
    public class SubscribableField<T> : SubscribableAction<T>
    {
        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                Invoke(value);
            }
        }

        public SubscribableField(T value = default)
        {
            Value = value;
        }
    }
}