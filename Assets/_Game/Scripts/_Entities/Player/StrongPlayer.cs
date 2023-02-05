using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongPlayer : Player
{
    [Header("Strong")]
    [SerializeField]
    private float detectionRadius;
    [SerializeField]
    private LayerMask detectionLayer;

    private DefaultBlock currentBlock;

    protected override bool InputsEnabled => state != State.NoInput;

    protected override bool SpecialEnabled => base.SpecialEnabled;

    #region Update

    protected override void OnUpdate()
    {
        BlockUpdate();
    }

    private void BlockUpdate()
    {
        currentBlock = null;

        if (!InputsEnabled || !SpecialEnabled) return;

        List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer));

        foreach (Collider collider in colliders)
        {
            DefaultBlock target = collider.GetComponentInParent<DefaultBlock>();

            if (target != null)
            {
                if (target.Draggable)
                {
                    currentBlock = target;
                    break;
                }
            }
        }

        GplayUI.OnSetSpecialOverlay(currentBlock != null);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Mechanics

    protected override void Special()
    {
        base.Special();
    }

    #endregion

}