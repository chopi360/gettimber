using UnityEngine;
using System.Collections;

public class InputBehaviour : MonoBehaviour {

	public Vector3 impulse = new Vector3(0.0f,0.0f,0.0f);

	
	public void OnMouseUp(){
		GameControllerBehaviour.instance.TouchResponse (impulse);
	}

}
