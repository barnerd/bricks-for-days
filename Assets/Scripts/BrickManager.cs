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

    public bool gameHasEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        LoadLevel();
    }

    public void LoadLevel(int difficulty = 1)
    {
        for (int x = 0; x < maxColumn; x++)
        {
            for (int y = 0; y < maxRow; y++)
            {
                CreateBrick(x, y, 1);
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
