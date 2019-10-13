using UnityEngine;
using UnityEngine.Events;
/*
 * Pulled from Ryan Hipple's talk at Unite Austin 2017
 * blog post: https://unity3d.com/how-to/architect-with-scriptable-objects
 * video: https://www.youtube.com/watch?v=raQ3iHhE_Kk&feature=youtu.be
 */

[System.Serializable]
public class ScriptablebjectUnityEvent : UnityEvent<ScriptableObject> { }

public class GameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public ScriptablebjectUnityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(ScriptableObject obj = null)
    {
        Response.Invoke(obj);
    }
}
