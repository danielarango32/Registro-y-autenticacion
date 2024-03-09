using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;

public class Pato : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI[] tableScore;
    private bool isMoving;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private float tiempo;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject PanelGameOver;
    [SerializeField] private GameObject PanelPuntaje;
    public Usuario usuario;
    string url = "https://sid-restapi.onrender.com";
    public string Token;
    public string Username;
    void Start()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();
        usuario = scoreManager.usuario;
        Token = PlayerPrefs.GetString("token");
        Username = PlayerPrefs.GetString("username");
        //Usuario.username = PlayerPrefs.GetString("username");

        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager component not assigned!");
        }
        else
        {
            StartCoroutine(EndGameAfterDelay(tiempo));
        }

    }
    void Update()
    {
        HandleMovement();
        HandleAnimation();

    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            isMoving = true;
            scoreManager.AddScore(1);
        }
        else
        {
            isMoving = false;
        }
    }

    void HandleAnimation()
    {
        animator.SetBool("IsMoving", isMoving);
    }

    private IEnumerator EndGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PanelGameOver.gameObject.SetActive(true);
        PanelPuntaje.gameObject.SetActive(false);
        EndGame();
        StartCoroutine(Tabla());
        
    }

    private IEnumerator Tabla()
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

                Usuario[] usuarios = data.usuarios;
                Usuario[] usuariosOrganizados = data.usuarios.OrderByDescending(user => user.data.score).Take(5).ToArray();

                for (int i = 0; i < 5; i++)
                {
                    tableScore[i].text = usuariosOrganizados[i].username + usuariosOrganizados[i].data.score;
                }
                

            }
            else
            {

                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
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
                
                

            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }
    private void EndGame()
    {
        Player.gameObject.SetActive(false);
        Usuario user = new Usuario();
        user.username = PlayerPrefs.GetString("username");
        user.data.score = scoreManager.Score;
        StartCoroutine("SetScore", JsonUtility.ToJson(usuario));
        Debug.Log("Game Over!");
        // You can load a new scene, display a game over message, or perform any other desired actions
    }
}

