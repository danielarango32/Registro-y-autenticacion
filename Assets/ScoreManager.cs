using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreManager : MonoBehaviour
{
    string url = "https://sid-restapi.onrender.com";
    public static ScoreManager Instance;
    public int Score = 0;
    private string Token;

    public Usuario Usuario {  get; set; }

    void Start()
    {
        Usuario = new Usuario();
        Usuario.username = PlayerPrefs.GetString("username");
        Usuario.data = new UserData();
        Token = PlayerPrefs.GetString("token");
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadPlayerScore();
    }

    public void AddScore(int scoreToAdd)
    {
        if (Usuario != null && Usuario.data != null)
        {
            Usuario.data.score += scoreToAdd;
            StartCoroutine("SetScore", JsonUtility.ToJson(Usuario));
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

    IEnumerator SetScore(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(url + "/api/usuarios", json);
        request.method = "PATCH";
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("x-token", Token);
        Debug.Log("Send request SetScore");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.GetResponseHeader("Content-Type"));
            Debug.Log(request.downloadHandler.text);

            if (request.responseCode == 200)
            {
                Debug.Log("SetScore exitoso");
                AuthData data = JsonUtility.FromJson<AuthData>(request.downloadHandler.text);
                Score = data.usuario.data.score;

            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }
}
