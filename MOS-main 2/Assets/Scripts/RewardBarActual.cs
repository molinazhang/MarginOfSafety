using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardBarActual : MonoBehaviour
{
    private GameObject greenBar;
    private TextMeshPro TMP;
    private int MAXVAL = 20;
    private GameObject text;


    void Update()
    {
        //code for reward bar
        float pointsEarned = PlayerPrefs.GetFloat("DistanceFromInitial");
        float totalPoints = MOSPlayer.total_distance / 2;

        
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
