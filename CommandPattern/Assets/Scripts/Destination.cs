using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GridGameEventBus.Publish(MovementEventType.ARRIVED_AT_DESTINATION);
        }
    }

}
