using UnityEngine;

public class PowerUp : ScriptableObject
{
    public IntVariable score;
    public IntVariable gameScore;

    public virtual void UsePowerUpPayload()
    {
        // spawn a cool effect

        // add score for collecting PowerUp
        gameScore.Value += score.Value;
    }
}
