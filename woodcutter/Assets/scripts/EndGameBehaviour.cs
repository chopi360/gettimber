using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGameBehaviour : MonoBehaviour {

	public Text cuttedWood;
	
	public GameObject newRecord;

	public void SetCuttedWood(int number){

		cuttedWood.text = number.ToString ();
		if(!PlayerPrefs.HasKey("record") || PlayerPrefs.GetInt("record") <= number){
			PlayerPrefs.SetInt("record", number);
			newRecord.SetActive(true);
		}
		GooglePlayController.instance.SaveResult(number);
		GooglePlayController.instance.ShowLeaderBoard();
	}

	void OnDisable(){
		newRecord.SetActive(false);
	}
}
