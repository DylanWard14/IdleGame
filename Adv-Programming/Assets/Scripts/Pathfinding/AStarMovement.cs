using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarMovement : MonoBehaviour
{

    public bool hasPath;
    public bool atTarget = false;
    public bool cantFindPath = false;
    private int pathIndex = 0;

    public List<AStarNode> myPath = new List<AStarNode>();
    private Pathing pathing;
    // Use this for initialization
    void Start ()
    {
        pathing = GameObject.Find("A*").GetComponent<Pathing>();
        hasPath = false;
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    public void GetAndFollowPath(Vector3 TargetPos)
    {
        cantFindPath = false;
        if (pathing.thereIsNoPath == false)
        {
            if (!hasPath)
            {
                if (myPath != null)
                {
                    myPath.Clear();
                }
                myPath = pathing.FindPath(this.transform.position, TargetPos);
                hasPath = true;
                pathIndex = 0;
            }

            if (hasPath && myPath != null)
            {
                if (pathIndex < myPath.Count - 1)
                {
                    atTarget = false;
                    this.transform.position = Vector3.MoveTowards(this.transform.position, myPath[pathIndex].worldPosition, 0.4f);
                    transform.LookAt(myPath[pathIndex].worldPosition);
                    if (this.transform.position == myPath[pathIndex].worldPosition)
                    {
                        pathIndex++;
                    }
                }
                else
                {
                    atTarget = true;
                    hasPath = false;
                }
            }
        }
        if (pathing.thereIsNoPath)
        {
            cantFindPath = true;
        }
        pathing.thereIsNoPath = false;
    }
}
