using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class countdown : MonoBehaviour
{
    private TextMeshProUGUI TMP;
    private float time_left;
    // Start is called before the first frame update
    void Start()
    {
        time_left = MOSPlayer.placement_time;
    }

    // Update is called once per frame
    void Update()
    {
        if (time_left > 0) {
            time_left = (MOSPlayer.placement_time - MOSPlayer.timer) ;
            TMP = GetComponent<TextMeshProUGUI>();
            TMP.text = $"{(int)(time_left + 1)}";
        }
    }
}


