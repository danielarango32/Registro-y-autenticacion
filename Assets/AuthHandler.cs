using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System;
using System.Linq;

public class AuthHandler : MonoBehaviour
{
    string url = "https://sid-restapi.onrender.com";
    public string Token {  get; set; }
    public string Username { get; set; }

    public GameObject PanelAuth;
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
        StartCoroutine("Registro",JsonUtility.ToJson(data));
    }

    public void enviarLogin()
    {
        AuthData data = new AuthData();
        data.username = GameObject.Find("InputFieldUsername").GetComponent<TMP_InputField>().text;
        data.password = GameObject.Find("InputFieldPassword").GetComponent<TMP_InputField>().text;
        StartCoroutine("Login", JsonUtility.ToJson(data));
    }

    IEnumerator Registro(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(url+"/api/usuarios", json);
        request.method = "POST";
        request.SetRequestHeader("Content-Type","application/json");
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
        
        request.SetRequestHeader("x-token",Token);
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

                Usuario[] usuarios = new Usuario[10];
                Usuario[] usuariosOrganizados = usuarios.OrderByDescending(user => user.data.score).Take(5).ToArray();
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }

    IEnumerator Patch(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(url + "/api/usuarios", json);
        request.method = "PATCH";
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("x-token", Token);
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

            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }
}

