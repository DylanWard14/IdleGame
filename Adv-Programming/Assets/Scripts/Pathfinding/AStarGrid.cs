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
                Vector3 worldPosition = new Vector3(worldBottomLeft.x + x, 0, worldBottomLeft.z + y);
                bool walkable = false;

                if (cellularAutomata.grid[x, y] == 0)
                {
                    walkable = true;
                }

                grid[x, y] = new AStarNode(walkable, worldPosition, x, y);
            }
        }
    }

    public AStarNode WorldToGridPos(Vector3 worldPos)
    {
        float gridPercentX = (worldPos.x + gridWorldSizeX / 2) / gridWorldSizeX;
        float gridPercentY = (worldPos.y + gridWorldSizeY / 2) / gridWorldSizeY;

        gridPercentX = Mathf.Clamp01(gridPercentX);

        int xPosition = Mathf.RoundToInt((gridSizeX - 1) * gridPercentX);
        int yPosition = Mathf.RoundToInt((gridSizeY - 1) * gridPercentY);

        return grid[xPosition, yPosition];
    }

    public List<AStarNode> FindNeighbours(AStarNode node)
    {
        List<AStarNode> neighbours = new List<AStarNode>;

        for (int x = node.gridPositionX - 1; x < node.gridPositionX + 1; x++)
        {
            for (int y = node.gridPositionY - 1; y < node.gridPositionY + 1; y++)
            {
                if (x == node.gridPositionX && y == node.gridPositionY)
                    continue;
                else if (x >= 0 || x <= gridSizeX || y >= 0 || y <= gridSizeY)
                {
                    neighbours.Add(node);
                }
            }
        }

        return neighbours;
    }

}
