using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
// using SplatterSystem;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class HitDetect : MonoBehaviour
{
    private TextMeshProUGUI endingText;

    [FormerlySerializedAs("handJoint")] public HingeJoint2D rivalHandJoint;

    [FormerlySerializedAs("footJoint")] public HingeJoint2D rivalFootJoint;

    public ActionForceScriptableObject actionForce;

    public GameObject body;

    private Rigidbody2D bodyRB2D;

    private MMF_Player hitBodyFeedback;

    private MMF_Player hitWeaponFeedback;

    public GameObject bloodStain;

    public GameObject player1BambooBloodStain;
    
    public GameObject player2BambooBloodStain;

    // private TrailRenderer weaponTrail;
    
    // public BitmapSplatterManager splatterManager;

    private Rigidbody2D weaponRb2D;

    private bool isHit = false;

    private float hitCoolDown = 0;

    void Start()
    {
        // hitBodyFeedback = GetComponentInParent<MMF_Player>();
        weaponRb2D = GetComponent<Rigidbody2D>();
        bodyRB2D = body.GetComponent<Rigidbody2D>();
        hitBodyFeedback = GameObject.Find("Hit Body Feedback").GetComponent<MMF_Player>();
        hitWeaponFeedback = GameObject.Find("Hit Weapon Feedback").GetComponent<MMF_Player>();
        endingText = GameObject.Find("Ending Text").GetComponent<TextMeshProUGUI>();
        // weaponTrail = GetComponentInChildren<TrailRenderer>();
        player1BambooBloodStain.SetActive(false);
        player2BambooBloodStain.SetActive(false);
    }
    
    void FixedUpdate()
    {
        // TrailSwitch();
        if (rivalFootJoint.enabled == false && rivalHandJoint.enabled == false)
        {
            GameManager.instance.isInGame = false;
            if (gameObject.CompareTag("Player 1 Weapon"))
            {
                endingText.text = "Left Wins";
            }
            
            if (gameObject.CompareTag("Player 2 Weapon"))
            {
                endingText.text = "Right Wins";
            }
        }

        if (isHit)
        {
            hitCoolDown += Time.deltaTime;
        }

        if (hitCoolDown >= 0.5f)
        {
            isHit = false;
            hitCoolDown = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (GameManager.instance.isInGame && isHit == false)
        {
            if (col.GetContact(0).relativeVelocity.magnitude >= 10f)
            {
                if(col.gameObject.CompareTag("Player 1 Upper Body") || col.gameObject.CompareTag("Player 2 Upper Body"))
                {
                    isHit = true;
                    Vector3 hitPoint = col.GetContact(0).point;
                    hitBodyFeedback.transform.position = hitPoint;
                    hitBodyFeedback.PlayFeedbacks();
                    rivalHandJoint.enabled = false;
                    int orderInLayer = col.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
                    bloodStain.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer + 1;
                    Instantiate<GameObject>(bloodStain, hitPoint, Quaternion.identity, col.gameObject.transform);
                    // if (col.gameObject.name == "Body" || col.gameObject.name == "Head")
                    // {
                    //     Instantiate<GameObject>(bloodStain, hitPoint, Quaternion.identity, col.gameObject.transform);
                    // }
                    // splatterManager.Spawn(hitPoint);
                    ShowBambooBloodStain();
                    
                }
        
                if(col.gameObject.CompareTag("Player 1 Lower Body") || col.gameObject.CompareTag("Player 2 Lower Body"))
                {
                    isHit = true;
                    Vector3 hitPoint = col.GetContact(0).point;
                    hitBodyFeedback.transform.position = hitPoint;
                    hitBodyFeedback.PlayFeedbacks();
                    rivalFootJoint.enabled = false;
                    int orderInLayer = col.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
                    bloodStain.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer + 1;
                    Instantiate<GameObject>(bloodStain, hitPoint, Quaternion.identity, col.gameObject.transform);
                    // splatterManager.Spawn(hitPoint);
                    ShowBambooBloodStain();
                }

                if (col.gameObject.CompareTag("Player 1 Weapon") || col.gameObject.CompareTag("Player 2 Weapon"))
                {
                    isHit = true;
                    hitWeaponFeedback.transform.position = col.GetContact(0).point;
                    hitWeaponFeedback.PlayFeedbacks();
                    
                    weaponRb2D.AddForceAtPosition(col.relativeVelocity * actionForce.weaponContactForce,col.GetContact(0).point, ForceMode2D.Impulse);

                    if (body.CompareTag("Player 1 Upper Body"))
                    {
                        // print("body force!");
                        bodyRB2D.AddForce(Vector2.left * actionForce.weaponContactBodyForce, ForceMode2D.Impulse);
                    }
                    
                    if (body.CompareTag("Player 2 Upper Body"))
                    {
                        // print("body force!");
                        bodyRB2D.AddForce(Vector2.right * actionForce.weaponContactBodyForce, ForceMode2D.Impulse);
                    }
                }
            }
        }
    }

    void ShowBambooBloodStain()
    {
        if (gameObject.CompareTag("Player 1 Weapon"))
        {
            player2BambooBloodStain.SetActive(true);
        }
        
        if (gameObject.CompareTag("Player 2 Weapon"))
        {
            player1BambooBloodStain.SetActive(true);
        }
    }

    // void TrailSwitch()
    // {
    //     if (weaponRb2D.angularVelocity > 5f)
    //     {
    //         weaponTrail.enabled = true;
    //     }
    //     else
    //     {
    //         weaponTrail.enabled = false;
    //     }
    // }

}
