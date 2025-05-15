using UnityEngine;
using UnityEngine.AI;

public class SimpleAi : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent thisAgent;

    public float threshold = 0.5f;

    //private Vector3 pervTargetPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisAgent = GetComponent<NavMeshAgent>();
        //pervTargetPosition = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        float agentDistance = Vector3.Distance(this.transform.position, target.position);

        if(agentDistance >= threshold)
        {
            thisAgent.SetDestination(target.position);
        }

        
        //if (Input.GetKeyDown(KeyCode.Space)) 
        //{
        //    this.transform.position = new Vector3(0, 0, 0);
        //}
    }
}
