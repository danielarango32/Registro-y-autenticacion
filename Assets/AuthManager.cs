using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    public string Token { get; set; }
    public string Username { get; set; }

    public GameObject PanelAuth; 
    public GameObject PanelMenu;

    string url = "https://sid-restapi.onrender.com";
    void Start()
    {
        Token = PlayerPrefs.GetString("token");
        if (string.IsNullOrEmpty(Token))
        {
            Debug.Log("No hay Token");
            PanelAuth.SetActive(true);
        }
        else
        {
            Username = PlayerPrefs.GetString("username");
            StartCoroutine("GetProfile");
        }
    }

    public void enviarRegistro()
    {
        AuthData data = new AuthData();
        data.username = GameObject.Find("InputFieldUsername").GetComponent<TMP_InputField>().text;
        data.password = GameObject.Find("InputFieldPassword").GetComponent<TMP_InputField>().text;
        StartCoroutine("Registro", JsonUtility.ToJson(data));
    }

    public void enviarLogin()
    {
        AuthData data = new AuthData();
        data.username = GameObject.Find("InputFieldUsername").GetComponent<TMP_InputField>().text;
        data.password = GameObject.Find("InputFieldPassword").GetComponent<TMP_InputField>().text;
        StartCoroutine("Login", JsonUtility.ToJson(data));
    }

    public void Scene()
    {
        SceneManager.LoadScene("Juego");
    }

    IEnumerator Registro(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(url + "/api/usuarios", json);
        request.method = "POST";
        request.SetRequestHeader("Content-Type", "application/json");
        Debug.Log("Send request Registro");
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
                Debug.Log("Registro exitoso"); // no ponerlo solo en consola
                StartCoroutine("Login", json);
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }

    IEnumerator Login(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(url + "/api/auth/login", json);
        request.method = "POST";
        request.SetRequestHeader("Content-Type", "application/json");
        Debug.Log("Send request login");
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
                Debug.Log("Login exitoso"); // no ponerlo solo en consola

                AuthData data = JsonUtility.FromJson<AuthData>(request.downloadHandler.text);
                Token = data.token;
                Username = data.usuario.username;

                PlayerPrefs.SetString("token", Token);
                PlayerPrefs.SetString("username", Username);
                PanelAuth.SetActive(false);
                PanelMenu.SetActive(true);
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
                PanelAuth.SetActive(true);
            }
        }
    }

    IEnumerator GetProfile()
    {
        UnityWebRequest request = UnityWebRequest.Get(url + "/api/usuarios/" + Username);

        request.SetRequestHeader("x-token", Token);
        Debug.Log("Send request GetProfile");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {

            if (request.responseCode == 200)
            {
                Debug.Log("Get Profile exitoso"); // no ponerlo solo en consola

                AuthData data = JsonUtility.FromJson<AuthData>(request.downloadHandler.text);

                Debug.Log("El usuario " + data.usuario.username + "se encuentra autenticado y su puntaje es " + data.usuario.data.score);

                PanelMenu.SetActive(true);
            }
            else
            {
                PanelAuth.SetActive(true);
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }
}

[System.Serializable]
public class AuthData
{
    public string username;
    public string password;
    public Usuario usuario;
    public Usuario[] usuarios;
    public string token;
    public int score;
}

[System.Serializable]
public class Usuario
{
    public string _id;
    public string username;
    public UserData data;
    public Usuario()
    {
        data = new UserData();
    }

}

[System.Serializable]
public class UserData
{
    public int score;
    public string appId;
}
