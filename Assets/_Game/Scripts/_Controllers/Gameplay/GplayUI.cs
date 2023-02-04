﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GplayUI : UIManager
{
    private const int menuIndex = 0;

    [Header("Components")]
    [Header("GplayUI")]
    [SerializeField]
    private HUDController hudController;
    [SerializeField]
    private PauseController pauseController;

    #region Singleton Override

    public new static GplayUI Instance => GetSingletonOverride<GplayUI>();

    #endregion

    #region Start

    protected override void OnStart()
    {
        base.OnStart();

        Instance.pauseController.SetVisible(false, true);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Static Methods

    public static void OnSetPause(bool value)
    {
        Instance.hudController.SetVisible(!value);
        Instance.pauseController.SetVisible(value);
    }

    #endregion

    #region Public Methods

    public void GoToMenu()
    {
        SetInteractions(false);

        SetFade(true, false, () => GambaFunctions.LoadScene(menuIndex));
    }

    #endregion

}