using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonkeyGame{
	
	public class Spawner : MonoBehaviour {

		public enum ItemType{
			TYPE_ITEM,
			TYPE_FOOD,
			TYPE_JUNK,

			
		}

		public Transform spawnPoint;

		public float heightOffset = 1f;
		
		[Range(0.1f, 5)]
		public float spawnMinRadius;

		[Range(0.1f, 5)]
		public float spawnMaxRadius;

		public int maxSpawnedFood = 3;
		public int maxSpawnedJunk  = 3;
		private int spawnedFood;
		private int spawnedJunk;

		public GameObject[] spawnItems;

		private GameObject[] food;
		private GameObject[] junk;

		private int spawnedCount;

		bool saved = false;

		// Use this for initialization
		void Start () {
			if(spawnMaxRadius <= spawnMinRadius)spawnMaxRadius = spawnMinRadius+1;			

			List<GameObject> foodList = new List<GameObject>();
			List<GameObject> junkList = new List<GameObject>();
			for(int i =0; i < spawnItems.Length; i++)
			{
				if(spawnItems[i].tag == "Edible")
					foodList.Add(spawnItems[i]);
				else
					junkList.Add(spawnItems[i]);
			}
			food = foodList.ToArray();
			junk = junkList.ToArray();
		}

		// Update is called once per frame
		void FixedUpdate() {
			if(spawnedFood < maxSpawnedFood){
				spawn(ItemType.TYPE_FOOD);
			}
			if(spawnedJunk < maxSpawnedJunk){
				spawn(ItemType.TYPE_JUNK);
			}		
		}

		void spawn(ItemType type){
			GameObject obj;
			if(type == ItemType.TYPE_FOOD){
				obj = GameObject.Instantiate( food[Random.Range(0, food.Length)], transform, false );
				spawnedFood++;
			}
			else{
				obj = GameObject.Instantiate( junk[Random.Range(0, food.Length)], transform, false );
				spawnedJunk++;
			}				
			
			obj.transform.SetPositionAndRotation( spawnPoint.position, spawnPoint.rotation);			
			obj.transform.Translate( spawnRandomPos(heightOffset) );			
			obj.GetComponent<Throwable>().OnDestroyEvent += itemDied;
		}
		Vector3 spawnRandomPos(float heightOffset = 0)
		{
			float radius = Random.Range(spawnMinRadius, spawnMaxRadius);
			float angle = Random.Range(0, 360);

			return new Vector3( radius * Mathf.Cos( Mathf.Deg2Rad * angle ), heightOffset,  radius * Mathf.Sin( Mathf.Deg2Rad * angle ));
		}

		private void itemDied(){
			spawnedFood--;
		}
	}
}
