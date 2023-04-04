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
    public TextMeshProUGUI endingText;

    [FormerlySerializedAs("handJoint")] public HingeJoint2D rivalHandJoint;

    [FormerlySerializedAs("footJoint")] public HingeJoint2D rivalFootJoint;

    public float weaponContactForce = 10;

    public MMF_Player hitBodyFeedback;

    public MMF_Player hitWeaponFeedback;

    public GameObject bloodStain;

    public GameObject player1BambooBloodStain;
    
    public GameObject player2BambooBloodStain;
    
    // public BitmapSplatterManager splatterManager;

    private Rigidbody2D weaponRb2D;

    private bool isHit = false;

    private float hitCoolDown = 0;
    // Start is called before the first frame update
    void Start()
    {
        // hitBodyFeedback = GetComponentInParent<MMF_Player>();
        weaponRb2D = GetComponent<Rigidbody2D>();
        player1BambooBloodStain.SetActive(false);
        player2BambooBloodStain.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rivalFootJoint.enabled == false && rivalHandJoint.enabled == false)
        {
            GameManager.instance.isInGame = false;
            if (gameObject.CompareTag("Player 1 Weapon"))
            {
                endingText.text = "Player 1 Wins";
            }
            
            if (gameObject.CompareTag("Player 2 Weapon"))
            {
                endingText.text = "Player 2 Wins";
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
                    weaponRb2D.AddForceAtPosition(col.relativeVelocity * weaponContactForce,col.GetContact(0).point, ForceMode2D.Impulse);
                }
            }
            
            // if (col.gameObject.CompareTag("Ground"))
            // {
            //     GameManager.instance.isInGame = false;
            //     if (gameObject.CompareTag("Player 1 Weapon"))
            //     {
            //         endingText.text = "Player 2 Wins";
            //     }
            //
            //     if (gameObject.CompareTag("Player 2 Weapon"))
            //     {
            //         endingText.text = "Player 1 Wins";
            //     }
            // }
            
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

}
