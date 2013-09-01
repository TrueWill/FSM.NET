using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SampleWinFormsApp
{
    public class Model : IModel
    {
        private readonly BindingList<string> _availableEvents = new BindingList<string>();
        private string _currentState;
        private readonly IServiceClient _serviceClient;

        public Model(IServiceClient serviceClient)
        {
            if (serviceClient == null)
            {
                throw new ArgumentNullException("serviceClient");
            }

            _serviceClient = serviceClient;
        }

        public IEnumerable<string> AvailableEvents
        {
            get { return _availableEvents; }
        }

        public string CurrentState
        {
            get { return _currentState; }
            private set
            {
                var availableEvents = _serviceClient.GetAvailableEvents(value).ToList();

                // There has to be a better way to get data binding to work. Suggestions?
                _availableEvents.Clear();
                
                foreach (var availableEvent in availableEvents)
                {
                    _availableEvents.Add(availableEvent);
                }
                
                _currentState = value;

                OnPropertyChanged(null);
            }
        }

        public bool IsAnyEventAvailable
        {
            get { return AvailableEvents != null && AvailableEvents.Any(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Initialize()
        {
            // I'm not satisfied with this design - I'd like the model to be
            // valid at all times, but I don't want a service call in the
            // constructor. Lazy-loaded properties add the possibility of
            // exceptions on property access, and might make it more difficult
            // to add async support to the application to avoid UI freezes.
            // Suggestions welcome.

            CurrentState = _serviceClient.GetInitialState();
        }

        public void Transition(string triggeringEvent)
        {
            CurrentState = _serviceClient.GetNewState(CurrentState, triggeringEvent);
        }

        /// <remarks>
        /// A null property name indicates that all properties have changed.
        /// </remarks>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}