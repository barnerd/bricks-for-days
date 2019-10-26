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

    [Header("Level Shape Parameters")]
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

    [Header("Level Color Parameters")]
    [Range(0f, 1f)]
    public float chanceMirrorColors;
    [Range(0f, 1f)]
    public float chanceColorVerticalEdges;
    [Range(0f, 1f)]
    public float chanceColorHorizontalEdges;
    [Range(0f, 1f)]
    public float chanceColorBottomEdge;
    [Range(0f, 1f)]
    public float chanceColorAlternating;

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
         * Rainbow, with different slopes [-1, 0, 1]
         * Mirrored rainbow
         * bool verticalEdges colors
         * bool horizontalEdges colors
         * bool bottonEdge color
         * bool alternating
         * int colorChange [0, ±1, ±2, ±3]
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
        bool mirroredTop, mirroredBottom, mirroredVertically;
        bool halfTop, halfBottom, halfLeft, halfRight;
        int numHalfCols, numHalfRows;

        int scorePerRow;
        float minScorePerRow, maxScorePerRow;

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
            Debug.Log("numHalfRows: " + numHalfRows);
            Debug.Log("numHalfCols: " + numHalfCols);

            int middleRow = currentRow + ((halfTop) ? numHalfRows : 0);
            Debug.Log("middleRow: " + middleRow);

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

            // color parameters
            float colorSlope = Random.Range(-4, 4f); // slopes of -1, 0, 1, treat ±3.5 as infinity
            bool colorMirrored = Random.Range(0f, 1f) < chanceMirrorColors;
            bool colorVerticalEdges = Random.Range(0f, 1f) < chanceColorVerticalEdges;
            bool colorHorizontalEdges = Random.Range(0f, 1f) < chanceColorHorizontalEdges;
            bool colorBottomEdge = Random.Range(0f, 1f) < chanceColorBottomEdge;
            bool colorAlternating = Random.Range(0f, 1f) < chanceColorAlternating;
            int colorChange = Random.Range(-2, 2 + 1); // changes of [0, ±1, ±2]
            int colorBase = Mathf.CeilToInt(Mathf.Sqrt(Random.Range(minScorePerRow, maxScorePerRow) / (2 * numHalfCols + 1)));
            int color;
            int colorEdge = 9;
            // TODO: Set center "edge" to 0 for shapes on either side
            // TODO: add colorEdges: figure out when they look good
            // TODO: add barriers
            // [gridHalfColumns, middleRow] = colorBase
            // if edges && colorEdge then [x,y] = colorBase + colorChange;

            Debug.Log("colorSlope: " + colorSlope);
            Debug.Log("colorMirrored: " + colorMirrored);
            Debug.Log("colorAlternating: " + colorAlternating);
            Debug.Log("colorChange: " + colorChange);
            Debug.Log("colorBase: " + colorBase);

            pattern = Random.Range(1, 2);
            switch (pattern)
            {
                case 1: // rectangle
                    // determine slope of the top/bottom edges
                    int slope = Random.Range(-1, 1 + 1); // slopes of -1, 0, 1
                    Debug.Log("slope: " + slope);
                    int slopeRise, slopeRun;
                    int topShift = ((slope == 0 || (mirroredTop && slope > 0)) ? 0 : numHalfCols);
                    int bottomShift = ((slope == 0 || (!mirroredVertically && mirroredBottom && slope < 0) || (mirroredVertically && mirroredBottom && slope > 0)) ? 0 : -numHalfCols) - 1;

                    // draw top/bottom edge
                    if (numHalfRows >= 1)
                    {
                        if (halfTop)
                        {
                            // draw center of the top edge
                            slopeRun = 0;
                            slopeRise = -numHalfRows + topShift;
                            if (slopeRise < 0 && middleRow + slopeRise >= 0)
                            {
                                color = DetermineColor(slopeRun, slopeRise, colorSlope, colorChange, colorAlternating, colorMirrored);
                                color = (colorHorizontalEdges && color > 0) ? colorEdge : color;
                                bricks[gridHalfColumns + slopeRun, middleRow + slopeRise] = colorBase + color;
                                difficultyScore -= color;
                            }
                        }
                        if (halfBottom)
                        {
                            // draw center of the bottom edge
                            slopeRun = 0;
                            slopeRise = numHalfRows + bottomShift;
                            if (slopeRise >= 0 && middleRow + slopeRise < gridRows)
                            {
                                color = DetermineColor(slopeRun, slopeRise, colorSlope, colorChange, colorAlternating, colorMirrored);
                                bricks[gridHalfColumns + slopeRun, middleRow + slopeRise] = colorBase + color;
                                difficultyScore -= color;
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
                                    slopeRun = x;
                                    slopeRise = -numHalfRows + topShift - slope * x * (mirroredTop ? -1 : 1);
                                    if (slopeRise < 0 && middleRow + slopeRise >= 0)
                                    {
                                        color = DetermineColor(slopeRun, slopeRise, colorSlope, colorChange, colorAlternating, colorMirrored);
                                        bricks[gridHalfColumns + slopeRun, middleRow + slopeRise] = colorBase + color;
                                        difficultyScore -= color;
                                    }
                                }
                                if (halfLeft)
                                {
                                    // left center area
                                    slopeRun = -x;
                                    slopeRise = -numHalfRows + topShift - slope * -x;
                                    if (slopeRise < 0 && middleRow + slopeRise >= 0)
                                    {
                                        color = DetermineColor(slopeRun, slopeRise, colorSlope, colorChange, colorAlternating, colorMirrored);
                                        bricks[gridHalfColumns + slopeRun, middleRow + slopeRise] = colorBase + color;
                                        difficultyScore -= color;
                                    }
                                }
                            }
                            if (halfBottom)
                            {
                                if (halfRight)
                                {
                                    // right center area
                                    slopeRun = x;
                                    slopeRise = numHalfRows + bottomShift - slope * x * (mirroredBottom ? -1 : 1) * (mirroredVertically ? -1 : 1);
                                    if (slopeRise >= 0 && middleRow + slopeRise < gridRows)
                                    {
                                        color = DetermineColor(slopeRun, slopeRise, colorSlope, colorChange, colorAlternating, colorMirrored);
                                        bricks[gridHalfColumns + slopeRun, middleRow + slopeRise] = colorBase + color;
                                        difficultyScore -= color;
                                    }
                                }
                                if (halfLeft)
                                {
                                    // left center area
                                    slopeRun = -x;
                                    slopeRise = numHalfRows + bottomShift - slope * -x * (mirroredVertically ? -1 : 1);
                                    if (slopeRise >= 0 && middleRow + slopeRise < gridRows)
                                    {
                                        color = DetermineColor(slopeRun, slopeRise, colorSlope, colorChange, colorAlternating, colorMirrored);
                                        bricks[gridHalfColumns + slopeRun, middleRow + slopeRise] = colorBase + color;
                                        difficultyScore -= color;
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
                                    slopeRun = numHalfCols;
                                    slopeRise = -numHalfRows + topShift - slope * numHalfCols * (mirroredTop ? -1 : 1);
                                    if (slopeRise < 0 && middleRow + slopeRise >= 0)
                                    {
                                        color = DetermineColor(slopeRun, slopeRise, colorSlope, colorChange, colorAlternating, colorMirrored);
                                        bricks[gridHalfColumns + slopeRun, middleRow + slopeRise] = colorBase + color;
                                        difficultyScore -= color;
                                    }
                                }
                                if (halfLeft)
                                {
                                    // top left corner
                                    slopeRun = -numHalfCols;
                                    slopeRise = -numHalfRows + topShift - slope * -numHalfCols;
                                    if (slopeRise < 0 && middleRow + slopeRise >= 0)
                                    {
                                        color = DetermineColor(slopeRun, slopeRise, colorSlope, colorChange, colorAlternating, colorMirrored);
                                        bricks[gridHalfColumns + slopeRun, middleRow + slopeRise] = colorBase + color;
                                        difficultyScore -= color;
                                    }
                                }
                            }
                            if (halfBottom)
                            {
                                if (halfRight)
                                {
                                    // bottom right corner
                                    slopeRun = numHalfCols;
                                    slopeRise = numHalfRows + bottomShift - slope * numHalfCols * (mirroredBottom ? -1 : 1) * (mirroredVertically ? -1 : 1);
                                    if (slopeRise >= 0 && middleRow + slopeRise < gridRows)
                                    {
                                        color = DetermineColor(slopeRun, slopeRise, colorSlope, colorChange, colorAlternating, colorMirrored);
                                        bricks[gridHalfColumns + slopeRun, middleRow + slopeRise] = colorBase + color;
                                        difficultyScore -= color;
                                    }
                                }
                                if (halfLeft)
                                {
                                    // bottom left corner
                                    slopeRun = -numHalfCols;
                                    slopeRise = numHalfRows + bottomShift - slope * -numHalfCols * (mirroredVertically ? -1 : 1);
                                    if (slopeRise >= 0 && middleRow + slopeRise < gridRows)
                                    {
                                        color = DetermineColor(slopeRun, slopeRise, colorSlope, colorChange, colorAlternating, colorMirrored);
                                        bricks[gridHalfColumns + slopeRun, middleRow + slopeRise] = colorBase + color;
                                        difficultyScore -= color;
                                    }
                                }
                            }
                        }

                        if (halfTop)
                        {
                            currentRow++;
                            rowsRemaining--;
                        }
                        if (halfBottom)
                        {
                            currentRow++;
                            rowsRemaining--;
                        }
                    }

                    // main part of the shape
                    for (int y = 1; y < numHalfRows; y++)
                    {
                        if (halfTop)
                        {
                            // draw center "edge"
                            slopeRun = 0;
                            slopeRise = -numHalfRows + y + topShift;
                            if (slopeRise < 0 && middleRow + slopeRise >= 0)
                            {
                                color = DetermineColor(slopeRun, slopeRise, colorSlope, colorChange, colorAlternating, colorMirrored);
                                bricks[gridHalfColumns + slopeRun, middleRow + slopeRise] = colorBase + color;
                                difficultyScore -= color;
                            }
                        }
                        if (halfBottom)
                        {
                            // draw center "edge"
                            slopeRun = 0;
                            slopeRise = numHalfRows - y + bottomShift;
                            if (slopeRise >= 0 && middleRow + slopeRise < gridRows)
                            {
                                color = DetermineColor(slopeRun, slopeRise, colorSlope, colorChange, colorAlternating, colorMirrored);
                                bricks[gridHalfColumns + slopeRun, middleRow + slopeRise] = colorBase + color;
                                difficultyScore -= color;
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
                                    slopeRun = x;
                                    slopeRise = -numHalfRows + y + topShift - slope * x * (mirroredTop ? -1 : 1);
                                    if (slopeRise < 0 && middleRow + slopeRise >= 0)
                                    {
                                        color = DetermineColor(slopeRun, slopeRise, colorSlope, colorChange, colorAlternating, colorMirrored);
                                        bricks[gridHalfColumns + slopeRun, middleRow + slopeRise] = colorBase + color;
                                        difficultyScore -= color;
                                    }
                                }
                                if (halfLeft)
                                {
                                    // left center area
                                    slopeRun = -x;
                                    slopeRise = -numHalfRows + y + topShift - slope * -x;
                                    if (slopeRise < 0 && middleRow + slopeRise >= 0)
                                    {
                                        color = DetermineColor(slopeRun, slopeRise, colorSlope, colorChange, colorAlternating, colorMirrored);
                                        bricks[gridHalfColumns + slopeRun, middleRow + slopeRise] = colorBase + color;
                                        difficultyScore -= color;
                                    }
                                }
                            }
                            if (halfBottom)
                            {
                                if (halfRight)
                                {
                                    // right center area
                                    slopeRun = x;
                                    slopeRise = numHalfRows - y + bottomShift - slope * x * (mirroredBottom ? -1 : 1) * (mirroredVertically ? -1 : 1);
                                    if (slopeRise >= 0 && middleRow + slopeRise < gridRows)
                                    {
                                        color = DetermineColor(slopeRun, slopeRise, colorSlope, colorChange, colorAlternating, colorMirrored);
                                        bricks[gridHalfColumns + slopeRun, middleRow + slopeRise] = colorBase + color;
                                        difficultyScore -= color;
                                    }
                                }
                                if (halfLeft)
                                {
                                    // left center area
                                    slopeRun = -x;
                                    slopeRise = numHalfRows - y + bottomShift - slope * -x * (mirroredVertically ? -1 : 1);
                                    if (slopeRise >= 0 && middleRow + slopeRise < gridRows)
                                    {
                                        color = DetermineColor(slopeRun, slopeRise, colorSlope, colorChange, colorAlternating, colorMirrored);
                                        bricks[gridHalfColumns + slopeRun, middleRow + slopeRise] = colorBase + color;
                                        difficultyScore -= color;
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
                                    slopeRun = numHalfCols;
                                    slopeRise = -numHalfRows + y + topShift - slope * numHalfCols * (mirroredTop ? -1 : 1);
                                    if (slopeRise < 0 && middleRow + slopeRise >= 0)
                                    {
                                        color = DetermineColor(slopeRun, slopeRise, colorSlope, colorChange, colorAlternating, colorMirrored);
                                        bricks[gridHalfColumns + slopeRun, middleRow + slopeRise] = colorBase + color;
                                        difficultyScore -= color;
                                    }
                                }
                                if (halfLeft)
                                {
                                    // left edge
                                    slopeRun = -numHalfCols;
                                    slopeRise = -numHalfRows + y + topShift - slope * -numHalfCols;
                                    if (slopeRise < 0 && middleRow + slopeRise >= 0)
                                    {
                                        color = DetermineColor(slopeRun, slopeRise, colorSlope, colorChange, colorAlternating, colorMirrored);
                                        bricks[gridHalfColumns + slopeRun, middleRow + slopeRise] = colorBase + color;
                                        difficultyScore -= color;

                                    }
                                }
                            }
                            if (halfBottom)
                            {
                                if (halfRight)
                                {
                                    // right edge
                                    slopeRun = numHalfCols;
                                    slopeRise = numHalfRows - y + bottomShift - slope * numHalfCols * (mirroredBottom ? -1 : 1) * (mirroredVertically ? -1 : 1);
                                    if (slopeRise >= 0 && middleRow + slopeRise < gridRows)
                                    {
                                        color = DetermineColor(slopeRun, slopeRise, colorSlope, colorChange, colorAlternating, colorMirrored);
                                        bricks[gridHalfColumns + slopeRun, middleRow + slopeRise] = colorBase + color;
                                        difficultyScore -= color;
                                    }
                                }
                                if (halfLeft)
                                {
                                    // left edge
                                    slopeRun = -numHalfCols;
                                    slopeRise = numHalfRows - y + bottomShift - slope * -numHalfCols * (mirroredVertically ? -1 : 1);
                                    if (slopeRise >= 0 && middleRow + slopeRise < gridRows)
                                    {
                                        color = DetermineColor(slopeRun, slopeRise, colorSlope, colorChange, colorAlternating, colorMirrored);
                                        bricks[gridHalfColumns + slopeRun, middleRow + slopeRise] = colorBase + color;
                                        difficultyScore -= color;
                                    }
                                }
                            }
                        }

                        if (halfTop)
                        {
                            currentRow++;
                            rowsRemaining--;
                        }
                        if (halfBottom)
                        {
                            currentRow++;
                            rowsRemaining--;
                        }
                    }
                    break;
                case 2: // circle
                    break;
                default:
                    break;
            }
        }

        for (int y = 0; y < bricks.GetLength(1); y++)
        {
            for (int x = 0; x < bricks.GetLength(0); x++)
            {
                // TODO: add ability to create barrier
                if (bricks[x, y] > 0)
                {
                    CreateBrick(x, y, Mathf.Min(bricks[x, y], NumBrickLevels));
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

    int DetermineColor(int _x, int _y, float _slope, int _change, bool _alternate, bool _mirror)
    {
        // [colorX, colorY] is distance from [gridHalfColumns, middleRow]
        int _distance = Mathf.RoundToInt(_slope * _x) + _y;

        if (Mathf.Abs(_slope) > 3.5f)
        {
            // slope is infinity
            _distance = _x;
        }

        if (_mirror)
        {
            _distance = Mathf.Abs(_distance);
        }

        if (_alternate)
        {
            _distance = Mathf.Abs(_distance) % 2;
        }

        return (_distance) * _change;
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

        chanceMirrorColors = Mathf.Clamp01(chanceMirrorColors);
        chanceColorVerticalEdges = Mathf.Clamp01(chanceColorVerticalEdges);
        chanceColorHorizontalEdges = Mathf.Clamp01(chanceColorHorizontalEdges);
        chanceColorBottomEdge = Mathf.Clamp01(chanceColorBottomEdge);
        chanceColorAlternating = Mathf.Clamp01(chanceColorAlternating);
    }
}
