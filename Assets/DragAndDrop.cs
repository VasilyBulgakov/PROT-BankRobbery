using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragAndDrop : MonoBehaviour {

	public float maxDist = 20;	
	public float rotationSensetivity = 25;

	public GameObject dragToTarget;
	public GameObject prefabToDrag;

	private MakePuzle gridControl;


	private Camera cam;

	GameObject pickedObj;

	bool picked = false;

	// Use this for initialization
	void Start () {
		cam = Camera.main;
		if(dragToTarget == null)
			dragToTarget = GameObject.Find("GamingCanvas");		

		gridControl = dragToTarget.GetComponent<MakePuzle>();

		if(prefabToDrag == null)
			prefabToDrag = gridControl.piece;	
		
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetMouseButton(0) )
		{			
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if( Physics.Raycast(ray, out hit, maxDist) )
			{
				Debug.Log("Hit: " + hit.collider.gameObject.name);
				Debug.DrawLine(ray.origin, hit.point, Color.red, 1);
				


				GameObject hitObj = hit.transform.gameObject;
				if( !picked && hitObj.tag == prefabToDrag.tag ) 
					pick(hitObj);
				else if( picked && hitObj.tag  == dragToTarget.tag )
					place(hitObj, hit.point);				
			}
			else
				Debug.Log("No Hit");
		}	
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if( picked && scroll != 0 )	
		{
			rotate(scroll);
		}
	}

	private void pick(GameObject hitObj)
	{	
		Rigidbody rbp = hitObj.GetComponent<Rigidbody>();
		if(rbp.constraints == RigidbodyConstraints.FreezeAll) return;

		pickedObj = hitObj;
		rbp.constraints = RigidbodyConstraints.FreezeAll;

		Vector3 pickPlaceDir =  cam.transform.forward - cam.transform.up*0.25f;

		pickedObj.transform.position = cam.transform.position + pickPlaceDir*2;
		pickedObj.transform.rotation = dragToTarget.transform.rotation;	
		
		picked = true;		
	}
	private void place(GameObject hitObj, Vector3 pos)
	{
		//to local coordinates and find grid grid pos
		var v = gridControl.localPosToGrid( hitObj.transform.InverseTransformPoint(pos) );

		Debug.Log("Local:" + pos);
		Debug.Log("Grid: x" + v.x + " y:" + v.y);

		
		Debug.Log("Piece picked: " + pickedObj.name);
		Debug.Log("Piece in place: " + gridControl.getPieceAt(v).name);
		//TODO: check if hitted grid corresponds with picked obj
		if( gridControl.tryToPutPieceAt(pickedObj, v) )
		{			
			picked = false;
			pickedObj = null;
		}
		else
			drop(pos);
	}

	private void drop(Vector3 dropPos)
	{
		Rigidbody rb = pickedObj.GetComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.None;
		rb.MovePosition(dropPos);
		picked = false;
		pickedObj = null;
	}

	private void rotate(float angle)
	{
		if( !picked ) return;
		pickedObj.transform.Rotate( pickedObj.transform.forward, angle * rotationSensetivity);

	}
}
