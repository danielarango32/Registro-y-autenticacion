using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public ScoreManager scoreManager;
    void Start()
    {
        DontDestroyOnLoad(scoreManager);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
