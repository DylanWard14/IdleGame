using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyClass thisEnemy;

    public int maxHealth;
    public int scoreReward;
	// Use this for initialization
	void Start ()
    {
        thisEnemy = new EnemyClass(maxHealth, scoreReward, this.gameObject);
	}
	
	// Update is called once per frame
	void Update ()
    {
        //thisEnemy.OnDeath();
        RunThis(CallBackFunction);
	}

    void CallBackFunction()
    {
        if (thisEnemy.currentHealth <= 0)
        {
            thisEnemy.OnDeath();
            Debug.Log("Die");
        }
    }

    public delegate void CallBack();
    public void RunThis(CallBack callBkFunc)
    {
        //Debug.Log("Running this");
        callBkFunc();
    }
}
