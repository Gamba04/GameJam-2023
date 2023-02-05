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
    private Transform playersParent;
    [SerializeField]
    private List<Root> roots;
    [SerializeField]
    private List<Door> doors;

    [Header("Settings")]
    [SerializeField]
    private float rootDelay;

    [Header("Info")]
    [ReadOnly, SerializeField]
    private Player activePlayer;

    private Timer.CancelRequest onCancelPlayerSwitch = new Timer.CancelRequest();

    public event Action<Player> onSetActivePlayer;
    public event Action<LevelTag> onLoadLevel;

    #region Init

    public void Init()
    {
        SetActivePlayer(startingPlayer);
        RootsSetup();
        DoorsSetup();
    }

    private void RootsSetup()
    {
        foreach (Root root in roots)
        {
            root.Init(playersParent);

            root.onSetActivePlayer += SetActivePlayer;
        }
    }

    private void DoorsSetup()
    {
        foreach (Door door in doors)
        {
            door.onLoadLevel += OnLoadLevel;
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private void SetActivePlayer(Player player)
    {
        if (activePlayer != null) activePlayer.Root();
        activePlayer = player;

        Timer.CallOnDelay(() => player.Unroot(), rootDelay, onCancelPlayerSwitch, "Root transition");

        onSetActivePlayer?.Invoke(player);

        GplayUI.OnSetInteractionOverlay(false);
    }

    private void OnLoadLevel(LevelTag levelTag)
    {
        onCancelPlayerSwitch.Cancel();

        activePlayer.Root();

        GplayUI.SetFade(true, FadeColor.Default, onTransitionEnd: () =>
        {
            onLoadLevel?.Invoke(levelTag);

            GplayUI.SetFade(false, FadeColor.Default);
        });
    }

    #endregion

}