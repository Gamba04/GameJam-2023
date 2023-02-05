using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GayserButton : MonoBehaviour , IInteractable
{
    [SerializeField] 
    private Gaysers gayser;
    public void Interact(Player player)
    {
        gayser.SetState(!gayser.GetState());
    }
}
