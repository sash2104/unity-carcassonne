using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using D3VRestWrapper;
using Object = System.Object;
using SimpleJSON;

public class GameManager : MonoBehaviour {

	private BoardManager boardScript;						//Store a reference to our BoardManager which will set up the level.
	private Slot slot; // show next tile here.
	private Dropdown candidateDropdown; // show next tile placement candidates.
	public static GameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.
	D3VRest http; // http rest api wrapper
	JSONNode json;

	//Awake is always called before any Start functions
	void Awake()
	{
		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);	

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);

		//Get a component reference to the attached BoardManager script
		boardScript = GetComponent<BoardManager>();
		slot = GameObject.FindGameObjectWithTag ("Panel").GetComponent <Slot>();
		candidateDropdown = GameObject.FindGameObjectWithTag ("Dropdown").GetComponent<Dropdown>();
		http = GameObject.Find("D3VRest").GetComponent<D3VRest>();

		//Call the InitGame function to initialize the first level 
		InitGame();
	}

	//Initializes the game for each level.
	void InitGame()
	{
		//Call the SetupScene function of the BoardManager script.
		boardScript.SetupScene();
	}

	JSONNode ReadInput (Object data) {
		//Data has been Received. Lets convert the returned object to JSON data
		var www = JSON.Parse(data.ToString());

		//CHeck if Error is present or not. ( and hopefully not!)
		if (www ["err"].AsBool) {
			Debug.LogError("Error Occured! : " + www["err"].ToString());
			return null;
		}
		//Check if data is present or not
		if (www ["data"] == null) {
			Debug.LogWarning("JSON data is null");
			return null;
		}
		JSONNode ret = www ["data"];
		return ret;
	}

	// setNextTileボタンが押された時に呼ばれる. 指定位置にタイルを配置. 
	public void PutTile () {
		int tile_id = slot.getNextTileID ();
		JSONNode candidates = json ["candidates"];
		int candidate_id = candidateDropdown.value;
		int x = candidates [candidate_id] [0].AsInt;
		int y = candidates [candidate_id] [1].AsInt;
		// put tile
		boardScript.PutTile (tile_id, x, y);
	}

	// setMeepleボタンが押された時に呼ばれる. 
	// TODO: 指定位置にミープルを配置. 
	public void PutMeeple () {
		int player_id = 0;
		int x = 0;
		int y = 0;
		// put meeple
		boardScript.PutMeeple (player_id, x, y, 0);
	}

	void PutTile (JSONNode tile) {
		Debug.Log(tile);
		int tile_id = tile [0].AsInt;
		int x = tile [1].AsInt;
		int y = tile [2].AsInt;
		// put tile
		boardScript.PutTile (tile_id, x, y);
	}

	void Step (Object data) {
		// read input from ai server
		json = ReadInput (data);
		 
		PutTile(json ["tile"]);

		// show next tile
		int next_tile_id = json["next_tile"].AsInt;
		slot.SetNextTile(next_tile_id);

		// show next tile placement candidates
		candidateDropdown.ClearOptions();
		JSONNode candidates = json ["candidates"];
		for (int i = 0; i < candidates.Count; i++) {
			string x_str = candidates [i][0];
			string y_str = candidates [i][1];
			candidateDropdown.options.Add(new Dropdown.OptionData { text = "(" + x_str + "," + y_str + ")" });
		}
		candidateDropdown.RefreshShownValue();
	}

	public void SendButtonClick() {
		//string hash = Random.value.ToString();
		string hash = "random";
		string url = "http://ik1-329-24771.vs.sakura.ne.jp/carcassonne?random=" + hash;
		http.GET (url, Step);
	}
}
