using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SageTree : MonoBehaviour, IInteractable
{
    private static Queue<DialogueSetup> queue;

    [SerializeField]
    private Queue<DialogueSetup> setup;
    

    private void Start()
    {
        if (queue != null)
        {
            queue = setup;
        }
    }

    public void Pop() 
    { 
        GplayUI.SetDialogue(queue.Dequeue().Messages);
    }

    public void Interact()
    {
        Pop();
    }
}
