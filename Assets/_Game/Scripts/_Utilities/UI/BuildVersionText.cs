using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class BuildVersionText : MonoBehaviour
{
    [SerializeField]
    private Text text;

#if UNITY_EDITOR

    private void Update()
    {
        if (!Application.isPlaying)
        {
            if (text) text.text = Application.version;
        }
    }

#endif

}