using System;
using System.Collections;
using System.Collections.Generic;
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
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical);

        if (movement != Vector2.zero) onMovement?.Invoke(movement);
    }

    #endregion

}