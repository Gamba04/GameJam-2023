using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    [SerializeField]
    private LevelsSetup setup;

    [Header("Components")]
    [SerializeField]
    private Transform levelParent;

    [Header("Info")]
    [ReadOnly, SerializeField]
    private Level currentLevel;

    #region Init

    public void Init()
    {
        LoadCurrentLevel();
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public void LoadLevel(LevelTag tag) => LoadLevel(setup.GetLevel(tag));

    public void LoadCurrentLevel() => LoadLevel(setup.GetCurrentLevel());

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private void LoadLevel(LevelsSetup.LevelModel level)
    {
        // Destroy previous level
        if (currentLevel != null) Destroy(currentLevel);

        // Instantiate level
        currentLevel = Instantiate(level.level, levelParent);
        currentLevel.name = level.tag.ToString();

        currentLevel.Init();
    }

    #endregion

}