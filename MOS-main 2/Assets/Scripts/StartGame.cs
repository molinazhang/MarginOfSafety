using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
	[SerializeField]
	private GameObject confidenceView;

	public void ShowConfidenceView() 
	{
		confidenceView.gameObject.SetActive(true);
	}

	public void PlayGame()
	{
		SceneManager.LoadScene("GameScene");
	}

	public void ResetGame()
	{
		PlayerPrefs.SetInt("TotalScore", 0);
		PlayerPrefs.SetInt("Trials", 0);
		DataManager.ResetTrialRun();
		SceneManager.LoadScene("MarginOfSafety");
	}
}
