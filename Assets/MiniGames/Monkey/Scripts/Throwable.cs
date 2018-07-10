using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonkeyGame{

	public class Throwable : MonoBehaviour {

		bool collidedWithMonkey = false;

		public UnityEngine.Events.UnityEvent onDestroyed;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if(transform.position.y  < -10)
				Destroy(gameObject);
		}
		private void OnDestroy() {
			onDestroyed.Invoke();
		}

		private void OnCollisionEnter(Collision other) {
			if(other.gameObject.tag == "Monkey")
			{
				other.gameObject.GetComponent<MonkeyActions>().interact(gameObject);
			}
		}
	}
}