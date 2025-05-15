using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    // The prefab to spawn as platforms.
    public GameObject platformPrefab;

    // Width and height of the grid.
    public int width = 10;
    public int height = 10;

    // Distance between each platform along the X and Z axes.
    public float spacing = 2.0f;

    // Chance (0 to 1) for a platform to spawn at each grid cell.
    [Range(0f, 1f)]
    public float spawnChance = 0.7f;

    void Start()
    {
        GenerateGrid();  // Begin generating the platforms.
    }

    // Generates a grid of platforms based on the set width, height, spacing, and spawn chance.
    void GenerateGrid()
    {
        // Loop through each X and Z coordinate in the grid.
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                // Randomly decide whether to spawn a platform at this position.
                if (Random.value < spawnChance)
                {
                    // Calculate world position based on grid coordinates and spacing.
                    Vector3 position = new Vector3(x * spacing, 0, z * spacing);

                    // Instantiate (spawn) the platform prefab at the calculated position.
                    Instantiate(platformPrefab, position, Quaternion.identity);
                }
            }
        }
    }
}
