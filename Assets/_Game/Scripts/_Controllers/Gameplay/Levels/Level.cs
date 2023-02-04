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

    #region Init

    public void Init()
    {
        SetupPlayers();
    }

    private void SetupPlayers()
    {
        if (players.Count == 0) throw new Exception("No players assigned");

        players.ForEach((player, index) => player.Active = index == 0);
    }

    #endregion

}