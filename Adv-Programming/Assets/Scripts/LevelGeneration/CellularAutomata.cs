using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 1. create a 2d array of ints at the size that is inputed
 * 2. loop through 2d array randomly adding walls
 * 3. smooth out the grid by looping through the array several times changing if this tile is wall or floor tile depending on how many neighbours there are
 */

public class CellularAutomata : MonoBehaviour
{
    [Range (1,100)]
    public int probability;
    public int gridWidth;
    public int gridHeight;
    public int[,] grid;

	// Use this for initialization
	void Start ()
    {
        grid = new int[gridWidth, gridHeight];

        GenerateGrid();

        for (int i = 0; i < 4; i++)
        {
            SmoothGrid();
        }

        //SpawnObjects();
        StartMarchingSquares();
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
                int randNumber = Random.Range(0, 100);
                if (randNumber <= probability && x != 0 && x != grid.GetLength(0) - 1 && y != 0 && y != grid.GetLength(1) - 1)
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

    void SmoothGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (x != 0 && x != grid.GetLength(0) -1 && y != 0 && y != grid.GetLength(1) -1)
                {
                    if (GetAmountOfNeighbours(x, y) > 4)
                    {
                        grid[x, y] = 1;
                    }
                    else if (GetAmountOfNeighbours(x, y) < 4)
                    {
                        grid[x, y] = 0;
                    } 
                }
            }
        }
    }

    public int GetAmountOfNeighbours(int gridX, int gridY) // searchings in a 3 by 3 grid around the target location
    {
        int neighbours = 0;

        for (int x = gridX - 1; x <= gridX + 1; x++)
        {
            for (int y = gridY - 1; y <= gridY + 1; y++)
            {
                if (grid[x, y] == 1)
                {
                    neighbours++;
                }
            }
        }

        return neighbours;
    }

    public int GetAmountOfNeighbours(int gridX, int gridY, int searchArea) // overload method that can have a larger search area
    {
        int neighbours = 0;

        for (int x = gridX - searchArea; x <= gridX + searchArea; x++)
        {
            for (int y = gridY - searchArea; y <= gridY + searchArea; y++)
            {
                if (x >= 0 && x <= gridX && y >= 0 && y <= gridY)
                {
                    if (grid[x, y] == 1)
                    {
                        neighbours++;
                    } 
                }
            }
        }

        return neighbours;
    }

    void StartMarchingSquares()
    {
        MarchingSquares marchingSquares = GetComponent<MarchingSquares>();
        MarchingSquares lavaMarchingSquares = GameObject.FindGameObjectWithTag("Lava").GetComponent<MarchingSquares>();
        

        marchingSquares.GenerateMesh(grid, 1);
        lavaMarchingSquares.GenerateMesh(grid, 1);
    }
}
