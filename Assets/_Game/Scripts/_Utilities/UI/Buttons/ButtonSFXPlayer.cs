using System.Collections.Generic;
using UnityEngine;

public class ButtonSFXPlayer : MonoBehaviour
{
    [SerializeField]
    private ButtonBase button;

    [Space]
    [SerializeField]
    private List<SFXTag> hover;
    [SerializeField]
    private List<SFXTag> click;

    private void Start()
    {
        button.onHover += () => PlaySFX(hover);
        button.onButtonClick += () => PlaySFX(click);
    }

    private void PlaySFX(List<SFXTag> sfx)
    {
        if (sfx == null || sfx.Count == 0) return;

        SFXPlayer.PlayRandomSFX(sfx);
    }

    #region Editor

#if UNITY_EDITOR

    private void OnValidate()
    {
        if (button == null)
        {
            button = GetComponent<ButtonBase>();
        }
    }

#endif

    #endregion

}