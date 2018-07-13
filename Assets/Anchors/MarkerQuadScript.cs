using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerQuadScript : MonoBehaviour {


	GameObject quad;
	// Use this for initialization
	void Start () {
				
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void setARImage(ARReferenceImage img)
	{
		if(quad == null)
		{
			foreach(Transform child in transform)
			{
				if(child == transform) continue;
				quad = child.gameObject;
				break;
			}
		}
		name = "Quad_" + img.imageName;
		quad.transform.localScale = new Vector3(img.physicalSize, img.physicalSize, img.physicalSize);
		
		quad.GetComponent<Renderer>().material.SetTexture("_MainTex", img.imageTexture);
	}
}
