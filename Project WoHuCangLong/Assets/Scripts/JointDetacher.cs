using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointDetacher : MonoBehaviour


{
    public HingeJoint2D player1FootJoint;

    public HingeJoint2D player1HandJoint;

    public HingeJoint2D player2FootJoint;

    public HingeJoint2D player2HandJoint;

    private float Player1HP
    {
        get { return GameManager.instance.player1HP; }
        set { GameManager.instance.player1HP = value; }
    }

    private float Player2HP
    {
        get { return GameManager.instance.player2HP; }
        set { GameManager.instance.player2HP = value; }
    }
    
    void FixedUpdate()
    {
        if (Player1HP <= 50)
        {
            player1FootJoint.enabled = false;
        }
        

        if (Player2HP <= 50)
        {
            player2FootJoint.enabled = false;
        }
        
        if (Player1HP <= 0)
        {
            player1HandJoint.enabled = false;
        }
        
        if (Player2HP <= 0)
        {
            player2HandJoint.enabled = false;
        }
    }
}
