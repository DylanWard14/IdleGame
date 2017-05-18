using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public int currentHealth;
    public int maxHealth;
    public GameObject model;

    public Character(int _MaxHealth, GameObject _Model)
    {
        maxHealth = _MaxHealth;
        currentHealth = maxHealth;
        model = _Model;
    }
}

public class PlayerClass : Character, ICharacter
{
    public float speed;

    public PlayerClass(int _MaxHealth, float _Speed, GameObject _Model) : base (_MaxHealth, _Model)
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

    public EnemyClass(int _MaxHealth, int _ScoreReward, GameObject _Model): base (_MaxHealth, _Model)
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
