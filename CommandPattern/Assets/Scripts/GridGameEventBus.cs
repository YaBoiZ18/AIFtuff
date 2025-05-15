using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Assets.Scripts
{
  
    /// <summary>
    /// PlanerLanderEventBus exists so that we can reuse the
    /// Generic Event Bus with a specific type, MovementEventType.
    /// This also allows us to use PlandetLanderEventBus in our code 
    /// rather than EventBus<MovementEventType> each time we need to use it.
    /// </summary>
    public class GridGameEventBus: EventBus<MovementEventType>
    {
        // There could be other items specific to the PlanerLanderEventBus at 
        // a later time.
    }

    /// <summary>
    /// Basic class that handles Subscribe, Unsubscribe and Publish for 
    /// any class that inherits from this class which uses a enum.
    /// This code is a slight modification of what is in the book.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventBus<T> where T: System.Enum
    {
        /// <summary>
        /// Event stores the list of listeners (methods in other scripts)
        /// that need to "know" when a message of the type eventType is 
        /// published.  Type eventType is the enumerated type like 
        /// START, STOP, PAUSE.
        /// </summary>
        private static readonly
        IDictionary<T, UnityEvent>
            Events = new Dictionary<T, UnityEvent>();

        /// <summary>
        /// A script calls the derived EventBus class with the Subscribe Method 
        /// as in:
        /// PlanetLanderEventBus.Subscribe( MovementEventType.START, << a method from the script that calls subscribe >> )
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="listener"></param>
        public static void Subscribe
            (T eventType, UnityAction listener)
        {

            UnityEvent thisEvent;

            if (Events.TryGetValue(eventType, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Events.Add(eventType, thisEvent);
            }
        }
        public static void Unsubscribe
            (T type, UnityAction listener)
        {

            UnityEvent thisEvent;

            if (Events.TryGetValue(type, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void Publish(T type)
        {

            UnityEvent thisEvent;

            if (Events.TryGetValue(type, out thisEvent))
            {
                thisEvent.Invoke();
            }
        }
    }
}

