using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SessionScore : MonoBehaviour
{
    private TextMeshPro TMP;
    public static int prevBest = 0;
    // Start is called before the first frame update
    void Start()

    {
        int total = 5 * ScoreDisplay.nTrialsPerSet;
        TMP = GetComponent<TextMeshPro>();
        TMP.text = $"You scored {ScoreDisplay.score} points out of a maximum of {total} points\n Previous best: {prevBest}";
        if (ScoreDisplay.score > prevBest)
        {
            prevBest = ScoreDisplay.score;
        }
        ScoreDisplay.score = 0;
    }
}
