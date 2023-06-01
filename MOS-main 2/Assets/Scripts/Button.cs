using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Button : MonoBehaviour
{
	public Color highlighColor = Color.yellow;
	public Color color = Color.white;

	Vector3 mouseDownScale = new Vector3(19f, 10f, 1f);
	Vector3 mouseUpScale = new Vector3(19.5f, 10.5f, 1f);
	private bool available;
	private SpriteRenderer spriteRenderer;
	private TMPro.TextMeshPro text;

	private void Awake()
	{
		available = false;
		spriteRenderer = GetComponent<SpriteRenderer>();
		text = GetComponentInChildren<TMPro.TextMeshPro>();
		spriteRenderer.enabled = false;
		text.enabled = false;
	}

	public void SetAvailable()
	{
		available = true;
		spriteRenderer.enabled = true;
		text.enabled = true;
	}

	void OnMouseEnter()
	{
		SpriteRenderer sprite = GetComponent<SpriteRenderer>();
		sprite.color = highlighColor;
	}

	void OnMouseExit()
	{
		SpriteRenderer sprite = GetComponent<SpriteRenderer>();
		sprite.color = color;
	}

	void OnMouseDown()
	{
		transform.localScale = mouseDownScale;
	}

	void OnMouseUp()
	{
		transform.localScale = mouseUpScale;
		if (!available) return;
		Message message = GameObject.FindWithTag("Finish").GetComponent<Message>();
		message.reset();

		Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
		player.reset();


		ScoreDisplay score = GameObject.FindWithTag("Score").GetComponent<ScoreDisplay>();
		score.reset();

		Predator predator = GameObject.FindWithTag("Predator").GetComponent<Predator>();
		predator.reset();
		DataManager.AddTrialToCurrentRun(DataManager.RecordingTrial);
		DataManager.SaveDataWithOverwrite();
		SceneManager.LoadScene("MarginOfSafety");


		Time.timeScale = 1f;
	}
}
