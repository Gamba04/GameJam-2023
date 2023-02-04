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
    public class LevelModel
    {
        [SerializeField, HideInInspector] private string name;

        [HideInInspector]
        public LevelTag tag;

        public Level level;

        public void SetName(LevelTag tag)
        {
            this.tag = tag;

            name = tag.ToString();
        }
    }

    #endregion

    [SerializeField]
    private LevelTag currentLevel;

    [Space]
    [SerializeField]
    private List<LevelModel> levels;

    public LevelModel GetCurrentLevel() => levels[(int)currentLevel];

    public LevelModel GetLevel(LevelTag tag) => levels[(int)tag];

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