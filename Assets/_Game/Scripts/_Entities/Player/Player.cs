using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    [SerializeField]
    private bool active = true;

    [Header("Components")]
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private PlayerInput input;

    [Header("Settings")]
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float speed;

    public bool Active { get => active; set => active = value; }

    #region Start

    private void Start()
    {
        EventsStart();
    }

    private void EventsStart()
    {
        input.onMovement += OnMovement;
        input.onJump += OnJump;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Mechanics

    protected virtual void OnMovement(Vector2 input)
    {
        input.Normalize();

        // Get velocity
        Vector2 velocity = new Vector2(rb.velocity.x, rb.velocity.z);

        if (input.magnitude > 0) // Moving
        {
            // Acceleration
            velocity += input * acceleration * Time.deltaTime;

            // Speed limit
            if (velocity.magnitude > speed) velocity = velocity.normalized * speed;
        }
        else // Braking
        {
            // Deceleration
            float currentSpeed = velocity.magnitude;

            currentSpeed -= acceleration * Time.deltaTime;
            currentSpeed = Mathf.Max(currentSpeed, 0);

            velocity = velocity.normalized * currentSpeed;
        }

        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.y);
    }

    protected virtual void OnJump()
    {

    }

    #endregion

}