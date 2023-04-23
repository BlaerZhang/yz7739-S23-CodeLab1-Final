using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;

//set menu name
[CreateAssetMenu(
    menuName = "Scriptable Object/Action Force",
    fileName = "New Action Force",
    order = 0
    )]

public class ActionForceScriptableObject : ScriptableObject
{
    public float bendingForceAmount;
    public float wavingForceAmount;
    public float climbingForceAmount;
    public float weaponContactForce;
    public float weaponContactBodyForce;
    // public float leftJoystickClimbLockThreshold;
    public float climbLockThresholdDeltaAngle;
}
