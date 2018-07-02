using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.iOS;
public class DoScan : MonoBehaviour {

	public delegate void OnAllScanned(GameObject POI_Parent);
	public static OnAllScanned scannedAllPOIs;

	public bool debug = false;
	public GameObject target;
	public float radius = 2f;

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




	private void FixedUpdate() {
		scan(new Vector3(Screen.width/2, Screen.height/2, 0));		
	}


	private void scan(Vector3 screenPos)
	{
		Vector3 hit;
		if( !getHitFromScene(screenPos, out hit))
		{
			if( !getHitFromARkit(screenPos, out hit) )
			{		
				if(debug)		
				{
					Destroy(instanceSphere);
					foreach(var s in instanceSpheres)
						Destroy(s);
				}

				return;
			}
		}     
		if(debug)
		{				
			Debug.DrawLine(Camera.main.transform.position, hit,  Color.red, 1);
			
			if(instanceSphere == null)
			{			
				instanceSphere = GameObject.Instantiate(debugSphere, hit, Quaternion.identity);
			}		
			else
				instanceSphere.transform.position = hit;		
			instanceSphere.transform.localScale = new Vector3(radius*2, radius*2, radius*2);
			foreach(var s in instanceSpheres)
			{
				Destroy(s);
			}
		}


		if( isAllInsideSphere(hit, radius, scanPOIs) )
		{
			scannedAllPOIs(target);			
		}		
	}

	private bool getHitFromARkit(Vector3 screenPos, out Vector3 hitPos)
	{
		hitPos = Vector3.zero;
		var screenPosition = Camera.main.ScreenToViewportPoint( screenPos );
		ARPoint point = new ARPoint { x = screenPosition.x, y = screenPosition.y };

		
		ARHitTestResultType[] resultTypes = {
			//ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingGeometry,
			ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
			// if you want to use infinite planes use this:
			//ARHitTestResultType.ARHitTestResultTypeExistingPlane,
			ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane, 
			ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane, 
			ARHitTestResultType.ARHitTestResultTypeFeaturePoint
		}; 
		
		

		foreach(var type in resultTypes)
		{
			List<ARHitTestResult> hitResults = 
					UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, type);
			if (hitResults.Count > 0) {
				foreach (var hitResult in hitResults) {  
					hitPos = UnityARMatrixOps.GetPosition (hitResult.worldTransform);                 
					return true; 
				}
			}
		}

		return false;
	}

	private bool getHitFromScene(Vector3 screenPos, out Vector3 hitPos)
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (screenPos);
		if( Physics.Raycast(ray, out hit, 30.0f, 1 << 8))
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
			if(debug)
				instanceSpheres.Add( Instantiate(debugSpheres, poi.position, Quaternion.identity) );
		}
		return true;
	}

	private float sqrDist(Vector3 a, Vector3 b)
	{		
		return (Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2) + Mathf.Pow(a.z - b.z, 2));
	}
}
