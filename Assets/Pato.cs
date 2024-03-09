using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class Pato : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField]private Animator animator;
    [SerializeField] private TMP_Text Puntaje;
    private bool isMoving;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private float tiempo = 30f;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject PanelGameOver;
    public Usuario Usuario { get; set; }
    string url = "https://sid-restapi.onrender.com";
    void Start()
    {
        Usuario.username = PlayerPrefs.GetString("username");

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
        EndGame();
    }

    private void EndGame()
    {
        Player.gameObject.SetActive(false);
        Debug.Log("Game Over!");
        // You can load a new scene, display a game over message, or perform any other desired actions
    }
}

