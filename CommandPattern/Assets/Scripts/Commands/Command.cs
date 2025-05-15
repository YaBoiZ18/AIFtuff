using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    /// <summary>
    /// Base class for all movement
    /// </summary>
    abstract public class Command
    {

        /// <summary>
        /// Step 10.
        /// Explain this method. 
        /// 
        /// The method checks whether a given Marker can be visited or used in the game.
        /// It evaluates the marker based on three conditions:
        /// </summary>
        /// <param name="marker">The marker that may be visited</param>
        /// <returns></returns>
        protected bool CanExecuteMarker(Marker marker)
        {
            if (marker == null)
                return false;
            if (marker.HasBeenVisited)
                return false;
            if (GameManager.Instance.IsBarrier(marker))
                return false;
            else
                return true;
        }
        
        protected Vector3 MoveHorizontal(Marker marker, Vector3 playerPosition)
        {
            Vector3 diff = Vector3.zero;
            diff = marker.transform.position - playerPosition;
            diff = new Vector3(Mathf.RoundToInt(diff.x), 0, 0);
            return diff;
        }

        protected Vector3 MoveVertical(Marker marker, Vector3 playerPosition)
        {
            Vector3 diff = Vector3.zero;
            diff = marker.transform.position - playerPosition;
            diff = new Vector3(0, 0, Mathf.RoundToInt(diff.z));
            return diff;
        }
        
        /// <summary>
        /// Calculate the Vector3 needed to move to the marker
        /// </summary>
        /// <param name="marker">The selected marker to go to</param>
        /// <param name="position">The current position of the player</param>
        /// <returns></returns>
        public abstract Vector3 Execute(Marker marker, Vector3 playerPosition);


        public abstract bool CanExecute(Dictionary<Direction, Marker> markers);
    }
}
