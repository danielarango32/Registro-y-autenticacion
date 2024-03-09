using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Networking;

public class Pato : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField]private Animator animator;
    [SerializeField] private TMP_Text Puntaje;
    [SerializeField] private TMP_Text TablaP;
    private bool isMoving;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private float tiempo;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject PanelGameOver;
    [SerializeField] private GameObject PanelPuntaje;
    public Usuario Usuario { get; set; }
    string url = "https://sid-restapi.onrender.com";
    public string Token { get; set; }
    public string Username { get; set; }
    void Start()
    {
        Usuario = new Usuario();
        Usuario.username = PlayerPrefs.GetString("username");
        Usuario.data = new UserData(); // Initialize the data property

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
        Puntaje.text = "Puntaje: " + Usuario.data.score;
        Debug.Log(Usuario.data.score);
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

                Usuario[] usuarios = new Usuario[10];
                Usuario[] usuariosOrganizados = usuarios.OrderByDescending(user => user.data.score).Take(5).ToArray();

                TablaP.text = usuariosOrganizados.ToString();

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

        Debug.Log("Game Over!");
        // You can load a new scene, display a game over message, or perform any other desired actions
    }
}

