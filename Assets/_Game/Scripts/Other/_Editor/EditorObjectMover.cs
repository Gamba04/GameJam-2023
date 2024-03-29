﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace Editor
{
    public class EditorObjectMover : MonoBehaviour
    {
        [Tooltip("If true the selected objects will be the active selected objects in scene.")]
        [SerializeField]
        private bool useSelection;
        [Header("Translate")]
        [Tooltip("Use this to move selected objects the selected amount of displacement.")]
        [SerializeField]
        private Vector3 displacement;
        [SerializeField]
        private bool move;
        [Header("Perfect")]
        [Tooltip("Use this to round the position of selected objects to the nearest multiple of the Round Factor.")]
        [SerializeField]
        private float roundFactor = 1;
        [SerializeField]
        private bool perfect;

        [Header("Objects ------------------------------------------------------------------------------------------------------------------------------------------------------------------------")]
        [SerializeField]
        [Tooltip("Clear selected objects.")]
        private bool clear;
        [SerializeField]
        [Tooltip("Select all children of these objects.")]
        private List<Transform> parents = new List<Transform>();
        [SerializeField]
        [Tooltip("Select these specific objects.")]
        private List<Transform> singleObjects = new List<Transform>();
        [ReadOnly, SerializeField]
        [Tooltip("The total selection of objects.")]
        private List<Transform> allObjects = new List<Transform>();

        private float lastParentCount;
        private float lastSingleCount;

        #region Operations

        private void RoundObjects()
        {
#if UNITY_EDITOR
            if (!useSelection)
            {
                for (int i = 0; i < allObjects.Count; i++)
                {
                    allObjects[i].position = RoundPosition(allObjects[i].position);
                }
            }
            else
            {

                for (int i = 0; i < Selection.transforms.Length; i++)
                {
                    Selection.transforms[i].position = RoundPosition(Selection.transforms[i].position);
                }
            }
#endif
        }

        private Vector3 RoundPosition(Vector3 pos)
        {

            if (roundFactor > 0)
            {
                pos.x = Mathf.Round(pos.x / roundFactor) * roundFactor;
                pos.y = Mathf.Round(pos.y / roundFactor) * roundFactor;
                pos.z = Mathf.Round(pos.z / roundFactor) * roundFactor;
            }

            return pos;
        }

        private void Translate()
        {
#if UNITY_EDITOR
            if (!useSelection)
            {
                for (int i = 0; i < allObjects.Count; i++)
                {
                    Selection.transforms[i].position += displacement;
                }
            }
            else
            {
                for (int i = 0; i < Selection.transforms.Length; i++)
                {
                    Selection.transforms[i].position += displacement;
                }
            }
#endif
        }

        #endregion

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        #region Objects

        private void UpdateObjects()
        {
            allObjects.Clear();

            for (int i = 0; i < parents.Count; i++)
            {
                for (int c = 0; c < parents[i].childCount; c++)
                {
                    allObjects.Add(parents[i].GetChild(c));
                }
            }

            for (int i = 0; i < singleObjects.Count; i++)
            {
                allObjects.Add(singleObjects[i]);
            }
        }

        private float CalculateTargetAmount()
        {
            float r = 0;

            for (int i = 0; i < parents.Count; i++)
            {
                r += parents[i].childCount;
            }

            r += singleObjects.Count;

            return r;
        }

        #endregion

#if UNITY_EDITOR

        private void OnValidate()
        {
            float targetAllObjectCount = CalculateTargetAmount();

            if (parents.Count != lastParentCount || singleObjects.Count != lastSingleCount || allObjects.Count != targetAllObjectCount)
            {
                lastParentCount = parents.Count;
                lastSingleCount = singleObjects.Count;

                UpdateObjects();
            }

            if (move) // ---------------------------------------
            {
                move = false;

                Translate();
            }

            if (perfect) // ---------------------------------------
            {
                perfect = false;

                RoundObjects();
            }

            if (clear) // ---------------------------------------
            {
                clear = false;

                parents.Clear();
                singleObjects.Clear();
                allObjects.Clear();
            }
        }

#endif

    }
}