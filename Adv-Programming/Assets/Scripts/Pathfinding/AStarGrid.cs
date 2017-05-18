using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarGrid : MonoBehaviour
{
    private int gridSizeX, gridSizeY; // the size of the grid
    private float nodeDiameter; // the overal size of each nide

    public float nodeRadius; // the radius of each node
    public AStarNode[,] grid; // a 2d array of nodes
    public int gridWorldSizeX; // the size of the world on the x
    public int gridWorldSizeY; // the size of the world on the y
    public LayerMask unwalkableMask; // reference to the mask we want to check for collisions
    public CellularAutomata cellularAutomata; // reference to the cellular automata

    public GameObject cube;

	// Use this for initialization
	void Start ()
    {
        nodeDiameter = nodeRadius * 2; // sets the diameter
        gridSizeX = (int)(gridWorldSizeX / nodeDiameter); // sets the size of the grid on the x
        gridSizeY = (int)(gridWorldSizeY / nodeDiameter); // sets the size of the grid on the y

        CreateGrid(); // calls the create grid function
    }

    void CreateGrid()
    {
        Vector3 worldBottomLeft = new Vector3(-gridSizeX / 2 * nodeDiameter, 0, -gridSizeY / 2 * nodeDiameter); // gets the bottom left of the world
        grid = new AStarNode[gridSizeX, gridSizeY]; // instatate a the grid with the size of grid x and y

        for (int x = 0; x < grid.GetLength(0); x++) // loop through the grid x
        {
            for (int y = 0; y < grid.GetLength(1); y++) // loop through the grid y
            {
                Vector3 worldPosition = new Vector3(worldBottomLeft.x + x, 0, worldBottomLeft.z + y); // using the bottom left position of the world we can easily calculate the world position of this node

                //bool walkable = false; // reads if this location is walkable based on the cellular automata value
                //if (cellularAutomata.grid[x, y] == 0) // doesnt work properly has it sometimes allows the player to walk through walls.
                //{
                  //  walkable = true;
                //}
                bool walkable = !(Physics.CheckBox(worldPosition, new Vector3(nodeRadius,nodeRadius,nodeRadius), Quaternion.identity, unwalkableMask)); // sets walkable to true if there is no collision at this position

                grid[x, y] = new AStarNode(walkable, worldPosition, x, y); // creates a new node with the following properties at grid position x and y
            }
        }
    }

    /// <summary>
    /// Gets a grid position from the world position of a node
    /// </summary>
    /// <param name="worldPos"> the world position to convert</param>
    /// <returns></returns>
    public AStarNode WorldToGridPos(Vector3 worldPos)
    {
        float gridPercentX = (worldPos.x + gridWorldSizeX / 2) / gridWorldSizeX; // calculates how far along the x the position is
        float gridPercentY = (worldPos.z + gridWorldSizeY / 2) / gridWorldSizeY; // calculates how far along the y the position is

        gridPercentX = Mathf.Clamp01(gridPercentX); // clamps the position between 0 and 1
        gridPercentY = Mathf.Clamp01(gridPercentY);

        int xPosition = Mathf.RoundToInt((gridSizeX - 1) * gridPercentX); // calculates the grid position on the x and y
        int yPosition = Mathf.RoundToInt((gridSizeY - 1) * gridPercentY);

        return grid[xPosition, yPosition]; // returns the node at this position
    }


    /// <summary>
    /// Finds the neighbours of the input node
    /// </summary>
    /// <param name="node"> the node to find the neighbours of</param>
    /// <returns></returns>
    public List<AStarNode> FindNeighbours(AStarNode node)
    {
        List<AStarNode> neighbours = new List<AStarNode>(); // creates a new list of nodes

        for (int x = node.gridPositionX - 1; x <= node.gridPositionX + 1; x++) // loops through all the neighbours on the x
        {
            for (int y = node.gridPositionY - 1; y <= node.gridPositionY + 1; y++) // loops through all the neighbours on the y
            {
                if (x == node.gridPositionX && y == node.gridPositionY) // if x and y == the input nodes position
                {
                    continue; // skip
                }

                if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY) // else check if we are inside the array
                {
                    neighbours.Add(grid[x,y]); // then add the neighbour at grid position x and y
                }
            }
        }

        return neighbours; // return the neighbours list
    }
}
