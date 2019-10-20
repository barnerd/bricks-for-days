using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Bricks")]
    public GameObject brickPrefab;
    public GameObject brickSlots;

    [Space]

    [Header("Grid Size")]
    public int maxColumn = 17; // should always be odd
    public int maxRow = 15;

    public KeyCode loadLevel; // define as l

    public int NumBrickLevels = 7;
    public IntReference gameScore;
    public GameEvent OnLevelStart;

    public WeightedObjects powerUpLootTable;

    // Start is called before the first frame update
    void Start()
    {
        LoadLevel();
    }

    // Called once a frame.
    void Update()
    {
        if (Input.GetKeyUp(loadLevel))
        {
            OnLevelStart.Raise();
        }
    }

    public void LoadLevel()
    {
        // Ensure all bricks are gone
        foreach (var b in GameObject.FindGameObjectsWithTag("brick"))
        {
            Destroy(b);
        }

        // define patterns
        // randomly select patterns
        // randomly place patterns

        /* ********* Patterns *********
         * 
         * Whole Row or Rows
         * Blank Row
         * Checkered
         * Mirrored or offset
         * Whole Column or groups of columns
         * Diamonds
         * 
         * ***************************/

        int rowsRemaining = maxRow;
        int pattern;
        bool mirrored, rainbow, ascending;

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

                    rainbow = Random.Range(0, 2) == 0;
                    ascending = Random.Range(0, 2) == 0 && rainbow;
                    int minBrickLevel = Random.Range(1, NumBrickLevels + 1 - numRows);
                    int maxBrickLevel = Random.Range(minBrickLevel, NumBrickLevels + 1);
                    int brickLevel = rainbow ? (ascending ? minBrickLevel : maxBrickLevel) : Random.Range(1, NumBrickLevels + 1);

                    for (int y = 0; y < numRows; y++)
                    {
                        for (int x = (checkered && y % 2 == 0) ? minCol + 1 : minCol; x <= maxCol; x += checkered ? 2 : 1)
                        {
                            CreateBrick(x, rowsRemaining - 1 - y, brickLevel);
                        }

                        if (rainbow)
                        {
                            brickLevel += ascending ? 1 : -1;
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

    void CreateBrick(int _col, int _row, int _level)
    {
        Vector3 p = brickSlots.transform.Find("Row" + _row).Find("Col" + _col).position;

        Brick b = Instantiate(brickPrefab, p, Quaternion.identity).GetComponent<Brick>();
        // make child of levelManager
        b.transform.parent = transform;

        b.SetLevel(_level);
        b.powerUp = powerUpLootTable.TakeOne();
    }
}
