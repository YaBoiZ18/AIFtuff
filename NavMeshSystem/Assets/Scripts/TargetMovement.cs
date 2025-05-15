using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    public GameObject playingArea;
    public float maxX = 0f;
    public float maxZ = 5f;
    public float scale = 5f;

    private Bounds bounds;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bounds = GetComponent<MeshRenderer>().bounds;
        maxX = bounds.size.x * scale;
        maxZ = bounds.size.z * scale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClick()
    {
        SetRandomPosition();
    }

    void SetRandomPosition()
    {
        float randomX = Random.Range(0, maxX);
        float randomZ = Random.Range(0, maxZ);
        this.transform.position = new Vector3(randomX, this.transform.position.y, randomZ);
    }
}
