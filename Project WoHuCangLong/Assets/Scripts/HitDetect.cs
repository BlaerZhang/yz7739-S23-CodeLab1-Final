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
    // private TextMeshProUGUI endingText;

    // public HingeJoint2D rivalHandJoint;

    // public HingeJoint2D rivalFootJoint;

    public ActionForceScriptableObject actionForce;

    public GameObject body;

    public GameObject rivalBody;

    private Rigidbody2D bodyRB2D;

    private Rigidbody2D rivalBodyRB2D;

    private MMF_Player hitBodyFeedbackSmall;
    
    private MMF_Player hitBodyFeedbackMedium;
    
    private MMF_Player hitBodyFeedbackLarge;

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
        rivalBodyRB2D = rivalBody.GetComponent<Rigidbody2D>();
        hitBodyFeedbackSmall = GameObject.Find("Hit Body Feedback Small").GetComponent<MMF_Player>();
        hitBodyFeedbackMedium = GameObject.Find("Hit Body Feedback Medium").GetComponent<MMF_Player>();
        hitBodyFeedbackLarge = GameObject.Find("Hit Body Feedback Large").GetComponent<MMF_Player>();
        hitWeaponFeedback = GameObject.Find("Hit Weapon Feedback").GetComponent<MMF_Player>();
        // endingText = GameObject.Find("Ending Text").GetComponent<TextMeshProUGUI>();
        // weaponTrail = GetComponentInChildren<TrailRenderer>();
        player1BambooBloodStain.SetActive(false);
        player2BambooBloodStain.SetActive(false);
    }
    
    void FixedUpdate()
    {
        // if (rivalFootJoint.enabled == false && rivalHandJoint.enabled == false)
        // {
        //     GameManager.instance.isInGame = false;
        //     if (gameObject.CompareTag("Player 1 Weapon"))
        //     {
        //         endingText.text = "Left Wins";
        //     }
        //     
        //     if (gameObject.CompareTag("Player 2 Weapon"))
        //     {
        //         endingText.text = "Right Wins";
        //     }
        // }

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
        if (GameManager.instance.isInRound && isHit == false)
        {
            WeaponContact(col);
            if (col.gameObject.name.Contains("Head") || col.gameObject.name.Contains("Body") ||
                col.gameObject.name.Contains("Hand") || col.gameObject.name.Contains("Leg"))
            {
                float hitVelocity = col.GetContact(0).relativeVelocity.magnitude;
                // float bodyVelocity = bodyRB2D.velocity.magnitude;
                float relativeBodyVelocity = (bodyRB2D.velocity - rivalBodyRB2D.velocity).magnitude;

                if (hitVelocity >= actionForce.hitVelocityThreshold)
                {
                    isHit = true;
                    Vector3 hitPoint = col.GetContact(0).point;
                    int orderInLayer = col.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
                    bloodStain.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer + 1;
                    Instantiate<GameObject>(bloodStain, hitPoint, Quaternion.identity, col.gameObject.transform);
                    ShowBambooBloodStain();
                    float damage = actionForce.baseDamage * GetHitVelocityMultiplier(hitVelocity) *
                                   GetBodyVelocityMultiplier(relativeBodyVelocity) *
                                   GameManager.instance.bodyHitMultiplier[DetectBodyPart(col)];
                    
                    DealDamage(damage); 
                    PlayHitFeedback(damage, hitPoint);
                    
                    print(damage + "||" + "Body Velocity: " + relativeBodyVelocity + "\n" + "Hit Velocity: " + hitVelocity);
                }

                // if(col.gameObject.CompareTag("Player 1 Upper Body") || col.gameObject.CompareTag("Player 2 Upper Body"))
                    // {
                    //     isHit = true;
                    //     Vector3 hitPoint = col.GetContact(0).point;
                    //     hitBodyFeedback.transform.position = hitPoint;
                    //     hitBodyFeedback.PlayFeedbacks();
                    //     rivalHandJoint.enabled = false;
                    //     int orderInLayer = col.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
                    //     bloodStain.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer + 1;
                    //     Instantiate<GameObject>(bloodStain, hitPoint, Quaternion.identity, col.gameObject.transform);
                    //     ShowBambooBloodStain();
                    //     
                    // }
                    //
                    // if(col.gameObject.CompareTag("Player 1 Lower Body") || col.gameObject.CompareTag("Player 2 Lower Body"))
                    // {
                    //     isHit = true;
                    //     Vector3 hitPoint = col.GetContact(0).point;
                    //     hitBodyFeedback.transform.position = hitPoint;
                    //     hitBodyFeedback.PlayFeedbacks();
                    //     rivalFootJoint.enabled = false;
                    //     int orderInLayer = col.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
                    //     bloodStain.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer + 1;
                    //     Instantiate<GameObject>(bloodStain, hitPoint, Quaternion.identity, col.gameObject.transform);
                    //     ShowBambooBloodStain();
                    // }
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

    void WeaponContact(Collision2D col)
    {
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

    void DealDamage(float damage)
    {
        if (gameObject.CompareTag("Player 1 Weapon"))
        {
            GameManager.instance.player2HP -= damage;
        }

        if (gameObject.CompareTag("Player 2 Weapon"))
        {
            GameManager.instance.player1HP -= damage;
        }
    }

    string DetectBodyPart(Collision2D col)
    {
        string key = "";
        
        if (col.gameObject.name.Contains("Head"))
        {
            key = "Head";
        }

        if (col.gameObject.name.Contains("Body"))
        {
            key = "Body";
        }

        if (col.gameObject.name.Contains("Hand"))
        {
            key = "Hand";
        }

        if (col.gameObject.name.Contains("Leg"))
        {
            key = "Leg";
        }
        
        return key;
    }

    float GetHitVelocityMultiplier(float hitVelocity)
    {
        float hitVelocityMultiplier = 1;
        
        if (hitVelocity <= actionForce.smallHitVelocityMax)
        {
            hitVelocityMultiplier = actionForce.smallHitVelocityMultiplier;
        }
        else if (hitVelocity > actionForce.smallHitVelocityMax &&
                 hitVelocity <= actionForce.mediumHitVelocityMax)
        {
            hitVelocityMultiplier = actionForce.mediumHitVelocityMultiplier;
        }
        else if (hitVelocity > actionForce.mediumHitVelocityMax)
        {
            hitVelocityMultiplier = actionForce.largeHitVelocityMultiplier;
        }

        return hitVelocityMultiplier;
    }
    

    float GetBodyVelocityMultiplier(float bodyVelocity)
    {
        float bodyVelocityMultiplier = 1;
        
        if (bodyVelocity <= 5)
        {
            bodyVelocityMultiplier = 1;
        }
        else if (bodyVelocity >= 5)
        {
            bodyVelocityMultiplier = bodyVelocity / 5f;
        }
        
        return bodyVelocityMultiplier;
    }

    void PlayHitFeedback(float damage, Vector3 hitPoint)
    {
        if (damage <= 20)
        {
            hitBodyFeedbackSmall.transform.position = hitPoint;
            hitBodyFeedbackSmall.PlayFeedbacks();
        }
        else if (damage > 20 && damage <= 40) 
        {
            hitBodyFeedbackMedium.transform.position = hitPoint;
            hitBodyFeedbackMedium.PlayFeedbacks();
        }
        else if (damage > 40)
        {
            hitBodyFeedbackLarge.transform.position = hitPoint;
            hitBodyFeedbackLarge.PlayFeedbacks();
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
