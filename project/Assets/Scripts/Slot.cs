using UnityEngine;
using System.Collections;

public class Slot : MonoBehaviour {

	private Database db;
	private Tile nextTile;

	// Use this for initialization
	void Start () {
		db = GameObject.FindGameObjectWithTag ("Database").GetComponent <Database>();
		SetNextTile (1);
	}

	public void SetNextTile (int tileID) {
		nextTile = db.tiles [tileID].GetComponent <Tile> ();
	}

	public int getNextTileID () {
		return nextTile.tileID;
	}

	// Update is called once per frame
	void Update () {

	}

	void OnGUI () {
		GUI.Box(new Rect(10, 10, 100, 100), nextTile.tileIcon);
	}
}
