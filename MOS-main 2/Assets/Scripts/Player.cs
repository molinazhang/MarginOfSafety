using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float speed = 4f;

	private Rigidbody2D body;
	private bool moving = false;

	public static Vector2 initialPosition = new Vector2(49.5f, 1f);

	/**
    reset the position and state of player
    */
	public void reset()
	{
		body.velocity = new Vector2(0f, 0f);
		transform.position = initialPosition - new Vector2(PlayerPrefs.GetFloat("DistanceFromInitial"), 0);
		DataManager.RecordingTrial.Location = transform.position.x;
		moving = false;
	}


	void Start()
	{
		body = GetComponent<Rigidbody2D>();
		moving = true;
		reset();
	}


	void Update()
	{
		//if (Input.GetKeyDown(KeyCode.Space)) No need to press space as of new specs
		//{
		//    moving = true;
		//}
		moving = true;
		if (moving)
		{
			Vector2 movement = new Vector2(speed, body.velocity.y);
			body.velocity = movement;
		}

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Message result = GameObject.FindWithTag("Finish").GetComponent<Message>();
		ScoreDisplay score = FindObjectOfType<ScoreDisplay>();

		if (other.gameObject.layer == 6)
		{
			score.caught();
			result.caught();
		}
		else if (other.gameObject.layer == 8)
		{
			score.escaped();
			result.escaped();
		}
		Time.timeScale = 0;
		FindObjectOfType<Button>().SetAvailable();
	}
}
