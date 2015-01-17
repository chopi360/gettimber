using UnityEngine;
using System.Collections;

public class Trunk : MonoBehaviour {


	private bool move = false; 
	private bool moveToR = false;
	private Vector3 moveTo;
	private Vector3 target;
	
	// Update is called once per frame
	void Update () {
		if (move) {
			transform.position += moveTo * Time.deltaTime * 3.0f;
			transform.Rotate( new Vector3(0.0f, 0.0f, 10.0f));
			if (moveToR && transform.position.x >= target.x) {
				StopAnim ();
			}
			if(!moveToR && transform.position.x <= target.x){
				StopAnim ();
			}
		}


	}

	public void SetDir(Vector3 moveTo){
		this.moveTo = moveTo;
		moveToR = moveTo.x > 0;
		target = transform.position + moveTo;
		move = true;
	}

	public float GetHeight(){
		return gameObject.GetComponent<SpriteRenderer> ().renderer.bounds.size.y;
	}

	void StopAnim (){
		transform.rotation = Quaternion.Euler( new Vector3(0.0f, 0.0f, 0.0f));
		GameControllerBehaviour.instance.RemoveFromUsed (gameObject);
		move = false;
	}
}
