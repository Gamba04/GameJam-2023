using System;
using System.Collections.Generic;
using UnityEngine;

public enum FadeColor
{
    Default,
    Cinematic,
}

[CreateAssetMenu(fileName = "FadeColorsSetup", menuName = "Setup/Fade Colors Setup", order = 1)]
public class FadeColorsSetup : ScriptableObject
{

    #region Serializable

    [Serializable]
    public class FadeColorInfo
    {
        [SerializeField, HideInInspector] private string name;

        public Color color = Color.white;

        public void SetName(FadeColor fadeColor)
        {
            name = fadeColor.ToString();
        }
    }

    #endregion

    [SerializeField]
    private List<FadeColorInfo> fadeColors;

    public Color GetColor(FadeColor fadeColor) => fadeColors[(int)fadeColor].color;

    #region Editor

#if UNITY_EDITOR

    private void OnValidate()
    {
        fadeColors.Resize<FadeColorInfo, FadeColor>();
        fadeColors.ForEach((fadeColor, index) => fadeColor.SetName((FadeColor)index));
    }

#endif

    #endregion

}