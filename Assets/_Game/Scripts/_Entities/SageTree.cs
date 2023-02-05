using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SageTree : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Queue<DialogueSetup> setup;

    private static Queue<DialogueSetup> queue;

    #region Start

    private void Start()
    {
        SetupQueue();
    }

    private void SetupQueue()
    {
        if (queue != null) queue = setup;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region IInteractable

    public bool Enabled => true;

    public void Interact(Player player)
    {
        Pop();
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    public void Pop()
    {
        GplayUI.SetDialogue(queue.Dequeue().Messages);
    }

    #endregion

}
