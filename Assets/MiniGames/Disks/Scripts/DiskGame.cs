using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DiskGame
{	
	public class DiskGame : MonoBehaviour {

		public UnityEngine.Events.UnityEvent allAligned;		

		public float precision = 10;

		public List<Disk> items;
		private int aligned;

		// Use this for initialization
		void Start () {		
			foreach(Transform child in transform)
			{
				if( child.gameObject.tag == "DiskRoot" ){					
					Disk d = child.gameObject.GetComponent<Disk>();	
					items.Add(d);				
					d.onAlign.AddListener(itemGotAligned);					
				}
			}
		}

		Disk selectedObject;

		float holding;
		Vector3 clickDir;

		
		// Update is called once per frame
		void Update () {
			#if UNITY_EDITOR
			if(Input.GetMouseButton(0))
			{		
				if( !selectedObject ){
					GameObject obj = selectObject(Input.mousePosition);
					if(obj){						
						selectedObject = obj.GetComponent<Disk>();
					}
				}	

				if(holding < 0 && selectedObject) {
					rotateByPixelCoordMove(Input.mousePosition);
				}
				else {	
					holding -= Time.deltaTime;
				}
			}		
			if(Input.GetMouseButtonUp(0))
			{
				selectedObject = null;
				checkAlignment();
			}	
			#else
			if(Input.touchCount > 0)
			{		
				Touch touch = Input.GetTouch(0);

				if(touch.phase == TouchPhase.Began)
				{						
					GameObject obj = selectObject(new Vector3(touch.position.x, touch.position.y));
					if(obj){						
						selectedObject = obj.GetComponent<Disk>();
					}					
				}
				if(touch.phase == TouchPhase.Moved)	
				{
					rotateByPixelCoordMove(new Vector3(touch.position.x, touch.position.y));
				}
				if(touch.phase == TouchPhase.Ended)	
				{
					selectedObject = null;
					checkAlignment();
				}			
			}
			#endif
		}

		
		private GameObject selectObject(Vector3 pixelCoord){
			Ray ray = Camera.main.ScreenPointToRay(pixelCoord);
			RaycastHit hit;
			Debug.Log("selecting");
			if(Physics.Raycast(ray, out hit, 10f, 1 << LayerMask.NameToLayer("Disk")))
			{				
				GameObject obj = hit.collider.transform.parent.gameObject;
				Debug.Log("selected" + hit.collider.name);
				if(obj.tag == "DiskRoot")
				{				
					clickDir =  hit.point - obj.transform.position;	
					return obj;
				}
			}
			else
			{
				Debug.Log("no hit");
			}
			return null;
		}

		private void rotateByPixelCoordMove(Vector3 pixelCoord){
			Debug.Log("rotating");

			Ray ray = Camera.main.ScreenPointToRay(pixelCoord);
			RaycastHit hit;					
			if(Physics.Raycast(ray, out hit, 10f, 1 << LayerMask.NameToLayer("Disk")))
			{				
				GameObject obj = hit.collider.transform.parent.gameObject;						
				if(obj == selectedObject.gameObject)
				{
					Vector3 newDir =  hit.point - obj.transform.position;								
					selectedObject.rotateItem( Vector3.SignedAngle(clickDir, newDir, obj.transform.up), precision );	
					
					clickDir = newDir;
				}
			}					
		}

		private void itemGotAligned(GameObject alignedItem){
			Debug.Log(alignedItem.name + "isAligned");
		}

		private void checkAlignment()
		{
			int count = items.Count;
			foreach(Disk item in items){
				if(item.isAligned) count--;
			}
			if(count == 0)
			{
				allAligned.Invoke();
			}
		}
		
	}
}