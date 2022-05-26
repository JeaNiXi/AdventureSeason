using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }


    public enum GlobalPlayerState
    {
        ALIVE,
        DEAD
    }
    public enum GlobalGameState
    {
        RUNNING,
        PAUSED,
        UIPAUSED
    }

    public GlobalPlayerState GPState = GlobalPlayerState.ALIVE;
    public GlobalGameState GGState = GlobalGameState.RUNNING;

    public Transform Arzued;

}
