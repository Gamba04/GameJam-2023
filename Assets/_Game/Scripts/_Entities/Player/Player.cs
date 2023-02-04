using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
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
    [SerializeField]
    private float deceleration;

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

    #region Input

    protected virtual void OnMovement(Vector2 input)
    {
        // Get velocity
        Vector2 velocity = new Vector2(rb.velocity.x, rb.velocity.z);

        if (input.magnitude > 0) // Moving
        {
            // Acceleration
            velocity += input * acceleration * Time.deltaTime;

            // Speed limit
            if (velocity.magnitude > speed) velocity.SetMagnitude(speed);
        }
        else // Braking
        {
            // Deceleration
            float currentSpeed = velocity.magnitude;

            currentSpeed -= deceleration * Time.deltaTime;
            currentSpeed = Mathf.Max(currentSpeed, 0);

            velocity.SetMagnitude(currentSpeed);
        }
    }

    protected virtual void OnJump()
    {

    }

    #endregion

}