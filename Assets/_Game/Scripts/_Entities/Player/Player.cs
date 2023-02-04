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
    protected Rigidbody rb;
    [SerializeField]
    private PlayerInput input;
    [SerializeField]
    protected Animator anim;

    [Header("Settings")]
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float groundDeceleration;
    [SerializeField]
    private float airDeceleration;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float terminalVelocity;

    [Space]
    [SerializeField]
    private int worldLayer;
    [SerializeField]
    private float groundCollisionHeight;

    [Header("Info")]
    [ReadOnly, SerializeField]
    private bool grounded;

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
        input.onSpecial += OnSpecial;
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

            currentSpeed -= (grounded ? groundDeceleration : airDeceleration) * Time.deltaTime;
            currentSpeed = Mathf.Max(currentSpeed, 0);

            velocity = velocity.normalized * currentSpeed;
        }

        float yVelocity = rb.velocity.y;
        yVelocity = Mathf.Max(yVelocity, -terminalVelocity);

        rb.velocity = new Vector3(velocity.x, yVelocity, velocity.y);
    }

    protected virtual void OnJump()
    {
        if (!grounded) return;

        Vector3 velocity = rb.velocity;

        velocity.y = jumpSpeed;

        rb.velocity = velocity;

        SetGrounded(false);
    }

    protected virtual void OnSpecial()
    {
        anim.SetTrigger("Special");
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Collision Detection

    private void OnCollisionEnter(Collision collision)
    {
        CheckGroundCollision(collision, belowHeight => SetGrounded(belowHeight));
    }

    private void OnCollisionStay(Collision collision)
    {
        CheckGroundCollision(collision, belowHeight => SetGrounded(belowHeight));
    }

    private void OnCollisionExit(Collision collision)
    {
        CheckGroundCollision(collision, belowHeight => SetGrounded(false));
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private void CheckGroundCollision(Collision collision, Action<bool> onCollision)
    {
        if (collision.collider.gameObject.layer == worldLayer) // World
        {
            List<ContactPoint> contacts = new List<ContactPoint>(collision.contacts);

            bool belowHeight = contacts.Exists(contact => contact.point.y < transform.position.y + groundCollisionHeight);

            onCollision?.Invoke(belowHeight);
        }
    }

    private void SetGrounded(bool value)
    {
        grounded = value;

        anim.SetBool("Grounded", value);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        DrawGroundCollisionHeight();
    }

    private void DrawGroundCollisionHeight()
    {
        Gizmos.color = Color.black;

        float width = 2;

        Vector3 pivot = Vector3.up * groundCollisionHeight;

        Gizmos.DrawLine(transform.position + pivot + Vector3.forward * width, transform.position + pivot + Vector3.back * width);
        Gizmos.DrawLine(transform.position + pivot + Vector3.left * width, transform.position + pivot + Vector3.right * width);
    }

#endif

    #endregion

}