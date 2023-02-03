using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : UISection
{
    [Header("Components")]
    [SerializeField]
    private Animator anim;

    #region Public Methods

    public override void SetVisible(bool value, bool instant = false)
    {

    }

    #endregion

}