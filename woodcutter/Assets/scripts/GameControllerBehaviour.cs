using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameControllerBehaviour : MonoBehaviour {

	public Text woodCuttedText;
	public Vector3 initPos;
	public const int MAX_WOOD_SHOWN = 6;
	public List<GameObject> treeModules = new List<GameObject> ();
	public SpriteRenderer playerSprite;
	public Sprite[] playerSprites;

	public Transform playerPosLeft;
	public Transform playerPosRight;

	private int woodCuttedInt = 0;
	private bool playing = false;
	public GameObject endMenu;
	public GameObject startMenu;
	public GameObject player;
	public static GameControllerBehaviour instance;

	private List<GameObject> usedTreeModules = new List<GameObject> ();


	// Use this for initialization
	void Awake () {
		if (GameControllerBehaviour.instance == null) {
			Init();
		} else {
			Destroy(this.gameObject);
		}
	}

	void Init (){
		instance = this;
		CreateTree ();
	}

	void CreateTree (){
		for (int i = 0; i < MAX_WOOD_SHOWN; i++) {
			int elementIndex = i == 0? 0 : Random.Range(1, treeModules.Count);
			usedTreeModules.Add(treeModules[elementIndex]);
			treeModules.Remove(usedTreeModules[usedTreeModules.Count - 1]);
			float woodHeight = usedTreeModules[usedTreeModules.Count - 1].GetComponent<Trunk>().GetHeight();
			Vector3 newPos = usedTreeModules[usedTreeModules.Count - 1].transform.localPosition;
			newPos.y = initPos.y + i * woodHeight;
			usedTreeModules[usedTreeModules.Count - 1].transform.localPosition = newPos;
			usedTreeModules[usedTreeModules.Count - 1].SetActive(true);
		}
	}

	public void ResetGameplay(){
		endMenu.SetActive (false);
		playing = true;
		for (int i = 0; i < usedTreeModules.Count; i++) {
			treeModules.Add(usedTreeModules[i]);
			usedTreeModules[i].SetActive(false);
		}
		usedTreeModules.Clear ();
		CreateTree ();
		woodCuttedInt = 0;
	}

	public void StartGame(){
		startMenu.SetActive (false);
		ResetGameplay();
	}
	
	public void TouchResponse(Vector3 dir){
		if (playing) {
			string touched = dir.x > 0 ? "l" : "r";

			if(dir.x > 0){
				player.transform.position = playerPosLeft.position;
				player.transform.rotation = playerPosLeft.rotation;
			}else{
				player.transform.position = playerPosRight.position;
				player.transform.rotation = playerPosRight.rotation;
			}

			bool isDead = GetDeadState (touched, dir);
			if (!isDead) {
					woodCuttedInt++;
					woodCuttedText.text = woodCuttedInt.ToString ();
					MoveWood (dir);
					MoveAllDown ();
					AnimatePlayer();
			} else {
					endMenu.SetActive (true);
					playing = false;
					endMenu.GetComponent<EndGameBehaviour> ().SetCuttedWood (woodCuttedInt);

			}
		}
	}
	//Returns true if the click will kill the player.
	bool GetDeadState (string touched, Vector3 dir)	{
		bool toRet = false;

		Component left = usedTreeModules [0].GetComponent<TrunkLeft>();
		if (left != null) {
			toRet = touched == "l";
		}

		Component right = usedTreeModules [0].GetComponent<TrunkRight>(); 
		if (right!= null) {
			toRet = touched == "r";
		}

		return toRet;
	}

	void MoveWood (Vector3 dir)	{
		usedTreeModules [0].GetComponent<Trunk> ().SetDir (dir);
		usedTreeModules.Remove (usedTreeModules[0]);
	}

	public void RemoveFromUsed(GameObject treePart){
	
		treeModules.Add (treePart);
		usedTreeModules.Remove (treePart);
		treePart.transform.position = Vector3.zero;
		treePart.transform.localPosition = Vector3.zero;
		treePart.SetActive (false);
	}

	void MoveAllDown (){
		LoadRandWood ();
		for (int i = 0; i < usedTreeModules.Count; i++) {
			float woodHeight = usedTreeModules[i].GetComponent<Trunk>().GetHeight();
			Vector3 newPos = usedTreeModules[i].transform.localPosition;
			newPos.y = newPos.y - woodHeight;
			usedTreeModules[i].transform.localPosition = newPos;
		}
	}

	void LoadRandWood (){
		int elementIndex = Random.Range(1, treeModules.Count);
		usedTreeModules.Add(treeModules[elementIndex]);
		treeModules.Remove(usedTreeModules[usedTreeModules.Count - 1]);
		float woodHeight = usedTreeModules[usedTreeModules.Count - 1].GetComponent<Trunk>().GetHeight();
		Vector3 newPos = usedTreeModules[usedTreeModules.Count - 1].transform.localPosition;
		newPos.y = initPos.y + MAX_WOOD_SHOWN * woodHeight;
		usedTreeModules[usedTreeModules.Count - 1].transform.localPosition = newPos;
		usedTreeModules[usedTreeModules.Count - 1].SetActive(true);
	}

	void AnimatePlayer (){
		playerSprite.sprite = playerSprites [1];
		playerSprite.sprite = playerSprites [2];
		playerSprite.sprite = playerSprites [0];
	}
}
