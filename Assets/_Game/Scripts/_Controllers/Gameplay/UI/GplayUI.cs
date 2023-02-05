using System;
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
    [SerializeField]
    private GameObject interactPrompt;
    [SerializeField]
    private DialogueController dialogueController;

    #region Singleton Override

    public new static GplayUI Instance => GetSingletonOverride<GplayUI>();

    #endregion

    #region Start

    protected override void OnStart()
    {
        base.OnStart();

        Instance.pauseController.SetVisible(false, true);

        OnSetInteractionOverlay(false);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Static Methods

    public static void OnSetPause(bool value)
    {
        Instance.hudController.SetVisible(!value);
        Instance.pauseController.SetVisible(value);
    }

    /// <summary> Set UI overlay for interactions available. </summary>
    public static void OnSetInteractionOverlay(bool value)
    {
        Instance.interactPrompt.SetActive(value);
    }

    public static void SetDialogue(List<MessageInfo> list)
    {
        Instance.dialogueController.SetDialogue(list);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public void GoToMenu()
    {
        SetInteractions(false);

        SetFade(true, FadeColor.Default, false, () => GambaFunctions.LoadScene(menuIndex));
    }

    #endregion

}