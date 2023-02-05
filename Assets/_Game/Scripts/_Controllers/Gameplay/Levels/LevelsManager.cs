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

    public event Action<Player> onSetActivePlayer;

    #region Init

    public void Init()
    {
        LoadCurrentLevel();
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private void LoadLevel(LevelTag tag) => LoadLevel(setup.GetLevel(tag));

    private void LoadCurrentLevel() => LoadLevel(setup.GetCurrentLevel());

    private void LoadLevel(LevelsSetup.LevelModel level)
    {
        // Destroy previous level
        if (currentLevel != null) Destroy(currentLevel.gameObject);

        // Instantiate level
        currentLevel = Instantiate(level.level, levelParent);
        currentLevel.name = level.tag.ToString();

        currentLevel.onSetActivePlayer += onSetActivePlayer;
        currentLevel.onLoadLevel += LoadLevel;

        currentLevel.Init();
    }

    #endregion

}