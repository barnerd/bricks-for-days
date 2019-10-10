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
    public int scorePerBrickLevel;
    public IntVariable gameScore;
    public GameEvent OnLevelComplete;

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
            OnLevelComplete.Raise();
        }
    }

    public void LoadLevel()
    {
        int[,] grid = new int[maxColumn, maxRow];
        GameObject[,] powerUpPrefabs = new GameObject[maxColumn, maxRow];

        // Ensure all bricks are gone
        foreach (var b in GameObject.FindGameObjectsWithTag("brick"))
        {
            Destroy(b);
        }

        // define patterns
        // randomly select patterns
        // randomly place patterns

        /* Start with empty grid
         * Create patterns
         * Add borders / unbreakable brick patterns
         * check level score
         * adjust somehow
         * measure brickScore:powerUpScore ->
         * have ratio of 1 for easier levels
         * have ratio of 1/5 for harder levels
                 * ******** Difficulty ********
                 * maxBrickLevel = score / 10,000 -> level 8 at 70,000
                 * bricksPerPowerUp = score / 5,000 -> no powerups at 760,000
                 *
                 * 
                 *
                 * 
                 *
                 ********** Patterns *********
                 * 
                 * Whole Row or Rows
                 * Blank Row
                 * Checkered
                 * Mirrored or offset
                 * Whole Column or groups of columns
                 * Diamonds
                 * Horizontal Rainbow
                 * Vertical Rainbow
                 * 
                 * https://yumyummatt.wordpress.com/tag/powerups/
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
                    int minBrickLevel = Random.Range(1, Mathf.Min(NumBrickLevels + 1 - numRows, (int)Mathf.Ceil(gameScore.Value / scorePerBrickLevel)));
                    int maxBrickLevel = Random.Range(minBrickLevel, Mathf.Min(NumBrickLevels + 1, (int)Mathf.Ceil(gameScore.Value / scorePerBrickLevel)));
                    int brickLevel = rainbow ? (ascending ? minBrickLevel : maxBrickLevel) : Random.Range(1, Mathf.Min(NumBrickLevels + 1, (int)Mathf.Ceil(gameScore.Value / scorePerBrickLevel)));

                    for (int y = 0; y < numRows; y++)
                    {
                        for (int x = (checkered && y % 2 == 0) ? minCol + 1 : minCol; x <= maxCol; x += checkered ? 2 : 1)
                        {
                            grid[x, rowsRemaining - 1 - y] = brickLevel;
                            powerUpPrefabs[x, rowsRemaining - 1 - y] = powerUpLootTable.TakeOne();
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

        // get level score
        int totalBrickScore = 0;
        int totalPowerUpScore = 0;
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] > 0)
                {
                    totalBrickScore += grid[x, y] * brickPrefab.GetComponent<Brick>().scoreMultiplierPerLevel;
                    if (powerUpPrefabs[x, y] != null)
                    {
                        totalPowerUpScore += powerUpPrefabs[x, y].GetComponent<PowerUpBehaviour>().powerUp.score.Value;
                    }
                }
            }
        }
        Debug.Log(totalBrickScore + ":" + totalPowerUpScore);

        // turn grid into bricks
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] > 0)
                {
                    CreateBrick(x, y, grid[x, y], powerUpPrefabs[x, y]);
                }
            }
        }
    }
    // 153:0 score for full board level 1, no powerup
    // 153:1530 score for full board level 1, all powerup
    // 1071:0 score for full board level 8, no powerup
    // 2601:1530 score for full board level 8, all powerups

    void CreateBrick(int _col, int _row, int _level, GameObject _powerUp)
    {
        Vector3 p = brickSlots.transform.Find("Row" + _row).Find("Col" + _col).position;

        Brick b = Instantiate(brickPrefab, p, Quaternion.identity).GetComponent<Brick>();
        // make child of levelManager
        b.transform.parent = transform;

        b.SetLevel(_level);
        b.powerUp = _powerUp;
    }
}
