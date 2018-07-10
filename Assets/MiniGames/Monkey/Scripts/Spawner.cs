using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonkeyGame{
		
	public class Spawner : MonoBehaviour {

		public Transform spawnPoint;
		[Range(1, 10)]
		public float spawnMinRadius;

		[Range(1, 10)]
		public float spawnMaxRadius;

		public int maxSpawnedObjects;

		public GameObject[] spawnItems;

		private List<GameObject> food;

		private int spawnedCount;

		

		// Use this for initialization
		void Start () {
			if(spawnMaxRadius <= spawnMinRadius)spawnMaxRadius = spawnMinRadius+1;
			food = new List<GameObject>();
			for(int i =0; i < spawnItems.Length; i++)
				if(spawnItems[i].tag == "Edible")
					food.Add(spawnItems[i]);
		}
		
		// Update is called once per frame
		void Update () {
			if(spawnedCount < maxSpawnedObjects){
				spawnObject(false);
			}
		}

		Vector3 spawnRandomPos(float height = 0)
		{
			float radius = Random.Range(spawnMinRadius, spawnMaxRadius);
			float angle = Random.Range(0, 360);

			return new Vector3( radius * Mathf.Cos( Mathf.Deg2Rad * angle ), height,  radius * Mathf.Sin( Mathf.Deg2Rad * angle ));
		}

		void spawnObject(bool edibleOnly){		
			GameObject obj;	
			if(edibleOnly){
				int index = Random.Range(0, food.Count);
				obj = GameObject.Instantiate(food[index], transform, false);
			}			
			else
			{	int index = Random.Range(0, spawnItems.Length);			
				obj = GameObject.Instantiate(spawnItems[index], transform, false);
			}
			
			obj.transform.SetPositionAndRotation( spawnPoint.position, spawnPoint.rotation);			
			obj.transform.Translate(spawnRandomPos(3f));

			obj.GetComponent<Throwable>().onDestroyed.AddListener(spawnedObjectDestroyed);		

			spawnedCount++;
		}

		void spawnedObjectDestroyed(){
			spawnedCount--;
			spawnObject(true);
		}
	}
}
