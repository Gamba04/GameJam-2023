using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector2> onMovement;
    public event Action onJump;
    public event Action onSpecial;
    public event Action onInteract;

    #region Update

    private void Update()
    {
        MovementUpdate();
        ButtonsUpdate();
    }

    private void MovementUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical);

        onMovement?.Invoke(movement);
    }

    private void ButtonsUpdate()
    {
        CheckButton("Jump", onJump);
        CheckButton("Special", onSpecial);
        CheckButton("Interact", onInteract);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private void CheckButton(string name, Action action)
    {
        if (Input.GetButtonDown(name)) action?.Invoke();
    }

    #endregion

}