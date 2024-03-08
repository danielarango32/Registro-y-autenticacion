using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System;

public class AuthHandler : MonoBehaviour
{
    string url = "https://sid-restapi.onrender.com";
    void Start()
    {
        
    }

    public void enviarRegistro()
    {
        AuthData data = new AuthData();
        string username = GameObject.Find("InputFieldUsername").GetComponent<TMP_InputField>().text;
        string password = GameObject.Find("InputFieldPassword").GetComponent<TMP_InputField>().text;
        StartCoroutine("Registro",JsonUtility.ToJson(data));
    }

    public void enviarLogin()
    {

    }

    IEnumerator Registro(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(url+"/api/usuarios", json);
        request.method = UnityWebRequest.kHttpVerbPOST;
        request.SetRequestHeader("Content-Type","applicatio/json");
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
                StartCoroutine(Login);
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
        request.method = UnityWebRequest.kHttpVerbPOST;
        request.SetRequestHeader("Content-Type", "applicatio/json");
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
                
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }
}

public class AuthData
{
    public string username;
    public string password;
    public Usuario usuario;
}

[System.Serializable]
public class Usuario
{
    public string _id;
    public string username;
}