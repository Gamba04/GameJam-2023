using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Player startingPlayer;
    [SerializeField]
    private List<Root> roots;
    [SerializeField]
    private Transform playersParent;

    [Header("Settings")]
    [SerializeField]
    private float rootDelay;

    [Header("Info")]
    [ReadOnly, SerializeField]
    private Player activePlayer;

    public event Action<Player> onSetActivePlayer;

    #region Init

    public void Init()
    {
        SetActivePlayer(startingPlayer);
        RootsSetup();
    }

    private void RootsSetup()
    {
        foreach (Root root in roots)
        {
            root.Init(playersParent);

            root.onSetActivePlayer += SetActivePlayer;
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private void SetActivePlayer(Player player)
    {
        if (activePlayer != null) activePlayer.Root();
        activePlayer = player;

        Timer.CallOnDelay(() => player.Unroot(), rootDelay, "Root transition");

        onSetActivePlayer?.Invoke(player);
    }

    #endregion

}