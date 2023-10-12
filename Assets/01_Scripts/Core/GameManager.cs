using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private static GameManager instance;

    public bool AiWin = false;
    public bool PlayerWin1 = false;
    public bool PlayerWin2 = false;

    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
