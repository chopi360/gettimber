using UnityEngine;
using System.Collections;

public class Trunk : MonoBehaviour {
	//private void 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public float GetHeight(){
		return gameObject.GetComponent<SpriteRenderer> ().renderer.bounds.size.y;
	}
}
