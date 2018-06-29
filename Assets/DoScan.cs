using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoScan : MonoBehaviour {

	public GameObject target;

	public GameObject debugSphere;

	public List<Transform> scanPOIs;

	// Use this for initialization
	void Start () {
		foreach(var tr in target.GetComponentsInChildren<Transform>())
		{
			if(tr.CompareTag("ScanPOI"))
			{
				scanPOIs.Add(tr);
			}
		}
	}

	private void OnGUI() {
		if (GUI.Button(new Rect(10, 70, 50, 30), "ScanCenter"))
            scan();
	}
	private void scan()
	{
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		RaycastHit hit;
		Physics.Raycast(ray.origin,ray.direction,out hit, 10.0f, 1 << 8);
		Debug.DrawLine(ray.origin, hit.point,  Color.red, 1);
		GameObject.Instantiate(debugSphere, hit.point, Quaternion.identity);
	}

	// Update is called once per frame
	void Update () {
		
		
	}
}
