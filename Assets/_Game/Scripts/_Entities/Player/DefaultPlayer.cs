using UnityEngine;

public class DefaultPlayer : Player
{
    [Header("Default")]
    [SerializeField]
    private float superJumpDelay;
    [SerializeField]
    private float superJumpSpeed;

    protected override void Special()
    {
        Timer.CallOnDelay(() => Jump(superJumpSpeed), superJumpDelay, "Super Jump");
    }
}