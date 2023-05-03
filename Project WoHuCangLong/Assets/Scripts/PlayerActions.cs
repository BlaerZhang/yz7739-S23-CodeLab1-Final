using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerActions : MonoBehaviour
{
    public ActionForceScriptableObject actionForce;
    
    // public float bendingforceAmount = 1;

    // public float wavingforceAmount = 20;

    private Rigidbody2D bodyRb2D;

    private Rigidbody2D weaponRb2D;
    
    public GameObject body;

    public GameObject weapon;

    public GameObject grabber;

    private Rigidbody2D[] grabberRB2D;

    public GameObject handGrabber;

    public GameObject footGrabber;

    private Vector2 bendingVector2;

    private float leftJoystickVerticalAmount;

    private float climbLockDeltaAngle;

    private Vector2 wavingVector2;

    private float rightTriggerValue = 0;

    private float leftTriggerValue = 0;

    private FixedJoint2D handGrabberFixedJoint2D;

    private FixedJoint2D footGrabberFixedJoint2D;

    private bool isLocked = true;

    private GrabberCollisionDetect handColDectect;

    private GrabberCollisionDetect footColDetect;

    // private PlayerInputActions playerInputActions;
    
    void Start()
    {
        bodyRb2D = body.GetComponent<Rigidbody2D>();
        weaponRb2D = weapon.GetComponent<Rigidbody2D>();
        grabberRB2D = grabber.GetComponentsInChildren<Rigidbody2D>();
        handGrabberFixedJoint2D = handGrabber.GetComponent<FixedJoint2D>();
        footGrabberFixedJoint2D = footGrabber.GetComponent<FixedJoint2D>();
        handColDectect = handGrabber.GetComponent<GrabberCollisionDetect>();
        footColDetect = footGrabber.GetComponent<GrabberCollisionDetect>();

        // playerInputActions = new PlayerInputActions();
        // playerInputActions.Gameplay.Enable();
        // playerInputActions.Gameplay.Bending.performed += Bending;
        // playerInputActions.Gameplay.WavingWeapon.performed += WavingWeapon;
    }
    
    void FixedUpdate()
    {
        if (GameManager.instance.isInRound)
        {
            Bending();
            WavingWeapon();
            // ClimbUp();
            // ClimbDown();
            ClimbDownAntiGravityForce();
        }
        GrabberLock();
    }

    void Bending()
    {
        bodyRb2D.AddForce(actionForce.bendingForceAmount * bendingVector2, ForceMode2D.Force);
    }

    void WavingWeapon()
    {
        weaponRb2D.AddForce(actionForce.wavingForceAmount * wavingVector2, ForceMode2D.Force);
        // Quaternion rotation = Quaternion.LookRotation(Vector3.forward,wavingVector2);
        // weaponRb2D.SetRotation(rotation);
    }

    void ClimbUp()
    {
        foreach (Rigidbody2D rb2D in grabberRB2D)
        {
            rb2D.AddRelativeForce(Vector2.up * rightTriggerValue * actionForce.climbingForceAmount);
        }
    }

    void ClimbDown()
    {
        foreach (Rigidbody2D rb2D in grabberRB2D)
        {
            rb2D.AddRelativeForce(Vector2.down * leftTriggerValue * actionForce.climbingForceAmount);
        }
    }

    void ClimbDownAntiGravityForce()
    {
        if (climbLockDeltaAngle > 180 - actionForce.climbLockThresholdDeltaAngle)
        {
            foreach (Rigidbody2D rb2D in grabberRB2D)
            {
                rb2D.AddRelativeForce(Vector2.up * actionForce.climbingForceAmount);
            }
        }
    }

    public void OnBending(InputAction.CallbackContext context)
    {
        // Vector2 inputVector = context.ReadValue<Vector2>();
        // rb2D.AddForce(forceAmount * inputVector, ForceMode2D.Force);
        bendingVector2 = context.ReadValue<Vector2>();
        leftJoystickVerticalAmount = bendingVector2.y;
        // print(climbLockDeltaAngle);
        // print(leftJoystickVerticalAmount);
        // Debug.Log(bendingVector2);
    }

    public void OnWavingWeapon(InputAction.CallbackContext context)
    {
        wavingVector2 = context.ReadValue<Vector2>();
        // Debug.Log(wavingVector2);
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (!GameManager.instance.isInMatch)
        {
            GameManager.instance.ResetRound();
            GameManager.instance.player1WinsCount = 0;
            GameManager.instance.player2WinsCount = 0;
        }
    }
    
    public void OnReload(InputAction.CallbackContext context)
    {
        GameManager.instance.ResetRound();
        GameManager.instance.player1WinsCount = 0;
        GameManager.instance.player2WinsCount = 0;
    }

    public void OnClimbUp(InputAction.CallbackContext context)
    {
        rightTriggerValue = context.ReadValue<float>();
    }

    public void OnClimbDown(InputAction.CallbackContext context)
    {
        leftTriggerValue = context.ReadValue<float>();
    }

    void GrabberLock()
    {
        // Quaternion averageQuaternion =
        //     Quaternion.Lerp(handGrabber.transform.rotation, footGrabber.transform.rotation, 0.5f);

        climbLockDeltaAngle =
            Quaternion.Angle(handGrabber.transform.rotation, Quaternion.LookRotation(Vector3.forward, bendingVector2));

        if (!isLocked)
        {
            if (climbLockDeltaAngle >= actionForce.climbLockThresholdDeltaAngle &&
                climbLockDeltaAngle <= 180 - actionForce.climbLockThresholdDeltaAngle) 
            {
                handGrabberFixedJoint2D.enabled = true;
                handGrabberFixedJoint2D.connectedBody = handColDectect.GetGrabberCollisionRigidbody2D();
                footGrabberFixedJoint2D.enabled = true;
                footGrabberFixedJoint2D.connectedBody = footColDetect.GetGrabberCollisionRigidbody2D();
                isLocked = true;
            }
        
            else if (bendingVector2 == Vector2.zero)
            {
                handGrabberFixedJoint2D.enabled = true;
                handGrabberFixedJoint2D.connectedBody = handColDectect.GetGrabberCollisionRigidbody2D();
                footGrabberFixedJoint2D.enabled = true;
                footGrabberFixedJoint2D.connectedBody = footColDetect.GetGrabberCollisionRigidbody2D();
                isLocked = true;
            }
        }
       
        
        if (GameManager.instance.isInRound)
        {
            if (bendingVector2 != Vector2.zero)
            {
                if (climbLockDeltaAngle < actionForce.climbLockThresholdDeltaAngle ||
                    climbLockDeltaAngle > 180 - actionForce.climbLockThresholdDeltaAngle) 
                {
                    isLocked = false;
                    handGrabberFixedJoint2D.enabled = false;
                    footGrabberFixedJoint2D.enabled = false;
                }
            }
        }
    }
}
