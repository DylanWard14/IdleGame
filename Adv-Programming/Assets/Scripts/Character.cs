using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public int currentHealth;
    public int maxHealth;

    public Character(int _MaxHealth)
    {
        maxHealth = _MaxHealth;
        currentHealth = maxHealth;
    }
}

public class PlayerClass : Character, ICharacter
{
    public float speed;
    public GameObject model;

    public PlayerClass(int _MaxHealth, float _Speed) : base (_MaxHealth)
    {
        speed = _Speed;
    }

    public void OnDeath()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Player Dead");
        }
    }

    public void ApplyDamage(int damage)
    {
        currentHealth -= damage;
    }
}

public class EnemyClass : Character, ICharacter
{
    public int ScoreReward;
    public GameObject model;

    public EnemyClass(int _MaxHealth, int _ScoreReward): base (_MaxHealth)
    {
        ScoreReward = _ScoreReward;
    }

    public void OnDeath()
    {
        if (currentHealth <= 0)
        {
            GameObject.Find("GameController").GetComponent<GameController>().score += ScoreReward;
            MonoBehaviour.Destroy(model);
        }
    }

    public void ApplyDamage(int damage)
    {
        currentHealth -= damage;
    }
}

public interface ICharacter
{
    void OnDeath();
    void ApplyDamage(int damage);
}
