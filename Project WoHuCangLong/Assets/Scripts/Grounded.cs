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
            if (GameManager.instance.isInMatch)
            {
                subText.text += "\n" + GameManager.instance.player1WinsCount + " - " +
                                GameManager.instance.player2WinsCount + "\nGet Ready for the Next Round";
            }
            else
            {
                subText.text += "\n" + GameManager.instance.player1WinsCount + " - " +
                                GameManager.instance.player2WinsCount + "\nPress A or Space to Start a New Match";
            }
            
        }
    }
}
