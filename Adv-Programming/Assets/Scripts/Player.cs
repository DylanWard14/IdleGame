using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth; // the max health of the player
    public float speed; // the speed of the player
    public GameObject target; // this will be the target the player moves towards

    private PlayerClass thisPlayer;
    private AStarMovement movement; // reference to the movement class

	// Use this for initialization
	void Start ()
    {
        thisPlayer = new PlayerClass(maxHealth, speed, this.gameObject); // creates a new player object for this player
        movement = GetComponent<AStarMovement>(); // finds the movement class
        target = GameObject.Find("Target"); // finds the target
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (target)
        {
            Predicate<GameObject> p = CheckTargetTagEnemy; // using a predicate to test if the value is true
            bool result = p(target); // gets the true/false result from the predicate

            if (movement.atTarget) // if we are at the target
            {
                if (!p(target)) // if the target is not an enemy
                {
                    Debug.Log("Loading New level");
                    Application.LoadLevel("FloorCompleted"); // load a new scene becuase we are at the ladder
                }
                else if (p(target)) // if we are at an enemy
                {
                    target.GetComponent<Enemy>().thisEnemy.ApplyDamage(10); // damage the enemy
                    movement.atTarget = false; // sets the at target value to false incase we have killed the enemy
                }

            }

            movement.GetAndFollowPath(target.transform.position); // gets the path and makes the player follow it

            if (movement.cantFindPath && target.tag != ("Ladder")) // if there is no path and we arnt trying to go to the ladder
            {
                Destroy(target); // the target is unreachable so destory it
                movement.cantFindPath = false; // reset these values
                movement.hasPath = false;
            }
        }
        else if (!FindNewTarget()) // if we cant find a new enemy to move to
        {
            Debug.Log("Cant find target");
            movement.atTarget = false; // reset this value to stop it from reloading scene straight away
            target = GameObject.FindGameObjectWithTag("Ladder"); // set the target to the ladder
        }

        if (movement.cantFindPath && target.tag == ("Ladder")) // if we cant find a path and the target is the ladder
        {
            Application.LoadLevel("FloorCompleted"); // just reload the scene because we have cleared the level
        }
    }
    /// <summary>
    /// returns true if it has found an enemy and false if it can not
    /// </summary>
    /// <returns></returns>
    public bool FindNewTarget()
    {
        movement.atTarget = false;
        if (GameObject.FindGameObjectWithTag("Enemy"))
        {
            return target = GameObject.FindGameObjectWithTag("Enemy"); // returns true if found the enemy
            //return true;
        }
        else
            return false; // returns false if not

    }

    /// <summary>
    /// Checks if this object has the enemy tag
    /// </summary>
    /// <param name="obj"> the object to check </param>
    /// <returns></returns>
    private static bool CheckTargetTagEnemy(GameObject obj)
    {
        return obj.CompareTag("Enemy");
    }
}
