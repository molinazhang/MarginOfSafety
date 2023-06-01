using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Message : MonoBehaviour
{
    private TextMeshPro TMP;

    /**
    display result when caught
    */
    public void caught()
    {
        TMP.color = new Color32(255,0,0,255);
        TMP.text = "You are caught!";
    }

    /**
    display result when escaped
    */
    public void escaped()
    {
        TMP.color = new Color32(0,255,0,255);
        TMP.text = "You have escaped!";
    }

    /**
    reset the result to empty
    */
    public void reset()
    {
        //TMP.text = "Press Space Key to Escape";
       // TMP.color = new Color32(255,255,255,255);
    }
    
    void Start()
    {
        //body = GetComponent<Rigidbody3D>();
        TMP = GetComponent<TextMeshPro>();
        reset();
    }

    void Update()   
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    TMP.text = "";
        //}
    }
}
