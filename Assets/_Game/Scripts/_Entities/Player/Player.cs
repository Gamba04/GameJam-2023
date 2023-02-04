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
    [SerializeField]
    private Animator anim;

    [Header("Settings")]
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpSpeed;

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
        Vector3 velocity = rb.velocity;

        velocity.y = jumpSpeed;

        rb.velocity = velocity;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Collisions

    private void OnCollisionEnter(Collision collision)
    {
        CheckGroundCollision(collision, () => SetGrounded(true));

    }

    private void OnCollisionStay(Collision collision)
    {
        CheckGroundCollision(collision, () => SetGrounded(true));

    }

    private void OnCollisionExit(Collision collision)
    {
        CheckGroundCollision(collision, () => SetGrounded(false), false);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private void CheckGroundCollision(Collision collision, Action onCollision, bool checkHeight = true)
    {
        if (collision.collider.gameObject.layer == worldLayer) // World
        {
            if (checkHeight)
            {
                List<ContactPoint> contacts = new List<ContactPoint>(collision.contacts);

                if (contacts.Exists(contact => contact.point.y < transform.position.y + groundCollisionHeight))
                {
                    onCollision?.Invoke();
                }
            }
            else
            {
                onCollision?.Invoke();
            }
        }
    }

    private void SetGrounded(bool value)
    {
        grounded = value;

        //anim.SetBool("Grounded", value);
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

        Gizmos.DrawLine(pivot + Vector3.forward * width, pivot + Vector3.back * width);
        Gizmos.DrawLine(pivot + Vector3.left * width, pivot + Vector3.right * width);
    }

#endif

    #endregion

}