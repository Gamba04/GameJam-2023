using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour, IInteractable
{
    [Header("Components")]
    [SerializeField]
    private Player playerPrefab;
    [SerializeField]
    private Root endPoint;

    [Header("Settings")]
    [SerializeField]
    private Vector3 playerOffset;

    private Transform playersParent;

    private Player player;

    public event Action<Player> onSetActivePlayer;

    #region Init

    public void Init(Transform playersParent)
    {
        this.playersParent = playersParent;

        CreatePlayer();
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

    public void Interact()
    {
        onSetActivePlayer?.Invoke(endPoint.player);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + playerOffset, 0.1f);
    }

#endif

    #endregion

}