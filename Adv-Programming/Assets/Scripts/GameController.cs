using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Range(1, 100)]
    public int probabilityToSpawnEnemy;
    public int score;
    public GameObject enemy, ladder, player;


    private CellularAutomata cellularAutomata;
    private AStarGrid aStarGrid;

    // Use this for initialization
    void Start ()
    {
        bool laddarSpawned = false;
        cellularAutomata = GameObject.Find("MeshGenerator").GetComponent<CellularAutomata>();
        aStarGrid = GameObject.Find("A*").GetComponent<AStarGrid>();

        for (int x = 0; x < cellularAutomata.gridWidth; x++)
        {
            for (int y = 0; y < cellularAutomata.gridHeight; y++)
            {
                if (x != 0 && x != cellularAutomata.grid.GetLength(0) - 2 && y != 0 && y != cellularAutomata.grid.GetLength(1) - 2)
                {
                    if (cellularAutomata.GetAmountOfNeighbours(x, y, 2) == 0 && aStarGrid.grid[x, y].walkable) // geting the neighbour count here stoped the enemies from spawning on the edges
                    { //
                        if (GetRandomNumber() <= probabilityToSpawnEnemy)
                        {
                            Instantiate(enemy, aStarGrid.grid[x, y].worldPosition, Quaternion.identity);
                        }
                        else if (!laddarSpawned)
                        {
                            Instantiate(ladder, aStarGrid.grid[x, y].worldPosition, Quaternion.identity);
                            laddarSpawned = true;
                        }
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
        return Random.Range(0, 100);

    }

    public void AddScore(int scoreToAdd) // use this function somewhere
    {
        score += scoreToAdd;
    }

    public void AddScore(int scoreToAdd, int multiplier)
    {
        score += (scoreToAdd * multiplier);
    }
}
