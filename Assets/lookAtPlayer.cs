using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class lookAtPlayer : MonoBehaviour {

	private UnityEngine.UI.Text text;
	private UnityEngine.UI.Image bg;

	Tracking.WallMarker marker;
	
	public void Start() {
		text = GetComponentInChildren<UnityEngine.UI.Text>();
		bg = GetComponentInChildren<UnityEngine.UI.Image>();

		marker = transform.parent.GetComponent<Tracking.WallMarker>();
		setText(marker.gameObject.name);
	}

	private void FixedUpdate() {
		transform.LookAt(Camera.main.transform.position);
		transform.position = marker.transform.position;
	}

	public void setText(string txt)
	{
		if(text == null) return;
		text.text = txt;
	}
	public void setHighlight(Color clr)
	{
		if(bg == null) return;
		bg.color = clr;
	}
}
