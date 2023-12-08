using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Modifie les mouvements")] 
    [SerializeField] private float speed = 5f;

    [Header("Modifie le saut")] 
    [SerializeField] private float gravityFactor = 1f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpTime = 0f;

    [Header("Modifie le temps où le joueur saute après avoir quitté une plateforme")] [SerializeField]
    private float hangTime = 0.1f;

    [Header("Modifie le temps où l'input de saut a été enregistré")] [SerializeField]
    private float jumpBufferLength = 0.1f;

    [Header("Modifie la rapidité pour tomber du saut")] [SerializeField]
    private float fallMultiplier = 2.5f;

    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Modifie les paramètres de la roulade")] 
    [SerializeField] private float coolDownToRoll = 2f;
    [SerializeField] private float speedRoll = 10f;
    [SerializeField] private float rollDistance = 2f;
    [SerializeField] private float timeToEnableCollider = 2f;
    
    [Header("Component à set up")] 
    [SerializeField] private Rigidbody2D playerRigidbody2D;
    [SerializeField] private Collider2D playerCollider2D;

    [SerializeField] private RaycastDetection _raycastDetection;

    [Header("Ne pas set up")]
    [SerializeField] private float timeToGetRoll;
    //[SerializeField] private float jumpBufferCounter;
    [SerializeField] private float hangTimeCounter;

    private bool onRoll;
    private bool canJump => InputReader.instance.jump && hangTimeCounter >= 0; // j'ai rajouté le jump à changer avec jumpbuffer

    private Vector2 playerVelocity;

    private void Update()
    {
        CoyoteTime();
       // JumpBuffer();
    }

    private void FixedUpdate()
    {
        ModularMovement();
        SetGravity();
        ComputeGravity();
        //CanRoll();
        if (canJump) Jump();

        playerRigidbody2D.velocity = playerVelocity;
    }

    private void ModularMovement()
    {
        if (InputReader.instance.direction.x > 0 && !_raycastDetection.stopRight ||
            InputReader.instance.direction.x < 0 && !_raycastDetection.stopLeft)
        {
            playerVelocity.x = InputReader.instance.direction.x * speed;
        }
        else
        {
            playerVelocity.x = 0;
        }
    }

    private void SetGravity()
    {
        if (_raycastDetection.isGrounded)
        {
            playerVelocity.y = 0;
        }
        else
        {
            playerVelocity.y += Physics2D.gravity.y * Time.deltaTime * gravityFactor;
        }
    }

    private void Jump()
    { 
        hangTimeCounter = 0f;
      //  jumpBufferCounter = 0f;

      StartCoroutine(WaitToJump());
      playerVelocity.y = jumpForce;
      StopCoroutine(WaitToJump());
    }

    IEnumerator WaitToJump()
    {
        yield return new WaitForSeconds(0.5f);
        InputReader.instance.jump = false;
    }
    private void ComputeGravity()
    {
        bool isFalling = playerVelocity.y < 0;
        bool isReleasingJump = playerVelocity.y > 0 && !InputReader.instance.jump;

        if (!isFalling && !isReleasingJump)
        {
            return;
        }

        float factor = isFalling ? fallMultiplier : lowJumpMultiplier;
        playerVelocity += Vector2.up * Physics2D.gravity.y * (factor - 1) * Time.deltaTime;
    }

    private void CoyoteTime()
    {
        if (_raycastDetection.isGrounded) hangTimeCounter = hangTime;
        else hangTimeCounter -= Time.deltaTime;
    }

   /* private void JumpBuffer()
    {
        if (InputReader.instance.jump)
        {
            jumpBufferCounter = jumpBufferLength; 
            InputReader.instance.jump = false;
        }
        else jumpBufferCounter -= Time.deltaTime;
    }*/

   /* private void CanRoll()
    {
        timeToGetRoll += Time.deltaTime;

        if (timeToGetRoll >= coolDownToRoll && InputReader.instance.canRoll)
        {
            StartCoroutine(RollInvincibility());
            
           transform.position += InputReader.instance.direction.x > 0 ? Vector3.right * rollDistance : Vector3.left * rollDistance;
           StopCoroutine(RollInvincibility());
           timeToGetRoll = 0;
        }
    }*/

    IEnumerator RollInvincibility()
    {
        playerCollider2D.enabled = false;
        onRoll = true;
        Debug.Log("yo");
        yield return new WaitForSeconds(timeToEnableCollider);
        onRoll = false;
        playerCollider2D.enabled = true;
        Debug.Log("yo2");
    }
}