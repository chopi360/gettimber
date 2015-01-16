using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControllerBehaviour : MonoBehaviour {

	public Vector3 initPos;
	public const int MAX_WOOD_SHOWN = 6;
	public List<GameObject> treeModules = new List<GameObject> ();
	private List<GameObject> usedTreeModules = new List<GameObject> ();
	public static GameControllerBehaviour instance;
	public SpriteRenderer playerSprite;
	public Sprite[] playerSprites;
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
		for (int i = 0; i < MAX_WOOD_SHOWN; i++) {
			int elementIndex = i == 0? 0 : Random.Range(0, treeModules.Count);
			usedTreeModules.Add(treeModules[elementIndex]);
			treeModules.Remove(usedTreeModules[usedTreeModules.Count - 1]);
			float woodHeight = usedTreeModules[usedTreeModules.Count - 1].GetComponent<Trunk>().GetHeight();
			Vector3 newPos = usedTreeModules[usedTreeModules.Count - 1].transform.localPosition;
			newPos.y = initPos.y + i * woodHeight;
			usedTreeModules[usedTreeModules.Count - 1].transform.localPosition = newPos;
			usedTreeModules[usedTreeModules.Count - 1].SetActive(true);
		}
	
	}
	
	public void TouchResponse(Vector3 dir){

		string touched = dir.x > 0 ? "l" : "r";
		bool isDead = GetDeadState (touched, dir);
		if (!isDead) {
			MoveWood (dir);
			MoveAllDown ();
		}
	}

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
		usedTreeModules [0].transform.localPosition = Vector3.zero;
		treeModules.Add (usedTreeModules [0]);
		usedTreeModules.Remove (usedTreeModules [0]);
		treeModules [treeModules.Count - 1].SetActive (false);

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
		int elementIndex = Random.Range(0, treeModules.Count);
		usedTreeModules.Add(treeModules[elementIndex]);
		treeModules.Remove(usedTreeModules[usedTreeModules.Count - 1]);
		float woodHeight = usedTreeModules[usedTreeModules.Count - 1].GetComponent<Trunk>().GetHeight();
		Vector3 newPos = usedTreeModules[usedTreeModules.Count - 1].transform.localPosition;
		newPos.y = initPos.y + MAX_WOOD_SHOWN * woodHeight;
		usedTreeModules[usedTreeModules.Count - 1].transform.localPosition = newPos;
		usedTreeModules[usedTreeModules.Count - 1].SetActive(true);
	}
}
