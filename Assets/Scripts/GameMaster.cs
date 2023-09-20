using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private static GameMaster instance;
    public Vector2 lastCheckPointPos;
    public GameObject currentBonfire;

    //upgrade bools
    [Header ("Upgrade Bools")]
    public bool dblJumpBool;
    public bool dashBool;
    public bool glideBool;
  

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

        }
        else
        {
            Destroy(gameObject);
        }
       
    }
  
}
