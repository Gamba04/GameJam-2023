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
    [SerializeField]
    private Vector2 blockOffset;

    private DefaultBlock currentBlock;

    private bool dragging;

    protected override bool InputsEnabled => state != State.NoInput;

    protected override bool SpecialEnabled => currentBlock != null;

    #region Update

    protected override void OnUpdate()
    {
        BlockUpdate();
        SpecialUpdate();
    }

    private void BlockUpdate()
    {
        if (state != State.Normal) return;

        currentBlock = null;

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

    private void SpecialUpdate()
    {
        if (state != State.Special || !dragging) return;

        currentBlock.Drag(transform.position + transform.forward * blockOffset.x + transform.up * blockOffset.y);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Mechanics

    protected override void Special()
    {
        base.Special();

        if (dragging)
        {
            currentBlock.Drop();

            ChangeState(State.Normal);
        }
        else
        {
            currentBlock.Grab();
        }

        dragging = !dragging;
    }

    #endregion

}