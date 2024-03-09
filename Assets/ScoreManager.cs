using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreManager : MonoBehaviour
{
    string url = "https://sid-restapi.onrender.com";
    public static ScoreManager Instance;
    public int Score = 0;
    private string Token;

    [SerializeField] private TMP_Text Puntaje;

    public Usuario usuario;

    void Start()
    {
        usuario = new Usuario();
        usuario.username = PlayerPrefs.GetString("username");
        usuario.data = new UserData();
        Token = PlayerPrefs.GetString("token");
    }
    void Awake()
    {
        //if (Instance == null)
        //{
        //Instance = this;
        //DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //Destroy(gameObject);
        //}
        
        LoadPlayerScore();
    }

    public void AddScore(int scoreToAdd)
    {

        if (usuario != null && usuario.data != null)
        {
            usuario.data.score += scoreToAdd;
            Score = usuario.data.score;
            Puntaje.text = "Puntaje: " + Score;
        }
        else
        {
            Debug.LogWarning("Usuario or Usuario.data is null!");
        }
    }
    void OnDestroy()
    {
        SavePlayerScore();
    }


    void SavePlayerScore()
    {
        PlayerPrefs.SetInt("Score", Score);
        
    }

    void LoadPlayerScore()
    {
        Score = PlayerPrefs.GetInt("Score", 0);

    }

    
}
