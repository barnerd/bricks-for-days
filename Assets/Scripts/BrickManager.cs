using UnityEngine;

public class BrickManager : MonoBehaviour
{
    [Header("Game Objects")]
    public GameController gc;

    // Bricks
    public GameObject brickPrefab;

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

    Brick CreateBrick(int col, int row, int level)
    {
        Vector3 position = new Vector3(brickPrefab.transform.localScale.x * (col - ((maxColumn - 1) / 2)), brickPrefab.transform.localScale.y * (row - 7), 0);
        // figure out position of new Brick

        Brick b = Instantiate(brickPrefab, position, Quaternion.identity).GetComponent<Brick>();

        // init each brick
        b.initBrick(level, gc);

        // position brick
        return b;
    }
}
