using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGameBehaviour : MonoBehaviour {

	public Text cuttedWood;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void SetCuttedWood(int number){
		cuttedWood.text = "#" + number.ToString ();
	}
}
