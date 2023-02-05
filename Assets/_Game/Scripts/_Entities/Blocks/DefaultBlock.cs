using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DefaultBlock : MonoBehaviour
{
    [SerializeField]
    private bool draggable;

    public bool Draggable => draggable;

    #region Update

    private void Update()
    {
        if (Application.isPlaying)
        {

        }
        else
        {
#if UNITY_EDITOR
            EditorUpdate();
#endif
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

    private void EditorUpdate()
    {
        SnapToGrid();
    }

    private void SnapToGrid()
    {
        float snapFactor = 1;

        // Position
        Vector3 position = transform.position - Vector3.one * 0.5f;

        position /= snapFactor;

        position = new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Mathf.RoundToInt(position.z));

        position *= snapFactor;

        transform.position = position + Vector3.one * 0.5f;

        // Rotation
        Vector3 euler = transform.eulerAngles;

        euler /= 90;

        euler = new Vector3(Mathf.RoundToInt(euler.x), Mathf.RoundToInt(euler.y), Mathf.RoundToInt(euler.z));

        euler *= 90;

        transform.rotation = Quaternion.Euler(euler);
    }

    #endregion

}