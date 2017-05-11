using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarGrid : MonoBehaviour
{
    private int gridSizeX, gridSizeY;
    private float nodeDiameter;

    public float nodeRadius;
    public AStarNode[,] grid;
    public int gridWorldSizeX;
    public int gridWorldSizeY;
    public LayerMask walkableMask;
    public CellularAutomata cellularAutomata;

    public GameObject cube;

	// Use this for initialization
	void Start ()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = (int)(gridWorldSizeX / nodeDiameter);
        gridSizeY = (int)(gridWorldSizeY / nodeDiameter);
        CreateGrid(grid);
    }

    void CreateGrid(AStarNode[,] grid)
    {
        Vector3 worldBottomLeft = new Vector3(-gridSizeX / 2 * nodeDiameter, 0, -gridSizeY / 2 * nodeDiameter);
        Debug.Log(worldBottomLeft);
        grid = new AStarNode[gridSizeX, gridSizeY];

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Vector3 worldPosition = new Vector3(worldBottomLeft.x + x, 0, worldBottomLeft.y + y);
                bool walkable = false;

                if (cellularAutomata.grid[x, y] == 0)
                {
                    walkable = true;
                    Instantiate(cube, worldPosition, Quaternion.identity);
                }

                grid[x, y] = new AStarNode(walkable, worldPosition, x, y);
            }
        }
    }

}
