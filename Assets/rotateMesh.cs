using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class rotateMesh : MonoBehaviour {

	
	public Transform theObject;
	private Mesh theMesh;
	private Vector3[] originalVerts;
	private Vector3[] rotatedVerts;
	public float rotatedAngleY  = 0.0f;
 
 void Start() 
 {
     if (!theObject)
     {
         theObject = this.transform;
     }
     theMesh = theObject.GetComponent<MeshFilter>().mesh as Mesh;
     
     originalVerts = new Vector3[ theMesh.vertices.Length ];
     originalVerts = theMesh.vertices;
     
     rotatedVerts = new Vector3[ originalVerts.Length ];

	 RotateMesh();
 }
	
	void Update() 
	{
	}
	
	void RotateMesh() 
	{
		Quaternion qAngle  = Quaternion.AngleAxis( rotatedAngleY, Vector3.up );
		for (int vert  = 0; vert < originalVerts.Length; vert ++)
		{
			rotatedVerts[vert] = qAngle * originalVerts[vert];
		}
		
		theMesh.vertices = rotatedVerts;
	}
}
