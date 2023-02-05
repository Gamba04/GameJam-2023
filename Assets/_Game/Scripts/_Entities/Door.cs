using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class Door : MonoBehaviour
{
    [SerializeField]
    private LevelTag targetLevel;

    public event Action<LevelTag> onLoadLevel;

    private bool activated;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        Player target = other.GetComponentInParent<Player>();

        if (target != null)
        {
            activated = true;

            onLoadLevel?.Invoke(targetLevel);
        }
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            EditorUpdate();
        }
    }

    #region Editor

#if UNITY_EDITOR

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

#endif

    #endregion

}