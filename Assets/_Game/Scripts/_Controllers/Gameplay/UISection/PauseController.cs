using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : UISection
{
    [Header("Components")]
    [SerializeField]
    private RectTransform root;

    [Header("Settings")]
    [SerializeField]
    private TransitionVector2 positionTransition;
    [SerializeField]
    private Vector2 hiddenPosition;

    #region Update

    private void Update()
    {
        TransitionsUpdate();
    }

    private void TransitionsUpdate()
    {
        positionTransition.UpdateTransition(OnPositionTransition);
    }

    private void OnPositionTransition(Vector2 position)
    {
        root.anchoredPosition = position;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public override void SetVisible(bool value, bool instant = false)
    {
        if (instant)
        {
            root.gameObject.SetActive(value);
        }
        else
        {
            if (value) root.gameObject.SetActive(true);
            GplayUI.SetInteractions(false);

            positionTransition.value = value ? hiddenPosition : Vector2.zero;
            positionTransition.StartTransitionUnscaled(value ? Vector2.zero : hiddenPosition, () => OnSetVisible(value));
        }
    }

    public void Resume()
    {
        GplayManager.SetPause(false);
    }
    
    

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private void OnSetVisible(bool value)
    {
        if (!value) root.gameObject.SetActive(false);
        GplayUI.SetInteractions(true);
    }

    #endregion

}