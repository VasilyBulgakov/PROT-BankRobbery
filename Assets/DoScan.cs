using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.iOS;
public class DoScan : MonoBehaviour {

	public delegate void OnAllScanned(GameObject POI_Parent);
	public static OnAllScanned scannedAllPOIs;

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

	public float radius = 0.5f;

	


	private void scan()
	{

		Vector3 hit;
		if( !getHitFromScene(out hit) && !getHitFromARkit(out hit))
		{
			Destroy(instanceSphere);
			foreach(var s in instanceSpheres)
				Destroy(s);

			radius = 0.5f;
		}     

		Debug.DrawLine(Camera.main.transform.position, hit,  Color.red, 1);
		
		if(instanceSphere == null)
		{			
			instanceSphere = GameObject.Instantiate(debugSphere, hit, Quaternion.identity);
		}
		else
		{
			if( radius < 2 && sqrDist(hit, instanceSphere.transform.position) < Mathf.Pow(radius, 2) )
			{
				radius+=0.2f;				
			}
			instanceSphere.transform.position = hit;
		}
		
		instanceSphere.transform.localScale = new Vector3(radius*2, radius*2, radius*2);

		foreach(var s in instanceSpheres)
		{
			Destroy(s);
		}

		if( isAllInsideSphere(hit, radius, scanPOIs) )
		{
			scannedAllPOIs(target);
		}		
	}

	private bool getHitFromARkit(out Vector3 hitPos)
	{
		hitPos = Vector3.zero;
		var screenPosition = Camera.main.ScreenToViewportPoint(new Vector3(Screen.width/2, Screen.height/2));
		ARPoint point = new ARPoint { x = screenPosition.x, y = screenPosition.y };

		var resType = ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent;
		List<ARHitTestResult> hitResults = 
					UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resType);

		if (hitResults.Count > 0) {
			foreach (var hitResult in hitResults) {  
				hitPos = UnityARMatrixOps.GetPosition (hitResult.worldTransform);                 
				return true; 
			}
		}		
		return false;
	}

	private bool getHitFromScene(out Vector3 hitPos)
	{
		RaycastHit hit;
		if( Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward, out hit, 30.0f, 1 << 8))
		{
			hitPos = hit.point;
			return true;
		}     
		hitPos = Vector3.zero;
		return false;
	}


	private bool isAllInsideSphere(Vector3 point, float radius, List<Transform> pois)
	{
		foreach(var poi in pois)
		{
			if( sqrDist(poi.position, point ) > Mathf.Pow(radius, 2) )
				return false;
			instanceSpheres.Add( Instantiate(debugSpheres, poi.position, Quaternion.identity) );
		}
		return true;
	}

	private float sqrDist(Vector3 a, Vector3 b)
	{		
		return (Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2) + Mathf.Pow(a.z - b.z, 2));
	}

	// Update is called once per frame
	void Update () {
		
		
	}
}
