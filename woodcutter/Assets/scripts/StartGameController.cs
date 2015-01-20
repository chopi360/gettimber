using UnityEngine;
using System.Collections;

public class StartGameController : MonoBehaviour {

	public GameObject countDownIcon;
	void OnEnable(){
		countDownIcon.SetActive(false);
	}

	void OnDisable(){
		countDownIcon.SetActive(true);
	}
}
