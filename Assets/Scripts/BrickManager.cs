﻿using UnityEngine;

public class BrickManager : MonoBehaviour
{
    [Header("Game Objects")]
    public GameController gc;

    // Bricks
    public GameObject brickPrefab;
    public GameObject brickSlots;

    [Space]

    [Header("Grid Size")]

    public int maxColumn = 17; // should always be odd
    public int maxRow = 15;

    public KeyCode loadLevel; // define as l
    public bool gameHasEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        LoadLevel();
    }

    // Called once a frame.
    void Update()
    {
        // move paddle Left
        if (Input.GetKeyUp(loadLevel))
        {
            foreach (var b in GameObject.FindGameObjectsWithTag("brick"))
            {
                Destroy(b);
            }

            LoadLevel();
        }
    }

    public void LoadLevel(int difficulty = 1)
    {
        // define patterns
        // randomly select patterns
        // randomly place patterns

        /* ********* Patterns *********
         *
         * Full Fill
         * Whole Row or Rows
         * Blank Row
         * Whole Column or groups of columns
         * Diamonds
         * 
         * ***************************/

        int rowsRemaining = maxRow;
        int pattern;
        bool mirrored;

        while (rowsRemaining > 0)
        {
            pattern = Random.Range(1, 3); // TODO: Put back to 0, 3)

            mirrored = true;
            if (Random.Range(0, 2) == 0 && rowsRemaining > 3)
            {
                mirrored = false;
            }

            switch (pattern)
            {
                case 0:
                    // Put a pattern here
                    break;
                case 1:
                    // Pattern: group of rows, with variable num of columns, either mirrored or checkered
                    int numRows = Random.Range(mirrored ? 1 : 3, rowsRemaining);
                    int numCols = Random.Range(mirrored ? 1 : 2, mirrored ? (maxColumn - 1) / 2 + 1 : maxColumn);
                    int minCol, maxCol;
                    bool checkered = false;

                    if (mirrored)
                    {
                        minCol = (maxColumn - 1) / 2 - numCols;
                        maxCol = (maxColumn - 1) / 2 + numCols;
                    }
                    else
                    {
                        minCol = Random.Range(0, maxColumn - numCols);
                        maxCol = minCol + numCols;
                    }

                    if (numRows > 2 && numCols > 1 && Random.Range(0, 2) == 0)
                    {
                        checkered = true;
                    }

                    for (int y = 0; y < numRows; y++)
                    {
                        for (int x = (checkered && y % 2 == 0) ? minCol + 1 : minCol; x <= maxCol; x += checkered ? 2 : 1)
                        {
                            CreateBrick(x, rowsRemaining - 1 - y, 1);
                        }
                    }
                    rowsRemaining -= numRows;
                    break;
                case 2:
                    // Pattern: Blank Row
                    rowsRemaining -= 1;
                    break;
                default:
                    break;
            }
        }

    }

    void CreateBrick(int col, int row, int level)
    {
        Brick b = Instantiate(brickPrefab, brickSlots.transform.Find("Brick Slot " + col + "x" + row).position, Quaternion.identity).GetComponent<Brick>();

        // init each brick
        b.initBrick(level, gc);
    }
}
