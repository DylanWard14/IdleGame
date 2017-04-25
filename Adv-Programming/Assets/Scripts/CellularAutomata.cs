using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomata : MonoBehaviour
{
    [Range (1,100)]
    public int probability;
    public int gridWidth;
    public int gridHeight;
    public int[,] grid;

    public GameObject wall;
	// Use this for initialization
	void Start ()
    {
        grid = new int[gridWidth, gridHeight];

        GenerateGrid();

        SpawnObjects();
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    void GenerateGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Debug.Log("Running");
                int randNumber = Random.Range(0, 100);
                if (randNumber <= probability)
                {
                    grid[x, y] = 0;
                }
                else
                {
                    grid[x, y] = 1;
                }
            }
        }
    }

    void SpawnObjects()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] == 0)
                {
                    Vector3 spawnPos = new Vector3(x, 0, y);
                    Instantiate(wall, spawnPos, Quaternion.identity);
                }
            }
        }
    }
}
