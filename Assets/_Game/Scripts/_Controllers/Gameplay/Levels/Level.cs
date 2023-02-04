using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private List<Player> players;

    [Header("Info")]
    [ReadOnly, SerializeField]
    private Player activePlayer;

    public event Action<Player> onSetActivePlayer;

    #region Init

    public void Init()
    {
        SetupPlayers();
    }

    private void SetupPlayers()
    {
        if (players.Count == 0) throw new Exception("No players assigned");

        SetActivePlayer(players[0]);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private void SetActivePlayer(Player player)
    {
        activePlayer = player;

        players.ForEach(p => p.Active = p == player);

        onSetActivePlayer?.Invoke(player);
    }

    #endregion

}