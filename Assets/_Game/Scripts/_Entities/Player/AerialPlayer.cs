using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialPlayer : Player, IGaiserInteractable
{
    [Header("Aerial")]
    [SerializeField]
    private float specialJumpSpeed;
    [SerializeField]
    private float specialTerminalVelocity;
    [SerializeField]
    private float specialLeeway;
    [SerializeField]
    private float gaiserImpulse;

    protected override float TerminalVelocity => state == State.Special ? specialTerminalVelocity : base.TerminalVelocity;

    #region Mechanics

    protected override void Special()
    {
        base.Special();

        SetGroundedLeeway(specialLeeway);
        Jump(specialJumpSpeed);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region IGaiserInteractable

    public void Impulse()
    {
        if (state == State.Special)
        {
            Jump(gaiserImpulse * Time.deltaTime);
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    protected override void SetGrounded(bool value)
    {
        base.SetGrounded(value);

        //if (value) ChangeState(State.Normal);
    }

    #endregion

}