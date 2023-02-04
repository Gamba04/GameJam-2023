using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private new Camera camera;

    [Header("Settings")]
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private float speed;

    [Header("Info")]
    [ReadOnly, SerializeField]
    private Player activePlayer;
    [ReadOnly, SerializeField]
    private Vector3 targetPosition;

    #region Update

    private void FixedUpdate()
    {
        TargetUpdate();
        PositionUpdate();
    }

    private void TargetUpdate()
    {
        if (activePlayer != null)
        {
            targetPosition = activePlayer.transform.position + offset;
        }
    }

    private void PositionUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Mathf.Min(speed * Time.deltaTime, 1));
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public void SetActivePlayer(Player player)
    {
        activePlayer = player;
    }

    #endregion

}