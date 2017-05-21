using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyClass thisEnemy;

    public int maxHealth; // the max health of this enemy
    public int scoreReward; // the score rewared for kill the enemy
	// Use this for initialization
	void Start ()
    {
        thisEnemy = new EnemyClass(maxHealth, scoreReward, this.gameObject); // creates a new enemy object for this enemy
	}
	
	// Update is called once per frame
	void Update ()
    {
        RunThis(CallBackFunction); // checks the callback function
	}

    void CallBackFunction()
    {
        if (thisEnemy.currentHealth <= 0) // only run this if the enemies health if below 0
        {
            thisEnemy.OnDeath(); // call this function in the enemy class
            Debug.Log("Die");
        }
    }

    public delegate void CallBack();
    public void RunThis(CallBack FunctionToCallback)
    {
        FunctionToCallback();
    }
}
