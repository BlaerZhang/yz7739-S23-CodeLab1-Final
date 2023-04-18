using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector]
    public bool isInGame = true;
    
    void Start()
    {
        instance = this;
        isInGame = true;
    }
    
}
