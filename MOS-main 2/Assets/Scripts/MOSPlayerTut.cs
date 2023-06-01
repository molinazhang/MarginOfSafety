using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MOSPlayerTut : MonoBehaviour
{
    private float speed = 50f;
    public static float placement_time= 4.0f;
    public static float timer = 0.0f;
    public static Vector2 initialPosition = new Vector2(49.5f, 1f);
    private Rigidbody2D body;
    private GameObject player;
    private GameObject predator;
    private GameObject exit;
    public static float total_distance;
    public static float distance_from_predator;
    public static float distance_from_initial;

	private void Awake()
	{
        timer = 0;
	}

	private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        player = GameObject.Find("MOSPlayerTut");
        predator = GameObject.Find("Predator");
        exit = GameObject.Find("Exit");
        total_distance = Vector2.Distance(predator.transform.position, Player.initialPosition);
        player.transform.position = initialPosition;
        PlayerPrefs.SetInt("TotalScore", 0);
        PlayerPrefs.SetInt("Trials", 0);
        DataManager.ResetTrialRun();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer < placement_time) {
            distance_from_predator = player.transform.position.x - predator.transform.position.x;
            distance_from_initial = Player.initialPosition.x - player.transform.position.x;
            if (Input.GetKey(KeyCode.RightArrow) && distance_from_initial > 0) {
                Vector2 movement = new Vector2(speed, body.velocity.y);
                body.velocity = movement;
            }
            else if ( Input.GetKey(KeyCode.LeftArrow) && distance_from_predator > total_distance / 2) {
                Vector2 movement = new Vector2(-speed, body.velocity.y);
                body.velocity = movement;
            }
            else {
                body.velocity = new Vector2(0f,0f);
            }
        } else {
            body.velocity = new Vector2(0f,0f);
        }

    }

}
