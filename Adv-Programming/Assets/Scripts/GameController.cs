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
        cellularAutomata = GameObject.Find("MeshGenerator").GetComponent<CellularAutomata>();
        aStarGrid = GameObject.Find("A*").GetComponent<AStarGrid>();

        for (int x = 0; x < cellularAutomata.gridWidth; x++)
        {
            for (int y = 0; y < cellularAutomata.gridHeight; y++)
            {
                if (x != 0 && x != cellularAutomata.grid.GetLength(0) - 1 && y != 0 && y != cellularAutomata.grid.GetLength(1) - 1)
                {
                    if (cellularAutomata.GetAmountOfNeighbours(x, y) == 0 && aStarGrid.grid[x, y].walkable && GetRandomNumber() <= probabilityToSpawnEnemy) // geting the neighbour count here stoped the enemies from spawning on the edges
                    { //
                        Instantiate(enemy, aStarGrid.grid[x, y].worldPosition, Quaternion.identity);
                    }
                }
            }
        }

        Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(ladder, new Vector3(4, 0, 0), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    int GetRandomNumber()
    {
        return Random.Range(0, 100);

    }
}
