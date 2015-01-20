using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using System;

public class GooglePlayController : MonoBehaviour {
	public static GooglePlayController instance;
	private bool _logged = false;

	private string leaderBoardId = "leaderBoardId";

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

	public void ShowAchievements(){
		if(_logged){
			Social.ShowAchievementsUI();
		}
	}

	public bool Logged(){
		return _logged;
	}

	public void IncrementAchievement(string achievementId, int amount, Action<bool> onDone = null){
		if(_logged){
			PlayGamesPlatform.Instance.IncrementAchievement(
				achievementId, amount, (bool success) => {
				if(onDone != null){
					onDone(success);
				}	
			});
		}
	}

	public void UnlockAchievement(string achievementId, Action<bool> onDone = null){
		if(_logged){
			Social.ReportProgress(achievementId,100.0f,(bool success) =>{
				if(onDone != null){
					onDone(success);
				}	
			});
			
		}
	}
}
