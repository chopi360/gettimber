using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGameBehaviour : MonoBehaviour {

	public Text cuttedWood;
	
	public GameObject newRecord;

	public void SetCuttedWood(int number){

		cuttedWood.text = "#" + number.ToString ();
		if(!PlayerPrefs.HasKey("record") || PlayerPrefs.GetInt("record") <= number){
			Debug.Log("NUEVO RECORD!! : " + number);
			PlayerPrefs.SetInt("record", number);
			newRecord.SetActive(true);
		}

	}

	void OnDisable(){
		newRecord.SetActive(false);
	}
}
