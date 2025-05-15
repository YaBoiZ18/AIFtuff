using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    public Material Visited;
    private Material original;


    public Dictionary<Direction, Marker> neighbors;
    int row, col;
    public Vector2 rowCol => new Vector2(row, col);

    
    private void Awake()
    {
        GetMarkerRowCol();
    }
    /// <summary>
    /// Save the original Material 
    /// </summary>
    void Start()
    {
        original = this.GetComponent<MeshRenderer>().material;
        GridGameEventBus.Subscribe(MovementEventType.RESTART, ResetMaterial);
    }

    public override string ToString()
    {
        return $"Marker_R{row}_C{col}";
    }
    private void GetMarkerRowCol()
    {
        string suffix =
        this.gameObject.name.Replace("Marker_", "");
        if (int.TryParse(suffix, out col))
        {
            string rowSuffix =  
                gameObject.transform.parent.parent.gameObject.name.Replace("Row_", "");

            if (int.TryParse(rowSuffix, out row))
            {
                Debug.LogFormat("Row:{0}, col:{0}", row, col);
            }
        }
    }

    public void ResetMaterial()
    {
        transform.GetComponent<MeshRenderer>().material = original;
        HasBeenVisited = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool HasBeenVisited;

    private void OnTriggerEnter(Collider other)
    {
        HasBeenVisited = true;
        transform.GetComponent<MeshRenderer>().material = Visited;

        if (other.CompareTag("Player"))
        {
            CubeMover cubeMover = other.GetComponent<CubeMover>();
            if (cubeMover != null)
            {
                string row_name = 
                this.gameObject.transform.parent.parent.gameObject.name;
                Debug.LogFormat("Cube is set to this marker:{0} on Row:{1}", this.gameObject.name, row_name);
                GameManager.Instance.CurrentMarker = this;
                GridGameEventBus.Publish(MovementEventType.NEXT_MOVE);
            }
        }
    }
}
