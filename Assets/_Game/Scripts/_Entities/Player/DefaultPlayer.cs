using UnityEngine;

public class DefaultPlayer : Player
{
    [Header("Default")]
    [SerializeField]
    private float superJumpDelay;
    [SerializeField]
    private float superJumpSpeed;

    private Timer.CancelRequest onSuperJumpCancel = new Timer.CancelRequest();

    #region Mechanics

    protected override void Special()
    {
        base.Special();

        Timer.CallOnDelay(SuperJump, superJumpDelay, onSuperJumpCancel, "Super Jump");
    }

    private void SuperJump()
    {
        Jump(superJumpSpeed);

        ChangeState(State.Normal);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private void CancelSuperJump()
    {
        onSuperJumpCancel.Cancel();

        ChangeState(State.Normal);
    }

    #endregion

}