using System.Collections;
using System.Collections.Generic;
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
        thisPlayer = new PlayerClass(maxHealth, speed);
        thisPlayer.model = this.gameObject;
        movement = GetComponent<AStarMovement>();
        target = GameObject.Find("Target");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (target)
        {
            movement.GetAndFollowPath(target.transform.position);

            if (movement.cantFindPath && target.tag != ("Ladder"))
            {
                Destroy(target);
                movement.cantFindPath = false;
                movement.hasPath = false;
            }
            else if (movement.cantFindPath && target.tag == ("Ladder"))
            {
                Application.LoadLevel(Application.loadedLevel);
            }

            if (movement.atTarget)
            {
                if (target.tag == ("Enemy"))
                {
                    target.GetComponent<Enemy>().thisEnemy.ApplyDamage(1);
                }
                else if (target.tag == ("Ladder"))
                {
                    Application.LoadLevel(Application.loadedLevel);
                }
            }
        }
        else if (!FindNewTarget())
        {
            Debug.Log("Cant find target");
            target = GameObject.FindGameObjectWithTag("Ladder");
        }
    }

    public bool FindNewTarget()
    {
        movement.atTarget = false;
        if (GameObject.FindGameObjectWithTag("Enemy"))
        {
            target = GameObject.FindGameObjectWithTag("Enemy");
            return true;
        }
        else
            return false;

    }
}
