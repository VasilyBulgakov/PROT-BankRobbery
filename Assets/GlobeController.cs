using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GlobeController : MonoBehaviour {
	
	public GameObject rotateTarget;	

	//[SerializeField]
	private GameObject[] countries;
	public GameObject[] targetCountries;

	public List<GameObject> selected;	

	public float mouseSensetivity = 0.1f;

	

	[Header("Time for hold before you can rotate")]
	public float holdForRotateDelay = 0.5f;
	private float holding;

	public float selectOffset = 1.0f;


	Vector3 lastMousePos = Vector3.zero;
	Vector3 deltaDrag = Vector3.zero;


	private int needToSelect;
	private int wrongSelect;
	// Use this for initialization
	void Start () {	
		selected = new List<GameObject>();

		holding = holdForRotateDelay;

		GameObject holder = GameObject.Find("Countries");
		countries = new GameObject[holder.transform.childCount];
		for(int i =0; i < countries.Length; i++) 
			countries[i] = holder.transform.GetChild(i).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetMouseButton(0))
		{			
			if(holding < 0) {				
				doMouseRotation();
			}
			else {
				holding -= Time.deltaTime;
			}
		}
		if(Input.GetMouseButtonUp(0))
		{
			if(holding > 0)
			{				
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;

				if(Physics.Raycast(ray, out hit, 10, 1 << 13 ))
				{					
					Debug.DrawLine(ray.origin, hit.point, Color.red, 1);
					select(hit.collider.gameObject);
				}
			}

			deltaDrag = Vector3.zero;
			holding = holdForRotateDelay;
		}	
		lastMousePos = Input.mousePosition;	
	// #else
	// 	if(Input.touchCount > 0)
	// 	{
	// 		Touch touch = Input.GetTouch(0);
	// 		if(touch.phase == TouchPhase.Began)
	// 		{

	// 		}
	// 		else if(touch.phase == TouchPhase.Moved)
	// 		{
	// 			deltaDrag =  touch.deltaPosition;
	// 			rotateTarget.transform.Rotate(0, deltaDrag.x * mouseSensetivity * Time.deltaTime, 0);	
	// 		}
	// 		else if(touch.phase ==  TouchPhase.Ended)
	// 		{
	// 			if(holding > 0)
	// 			{				
	// 				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	// 				RaycastHit hit;

	// 				if(Physics.Raycast(ray, out hit, 10, 1 << 13 ))
	// 				{					
	// 					Debug.DrawLine(ray.origin, hit.point, Color.red, 1);
	// 					select(hit.collider.gameObject);
	// 				}
	// 			}

	// 			deltaDrag = Vector3.zero;
	// 			holding = holdForRotateDelay;
	// 		}
	// 		lastMousePos = touch.position;	
	// 	}
	// #endif
		
	}
	private void select(GameObject obj)
	{
		//TODO: make sortedSet
		Debug.Log("select: " + obj.name);

		if( selected.Contains(obj) ) {
			selected.Remove(obj);
			obj.GetComponent<MeshRenderer>().enabled = false;			
		}
		else{			
			selected.Add(obj);
			obj.GetComponent<MeshRenderer>().enabled = true;	
		}

		needToSelect = targetCountries.Count();
		wrongSelect = 0;
		foreach(GameObject c in selected)
		{
			if( targetCountries.Contains(c) ) needToSelect--;
			else wrongSelect++;
		}
		if(wrongSelect == 0 && needToSelect  == 0)
		{
			Debug.Log("WIN");
		}		
	}

	private void doMouseRotation()
	{
		deltaDrag =  lastMousePos - Input.mousePosition;
		rotateTarget.transform.Rotate(0, deltaDrag.x * mouseSensetivity * Time.deltaTime, 0);		
	}



}
