using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : UIManager
{   
    private const int menuIndex = 0;
    private const int gameplaySceneIndex = 1;

    #region Singleton Override

    public new static MenuUI Instance => GetSingletonOverride<MenuUI>();

    #endregion

    #region Static Methods

    public static void GoToGameplay()
    {
        SetInteractions(false);

        SetFade(true, false, () => GambaFunctions.LoadScene(gameplaySceneIndex));
    }

    public void GoToMenu()
    {
        SetInteractions(false);

        SetFade(true, false, () => GambaFunctions.LoadScene(menuIndex));
    }
    #endregion

}