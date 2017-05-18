using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarMovement : MonoBehaviour
{

    public bool hasPath; // will be true if a path has be calculated
    public bool atTarget = false; // true if we are at the target 
    public bool cantFindPath = false; // true if we cant find the path
    private int pathIndex = 0; // this will iterate through the path indexs

    public List<AStarNode> myPath = new List<AStarNode>(); // creates a new list of a star nodes
    private Pathing pathing; // reference to the pathing class
    // Use this for initialization
    void Start ()
    {
        pathing = GameObject.Find("A*").GetComponent<Pathing>(); // finds the pathing class
        hasPath = false; // we dont have a path at the start
	}

    /// <summary>
    /// allows the player to get and follow a path
    /// </summary>
    /// <param name="TargetPos"> this will be the point the player moves to</param>
    public void GetAndFollowPath(Vector3 TargetPos)
    {
        cantFindPath = false;
        if (pathing.thereIsNoPath == false)
        {
            if (!hasPath)
            {
                if (myPath != null)
                {
                    myPath.Clear(); // clears the path
                }
                myPath = pathing.FindPath(this.transform.position, TargetPos); // set the path to the result of this function
                hasPath = true; // we have a path
                pathIndex = 0; // reset the path index
            }

            if (hasPath && myPath != null) // if we have a path
            {
                if (pathIndex < myPath.Count - 1) // if we are not at the end of the path
                {
                    atTarget = false;
                    this.transform.position = Vector3.MoveTowards(this.transform.position, myPath[pathIndex].worldPosition, 0.4f); // move the player towards the next point in the path
                    transform.LookAt(myPath[pathIndex].worldPosition); // set the player to look at the next position in the path
                    if (this.transform.position == myPath[pathIndex].worldPosition) // if we get to the next position
                    {
                        pathIndex++; // increment the path index
                    }
                }
                else // else
                {
                    atTarget = true; // we are ath the target and we no longer have a path
                    hasPath = false;
                }
            }
        }

        cantFindPath = pathing.thereIsNoPath; // if there is no path than set the cant find path variable to true
        pathing.thereIsNoPath = false;
    }
}
