using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth;
    public float speed;
    public GameObject target;

    private PlayerClass thisPlayer;
    private AStarMovement movement;

	// Use this for initialization
	void Start ()
    {
        thisPlayer = new PlayerClass(maxHealth, speed, this.gameObject);
        movement = GetComponent<AStarMovement>();
        target = GameObject.Find("Target");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (target)
        {
            Predicate<GameObject> p = CheckTargetTagEnemy; // using a predicate to test if the value is true
            bool result = p(target);

            if (movement.atTarget)
            {
                if (!p(target))
                {
                    Debug.Log("Loading New level");
                    Application.LoadLevel("FloorCompleted");
                }
                else if (p(target))
                {
                    target.GetComponent<Enemy>().thisEnemy.ApplyDamage(10);
                    movement.atTarget = false;
                }

            }

            movement.GetAndFollowPath(target.transform.position);

            if (movement.cantFindPath && target.tag != ("Ladder"))
            {
                Destroy(target);
                movement.cantFindPath = false;
                movement.hasPath = false;
            }
        }
        else if (!FindNewTarget())
        {
            Debug.Log("Cant find target");
            movement.atTarget = false;
            target = GameObject.FindGameObjectWithTag("Ladder");
        }

        if (movement.cantFindPath && target.tag == ("Ladder"))
        {
            Application.LoadLevel("FloorCompleted");
        }
    }

    public bool FindNewTarget()
    {
        movement.atTarget = false;
        if (GameObject.FindGameObjectWithTag("Enemy"))
        {
            return target = GameObject.FindGameObjectWithTag("Enemy");
            //return true;
        }
        else
            return false;

    }

    private static bool CheckTargetTagEnemy(GameObject obj)
    {
        return obj.CompareTag("Enemy");
    }
}
