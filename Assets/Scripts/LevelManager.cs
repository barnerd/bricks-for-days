using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Bricks")]
    public GameObject brickPrefab;
    private Vector3 brickSize;

    [Header("Boundaries")]
    public EdgeCollider2D topWall;
    private float maxHeight;
    public EdgeCollider2D leftWall;
    public EdgeCollider2D rightWall;
    public Transform bottomBoundary;

    [Header("Grid Size")]
    public Vector2 gridOffset;
    public Vector2 gridSpacing;
    private int gridColumns; // should always be odd
    private int gridRows;
    private int gridHalfColumns; // half of gridColumns - 1
    private float gridRowHeight;
    private float gridColumnWidth;

    [Space]

    public int NumBrickLevels = 7;
    public IntReference gameScore;

    public WeightedObjects powerUpLootTable;

    // Start is called before the first frame update
    void Start()
    {
        maxHeight = topWall.offset.y;

        GameObject testBrick = Instantiate(brickPrefab);
        brickSize = testBrick.GetComponent<Collider2D>().bounds.size;
        DestroyImmediate(testBrick);

        gridRowHeight = brickSize.y + gridSpacing.y;
        gridColumnWidth = brickSize.x + gridSpacing.x;

        LoadLevel();
    }

    // Called once a frame.
    void Update()
    {
    }

    public void LoadLevel()
    {
        // Ensure all bricks are gone
        foreach (var b in GameObject.FindGameObjectsWithTag("brick"))
        {
            Destroy(b);
        }

        FindGridSize();

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

        int rowsRemaining = gridRows;
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

            pattern = 3;
            switch (pattern)
            {
                case 0:
                    // Put a pattern here
                    break;
                case 1:
                    // Pattern: group of rows, with variable num of columns, either mirrored or checkered
                    int numRows = Random.Range(mirrored ? 1 : 3, rowsRemaining);
                    int numCols = Random.Range(mirrored ? 1 : 2, mirrored ? (gridColumns - 1) / 2 + 1 : gridColumns);
                    int minCol, maxCol;
                    bool checkered = false;

                    if (mirrored)
                    {
                        minCol = (gridColumns - 1) / 2 - numCols;
                        maxCol = (gridColumns - 1) / 2 + numCols;
                    }
                    else
                    {
                        minCol = Random.Range(0, gridColumns - numCols);
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
                case 3:
                    for (int y = 0; y < gridRows; y++)
                    {
                        for (int x = 0; x < gridColumns; x++)
                        {
                            CreateBrick(x, y, 1);
                        }
                    }
                    rowsRemaining -= gridRows;
                    break;
                default:
                    break;
            }
        }

    }

    void FindGridSize()
    {
        float width = rightWall.offset.x - leftWall.offset.x - 2 * gridOffset.x; // right edge - left edge
        float height = maxHeight - bottomBoundary.position.y; // top edge - height of bottom row

        gridRows = Mathf.FloorToInt(height / gridRowHeight);
        gridColumns = Mathf.FloorToInt(width / gridColumnWidth);

        // should always be odd
        if(gridColumns % 2 == 0)
        {
            gridColumns--;
        }

        gridHalfColumns = (gridColumns - 1) / 2;

        Debug.Log("Grid size: " + gridColumns + " by " + gridRows);
    }

    void CreateBrick(int _col, int _row, int _level)
    {
        Vector3 p = new Vector3(gridColumnWidth * (_col - gridHalfColumns), maxHeight - gridRowHeight * _row - gridRowHeight / 2 - gridOffset.y, 0);

        Brick b = Instantiate(brickPrefab, p, Quaternion.identity).GetComponent<Brick>();
        // make child of levelManager
        b.transform.parent = transform;

        b.SetLevel(_level);
        b.powerUp = powerUpLootTable.TakeOne();
    }
}
