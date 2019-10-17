using UnityEngine;

public class PowerUp : ScriptableObject
{
    public IntReference score;
    public IntReference gameScore;
    public IntReference scoreMultiplier;

    public GameEvent _event;

    public virtual void UsePowerUpPayload()
    {
        // add score for collecting PowerUp
        gameScore.Value += score.Value * scoreMultiplier.Value;

        // spawn a cool effect
        if (_event != null)
        {
            _event.Raise(this);
        }
    }
}
