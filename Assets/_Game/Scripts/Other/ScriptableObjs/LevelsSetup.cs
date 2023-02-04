using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelTag
{
    Overworld,
}

[CreateAssetMenu(fileName = "LevelsSetup", menuName = "Setup/Levels Setup", order = 0)]
public class LevelsSetup : ScriptableObject
{
    #region Serializable

    [Serializable]
    private class LevelModel
    {
        [SerializeField, HideInInspector] private string name;

        [SerializeField]
        private Level level;

        public void SetName(LevelTag tag)
        {
            name = tag.ToString();
        }
    }

    #endregion

    [SerializeField]
    private LevelTag currentLevel;

    [Space]
    [SerializeField]
    private List<LevelModel> levels;

    #region Editor

#if UNITY_EDITOR

    private void OnValidate()
    {
        levels.Resize<LevelModel, LevelTag>();
        levels.ForEach((level, index) => level.SetName((LevelTag)index));
    }

#endif

    #endregion

}