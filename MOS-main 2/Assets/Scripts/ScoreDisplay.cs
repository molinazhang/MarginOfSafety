using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreDisplay : MonoBehaviour
{
	[Tooltip("Adjusts the number of trials per set - recommend multiples of 10")]
	public static int nTrialsPerSet = 10;
	//private Rigidbody3D body;
	//private bool score_increasing = true;
	public static int score = 0;
	private int trialScore = 0;

	private TextMeshPro TMP;
	private const int MAX_SCORE = 100;
	private const int MAX_DISTANT = 80;
	public int trialCount = 0;

	private void Awake()
	{
		TMP = GetComponent<TextMeshPro>();
		trialCount = PlayerPrefs.GetInt("Trials");
		trialCount++;
		PlayerPrefs.SetInt("Trials", trialCount);
		DisplayScores();
	}
	
	void Start()
	{
		reset();
	}

	private void DisplayScores()
	{
		TMP.text = $"Trial: {trialCount}\nScore: {trialScore}\nTotal: {score}";
	}

	/**
reset trial score, state, increasing trail count and changes color and distribution of predators if necessary
*/
	public void reset()
	{
		Predator predator = GameObject.FindWithTag("Predator").GetComponent<Predator>();
		trialScore = 0;
		//score_increasing = true;
		if (predator.shuffleColorsAfterNTrials)
		{
			//print("Trial Count: " + trialCount + " vs nTrialsPerSet" + nTrialsPerSet);
			if (trialCount == nTrialsPerSet) predator.SetColorShuffleActive(true);
		}
		if (trialCount % nTrialsPerSet == 0 && trialCount > 0)
		{
			predator.shuffle_color();
			if (trialCount >= (2 * nTrialsPerSet))
			{
				predator.change_distribution_type();
			}
			SceneManager.LoadScene("EndScreen");
		}
		//print("Trial Count: " + trialCount);
		//DisplayScores();
	}

	/**
    clean trial score when caught
    */
	public void caught()
	{
		DataManager.RecordingTrial.TrialOutcome = TrialOutcome.Captured;
		trialScore = 0;
	}

	/**
    add trial score to total score when escaped
    */
	public void escaped()
	{
		DataManager.RecordingTrial.TrialOutcome = TrialOutcome.Escaped;
		TMP.text = $"Trial: {trialCount}\nScore: {trialScore}\nTotal: {score}";
		score += trialScore;
	}

	void Update()
	{
		update_score();
	}

	public int GetTrialCount()
	{
		return PlayerPrefs.GetInt("Trials");
	}

	void update_score()
	{
		Vector2 playerPosition = GameObject.FindWithTag("Player").transform.position;

		Predator predator = GameObject.FindWithTag("Predator").GetComponent<Predator>();
		double attackingDistant = predator.get_attacking_distance();
		Vector2 predatorPosition = GameObject.FindWithTag("Predator").transform.position;

		trialScore = (int)(5 * (MAX_DISTANT - Vector2.Distance(playerPosition, predatorPosition)) /
		(MAX_DISTANT - attackingDistant)) + 1;
		//trialScore = (int)(100 * exp(1 - (MAX_DISTANT - attackingDistant) / 
		//(MAX_DISTANT - Vector2.Distance(playerPosition, predatorPosition)))) + 1
	}
}
