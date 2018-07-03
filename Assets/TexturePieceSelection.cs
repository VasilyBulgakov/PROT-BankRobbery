using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TexturePieceSelection : MonoBehaviour {



	private int texWidth;

	private int texHeight;

	public float row =0;
	public float col =0;


	 [Header("Ceil grid size")]
	public int rows =2;
	public int cols =2;


	private float scaleX = 1;
	private float scaleY = 1;




	Material material;

	// Use this for initialization
	void Start () {		
		material = GetComponent<Renderer>().material;
		texWidth = material.GetTexture("_MainTex").width;
		texHeight = material.GetTexture("_MainTex").height;		
	}
	
	// Update is called once per frame	

	void Update () {
		texWidth = material.GetTexture("_MainTex").width;
		texHeight = material.GetTexture("_MainTex").height;		

		if(rows <= 0) rows = 1;
		if(cols <= 0) cols = 1;	
		

		scaleX = 1 / (float)rows;	
		scaleY = 1 / (float)cols;


		material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));
		material.SetTextureOffset("_MainTex", new Vector2(col/rows,-row /cols - scaleY));
	}
}
