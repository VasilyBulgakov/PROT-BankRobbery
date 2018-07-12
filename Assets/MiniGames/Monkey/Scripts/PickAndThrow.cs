using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonkeyGame{
		
	public class PickAndThrow : MonoBehaviour {

		public float minSpeed = 1f;

		public float holdDist= 1f; 

		public float throwStrengthMuliplier = 0.01f;

		[Range(0, 60)]
		public float throwUpAddition = 10;

		GameObject selectedObject;

		Vector3 lastMousePos;
		float throwThreshold = 0.05f;
		Vector3 delta ;

		float startSwingTime;
		float swingDist;
		bool swingInProgress = false;


		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if( selectedObject ){					
				selectedObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * holdDist;
			}

			#if UNITY_EDITOR
			if(Input.GetMouseButton(0))
			{		
				Vector3 curMousePos = Input.mousePosition;
				if( !selectedObject ){
					pickObject(curMousePos);
				}				

				delta = curMousePos - lastMousePos;
				if(swingInProgress){
					updateSwing(delta.y);
				}					
				else {
					startSwing(delta.y);
				}

				lastMousePos = curMousePos;
			}		
			if(Input.GetMouseButtonUp(0))
			{				
				throwSelected( endSwing() * throwStrengthMuliplier / 100 );						
			}
			#else
			if(Input.touchCount > 0)
			{		
				Touch touch = Input.GetTouch(0);
				Vector3 curMousePos = touch.position;
				
				if(touch.phase == TouchPhase.Began)
				{
					if( !selectedObject )
					{
						pickObject(curMousePos);
					}
				}	
				if(touch.phase == TouchPhase.Moved)	
				{
					
					delta = curMousePos - lastMousePos;
					if(swingInProgress){
						updateSwing(delta.y);
					}					
					else {
						startSwing(delta.y);
					}
				}
				if(touch.phase == TouchPhase.Ended)
				{				
					throwSelected(endSwing()  * throwStrengthMuliplier / 100);						
				}

				lastMousePos = curMousePos;
			}
			#endif
		}


		private void startSwing(float delta){
			if(delta * Time.deltaTime < throwThreshold) return;			
			Debug.Log("Start swing");
			
			startSwingTime = Time.time;
			swingDist = 0;
			swingInProgress = true;
		}
		private void updateSwing(float delta){
			if(delta * Time.deltaTime < throwThreshold) stopSwing();
			swingDist += delta;		
		
		}		
		/// <returns> velocity of swing</returns>
		private float endSwing(){
			if(!swingInProgress) return 0;
			swingInProgress = false;
			return swingDist / (Time.time - startSwingTime);
		}
		private void stopSwing(){
			swingInProgress = false;
			swingDist = 0;
			Debug.Log("Stop swing");
		}


		private void pickObject(Vector3 pixelCoord){
			GameObject obj = clickObject(pixelCoord);
			if(obj){						
				selectedObject = obj;	
				selectedObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * holdDist;	
				ThrowableItem comp = selectedObject.GetComponent<ThrowableItem>();
				if(comp) 
					comp.holded = true;	
			}
		}

		private GameObject clickObject(Vector3 pixelCoord){
			Ray ray = Camera.main.ScreenPointToRay(pixelCoord);
			RaycastHit hit;
			
			if(Physics.Raycast(ray, out hit, 10f, 1 << LayerMask.NameToLayer("Throwable")))
			{				
				Debug.Log("clicked" + hit.collider.name);
				return hit.collider.gameObject;						
			}
			else
			{
				Debug.Log("no hit");
				Debug.DrawLine(ray.origin, ray.direction * 10f, Color.red);
			}
			return null;
		}

		private void throwSelected(float speed){
			if( !selectedObject )	return;	
			
			Vector3 vector = Camera.main.transform.forward.normalized;
			vector = Quaternion.AngleAxis(-throwUpAddition, Camera.main.transform.right) * vector;			
			vector = vector*speed + vector*minSpeed;

			Debug.Log("throw speed: " + vector.magnitude);
			selectedObject.GetComponent<Rigidbody>().AddForce(vector, ForceMode.VelocityChange);

			selectedObject.GetComponent<ThrowableItem>().holded = false;	
			selectedObject = null;			
		}
	}
}
