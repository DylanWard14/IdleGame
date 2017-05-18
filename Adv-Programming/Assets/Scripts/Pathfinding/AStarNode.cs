using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    public Vector3 worldPosition; // will hold the world position of this node
    public bool walkable; // will hold if this node is walkable
    public int gridPositionX; // this nodes grid position on the x
    public int gridPositionY; // this nodes gird position on the y
    public int gCost; // this nodes g cost, the distance from the start node
    public int hCost; // this nodes h cost, the distance from the end node

    public int fCost // this will return gcost + hcost
    {
        get { return gCost + hCost; }
    }

    public AStarNode parentNode; // this nodes parent, this is used for the path


    public AStarNode(bool _Walkable, Vector3 _worldPosition, int _x, int _y) // the nodes constructor
    {
        walkable = _Walkable; // sets the walkable varaible
        worldPosition = _worldPosition; // sets the world position
        gridPositionX = _x; // sets the grid positions
        gridPositionY = _y;
    }
}
