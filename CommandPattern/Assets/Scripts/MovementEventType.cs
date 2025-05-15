using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    /// <summary>
    /// The list of types of events that 
    /// the subscribers will be listening for
    /// </summary>
    public enum MovementEventType
    {
        START, STOP, RESTART,
        RANDOM_OBSTACLES,
        IMPACT,
        UPDATE_SCOREBOARD,
        ARRIVED_AT_DESTINATION,
        NEXT_MOVE

    }
}
