using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AuthHandler : MonoBehaviour
{
    string url = "https://sid-restapi.onrender.com";
    void Start()
    {
        
    }

    public void enviarRegistro()
    {

    }

    public void enviarLogin()
    {

    }

    IEnumerator Login(string json)
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(url, json);
        yield return request.SendWebRequest;
    }
}
