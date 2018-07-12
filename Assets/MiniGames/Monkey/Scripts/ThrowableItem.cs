using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonkeyGame{
	[RequireComponent(typeof(Rigidbody))]
	public class ThrowableItem : MonoBehaviour {
		public int nutrition = 0;
		
		float gravity = 2f;

		public delegate void DestroyEvent();
    	public event DestroyEvent OnDestroyEvent;

	
		Rigidbody rb;

		public bool holded = false;

		// Use this for initialization
		void Start () {			
 			rb = GetComponent<Rigidbody>();
		}
		
		// Update is called once per frame
		void FixedUpdate() {
			if(holded)									
				rb.MoveRotation(Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(22,0,22)*Time.fixedDeltaTime )	);
			else
				rb.AddForce( -Vector3.up * gravity * Time.fixedDeltaTime, ForceMode.VelocityChange);


			if(transform.position.y  < -10){							
				Destroy(gameObject);		
			}				
		}		

		private void OnDestroy() {
			OnDestroyEvent();
		}
		

		
		private void OnCollisionEnter(Collision other) {
			if(other.gameObject.tag == "Monkey" && !holded)
			{
				other.gameObject.GetComponent<MonkeyActions>().interact(gameObject);				
			}
		}
	}
}