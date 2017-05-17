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
        thisEnemy = new EnemyClass(maxHealth, scoreReward);
        thisEnemy.model = this.gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        thisEnemy.OnDeath();
	}
}
