using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GayserButton : MonoBehaviour , IInteractable
{
    [SerializeField] 
    private Gaysers gayser;
    public void Interact()
    {
        gayser.SetState(!gayser.GetState());
    }
}
