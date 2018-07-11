using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonkeyGame{

	public class MonkeyActions : MonoBehaviour {

		public UnityEngine.Events.UnityEvent OnWellFed;

		public UnityEngine.UI.Slider slider;



		public int maxFoodEaten = 10;

		public int foodEaten = 0;
		private bool wellFed = false;

		public float hungerTimer = 3f;
		public float lastFeedingTime =0f;
		

		// Use this for initialization
		void Start () {
			if(OnWellFed == null) OnWellFed = new UnityEngine.Events.UnityEvent();
			if(slider != null){
				slider.maxValue = maxFoodEaten;
				slider.value = foodEaten;
			}			
		}
		
		// Update is called once per frame
		void Update () {			
		}

		private void FixedUpdate() {
			lastFeedingTime += Time.fixedDeltaTime;
			if(lastFeedingTime > hungerTimer)
			{	
				if(foodEaten > 0)
					updateHunger(-1);				
			}
		}
		public void interact(GameObject item){
			if(item.tag == "Edible" && !wellFed)
			{
				catchFood(item);
			}
			else
			{
				brushOffJunk(item);
			}
		}

		private void updateHunger(int change){
			foodEaten += change;
			lastFeedingTime = 0;

			if(maxFoodEaten <= foodEaten){
				wellFed = true;
				OnWellFed.Invoke();
			}

			if(slider != null)
				slider.value = foodEaten;			
		}

		private void catchFood(GameObject food)
		{
			Debug.Log("Caught food" + food.name);
			
			Destroy(food);
			updateHunger(1);
			//do catch animation
		}
		private void brushOffJunk(GameObject junk)
		{
			Debug.Log("Brushed off junk " + junk.name);
			//do brushing off junk item animaion 
		}
	}
}
