using UnityEngine;

public class MenuFuncs : MonoBehaviour
{
    
    private Vector2 xPos = new Vector2(2200, 0);
    private Vector2 currentPos = Vector2.zero;


    [SerializeField]
    private TransitionVector2 transitionPos;
    [SerializeField]
    private RectTransform ui => transform as RectTransform;

    private void Update()
    {
        TransitionsUpdate();
    }
    private void TransitionsUpdate()
    {
        transitionPos.UpdateTransition(OnPositionTransition);
    }
    private void OnPositionTransition(Vector2 pos)
    {
        ui.anchoredPosition = pos;
    }
    private void TransitionTo(Vector2 pos)
    {
        MenuUI.SetInteractions(false);
        transitionPos.StartTransition( pos, () => MenuUI.SetInteractions(true));

    }
    public void TransitionLeft()
    {
        currentPos += xPos;
        TransitionTo(currentPos);
    }
    public void TransitionRight()
    {
        currentPos -= xPos;
        TransitionTo(currentPos);
    }
}

