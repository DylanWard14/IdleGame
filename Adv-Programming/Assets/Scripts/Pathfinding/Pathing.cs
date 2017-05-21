using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 1. add the start node to the open set and make it the current node
 * 2. loop through the open set
 * 3. if the node we are looking at in the open set now has a lower cost than the current node then add it to the open set
 * 4. remove the current node from the open set and add it to the closed set
 * 5. if we are at the target node then we have found the path so retrace the path back to the start  going through all the parents
 * 6. else
 * 7. check all the neighbours of the current node to see if there is a cheaper option
 * 8. if there is a cheaper option add it to the open set and make the current node the parent of that neighbour
 * 9. if we found a path then return the path
 */

public class Pathing : MonoBehaviour
{
    AStarGrid grid; // reference to the a star grid 

    public List<AStarNode> path = new List<AStarNode>(); // creates a new list of a star nodes for the path
    public bool thereIsNoPath = false; // this bool will be true when there is no path

    private void Awake()
    {
        grid = GetComponent<AStarGrid>(); // gets the a star grid
    }

    /// <summary>
    /// finds the shortest path to the end position
    /// </summary>
    /// <param name="startPos"> the starting position of the path</param>
    /// <param name="endPos"> the end position of the path</param>
    /// <returns></returns>
    public List<AStarNode> FindPath(Vector3 startPos, Vector3 endPos)
    {
        AStarNode startNode = grid.WorldToGridPos(startPos); // turns the start world position in the a grid position
        AStarNode endNode = grid.WorldToGridPos(endPos); // does the same for the end position

        List<AStarNode> openSet = new List<AStarNode>(); // creates a list of nodes for the open set, I am using a list so that i can remove and add items at will
        HashSet<AStarNode> closedSet = new HashSet<AStarNode>(); // creates a hashset for the closed set, I am using a closed set here becuase they are optimised for searching
        bool foundPath = false; // we currently do not have a path
        thereIsNoPath = false;
        openSet.Add(startNode); // add the start node to the open set

        while (openSet.Count > 0) // while the open set is not empty
        {
            AStarNode currentNode = openSet[0]; // set the current node to be the first node in the open set
            for (int i = 1; i < openSet.Count; i++) // loop through the items in the open set, starting at 1 becuase we used 0 to set the current node
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost) // if the node at open set [i] is cheaper
                {
                    if (openSet[i].hCost < currentNode.hCost)// or they have the same f cost and open set i's h cost is cheaper
                    {
                        currentNode = openSet[i]; // set the current node to equal open set i
                    }
                }
            }

            openSet.Remove(currentNode); // remove the current node from the open set

            closedSet.Add(currentNode); // add the current node to the closed set

            if (currentNode == endNode) // if the current node is the end node
            {
                foundPath = true; // we have found a path
                thereIsNoPath = false; // there is a path so set this to false
            }

            foreach (AStarNode neighbour in grid.FindNeighbours(currentNode)) // search through all the neighbours of the current node
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) // if the current neighbour is not walkable or is in the closed set
                {
                    continue; // skip it
                }

                int newMovementCost = currentNode.gCost + GetDistance(currentNode, neighbour); // calculate the new movement cost to the neighbour

                if (newMovementCost < neighbour.gCost || !openSet.Contains(neighbour)) // if the new movement cost is less then the neighbours g cost or the open set does not contain the neighbour
                {
                    neighbour.gCost = newMovementCost; // se the neighbours g cost to the new movement cost
                    neighbour.hCost = GetDistance(neighbour, endNode); // set the neighbours hcost to the distance between the neighbour and the end node
                    neighbour.parentNode = currentNode; // make the neighbours parent the current node

                    if (!openSet.Contains(neighbour)) // if the neighbour is not in the open set
                    {
                        openSet.Add(neighbour); // add it
                    }
                }
            }
        }

        if (foundPath) // if we found a path
        {
            return RetracePath(startNode, endNode); // return the path from the start node to the end node
        }
        else // if we did not find a path
        {
            Debug.Log("no path");
            thereIsNoPath = true; // there is no path becomes true
            return null; // return null
        }
    }

    /// <summary>
    /// retraces the path between the start and end nodes
    /// </summary>
    /// <param name="start"> the start of the path</param>
    /// <param name="end"> the end of the path</param>
    /// <returns></returns>
    List<AStarNode> RetracePath(AStarNode start, AStarNode end)
    {
        path.Clear(); // clears the path
        path = new List<AStarNode>();
        AStarNode currentNode = end; // start at the end node

        while (currentNode != start) // while the current node is not the start node
        {
            path.Add(currentNode); // add the current node to the path
            currentNode = currentNode.parentNode; // go to the current nodes parent
        }

        path.Reverse(); //reverse the path
        return path; // return it
    }

    /// <summary>
    /// Gets the distance between two points
    /// </summary>
    /// <param name="nodeA"></param>
    /// <param name="nodeB"></param>
    /// <returns></returns>
    public int GetDistance(AStarNode nodeA, AStarNode nodeB)
    {
        // using mathf.abs to return an unsigned value
        int distanceX = Mathf.Abs(nodeA.gridPositionX - nodeB.gridPositionX); // calculate the distance on the x
        int distanceY = Mathf.Abs(nodeA.gridPositionY - nodeB.gridPositionY); // calculate the distance on the y

        if (distanceX > distanceY) // if the x distance is greater
        { // 14 represents the cost of a diagonal movement and 10 represent the cost of a straight movement
            return 14 * distanceY + 10 * (distanceX - distanceY); // calculate the distance
        }
        else // if the y distance is great
        {
            return 14 * distanceX + 10 * (distanceY - distanceX); // calculate the distance
        }
    }
}
