using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    // STEP 1:
    // Create the four classes that implement the public abstract methods in Command.
    // MoveLeft, MoveRight, MoveUp, MoveDown
    // They all inherit from Command.
    // Each method that you write can have bodies that are 1-2 lines long. 
    // HINT: You can do these in one line.
    // In each class you need to override the CanExecute and the Execute commands.

    // For each of the classes CanExecute needs to call the protected CanExecuteMarker method
    // with the correct entry in the markers Dictionary.
    // For example with MoveLeft you need to pass the parameter 
    // markers[Direction.LEFT]

    // For the Execute method they need to call the protected MoveHorizontal or MoveVertical
    // method depending upon which kind of direction.

    class MoveLeft : Command
    {
        public override bool CanExecute(Dictionary<Direction, Marker> markers)
        {
            return CanExecuteMarker(markers[Direction.LEFT]); // Check if the marker to the left is valid
        }

        public override Vector3 Execute(Marker marker, Vector3 playerPosition)
        {
            return MoveHorizontal(marker, playerPosition); // Move left
        }
    }

    class MoveRight : Command
    {
        public override bool CanExecute(Dictionary<Direction, Marker> markers)
        {
            return CanExecuteMarker(markers[Direction.RIGHT]); // Check if the marker to the right is valid
        }

        public override Vector3 Execute(Marker marker, Vector3 playerPosition)
        {
            return MoveHorizontal(marker, playerPosition); // Move right
        }
    }

    class MoveUp : Command
    {
        public override bool CanExecute(Dictionary<Direction, Marker> markers)
        {
            return CanExecuteMarker(markers[Direction.UP]); // Check if the marker up is valid
        }

        public override Vector3 Execute(Marker marker, Vector3 playerPosition)
        {
            return MoveVertical(marker, playerPosition); // Move up
        }
    }

    class MoveDown : Command
    {
        public override bool CanExecute(Dictionary<Direction, Marker> markers)
        {
            return CanExecuteMarker(markers[Direction.DOWN]); // Check if the marker donw is valid
        }

        public override Vector3 Execute(Marker marker, Vector3 playerPosition)
        {
            return MoveVertical(marker, playerPosition); // Move down
        }
    }

}
