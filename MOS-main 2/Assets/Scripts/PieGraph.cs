using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PieGraph : MonoBehaviour
{
    public GameObject pointsEarnedWedge;

    void Start()
    {
        float pointsEarned = ScoreDisplay.score;
        float total = 5 * ScoreDisplay.nTrialsPerSet;

        pointsEarnedWedge = GameObject.Find("PointsEarned");
        Image pointsEarnedWedgeImage = pointsEarnedWedge.GetComponent<Image>();
        pointsEarnedWedgeImage.fillAmount =  pointsEarned / total;
    }

    
}
