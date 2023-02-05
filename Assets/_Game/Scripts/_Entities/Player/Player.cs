using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    private enum State
    {
        NoInput,
        Normal,
        Special,
        Rooted
    }

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
    private float directionSpeed;

    [Space]
    [SerializeField]
    private LayerMask interactableMask;
    [SerializeField]
    private float interactionRadius;
    [SerializeField]
    private int worldLayer;
    [SerializeField]
    private float groundCollisionHeight;

    [Header("Info")]
    [ReadOnly, SerializeField]
    private State state;
    [ReadOnly, SerializeField]
    private bool grounded;
    [ReadOnly, SerializeField]
    private Vector3 targetDir;

    private IInteractable interactable;

    public bool Active { get => active; set => active = value; }

    #region Start

    private void Start()
    {
        EventsStart();

        OtherStart();
    }

    private void EventsStart()
    {
        input.onMovement += OnMovement;
        input.onJump += OnJump;
        input.onSpecial += OnSpecial;
        input.onInteract += OnInteract;
    }

    private void OtherStart()
    {
        targetDir = transform.forward;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Update

    private void Update()
    {
        DirectionUpdate();
        InteractableUpdate();
    }

    private void DirectionUpdate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Mathf.Min(directionSpeed * Time.deltaTime, 1));
    }

    private void InteractableUpdate()
    {
        List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(transform.position, interactionRadius, interactableMask));

        foreach (Collider collider in colliders)
        {
            IInteractable target = collider.GetComponentInParent<IInteractable>();

            if (target != null)
            {
                interactable = target;
                break;
            }
        }

        GplayUI.OnSetInteractionOverlay(interactable != null);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region States

    private void ChangeState(State state)
    {
        if (this.state == state) return;

        OnStateExit(this.state);
        OnStateEnter(state);

        this.state = state;
    }

    private void OnStateEnter(State state)
    {
        switch (state)
        {
            case State.NoInput:

                break;

            case State.Normal:

                break;

            case State.Special:
                anim.SetTrigger("Special");
                break;

            case State.Rooted:
                anim.SetBool("Burried", true);
                break;
        }
    }

    private void OnStateExit(State state)
    {
        switch (state)
        {
            case State.NoInput:

                break;

            case State.Normal:

                break;

            case State.Special:

                break;

            case State.Rooted:
                anim.SetBool("Burried", false);
                break;
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Mechanics

    protected virtual void OnMovement(Vector2 input)
    {
        input.Normalize();

        SetDirection(input);

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

        Jump(jumpSpeed);
    }

    private void OnSpecial()
    {
        if (state != State.Special && !grounded) return;

        Special();
    }

    protected virtual void Special()
    {
        ChangeState(State.Special);
    }

    protected virtual void OnInteract()
    {
        interactable?.Interact(this);
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

    #region Public Methods

    public void Root()
    {
        ChangeState(State.Rooted);
    }

    public void Unroot()
    {
        ChangeState(State.Normal);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    protected void Jump(float speed)
    {
        Vector3 velocity = rb.velocity;

        velocity.y = speed;

        rb.velocity = velocity;

        SetGrounded(false);
    }

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

    private void SetDirection(Vector2 direction)
    {
        if (direction == Vector2.zero) return;

        targetDir = new Vector3(direction.x, 0, direction.y);
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