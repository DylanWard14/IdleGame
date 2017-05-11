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
    public LayerMask unwalkableMask;
    public CellularAutomata cellularAutomata;

    public GameObject cube;

	// Use this for initialization
	void Start ()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = (int)(gridWorldSizeX / nodeDiameter);
        gridSizeY = (int)(gridWorldSizeY / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        Vector3 worldBottomLeft = new Vector3(-gridSizeX / 2 * nodeDiameter, 0, -gridSizeY / 2 * nodeDiameter);
        Debug.Log(worldBottomLeft);
        grid = new AStarNode[gridSizeX, gridSizeY];

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Vector3 worldPosition = new Vector3(worldBottomLeft.x + x, 0, worldBottomLeft.z + y);

                /*bool walkable = false;
                if (cellularAutomata.grid[x, y] == 0)
                {
                    walkable = true;
                }*/
                bool walkable = !(Physics.CheckSphere(worldPosition, nodeRadius, unwalkableMask));

                grid[x, y] = new AStarNode(walkable, worldPosition, x, y);
            }
        }
    }

    public AStarNode WorldToGridPos(Vector3 worldPos)
    {
        float gridPercentX = (worldPos.x + gridWorldSizeX / 2) / gridWorldSizeX;
        float gridPercentY = (worldPos.z + gridWorldSizeY / 2) / gridWorldSizeY;

        gridPercentX = Mathf.Clamp01(gridPercentX);
        gridPercentY = Mathf.Clamp01(gridPercentY);

        int xPosition = Mathf.RoundToInt((gridSizeX - 1) * gridPercentX);
        int yPosition = Mathf.RoundToInt((gridSizeY - 1) * gridPercentY);

        return grid[xPosition, yPosition];
    }

    public List<AStarNode> FindNeighbours(AStarNode node)
    {
        List<AStarNode> neighbours = new List<AStarNode>();

        for (int x = node.gridPositionX - 1; x < node.gridPositionX + 1; x++) // this doesnt work, adds the same node each time.
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

        /*for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridPositionX + x;
                int checkY = node.gridPositionY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }*/

        return neighbours;
    }

    public List<AStarNode> path;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSizeX, 1, gridWorldSizeY));

        if (grid != null)
        {
            Debug.Log("Grid Not null");
            foreach (AStarNode n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (path != null)
                {
                   // Debug.Log("Path Not null");
                    if (path.Contains(n))
                    {
                        Gizmos.color = Color.black;
                    }
                }
                else
                {
                    //Debug.Log("Path null");
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }

        else
        {
            Debug.Log("Grid Null");
        }
    }

}
