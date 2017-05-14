using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathing : MonoBehaviour
{
    AStarGrid grid;

    public Transform seeker, target;

    public List<AStarNode> path = new List<AStarNode>();


    private void Awake()
    {
        grid = GetComponent<AStarGrid>();
    }

    private void Update()
    {
        //FindPath(seeker.position, target.position);
    }

    public void FindPath(Vector3 startPos, Vector3 endPos)
    {
        AStarNode startNode = grid.WorldToGridPos(startPos);
        AStarNode endNode = grid.WorldToGridPos(endPos);

        List<AStarNode> openSet = new List<AStarNode>();
        HashSet<AStarNode> closedSet = new HashSet<AStarNode>();

        openSet.Add(startNode);

        while (openSet.Count > 0)//
        {
            AStarNode currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++) //
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost) 
                {
                    if (openSet[i].hCost < currentNode.hCost)//
                    {
                        currentNode = openSet[i]; 
                    }
                }
            }

            openSet.Remove(currentNode);

            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                //retrace path
                RetracePath(startNode, endNode);
                return;
            }

            foreach (AStarNode neighbour in grid.FindNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parentNode = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    void RetracePath(AStarNode start, AStarNode end)
    {
        path.Clear();
        path = new List<AStarNode>();
        AStarNode currentNode = end;

        while (currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        path.Reverse();
        grid.path = path;
    }

    public int GetDistance(AStarNode nodeA, AStarNode nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridPositionX - nodeB.gridPositionX);
        int distanceY = Mathf.Abs(nodeA.gridPositionY - nodeB.gridPositionY);

        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        else
        {
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }
    }
}
