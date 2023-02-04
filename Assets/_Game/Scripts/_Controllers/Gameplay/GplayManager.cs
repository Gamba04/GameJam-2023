using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GplayManager : MonoBehaviour
{
    [SerializeField]
    private bool debugs = true;

    [Header("Components")]
    [SerializeField]
    private LevelsManager levelsManager;
    [SerializeField]
    private CameraController cameraController;

    public static bool Debugs => instance != null ? instance.debugs : true;

    public static bool GamePaused { get; private set; }

    #region Singleton

    private static GplayManager instance = null;

    public static GplayManager Instance
    {
        get
        {
            if (instance == null)
            {
                var sceneResult = FindObjectOfType<GplayManager>();

                if (sceneResult != null)
                {
                    instance = sceneResult;
                }
                else
                {
                    GameObject obj = new GameObject($"{GetTypeName(instance)}_Instance");
                    instance = obj.AddComponent<GplayManager>();
                }
            }

            return instance;
        }
    }

    private static string GetTypeName<T>(T obj) => typeof(T).Name;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        OnStart();
    }

    #endregion

    #region Start

    private void OnStart()
    {
        EventsSetup();

        levelsManager.Init();
    }

    private void EventsSetup()
    {
        levelsManager.onSetActivePlayer += cameraController.SetActivePlayer;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Update

    private void Update()
    {

    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Static Methods

    public static void SetPause(bool value)
    {
        GamePaused = value;
        Time.timeScale = value ? 0 : 1;

        GplayUI.OnSetPause(value);
    }

    #endregion

}