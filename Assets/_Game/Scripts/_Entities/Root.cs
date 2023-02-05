using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Root : MonoBehaviour, IInteractable
{
    [Header("Components")]
    [SerializeField]
    private Player playerPrefab;
    [SerializeField]
    private Root endPoint;

    [Header("Settings")]
    [SerializeField]
    private bool startOccupied;
    [SerializeField]
    private Vector3 playerOffset;

    private Transform playersParent;

    private Player player;

    public event Action<Player> onSetActivePlayer;

    #region Init

    public void Init(Transform playersParent)
    {
        this.playersParent = playersParent;

        if (startOccupied) CreatePlayer();
    }

    private void CreatePlayer()
    {
        player = Instantiate(playerPrefab, playersParent);
        player.name = playerPrefab.name;

        player.transform.position = transform.position;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region IInteractable

    public bool Enabled => player == null;

    public void Interact(Player player)
    {
        this.player = player;

        player.Root();

        onSetActivePlayer?.Invoke(endPoint.player);

        endPoint.player = null;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + playerOffset, 0.1f);
    }

    #region Update

    private void Update()
    {
        if (!Application.isPlaying)
        {
            EditorUpdate();
        }
    }

    private void EditorUpdate()
    {
        SnapToGrid();
    }

    private void SnapToGrid()
    {
        float snapFactor = 1;

        // Position
        Vector3 position = transform.position - Vector3.one * 0.5f;

        position /= snapFactor;

        position = new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Mathf.RoundToInt(position.z));

        position *= snapFactor;

        transform.position = position + Vector3.one * 0.5f;

        // Rotation
        Vector3 euler = transform.eulerAngles;

        euler /= 90;

        euler = new Vector3(Mathf.RoundToInt(euler.x), Mathf.RoundToInt(euler.y), Mathf.RoundToInt(euler.z));

        euler *= 90;

        transform.rotation = Quaternion.Euler(euler);
    }

    #endregion

#endif

    #endregion

}