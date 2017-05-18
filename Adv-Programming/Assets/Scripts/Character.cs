using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public int currentHealth; // this will hold this characters current health;
    public int maxHealth; // this will holder the character max health
    public GameObject model; // this will hold the characters model

    public Character(int _MaxHealth, GameObject _Model) // the character constructor
    {
        maxHealth = _MaxHealth; // sets this characters max health
        currentHealth = maxHealth; // makes the current health equal to the max health
        model = _Model; // sets the characters model
    }
}
/// <summary>
/// The player class inherits from the character class and the ICharacter interface
/// </summary>
public class PlayerClass : Character, ICharacter
{
    public float speed; // this will hold the players movement speed

    public PlayerClass(int _MaxHealth, float _Speed, GameObject _Model) : base (_MaxHealth, _Model) // sets the max health and model using the base constructor
    {
        speed = _Speed; // sets the player movement speed to the inputted speed
    }
    /// <summary>
    /// sets what will happen when the player dies
    /// </summary>
    public void OnDeath() // the player must implement this function as it is part of the ICharacter Interface
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Player Dead");
        }
    }
    /// <summary>
    /// deals damage to the player
    /// </summary>
    /// <param name="damage"> the amount of damage the player will take </param>
    public void ApplyDamage(int damage) // the player must implement this function as it is part of the ICharacter Interface
    {
        currentHealth -= damage;
    }
}

/// <summary>
/// The Enemy class inherits from the character class and the ICharacter interface
/// </summary>
public class EnemyClass : Character, ICharacter
{
    public int ScoreReward; // the score that will be rewarded to the player on the enemies death

    public EnemyClass(int _MaxHealth, int _ScoreReward, GameObject _Model): base (_MaxHealth, _Model) // the max health and model will be set using the base constructor
    {
        ScoreReward = _ScoreReward;
    }

    /// <summary>
    /// destroys the enemy when his heath is a 0 and adds the score reward to the players score
    /// </summary>
    public void OnDeath() // this function must be implmented as it is part of the ICharacter Interface
    {
        if (currentHealth <= 0)
        {
            GameObject.Find("GameController").GetComponent<GameController>().score += ScoreReward;
            MonoBehaviour.Destroy(model);
        }
    }

    /// <summary>
    /// Damages the enemy
    /// </summary>
    /// <param name="damage">The amount of damage to deal</param>
    public void ApplyDamage(int damage) // this function must be implmented as it is part of the ICharacter Interface
    {
        currentHealth -= damage;
    }
}

/// <summary>
/// The functions listed here must be implemented by all characters
/// </summary>
public interface ICharacter
{
    void OnDeath();
    void ApplyDamage(int damage);
}
