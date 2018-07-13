using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiLocalization : MonoBehaviour {
	private Tracking.PositioningBehaviour _positioningBehaviour;

	private float width;
    private float height;

	private GUIStyle _textStyle;

	// Use this for initialization	
	void Start () {
		_positioningBehaviour = GetComponent<Tracking.PositioningBehaviour>();

		width = Screen.width * 0.1f;
        height = Screen.height * 0.1f;

		_textStyle = new GUIStyle();
		_textStyle.alignment = TextAnchor.UpperCenter;
		_textStyle.fontSize = 35;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnGUI() {
		return; //    <==========           /////////////////////////////////////////////////
		if (_positioningBehaviour.IsFounding)
		{
			GUILayout.BeginArea(new Rect(Screen.width * 0.5f - width, height * 2f, width * 2, height));
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("<color=red>Plane =  </color>" + _positioningBehaviour.CountPlane, _textStyle, GUILayout.Width(width * 2), GUILayout.Height(height));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		
			GUILayout.BeginArea(new Rect(Screen.width * 0.5f - width, height * 1.5f, width * 2, height));
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("<color=blue>Founding floor</color>", _textStyle, GUILayout.Width(width * 2), GUILayout.Height(height));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

		if (GUILayout.Button("Localization", GUILayout.ExpandWidth(true), GUILayout.Width(width), GUILayout.Height(height)))
            {
				if (!_positioningBehaviour.IsFounding) 
				{
					_positioningBehaviour.StartLocalization ();
				} 
				else 
				{
					_positioningBehaviour.StopLocalization ();
				}
            }
	}
}
