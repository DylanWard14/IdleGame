using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarMovement : MonoBehaviour
{
    public Pathing pathing;
    public Transform enemy;

    public bool hasPath;
    public int pathIndex = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Fire1") && !hasPath)
        {
            pathing.FindPath(this.transform.position, enemy.position);
            hasPath = true;
            pathIndex = 0;
        }

        if (hasPath)
        {
            if (pathIndex < pathing.path.Count)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, pathing.path[pathIndex].worldPosition, 0.1f);
                if (this.transform.position == pathing.path[pathIndex].worldPosition)
                {
                    pathIndex++;
                }
            }
            else
            {
                hasPath = false;
            }
        }
    }
}
