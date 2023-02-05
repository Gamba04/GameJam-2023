using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSetup", menuName = "Setup/Dialogue Setup", order = 2)]
public class DialogueSetup : ScriptableObject
{
    [SerializeField]
    private List<MessageInfo> messages;
}