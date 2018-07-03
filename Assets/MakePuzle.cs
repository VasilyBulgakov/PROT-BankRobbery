using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tracking;
public class MakePuzle : MonoBehaviour {

	public delegate void OnCompletion();
	public static OnCompletion completedPuzzle;
	private const string PIECE_PREFIX = "Piece"; 



	public GameObject piece;
	public Vector3 pieceOffset = Vector3.zero;
	public int rows = 2;
	public int cols = 2;	


	public float fallChance = 0;

	public Transform explosion;
	public bool autoExplode = false;
	private GameObject[,] pieces;
	private List<GameObject> fallenPieces;
	private bool completed = false;
	public bool isCompleted{
		get{
			return completed;
		}
	}

	private Vector3 pieceScale;

	private Vector3 topLeftOffset;

	public float explosionForse = 5;
	float explosionTImeout = 1f;
	bool exploded = false;
	// Use this for initialization
	void Start () {		

		fallenPieces = new List<GameObject>();
		pieces  = new GameObject[rows, cols];	
		fallChance = 0.3f;			
		
		pieceScale = new Vector3(1.0f / cols, 1.0f / rows, 2.0f  / ( cols + rows ) );
		topLeftOffset = new Vector3(  transform.localScale.x / 2, transform.localScale.y / 2, 0);
 
		GetComponent<TrackableWorld>().correctEvent += onCorrection;

		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("CanvasPiece"), LayerMask.NameToLayer("Default"));
		//Physics.IgnoreLayerCollision(LayerMask.NameToLayer("CanvasPiece"), LayerMask.NameToLayer("CanvasPiece"));
		
		for(int x =0; x < cols; x++)
		{
			for(int y =0; y < rows; y++)
			{
				GameObject newPiece = GameObject.Instantiate(piece, transform);
				newPiece.transform.localPosition = gridPosToLocalCoord(new Vector2Int(x,y));
				newPiece.transform.localScale = pieceScale;
				newPiece.name = PIECE_PREFIX + x + y;

				var ctrl = newPiece.GetComponentInChildren<TexturePieceSelection>();
				ctrl.row = y;
				ctrl.col = x;
				ctrl.rows = rows;
				ctrl.cols = cols;

				var rb = newPiece.GetComponent<Rigidbody>();	
							
				
				if( Random.Range(0.01f, 1f) < fallChance){
					fallenPieces.Add(newPiece);					
				}
				
				rb.constraints = RigidbodyConstraints.FreezeAll;				

				
				pieces[x,y] = newPiece;

			}
		}
		updateCompletion();
	}
	

	
	private void FixedUpdate() {
		if( !exploded && autoExplode )
		{			
			if(explosionTImeout > 0)
				explosionTImeout -= Time.fixedDeltaTime;
			else
			{
				exploded = true;
				explode();
			}
		}
	}


	private void onCorrection(){
		if(exploded) return;		
		autoExplode = true;
	}
	public void explode(){	
		foreach(var p in fallenPieces)
		{	
			Rigidbody rb = p.GetComponent<Rigidbody>();
			rb.constraints = RigidbodyConstraints.None;
			if(explosion != null)
				rb.AddExplosionForce(explosionForse, explosion.position, 10, 1, ForceMode.VelocityChange);
			else
				rb.AddForce(Vector3.forward * explosionForse, ForceMode.VelocityChange);					
		}
	}

	public bool tryToPutPieceAt(GameObject piece, Vector2Int gridPos)
	{
		if( getPieceAt(gridPos) == piece )
		{
			piece.transform.localPosition = gridPosToLocalCoord(gridPos);
			piece.transform.rotation = transform.rotation;
			fallenPieces.Remove(piece);
			updateCompletion();
			return true;
		}
		return false;
	}

	public Vector2Int localPosToGrid(Vector3 pos)
	{		
		return new Vector2Int( (int)Mathf.Floor( (topLeftOffset.x -pos.x) / pieceScale.x),  
		(int)Mathf.Floor( (topLeftOffset.y -pos.y) / pieceScale.y) );
	}

	public Vector3 gridPosToLocalCoord(Vector2Int gridXY)
	{
		return Vector3.Scale( new Vector3(-gridXY.x, -gridXY.y, 0) + pieceOffset, pieceScale ) + topLeftOffset;	
	}
	public GameObject getPieceAt(Vector2Int gridXY)
	{
		if(gridXY.x < 0 || gridXY.x > cols || gridXY.y < 0 || gridXY.y > rows) return null;
		return pieces[gridXY.x, gridXY.y];
	}

	private void updateCompletion(){
		if(fallenPieces.Count > 0){
			completed = false;
		}
		else{
			Debug.Log("DONE!");
			completed = true;
			completedPuzzle();
		}
	}
}
