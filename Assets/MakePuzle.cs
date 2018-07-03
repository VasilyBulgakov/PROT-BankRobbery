using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MakePuzle : MonoBehaviour {

	public delegate void OnCompletion();
	public static OnCompletion completedPuzzle;
	private const string PIECE_PREFIX = "Piece"; 

	public Vector3 offset = Vector3.zero;

	public GameObject piece;
	public int rows = 2;
	public int cols = 2;	


	public float fallChance = 0;

	public Transform explosion;
	private GameObject[,] pieces;
	private List<GameObject> fallenPieces;
	private bool completed = false;
	public bool isCompleted{
		get{
			return completed;
		}
	}



	float explosionTImeout = 1f;
	bool exploded = false;
	// Use this for initialization
	void Start () {
		fallenPieces = new List<GameObject>();
		pieces  = new GameObject[rows, cols];	
		fallChance = 0.3f;			
		

		for(int x =0; x < cols; x++)
		{
			for(int y =0; y < rows; y++)
			{
				GameObject newPiece = GameObject.Instantiate(piece, transform);
				newPiece.transform.localPosition = offset;
				newPiece.transform.Translate(-x,-y,0);				
				newPiece.name = PIECE_PREFIX + x + y;

				var ctrl = newPiece.GetComponentInChildren<TexturePieceSelection>();
				ctrl.row = y;
				ctrl.col = x;
				ctrl.rows = rows;
				ctrl.cols = cols;

				var rb = newPiece.GetComponent<Rigidbody>();				
				
				if( Random.Range(0f, 1f) < fallChance){
					fallenPieces.Add(newPiece);					
				}
				else
					rb.constraints = RigidbodyConstraints.FreezeAll;				

				pieces[x,y] = newPiece;
			}
		}
		updateCompletion();
	}
	
	
	private void FixedUpdate() {
		if( !exploded )
		{			
			if(explosionTImeout > 0)
				explosionTImeout -= Time.fixedDeltaTime;
			else
			{
				exploded = true;
				foreach(var p in fallenPieces)
				{	if(explosion != null)
						p.GetComponent<Rigidbody>().AddExplosionForce(10, explosion.position, 10, 1, ForceMode.VelocityChange);
					else
						p.GetComponent<Rigidbody>().AddForce(Vector3.forward * 10, ForceMode.VelocityChange);
				}
			}
		}
	}

	public bool tryToPutPieceAt(GameObject piece, Vector2Int gridPos)
	{
		if( getPieceAt(gridPos) == piece )
		{
			piece.transform.localPosition = gridPosToCoord(gridPos);
			piece.transform.rotation = transform.rotation;
			fallenPieces.Remove(piece);
			updateCompletion();
			return true;
		}
		return false;
	}

	public bool match(GameObject objToMatch, int grid_x, int grid_y){
		if(grid_x < 0 || grid_x > cols) return false;
		if(grid_y < 0 || grid_y > rows) return false;

		return (objToMatch == pieces[grid_x, grid_y]);
	}

	public Vector2Int localPosToGrid(Vector3 pos)
	{		
		return new Vector2Int( (int)Mathf.Floor( -pos.x),  (int)Mathf.Floor( -pos.y) );
	}

	public Vector3 gridPosToCoord(Vector2Int gridXY)
	{
		return new Vector3(-gridXY.x, -gridXY.y, 0)  + offset;	
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
