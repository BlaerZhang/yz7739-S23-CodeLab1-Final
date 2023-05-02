using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TargetGroupDetacher : MonoBehaviour
{

    private CinemachineTargetGroup tG;
    private bool isDetached = false;

   void Start()
    {
        tG = GetComponent<CinemachineTargetGroup>();
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isInRound && !isDetached)
        {
            tG.RemoveMember(tG.m_Targets[0].target);
            tG.RemoveMember(tG.m_Targets[0].target);
            tG.m_Targets[0].radius = 5.5f;
            isDetached = true;
        }
    }
}
