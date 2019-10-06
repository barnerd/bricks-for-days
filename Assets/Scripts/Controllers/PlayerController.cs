using UnityEngine;

[CreateAssetMenu(fileName = "InputController", menuName = "Input Controller/Player Controller")]
public class PlayerController : InputController
{
    public KeyCode moveLeft; // define as a
    public KeyCode moveRight; // define as d

    public override void ProcessInput(GameObject obj)
    {
        Paddle paddle = obj.GetComponent<Paddle>();

        // move paddle Left
        if (Input.GetKey(moveLeft))
        {
            paddle.MoveLeft();
        }
        // move paddle Right
        else if (Input.GetKey(moveRight))
        {
            paddle.MoveRight();
        }
    }
}
