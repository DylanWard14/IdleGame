using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Range(1, 100)] // keeps the range between 1 and 100
    public int probabilityToSpawnEnemy; // this will be the probability of spawning an enemy on a particular tile
    public int score; // will contain the score
    public GameObject enemy, ladder, player; // gameobject prefabs for the player, enemy and ladder


    private CellularAutomata cellularAutomata; // allows me to access the CellularAutomata functions and variables
    private AStarGrid aStarGrid; // allows me to access the AStarGrid functions and variables

    // Use this for initialization
    void Start ()
    {
        bool laddarSpawned = false; // bool will control if a ladder has already been spawned
        cellularAutomata = GameObject.Find("MeshGenerator").GetComponent<CellularAutomata>(); // finds the cellular automata script in the scene
        aStarGrid = GameObject.Find("A*").GetComponent<AStarGrid>(); // finds the AStarGrid

        for (int x = 0; x < cellularAutomata.gridWidth; x++) // loops through the grids x
        {
            for (int y = 0; y < cellularAutomata.gridHeight; y++) // loops through the grids y
            {
                if (x != 0 && x != cellularAutomata.grid.GetLength(0) - 2 && y != 0 && y != cellularAutomata.grid.GetLength(1) - 2) // if we are within the bounds the array
                {
                    if (cellularAutomata.GetAmountOfNeighbours(x, y, 2) == 0 && aStarGrid.grid[x, y].walkable) // using the overloaded methoed of get neighbours to have a larger search area to stop things from spawning on walls and checks if the tile is walkable in the AStarGrid
                    { //
                        if (GetRandomNumber() <= probabilityToSpawnEnemy) // generate a random number and if it is within probabilty
                            Instantiate(enemy, aStarGrid.grid[x, y].worldPosition, Quaternion.identity); // spawn an enemy
                        else if (!laddarSpawned)
                            laddarSpawned = Instantiate(ladder, aStarGrid.grid[x, y].worldPosition, Quaternion.identity); // if no spawn a ladder and set the ladderspawned varaible to true
                    }
                }
            }
        }

        Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    int GetRandomNumber()
    {
        return Random.Range(0, 100); // get a random number between 0 and 100

    }
    /// <summary>
    /// adds score to the current score
    /// </summary>
    /// <param name="scoreToAdd"> this score will be added to the score </param>
    public void AddScore(int scoreToAdd) // use this function somewhere
    {
        score += scoreToAdd;
    }
    /// <summary>
    /// overload function
    /// allows you to add score with a multiplier
    /// </summary>
    /// <param name="scoreToAdd"> this will be the score to add</param>
    /// <param name="multiplier"> this multiplier will multiply the score being added</param>
    public void AddScore(int scoreToAdd, int multiplier)
    {
        score += (scoreToAdd * multiplier);
    }

    public int GetCurrentScore() // gets and returns the current score
    {
        return score;
    }
}
