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
	public Image	countDown;

	public Transform playerPosLeft;
	public Transform playerPosRight;

	private int woodCuttedInt = 0;

	public GameObject endMenu;
	public GameObject startMenu;
	public GameObject player;
	public GameObject creditsScreen;
	public static GameControllerBehaviour instance;

	private float timeToDeathConst;
	public float timePerCut = 0.05f;
	public float timeToDeath = 15.0f;
	private float timePlayed;
	private float timeAnimation = 0.0f;

	private enum states{
		none,
		pause,
		idle,
		animation_cut,
		dead

	}

	private states state = states.none;

	private List<GameObject> usedTreeModules = new List<GameObject> ();


	// Use this for initialization
	void Awake () {
		if (GameControllerBehaviour.instance == null) {
			Init();
		} else {
			Destroy(this.gameObject);
		}
	}

	void Update(){
		switch(state){
			case states.animation_cut:
				AnimatePlayer();
			break;

			case states.dead:
				playerSprite.sprite = playerSprites[3];
			break;

			case states.idle:
				AnimationIdle();
			break;
		}

		if(state != states.none && state != states.dead && state != states.pause){
			timeToDeath -= Time.deltaTime * 2.0f;
			countDown.fillAmount = timeToDeath / timeToDeathConst;
			if (timeToDeath <= 0) {
				endMenu.SetActive (true);
				endMenu.GetComponent<EndGameBehaviour> ().SetCuttedWood (woodCuttedInt);
				state = states.dead;
			}
		}


	}



	void Init (){
		instance = this;
		timeToDeathConst = timeToDeath;
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
		state = states.idle;
		timeToDeath = timeToDeathConst;
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
		if(state != states.none && state != states.dead && state != states.pause){
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
				timeAnimation = 0.0f;
				state = states.animation_cut;
				player.GetComponent<AudioSource>().Play();
				woodCuttedInt++;
				ProcessAchievements ();
				woodCuttedText.text = woodCuttedInt.ToString ();
				MoveWood (dir);
				MoveAllDown ();
				timeToDeath = timePerCut + timeToDeath > 15.0f? 15.0f : timePerCut + timeToDeath;
		
			} else {
					state = states.dead;
					endMenu.SetActive (true);
					
					endMenu.GetComponent<EndGameBehaviour> ().SetCuttedWood (woodCuttedInt);

			}
		}
	}

	void ProcessAchievements ()	{
		string unlockedAchievement = "";
		switch (woodCuttedInt) {
		case 1:
			unlockedAchievement = "CgkIscyVsr8eEAIQAg";
			break;
		case 10:
			unlockedAchievement = "CgkIscyVsr8eEAIQBA";
			break;
		case 50:
			unlockedAchievement = "CgkIscyVsr8eEAIQBQ";
			break;
		case 100:
			unlockedAchievement = "CgkIscyVsr8eEAIQBg";
			break;
		case 500:
			unlockedAchievement = "CgkIscyVsr8eEAIQBw";
			break;
		}
		if (!string.IsNullOrEmpty (unlockedAchievement)) {
			GooglePlayController.instance.UnlockAchievement (unlockedAchievement);
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
		timeAnimation += Time.deltaTime;
		if( timeAnimation > 0.00f){
			playerSprite.sprite = playerSprites [1];
		}
		if( timeAnimation > 0.10f){
			playerSprite.sprite = playerSprites [2];
		}

		if(timeAnimation > 0.20f){	
			playerSprite.sprite = playerSprites [0];
			state = states.idle;
			timeAnimation = 0.0f;
		}
	}

	void AnimationIdle (){
		timeAnimation += Time.deltaTime;
		if( timeAnimation > 0.00f){
			playerSprite.sprite = playerSprites [0];
		}
		if( timeAnimation > 0.40f){
			playerSprite.sprite = playerSprites [4];
		}
		if(timeAnimation > 0.8f){	
			timeAnimation = 0.0f;
		}
	}

	public void ShowHideCredits(){
		creditsScreen.SetActive(!creditsScreen.activeSelf);
		if(creditsScreen.activeSelf){
			GooglePlayController.instance.UnlockAchievement("CgkIscyVsr8eEAIQAw");
		}
	}
}
