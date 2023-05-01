using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDripper : MonoBehaviour
{
    private ParticleSystem drippingBlood;
    private float currentPlayerHP;

    void Start()
    {
        drippingBlood = GetComponent<ParticleSystem>();
    }
    
    void Update()
    {
        var bloodEmission = drippingBlood.emission;
        
        if (CompareTag("Player 1 Upper Body"))
        {
            currentPlayerHP = GameManager.instance.player1HP;
        }
        
        if (CompareTag("Player 2 Upper Body"))
        {
            currentPlayerHP = GameManager.instance.player2HP;
        }

        if (currentPlayerHP >= 75)
        {
           bloodEmission.SetBurst(0, new ParticleSystem.Burst(0,0,0, 1));
        }

        if (currentPlayerHP >= 33 && currentPlayerHP < 75)
        {
            bloodEmission.SetBurst(0, new ParticleSystem.Burst(0,3,9999999, 1));
        }

        if (currentPlayerHP < 33)
        {
            bloodEmission.SetBurst(0, new ParticleSystem.Burst(4.5f,3,9999999, 0.01f));
        }
    }
}
