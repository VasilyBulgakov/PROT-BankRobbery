using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoScan : MonoBehaviour {

	public GameObject target;

	public GameObject debugSphere;
	public GameObject debugSpheres;
	private GameObject instanceSphere;

	private List<GameObject> instanceSpheres;
	

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
		instanceSpheres = new List<GameObject>();
	}

	private void OnGUI() {
		var s = new GUIStyle();
		s.fontSize = 20;
		if (GUI.Button(new Rect(10, Screen.height - 250, 150, 130), "ScanCenter", s))
            scan();
	}

	public float radius = 1;
	private void scan()
	{

		RaycastHit hit;
		if( !Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward, out hit, 100.0f, 1 << 8))
		{
			Destroy(instanceSphere);
			foreach(var s in instanceSpheres)
			Destroy(s);

			radius = 1;
			return;
		}
		Debug.DrawLine(Camera.main.transform.position, hit.point,  Color.red, 1);
		
		if(instanceSphere == null)
		{			
			instanceSphere = GameObject.Instantiate(debugSphere, hit.point, Quaternion.identity);
		}

		instanceSphere.transform.position = hit.point;
		instanceSphere.transform.localScale = new Vector3(radius, radius, radius);

		foreach(var s in instanceSpheres)
			Destroy(s);

		foreach(var roi in scanPOIs)
		{			
			if(sqrDist(roi.position, instanceSphere.transform.position) < radius*radius)
			{
				instanceSpheres.Add( Instantiate(debugSpheres, roi.position, Quaternion.identity) );				
			}
			Debug.Log("dist: " + sqrDist(roi.position, hit.point));
			Debug.Log("radius2: " + radius*radius);
		}
	}

	private float sqrDist(Vector3 a, Vector3 b)
	{		
		return (Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2) + Mathf.Pow(a.z - b.z, 2));
	}

	// Update is called once per frame
	void Update () {
		
		
	}
}
