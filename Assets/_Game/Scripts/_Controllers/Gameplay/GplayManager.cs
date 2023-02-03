using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GplayManager : MonoBehaviour
{
    [SerializeField]
    private bool debugs = true;

    public static bool Debugs => instance != null ? instance.debugs : true;

    public static bool GamePaused { get; private set; }

    #region Start

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

    private void OnStart()
    {
        Timer.CallOnDelay(() => SetPause(true), 2);
        Timer.CallOnDelayUnscaled(() => SetPause(false), 5);
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