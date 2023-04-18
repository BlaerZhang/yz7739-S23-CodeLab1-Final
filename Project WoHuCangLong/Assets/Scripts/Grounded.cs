using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class Grounded : MonoBehaviour
{
    public MMF_Player groundFeedback;

    private bool isGrounded = false;
    
    private TextMeshProUGUI subText;
    
    void Start()
    {
        subText = GameObject.Find("Sub Text").GetComponent<TextMeshProUGUI>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (isGrounded == false)
        {
            isGrounded = true;
            groundFeedback.PlayFeedbacks();
            subText.text += "\nPress A or Space to Restart";
        }
    }
}
