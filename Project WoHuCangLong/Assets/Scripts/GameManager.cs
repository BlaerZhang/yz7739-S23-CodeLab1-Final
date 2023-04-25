using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector]
    public bool isInGame = true;
    
    private TextMeshProUGUI endingText;

    public float player1HP = 100;

    public float player2HP = 100;
    
    public Dictionary<string, float> bodyHitMultiplier = new Dictionary<string, float>();

    public float bodyMultiplier;

    public float headMultiplier;

    public float handMultiplier;

    public float legMultiplier;

    public HingeJoint2D player1FootJoint;

    public HingeJoint2D player1HandJoint;

    public HingeJoint2D player2FootJoint;

    public HingeJoint2D player2HandJoint;

    void Start()
    {
        instance = this;
        isInGame = true;
        player1HP = player2HP;
        
        endingText = GameObject.Find("Ending Text").GetComponent<TextMeshProUGUI>();

        bodyHitMultiplier.Add("Head", headMultiplier);
        bodyHitMultiplier.Add("Body", bodyMultiplier);
        bodyHitMultiplier.Add("Hand", handMultiplier);
        bodyHitMultiplier.Add("Leg", legMultiplier);
    }

    private void Update()
    {
        // print("P1 "+player1HP);
        // print("P2 "+player2HP);
        if (player1HP <= 50)
        {
            player1FootJoint.enabled = false;
        }
        

        if (player2HP <= 50)
        {
            player2FootJoint.enabled = false;
        }
        
        if (player1HP <= 0)
        {
            isInGame = false;
            player1HandJoint.enabled = false;
            endingText.text = "Right Wins";
        }
        
        if (player2HP <= 0)
        {
            isInGame = false;
            player2HandJoint.enabled = false;
            endingText.text = "Left Wins";
        }
    }
}
