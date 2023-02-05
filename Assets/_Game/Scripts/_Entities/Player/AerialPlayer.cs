using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialPlayer : Player
{
    [Header("Aerial")]
    [SerializeField]
    private float specialJumpSpeed;
    [SerializeField]
    private float specialTerminalVelocity;

    protected override float TerminalVelocity => state == State.Special ? specialTerminalVelocity : base.TerminalVelocity;

    #region Mechanics

    protected override void Special()
    {
        base.Special();

        Jump(specialJumpSpeed);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    protected override void SetGrounded(bool value)
    {
        base.SetGrounded(value);

        if (!value) ChangeState(State.Normal);
    }

    #endregion

}