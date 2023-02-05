using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    protected enum State
    {
        NoInput,
        Normal,
        Special
    }

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
    protected State state;
    [ReadOnly, SerializeField]
    private bool grounded;
    [ReadOnly, SerializeField]
    private Vector3 targetDir;

    private bool groundedLeeway;

    private IInteractable interactable;

    protected virtual float TerminalVelocity => terminalVelocity;

    protected virtual bool InputsEnabled => state == State.Normal;

    protected virtual bool SpecialEnabled => true;

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
        OnUpdate();
        DirectionUpdate();
        InteractableUpdate();
    }

    protected virtual void OnUpdate() { }

    private void DirectionUpdate()
    {
        if (state == State.NoInput) return;

        Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Mathf.Min(directionSpeed * Time.deltaTime, 1));
    }

    private void InteractableUpdate()
    {
        interactable = null;

        if (state == State.NoInput) return;

        List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(transform.position, interactionRadius, interactableMask));

        foreach (Collider collider in colliders)
        {
            IInteractable target = collider.GetComponentInParent<IInteractable>();

            if (target != null && target.Enabled)
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

    protected void ChangeState(State state)
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
                anim.SetBool("Special", true);
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
                anim.SetBool("Special", false);
                break;
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Mechanics

    protected virtual void OnMovement(Vector2 input)
    {
        if (!InputsEnabled) input = Vector2.zero;

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
        yVelocity = Mathf.Max(yVelocity, -TerminalVelocity);

        rb.velocity = new Vector3(velocity.x, yVelocity, velocity.y);
    }

    protected virtual void OnJump()
    {
        if (!InputsEnabled) return;

        if (!grounded) return;

        Jump(jumpSpeed);
    }

    protected virtual void OnInteract()
    {
        if (!InputsEnabled) return;

        interactable?.Interact(this);
    }

    private void OnSpecial()
    {
        if (!InputsEnabled || !grounded) return;

        Special();
    }

    protected virtual void Special()
    {
        ChangeState(State.Special);
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
        anim.SetBool("Rooted", true);

        ChangeState(State.NoInput);
    }

    public void Unroot()
    {
        anim.SetBool("Rooted", false);

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
            onCollision?.Invoke(CheckBelowHeight(collision));
        }
    }

    private bool CheckBelowHeight(Collision collision)
    {
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f, worldLayer)) return true;

        List<ContactPoint> contacts = new List<ContactPoint>(collision.contacts);

        if (contacts.Exists(contact => contact.point.y < transform.position.y + groundCollisionHeight)) return true;

        return false;
    }

    private void SetGrounded(bool value)
    {
        if (groundedLeeway && value) return;

        grounded = value;

        anim.SetBool("Grounded", value);

        OnSetGrounded(value);
    }

    protected virtual void OnSetGrounded(bool value) { }

    protected void SetGroundedLeeway(float duration)
    {
        groundedLeeway = true;

        Timer.CallOnDelay(() => groundedLeeway = false, duration, "Grounded Leeway");
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