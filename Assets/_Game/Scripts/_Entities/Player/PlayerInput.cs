using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector2> onMovement;
    public event Action onJump;
    public event Action onSpecial;

    #region Update

    private void Update()
    {
        MovementUpdate();
        JumpUpdate();
        SpecialUpdate();
    }

    private void MovementUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical);

        onMovement?.Invoke(movement);
    }

    private void JumpUpdate()
    {
        if (Input.GetButtonDown("Jump")) onJump?.Invoke();
    }

    private void SpecialUpdate()
    {
        if (Input.GetButtonDown("Special")) onSpecial?.Invoke();
    }

    #endregion

}