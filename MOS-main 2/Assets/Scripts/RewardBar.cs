using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RewardBar : MonoBehaviour
{
    private GameObject greenBar;
    private TextMeshPro TMP;
    private int MAXVAL = 20;
    private GameObject text;

    void Start()
    {
    }

    void Update()
    {
        //code for reward bar
        float pointsEarned = MOSPlayerTut.distance_from_initial;
        float totalPoints = MOSPlayerTut.total_distance / 2;

        
        greenBar = GameObject.Find("green_bar");
        Image greenBarImage = greenBar.GetComponent<Image>();
        greenBarImage.fillAmount =  pointsEarned / totalPoints;
        

        //code for reward text:
        
        text = GameObject.Find("score");
        TMP = text.GetComponent<TextMeshPro>();
        //displayed score normalized between 0 and MAXVAL
        int score = (int) (pointsEarned / totalPoints * MAXVAL);
        TMP.text = $"Reward: {score}";
        
    }
}
