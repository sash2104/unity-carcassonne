using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

	public int columns = 4;
	public int rows = 4;
	private Transform boardHolder;									//A variable to store a reference to the transform of our Board object.
	private Database db;

	void Awake ()
	{
		db = GameObject.FindGameObjectWithTag ("Database").GetComponent <Database>();
	}

	//Sets up the tiles of the game board.
	void BoardSetup ()
	{
		//Instantiate Board and set boardHolder to its transform.
		boardHolder = new GameObject ("Board").transform;

		//Loop along x axis.
		for(int x = 0; x < columns; x++)
		{
			//Loop along y axis.
			for(int y = 0; y < rows; y++)
			{
				//PutTile (Random.Range (0, db.tiles.Length), x, y);
			}
		}
	}

	public void PutTile (int tile_id, int x, int y)
	{
		Vector2 gridPosition = new Vector2 (x, y);
		GameObject tileChoice = db.tiles[tile_id];

		//Instantiate tileChoice at the position with no change in rotation
		GameObject instance = Instantiate (tileChoice, gridPosition, Quaternion.identity) as GameObject;

		//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
		instance.transform.SetParent (boardHolder);
	}
		
	// TODO : tile中のmeepleの位置をposition_idに応じて修正する
	public void PutMeeple (int player_id, int x, int y, int position_id)
	{
		Vector2 gridPosition = new Vector2 (x, y);
		GameObject meepleChoice = db.meeples[player_id];

		//Instantiate tileChoice at the position with no change in rotation
		GameObject instance = Instantiate (meepleChoice, gridPosition, Quaternion.identity) as GameObject;

		//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
		instance.transform.SetParent (boardHolder);

	}

	//SetupScene calls the previous functions to lay out the game board
	public void SetupScene ()
	{
		//Creates the carcassonne board
		BoardSetup ();
	}

	void Update() {
//		if (Input.GetMouseButtonDown (0)) {
//			Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//			Debug.Log ("(" + clickPosition.x.ToString() + "," + clickPosition.y.ToString() + ")");
//		}
	}

}
