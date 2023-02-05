using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaysers : MonoBehaviour
{
    [SerializeField]

    private void GayserImpulse(Collider other)
    {
        IGaiserInteractable interactable = other.GetComponentInParent<IGaiserInteractable>();
        if (interactable != null)
        {
            interactable.Impulse();
        }
    }
    private void OnTriggerEnter(Collider other)
    {   
        GayserImpulse(other);
    }
    private void OnTriggerStay(Collider other)
    {
        GayserImpulse(other);
    }
}
