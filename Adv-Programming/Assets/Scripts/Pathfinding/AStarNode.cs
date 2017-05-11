using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    public Vector3 worldPosition;
    public bool walkable;
    public int gridPositionX;
    public int gridPositionY;
    public int gCost;
    public int hCost;

    public int fCost
    {
        get { return gCost + hCost; }
    }

    public AStarNode parentNode;


    public AStarNode(bool _Walkable, Vector3 _worldPosition, int _x, int _y)
    {
        walkable = _Walkable;
        worldPosition = _worldPosition;
        gridPositionX = _x;
        gridPositionY = _y;
    }
}
