using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIManager : MonoBehaviour
{
    [SerializeField]
    private FadeColorsSetup fadeColorsSetup;

    [Header("Components")]
    [SerializeField]
    protected Canvas canvas;
    [SerializeField]
    protected Image fade;

    [Header("Settings")]
    [SerializeField]
    protected bool startWithFade = true;
    [SerializeField]
    protected TransitionColor fadeTransition;

    public static Canvas Canvas => Instance.canvas;

    public static bool IsOnTransition => Instance.fadeTransition.IsOnTransition;

    #region Singleton

    protected static UIManager instance;

    public static UIManager Instance => GambaFunctions.GetSingleton(ref instance);

    protected static T GetSingletonOverride<T>()
        where T : UIManager
    {
        T instance = UIManager.instance as T;

        return GambaFunctions.GetSingleton(ref instance);
    }

    private void Awake()
    {
        GambaFunctions.OnSingletonAwake(ref instance, this);
    }

    private void Start() => OnStart();

    #endregion

    #region Start

    protected virtual void OnStart()
    {
        FadeStart();
    }

    private void FadeStart()
    {
        if (!startWithFade) return;

        SetInteractions(false);

        SetFade(true, FadeColor.Default, true);
        SetFade(false, FadeColor.Default, onTransitionEnd: () => SetInteractions(true));
    }

    #endregion

    // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    #region Update

    private void Update()
    {
        TransitionsUpdate();
        OnUpdate();
    }

    protected virtual void OnUpdate() { }

    protected virtual void TransitionsUpdate()
    {
        fadeTransition.UpdateTransition(color => fade.color = color);
    }

    #endregion

    // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    #region Virtual Methods

    public virtual void OnSetInteractions(bool enabled) => ButtonBase.Interactable = enabled;

    #endregion

    // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    #region Static Methods

    public static Vector3 ScreenToCanvasPos(Vector2 position)
    {
        Canvas canvas = Canvas;

        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Vector3 newPos = canvas.worldCamera.ScreenToWorldPoint(position);
            newPos.z = canvas.transform.position.z;

            return newPos;
        }
        else
        {
            return position;
        }
    }

    public static Vector2 CanvasToScreenPos(Vector3 position)
    {
        Canvas canvas = Canvas;

        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            return canvas.worldCamera.WorldToScreenPoint(position);
        }
        else
        {
            return position;
        }
    }

    public static Vector3 ScreenToCanvasVector(Vector2 vector)
    {
        Canvas canvas = Canvas;

        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Vector2 newVec = vector / Screen.height * canvas.worldCamera.orthographicSize * 2;

            return newVec;
        }
        else
        {
            return vector;
        }
    }

    public static void SetFade(bool visible, FadeColor fadeColor, bool instant = false, Action onTransitionEnd = null)
    {
        Color color = Instance.fadeColorsSetup.GetColor(fadeColor);

        ButtonBase.Interactable = false;

        if (visible)
        {
            Instance.fade.gameObject.SetActive(true);
        }
        else
        {
            onTransitionEnd += () =>
            {
                ButtonBase.Interactable = true;

                Instance.fade.gameObject.SetActive(false);
            };
        }

        if (instant)
        {
            Color target = visible ? color : color.GetAlpha(0);

            Instance.fadeTransition.value = target;
            Instance.fade.color = target;

            onTransitionEnd?.Invoke();
        }
        else Instance.fadeTransition.StartTransition(visible ? color : color.GetAlpha(0), onTransitionEnd);
    }


    public static void SetInteractions(bool enabled) => Instance.OnSetInteractions(enabled);

    #endregion

}