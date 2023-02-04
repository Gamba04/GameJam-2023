using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector2> onMovement;
    public event Action onJump;

    #region Update

    private void Update()
    {
        MovementUpdate();
    }

    private void MovementUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical);

        onMovement?.Invoke(movement);
    }

    #endregion

}