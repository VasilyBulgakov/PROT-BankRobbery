using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonkeyGame{

	public class MonkeyActions : MonoBehaviour {

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
		public void interact(GameObject item){
			if(item.tag == "Edible")
			{
				catchFood(item);
			}
			else
			{
				brushOffJunk(item);
			}
		}

		private void catchFood(GameObject food)
		{
			Debug.Log("Caught food" + food.name);
			Destroy(food);
			//do catch animation
		}
		private void brushOffJunk(GameObject junk)
		{
			Debug.Log("Brushed off junk " + junk.name);
			//do brushing off junk item animaion 
		}
	}
}
