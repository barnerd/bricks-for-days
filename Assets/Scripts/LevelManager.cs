using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Bricks")]
    public GameObject brickPrefab;
    public int NumBrickLevels = 7;
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

    [Header("Level Parameters")]
    public int minDifficultyScore;
    [Range(0f, 1f)]
    public float gameScoreDifficultyWeight;
    [Range(0f, 1f)]
    public float highScoreDifficultyWeight;
    [Range(0f, 1f)]
    public float scoreVariance;
    [Range(0f, 1f)]
    public float chancePercentHalved;
    [Range(0f, 1f)]
    public float chanceMirrorTop;
    [Range(0f, 1f)]
    public float chanceMirrorBottom;
    [Range(0f, 1f)]
    public float chanceMirrorVertically;

    [Space]

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

        /* ********** Logic ***********
         *
         * Set Parameters
         * Pick Shape pattern
         * Pick Coloring pattern
         * Repeat for remaining rows
         * 
         * ******** Parameters ********
         *
         * slope = [0, ±.5, ±1, ±2, ±3]
         * bool mirrorTop
         * bool mirrorBottom
         * duplicated = !mirrored
         * bool halvedTop
         * bool halvedBottom
         * bool halvedLeft
         * bool halvedRight
         * bool alternating
         * bool fill
         * int verticalOffset
         * 
         * ********** Shapes **********
         * 
         * Rectangular group: rows x cols
         * Circle group: radius
         *
         * ********** Colors **********
         *
         * Rainbow, with different slopes
         * Mirrored rainbow
         * Edge colors
         *
         * ******** Difficulty ********
         *
         * difficulty score = current score * .9 + total HighScores * .1
         * brick level = sqrt (difficulty score / 1000)
         * barriers show up at 10 * 1000 and 25 * 1000
         * each row can provide difficultyScore ± 33% (+ min of 2) / numRows
         * ****************************/

        int[,] bricks = new int[gridColumns, gridRows];
        for (int y = 0; y < bricks.GetLength(1); y++)
        {
            for (int x = 0; x < bricks.GetLength(0); x++)
            {
                bricks[x, y] = 0;
            }
        }

        int difficultyScore = Mathf.CeilToInt(gameScore.Value * gameScoreDifficultyWeight) + minDifficultyScore;

        // currentRow = 0 if sqrt(difficultyScore) > gridRows, else try to center the level vertically
        int currentRow = Mathf.FloorToInt((gridRows - Mathf.Min(gridRows, Mathf.Sqrt(difficultyScore))) / 2);
        int rowsRemaining = gridRows - currentRow;
        int pattern;
        bool alternating, fill;
        bool mirroredTop, mirroredBottom, mirroredVertically;
        bool halfTop, halfBottom, halfLeft, halfRight;
        int numHalfCols, numHalfRows;
        float minScorePerRow, maxScorePerRow;

        int scorePerRow;

        Debug.Log("Overall difficultyScore: " + difficultyScore);
        Debug.Log("*********************");

        // count prevents infinite loop, only 10 tries are allowed
        // TODO: remove count. or should I?
        int count = 20;
        while (rowsRemaining > 0 && difficultyScore > 0 && count > 0)
        {
            count--;
            if (count == 0)
            {
                Debug.LogWarning("You've gone through the Level Manager loop too many times!");
            }

            Debug.Log("---------------------");
            Debug.Log("currentRow: " + currentRow);
            Debug.Log("rowsRemaining: " + rowsRemaining);
            Debug.Log("difficultyScore: " + difficultyScore);

            // determine min/max and average of score per row
            scorePerRow = Mathf.Max(1, Mathf.FloorToInt(difficultyScore / rowsRemaining)); // rowsRemaining needs to be much smaller
            minScorePerRow = Mathf.Max(0, scorePerRow * (1 - scoreVariance));
            maxScorePerRow = scorePerRow * (1 + scoreVariance);
            Debug.Log("minScorePerRow: " + minScorePerRow);
            Debug.Log("scorePerRow: " + scorePerRow);
            Debug.Log("maxScorePerRow: " + maxScorePerRow);

            // determine number of rows and columns the shape should be
            // halved so it's mirrorred
            numHalfCols = Mathf.Min(gridHalfColumns, Mathf.CeilToInt((Random.Range(minScorePerRow, maxScorePerRow) - 1) / 2));
            numHalfRows = Mathf.FloorToInt(Mathf.Min(Random.Range(1, maxScorePerRow), rowsRemaining) / 2);
            Debug.Log("numHalfRows: " + numHalfRows);
            Debug.Log("numHalfCols: " + numHalfCols);

            // check if only half should be displayed
            halfTop = Random.Range(0f, 1f) > chancePercentHalved;
            halfBottom = Random.Range(0f, 1f) > chancePercentHalved;
            if (!halfTop && !halfBottom) { halfTop = halfBottom = true; }
            halfLeft = Random.Range(0f, 1f) > chancePercentHalved;
            halfRight = Random.Range(0f, 1f) > chancePercentHalved;
            if (!halfLeft && !halfRight) { halfLeft = halfRight = true; }

            if (numHalfRows == 0)
            {
                numHalfRows = 1;
                if (rowsRemaining == 1)
                {
                    halfTop = true;
                    halfBottom = false;
                }
            }
            Debug.Log("halfTop: " + halfTop);
            Debug.Log("halfBottom: " + halfBottom);
            Debug.Log("halfLeft: " + halfLeft);
            Debug.Log("halfRight: " + halfRight);

            mirroredTop = Random.Range(0f, 1f) < chanceMirrorTop;
            mirroredBottom = Random.Range(0f, 1f) < chanceMirrorBottom;
            mirroredVertically = Random.Range(0f, 1f) < chanceMirrorVertically;
            if (mirroredVertically) mirroredBottom = mirroredTop;
            Debug.Log("mirroredTop: " + mirroredTop);
            Debug.Log("mirroredBottom: " + mirroredBottom);
            Debug.Log("mirroredVertically: " + mirroredVertically);

            pattern = Random.Range(1, 4);
            pattern = Random.Range(3, 4);
            pattern = Random.Range(1, 1);
            switch (pattern)
            {
                case 1: // rectangle
                    // determine slope of the top/bottom edges
                    int slope = Random.Range(0, 2 + 1) - 1; // slopes of -1, 0, 1
                    Debug.Log("slope: " + slope);
                    int slopeRise;
                    int topShift = ((slope == 0 || (mirroredTop && slope > 0)) ? 0 : numHalfCols);
                    int bottomShift = ((slope == 0 || (!mirroredVertically && mirroredBottom && slope < 0) || (mirroredVertically && mirroredBottom && slope > 0)) ? 0 : -numHalfCols) - 1;

                    int middleRow = currentRow + ((halfTop) ? numHalfRows : 0);
                    Debug.Log("middleRow: " + middleRow);

                    // draw top/bottom edge
                    if (numHalfRows >= 1)
                    {
                        if (halfTop)
                        {
                            // draw center of the top edge
                            slopeRise = -numHalfRows + topShift;
                            if (slopeRise < 0 && middleRow + slopeRise >= 0)
                            {
                                bricks[gridHalfColumns, middleRow + slopeRise] = 7;
                                difficultyScore -= 7;
                            }
                        }
                        if (halfBottom)
                        {
                            // draw center of the bottom edge
                            slopeRise = numHalfRows + bottomShift;
                            if (slopeRise >= 0 && middleRow + slopeRise < gridRows)
                            {
                                bricks[gridHalfColumns, middleRow + slopeRise] = 7;
                                difficultyScore -= 7;
                            }
                        }

                        // fill center areas
                        for (int x = 1; x <= numHalfCols - 1; x++)
                        {
                            if (halfTop)
                            {
                                if (halfRight)
                                {
                                    // right center area
                                    slopeRise = -numHalfRows + topShift - slope * x * (mirroredTop ? -1 : 1);
                                    if (slopeRise < 0 && middleRow + slopeRise >= 0)
                                    {
                                        bricks[gridHalfColumns + x, middleRow + slopeRise] = 2;
                                        difficultyScore -= 2;
                                    }
                                }
                                if (halfLeft)
                                {
                                    // left center area
                                    slopeRise = -numHalfRows + topShift - slope * -x;
                                    if (slopeRise < 0 && middleRow + slopeRise >= 0)
                                    {
                                        bricks[gridHalfColumns - x, middleRow + slopeRise] = 2;
                                        difficultyScore -= 2;
                                    }
                                }
                            }
                            if (halfBottom)
                            {
                                if (halfRight)
                                {
                                    // right center area
                                    slopeRise = numHalfRows + bottomShift - slope * x * (mirroredBottom ? -1 : 1) * (mirroredVertically ? -1 : 1);
                                    if (slopeRise >= 0 && middleRow + slopeRise < gridRows)
                                    {
                                        bricks[gridHalfColumns + x, middleRow + slopeRise] = 4;
                                        difficultyScore -= 4;
                                    }
                                }
                                if (halfLeft)
                                {
                                    // left center area
                                    slopeRise = numHalfRows + bottomShift - slope * -x * (mirroredVertically ? -1 : 1);
                                    if (slopeRise >= 0 && middleRow + slopeRise < gridRows)
                                    {
                                        bricks[gridHalfColumns - x, middleRow + slopeRise] = 4;
                                        difficultyScore -= 4;
                                    }
                                }
                            }
                        }

                        // outer corners
                        if (numHalfCols > 0)
                        {
                            if (halfTop)
                            {
                                if (halfRight)
                                {
                                    // top right corner
                                    slopeRise = -numHalfRows + topShift - slope * numHalfCols * (mirroredTop ? -1 : 1);
                                    if (slopeRise < 0 && middleRow + slopeRise >= 0)
                                    {
                                        bricks[gridHalfColumns + numHalfCols, middleRow + slopeRise] = 7;
                                        difficultyScore -= 7;
                                    }
                                }
                                if (halfLeft)
                                {
                                    // top left corner
                                    slopeRise = -numHalfRows + topShift - slope * -numHalfCols;
                                    if (slopeRise < 0 && middleRow + slopeRise >= 0)
                                    {
                                        bricks[gridHalfColumns - numHalfCols, middleRow + slopeRise] = 7;
                                        difficultyScore -= 7;
                                    }
                                }
                            }
                            if (halfBottom)
                            {
                                if (halfRight)
                                {
                                    // bottom right corner
                                    slopeRise = numHalfRows + bottomShift - slope * numHalfCols * (mirroredBottom ? -1 : 1) * (mirroredVertically ? -1 : 1);
                                    if (slopeRise >= 0 && middleRow + slopeRise < gridRows)
                                    {
                                        bricks[gridHalfColumns + numHalfCols, middleRow + slopeRise] = 7;
                                        difficultyScore -= 7;
                                    }
                                }
                                if (halfLeft)
                                {
                                    // bottom left corner
                                    slopeRise = numHalfRows + bottomShift - slope * -numHalfCols * (mirroredVertically ? -1 : 1);
                                    if (slopeRise >= 0 && middleRow + slopeRise < gridRows)
                                    {
                                        bricks[gridHalfColumns - numHalfCols, middleRow + slopeRise] = 7;
                                        difficultyScore -= 7;
                                    }
                                }
                            }
                        }

                        currentRow++;
                        rowsRemaining--;
                    }

                    // main part of the shape
                    for (int y = 1; y < numHalfRows; y++)
                    {
                        if (halfTop)
                        {
                            // draw center "edge"
                            slopeRise = -numHalfRows + y + topShift;
                            if (slopeRise < 0 && middleRow + slopeRise >= 0)
                            {
                                bricks[gridHalfColumns, middleRow + slopeRise] = 3;
                                difficultyScore -= 3;
                            }
                        }
                        if (halfBottom)
                        {
                            // draw center "edge"
                            slopeRise = numHalfRows - y + bottomShift;
                            if (slopeRise >= 0 && middleRow + slopeRise < gridRows)
                            {
                                bricks[gridHalfColumns, middleRow + slopeRise] = 3;
                                difficultyScore -= 3;
                            }
                        }

                        // fill center areas
                        for (int x = 1; x <= numHalfCols - 1; x++)
                        {
                            if (halfTop)
                            {
                                if (halfRight)
                                {
                                    // right center area
                                    slopeRise = -numHalfRows + y + topShift - slope * x * (mirroredTop ? -1 : 1);
                                    if (slopeRise < 0 && middleRow + slopeRise >= 0)
                                    {
                                        bricks[gridHalfColumns + x, middleRow + slopeRise] = 1;
                                        difficultyScore -= 1;
                                    }
                                }
                                if (halfLeft)
                                {
                                    // left center area
                                    slopeRise = -numHalfRows + y + topShift - slope * -x;
                                    if (slopeRise < 0 && middleRow + slopeRise >= 0)
                                    {
                                        bricks[gridHalfColumns - x, middleRow + slopeRise] = 1;
                                        difficultyScore -= 1;
                                    }
                                }
                            }
                            if (halfBottom)
                            {
                                if (halfRight)
                                {
                                    // right center area
                                    slopeRise = numHalfRows - y + bottomShift - slope * x * (mirroredBottom ? -1 : 1) * (mirroredVertically ? -1 : 1);
                                    if (slopeRise >= 0 && middleRow + slopeRise < gridRows)
                                    {
                                        bricks[gridHalfColumns + x, middleRow + slopeRise] = 1;
                                        difficultyScore -= 1;
                                    }
                                }
                                if (halfLeft)
                                {
                                    // left center area
                                    slopeRise = numHalfRows - y + bottomShift - slope * -x * (mirroredVertically ? -1 : 1);
                                    if (slopeRise >= 0 && middleRow + slopeRise < gridRows)
                                    {
                                        bricks[gridHalfColumns - x, middleRow + slopeRise] = 1;
                                        difficultyScore -= 1;
                                    }
                                }
                            }
                        }

                        // outer edges
                        if (numHalfCols > 0)
                        {
                            if (halfTop)
                            {
                                if (halfRight)
                                {
                                    // right edge
                                    slopeRise = -numHalfRows + y + topShift - slope * numHalfCols * (mirroredTop ? -1 : 1);
                                    if (slopeRise < 0 && middleRow + slopeRise >= 0)
                                    {
                                        bricks[gridHalfColumns + numHalfCols, middleRow + slopeRise] = 6;
                                        difficultyScore -= 6;
                                    }
                                }
                                if (halfLeft)
                                {
                                    // left edge
                                    slopeRise = -numHalfRows + y + topShift - slope * -numHalfCols;
                                    if (slopeRise < 0 && middleRow + slopeRise >= 0)
                                    {
                                        bricks[gridHalfColumns - numHalfCols, middleRow + slopeRise] = 5;
                                        difficultyScore -= 5;
                                    }
                                }
                            }
                            if (halfBottom)
                            {
                                if (halfRight)
                                {
                                    // right edge
                                    slopeRise = numHalfRows - y + bottomShift - slope * numHalfCols * (mirroredBottom ? -1 : 1) * (mirroredVertically ? -1 : 1);
                                    if (slopeRise >= 0 && middleRow + slopeRise < gridRows)
                                    {
                                        bricks[gridHalfColumns + numHalfCols, middleRow + slopeRise] = 6;
                                        difficultyScore -= 6;
                                    }
                                }
                                if (halfLeft)
                                {
                                    // left edge
                                    slopeRise = numHalfRows - y + bottomShift - slope * -numHalfCols * (mirroredVertically ? -1 : 1);
                                    if (slopeRise >= 0 && middleRow + slopeRise < gridRows)
                                    {
                                        bricks[gridHalfColumns - numHalfCols, middleRow + slopeRise] = 5;
                                        difficultyScore -= 5;
                                    }
                                }
                            }
                        }

                        currentRow++;
                        rowsRemaining--;
                    }
                    break;
                case 2: // circle
                    break;
                case 3: // full fill
                    for (; currentRow < gridRows; currentRow++)
                    {
                        for (int x = 0; x < gridColumns; x++)
                        {
                            bricks[x, currentRow] = 1;
                        }
                        rowsRemaining--;
                    }
                    break;
                default:
                    break;
            }
        }

        for (int y = 0; y < bricks.GetLength(1); y++)
        {
            for (int x = 0; x < bricks.GetLength(0); x++)
            {
                if (bricks[x, y] > 0)
                {
                    CreateBrick(x, y, bricks[x, y]);
                }
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
        if (gridColumns % 2 == 0)
        {
            gridColumns--;
        }

        gridHalfColumns = (gridColumns - 1) / 2;
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

    void OnValidate()
    {
        minDifficultyScore = Mathf.Max(1, minDifficultyScore);
        gameScoreDifficultyWeight = Mathf.Clamp01(gameScoreDifficultyWeight);
        highScoreDifficultyWeight = Mathf.Clamp01(highScoreDifficultyWeight);
        scoreVariance = Mathf.Max(0, scoreVariance);
        chancePercentHalved = Mathf.Clamp01(chancePercentHalved);
        chanceMirrorTop = Mathf.Clamp01(chanceMirrorTop);
        chanceMirrorBottom = Mathf.Clamp01(chanceMirrorBottom);
        chanceMirrorVertically = Mathf.Clamp01(chanceMirrorVertically);
    }
}

/*
 *             pattern = Random.Range(1, 3); // TODO: Put back to 0, 3)

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
*/
