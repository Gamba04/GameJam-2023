using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrooms : MonoBehaviour, IInteractable
{
    [SerializeField]
    private DialogueSetup shroomDialogue;
    
    #region IInteractable

    public bool Enabled => true;

    public void Interact(Player player)
    {
        GplayUI.SetDialogue(shroomDialogue.Messages);
    }

    #endregion
}
