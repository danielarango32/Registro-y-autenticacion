using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pato : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField]private Animator animator;
    private bool isMoving;
    private int score = 0;

    void Start()
    {
        score = 0;
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
            score = score + 1;
            Debug.Log(score);
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
}

