using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Runtime;

[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : Editor
{
	private GameEvent _event;

	void OnEnable()
	{
		_event = (GameEvent)target;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Raise Event"))
		{
			_event.Raise();
		}
	}
}
