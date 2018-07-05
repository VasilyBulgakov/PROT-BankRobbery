using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Country : MonoBehaviour {

	

	// Use this for initialization
	void Start () {	
		Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

		Transform[] transforms = GetComponentsInChildren<Transform>();				
		List<Vector3> points = new List<Vector3>();			
		foreach(var t in transforms){			
			if(t == transform) continue;
			points.Add(t.localPosition);
		}
        mesh.SetVertices(points);


		int triangleCount = points.Count - 2;
		List<int> indices = new List<int>();
		for(int i =1; i <= triangleCount; i++)
		{
			indices.Add(0);
			indices.Add(i);
			indices.Add(i+1);
		}
		//last segment
		indices.Add(0);
		indices.Add(1);
		indices.Add(triangleCount+1);

		mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
		
		GetComponent<MeshCollider>().sharedMesh = mesh;

		// string str = "";
		// foreach(int i in indices) str = str + i + " ";		
		// Debug.Log(str);		
	}
}
