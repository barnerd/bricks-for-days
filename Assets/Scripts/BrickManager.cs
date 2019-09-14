using UnityEngine;

public class BrickManager : MonoBehaviour
{
    public GameController gc;

    // Bricks
    public GameObject brickPrefab;

    // Start is called before the first frame update
    void Start()
    {
        LoadLevel();
    }

    public void LoadLevel(int difficulty = 1)
    {
        // bricks go from 0 to 9 in the x directions and 0 to 7 in the y
        for (int x = 0; x < 1; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                CreateBrick(x, y, 1);
            }
        }
    }

    Brick CreateBrick(int col, int row, int level)
    {
        Vector3 position = new Vector3((brickPrefab.transform.localScale.x + .05f) * (col - 4f), (brickPrefab.transform.localScale.y + .05f) * (row - 1f), 0);
        // figure out position of new Brick

        Brick b = Instantiate(brickPrefab, position, Quaternion.identity).GetComponent<Brick>();

        // init each brick
        b.initBrick(level, gc);

        // position brick
        return b;
    }
}
