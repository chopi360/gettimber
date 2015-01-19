using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GooglePlayController : MonoBehaviour {
	public static GooglePlayController instance;
	private bool _logged = false;

	private string leaderBoardId = "leaderBoard";

	void Awake(){
		if (GooglePlayController.instance == null) {
			Init();
		} else {
			Destroy(this.gameObject);
		}
	}

	void Init (){
		instance = this;
		PlayGamesPlatform.Activate();
	}

	// Use this for initialization
	void Start () {
		Social.localUser.Authenticate((bool success) => {
			_logged = success;
		});
	}
	

	public void SaveResult(int points){
		if(_logged){
			Social.ReportScore(points, leaderBoardId, (bool success) => {});
		}
	}

	public void ShowLeaderBoard(){
		PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderBoardId);
	}

	public bool Logged(){
		return _logged;
	}
}
