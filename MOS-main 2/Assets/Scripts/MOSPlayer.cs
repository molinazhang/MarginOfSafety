using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MOSPlayer : MonoBehaviour
{
    private float speed = 50f;
    public static float placement_time= 4.0f;
    public static float timer = 0.0f;
    private Rigidbody2D body;
    private GameObject player;
    private GameObject predator;
    private GameObject exit;
    public  static float total_distance; 
    public float distance_from_predator;//Removed Statics, the intended implementation doesn't work like that in Unity
    public float distance_from_initial;

	private void Awake()
	{
        timer = 0;
	}

	void Start()
    {
        body = GetComponent<Rigidbody2D>();
        player = gameObject;
        predator = GameObject.Find("Predator");
        exit = GameObject.Find("Exit");
        total_distance = Vector2.Distance(predator.transform.position, Player.initialPosition);
        Vector3 pos = new Vector3(Random.Range(predator.transform.position.x + total_distance / 2, Player.initialPosition.x), player.transform.position.y, player.transform.position.z);
        player.transform.position = pos;
        DataManager.StartRecordingTrial();
    }


    void Update()
    {
        timer += Time.deltaTime;
        if (timer < placement_time) {
            distance_from_predator = player.transform.position.x - predator.transform.position.x;
            distance_from_initial = Player.initialPosition.x - player.transform.position.x;
            PlayerPrefs.SetFloat("DistanceFromPredator", distance_from_predator);
            PlayerPrefs.SetFloat("DistanceFromInitial", distance_from_initial);
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
