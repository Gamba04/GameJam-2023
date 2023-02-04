using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public static class GambaFunctions
{

    #region Math

    public static float GetAngle(Vector2 point)
    {
        float pi = Mathf.PI;

        float x = point.x;
        float y = point.y;

        float r;

        if (x > 0)
        {
            if (y > 0) // Cuadrant: 1
            {
                r = Mathf.Atan(y / x);
            }
            else if (y < 0) // Cuadrant: 4
            {
                r = pi * 3 / 2f + (pi * 3 / 2f - (pi - Mathf.Atan(y / x)));
            }
            else // Right
            {
                r = 0;
            }
        }
        else if (x < 0)
        {
            if (y > 0) // Cuadrant: 2
            {
                r = pi * 1 / 2f + (pi * 1 / 2f + Mathf.Atan(y / x));

            }
            else if (y < 0) // Cuadrant: 3
            {
                r = pi + Mathf.Atan(y / x);

            }
            else // Left
            {
                r = pi;
            }
        }
        else
        {
            if (y > 0) // Up
            {
                r = pi * 1 / 2f;
            }
            else if (y < 0) // Down
            {
                r = pi * 3 / 2f;
            }
            else // Zero
            {
                r = 0;
            }
        }

        return r;
    }

    public static Vector2 AngleToVector(float angle) => new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

    public static Vector2 Bounce(Vector2 normal, Vector2 direction)
    {
        normal.Normalize();
        direction.Normalize();

        float dotNormalDirection = Vector2.Dot(direction.normalized, normal);

        Vector2 dashYComponent = normal * Mathf.Abs(dotNormalDirection);

        Vector2 dashXComponent;

        if (dotNormalDirection > 0)
        {
            dashXComponent = direction - dashYComponent;
        }
        else
        {
            dashXComponent = direction + dashYComponent;
        }

        return (dashXComponent + dashYComponent);
    }

    public static Vector2 Perpendicular(Vector2 direction) => new Vector2(direction.y, -direction.x);

    public static Color GetAlpha(this Color color, float value) => new Color(color.r, color.g, color.b, value);

    public static Vector2 VectorMult(Vector2 a, Vector2 b) => new Vector2(a.x * b.x, a.y * b.y);

    public static Vector3 VectorMult(Vector3 a, Vector3 b) => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);

    public static Vector3 GetScaleOf(float size)
    {
        Vector3 scale = Vector3.one * size;
        scale.z = 1;

        return scale;
    }

    public static bool RandomBool() => UnityEngine.Random.Range(0, 2) == 0;

    public static float VolumeToDB(float volume) => volume > 0 ? Mathf.Log(volume) * 20 : float.MinValue;

    #endregion

    // -------------------------------------------------------------------------------------------------------------------

    #region Gizmos

    public static void GizmosDraw2DArrow(Vector2 origin, Vector2 direction)
    {
        Vector2 head = origin + direction;
        Gizmos.DrawLine(origin, head);

        Vector2 perpendicular = new Vector2(direction.y, -direction.x);
        Gizmos.DrawLine(head, head + perpendicular * 0.1f - direction * 0.2f);
        Gizmos.DrawLine(head, head - perpendicular * 0.1f - direction * 0.2f);
    }

    public static void GizmosDraw2DArrow(Vector2 origin, Vector2 direction, Vector2 headSize)
    {
        Vector2 head = origin + direction;
        Gizmos.DrawLine(origin, head);

        Vector2 perpendicular = Perpendicular(direction);

        Gizmos.DrawLine(head, head + perpendicular.normalized * headSize / 2f - direction.normalized * headSize);
        Gizmos.DrawLine(head, head - perpendicular.normalized * headSize / 2f - direction.normalized * headSize);
    }

    public static void GizmosDrawPointedLine(Vector3 from, Vector3 to, float separation, int maxIters = 500)
    {
        float distance = (to - from).magnitude;
        float distanceToA = 0;
        float distanceToB = 0;
        int iter = 0;

        while (distanceToB < distance && iter < maxIters)
        {
            Vector3 offset = Vector3.zero;
            Vector3 pointA = from + (to - from).normalized * (iter) * separation;
            Vector3 pointB = from + (to - from).normalized * (iter + 0.5f) * separation;

            distanceToA = (pointA - from).magnitude;
            distanceToB = (pointB - from).magnitude;

            if (distanceToA < distance)
            {
                if (distanceToB > distance)
                {
                    offset = to - pointB;
                }

                Gizmos.DrawLine(pointA, pointB + offset);
            }

            iter++;
        }
    }

    #endregion

    // -------------------------------------------------------------------------------------------------------------------

    #region Lists & Enums

    /// <summary> Checks if Type has a parameterless constructor. </summary>
    public static bool TryDefaultConstructor<T>(out T result)
    {
        result = default;

        if (typeof(T).IsValueType) return false;

        var constructor = typeof(T).GetConstructor(Type.EmptyTypes);

        if (constructor == null) return false;

        result = (T)constructor.Invoke(new object[0]);
        return true;
    }

    private static void Resize<T>(this List<T> list, int length, Func<T> defaultElement)
    {
        if (list == null) list = new List<T>();
        if (list.Count == length) return;

        List<T> newList = new List<T>();

        for (int i = 0; i < length; i++)
        {
            if (i < list.Count)
            {
                newList.Add(list[i]);
            }
            else
            {
                T element = defaultElement != null ? defaultElement() : default;

                newList.Add(element);
            }
        }

        list.Clear();
        list.AddRange(newList);
    }

    /// <summary> Resize list to length without losing data. </summary>
    public static void Resize<T>(this List<T> list, int length)
        where T : new()
    {
        list.Resize(length, () => new T());
    }

    /// <summary> Resize list to length without losing data. New elements will be set to default value. </summary>
    public static void ResizeEmpty<T>(this List<T> list, int length)
    {
        list.Resize(length, () => default);
    }

    private static void Resize<T, E>(this List<T> list, Func<T> defaultElement)
        where E : Enum
    {
        int length = GetEnumLenght<E>();

        list.Resize(length, defaultElement);
    }

    /// <summary> Resize list to enum length without losing data. </summary>
    public static void Resize<T, E>(this List<T> list)
        where T : new()
        where E : Enum
    {
        list.Resize<T, E>(() => new T());
    }

    /// <summary> Resize list to enum length without losing data. New elements will be set to default value. </summary>
    public static void ResizeEmpty<T, E>(this List<T> list)
        where E : Enum
    {
        list.Resize<T, E>(() => default);
    }

    public static int GetEnumLenght<E>() where E : Enum => Enum.GetValues(typeof(E)).Length;

    public static void ForEach<T>(this List<T> list, Action<T, int> action)
    {
        if (list == null) return;

        for (int i = 0; i < list.Count; i++) action?.Invoke(list[i], i);
    }

    /// <summary> QuickSort a List of any type with a custom comparison method. </summary>
    /// <param name="comparison"> Custom comparison method which defines if element1 <, <=, ==, !=, >= or > element2. Returns an int which corresponds to an equivalent comparison with 0. </param>
    public static void QuickSort<T>(this List<T> list, Comparison<T> comparison)
    {
        if (list.Count <= 1) return;

        int pivot = list.Count - 1;

        // Create partitions
        List<T> left = new List<T>();
        List<T> right = new List<T>();

        for (int i = 0; i < list.Count - 1; i++)
        {
            List<T> partition = comparison(list[i], list[pivot]) <= 0 ? left : right;

            partition.Add(list[i]);
        }

        // Recurse
        left.QuickSort(comparison);
        right.QuickSort(comparison);

        // Join partitions
        T pivotElement = list[pivot];

        list.Clear();

        list.AddRange(left);
        list.Add(pivotElement);
        list.AddRange(right);
    }

    /// <summary> QuickSort a List of any type with default IComparable.CompareTo. </summary>
    public static void QuickSort<T>(this List<T> list) where T : IComparable<T>
    {
        list.QuickSort((e1, e2) => e1.CompareTo(e2));
    }

    public static void ShuffleList<T>(this List<T> list, bool forceChanging = false)
    {
        if (list == null || list.Count < 1) return;

        List<T> result = new List<T>();

        // Fill indexes
        List<int> indexes = new List<int>();
        for (int i = 0; i < list.Count; i++) indexes.Add(i);

        for (int i = 0; i < list.Count; i++)
        {
            List<int> newIndexes = new List<int>(indexes);
            if (forceChanging && newIndexes.Count > 1) newIndexes.Remove(i);

            int index = newIndexes[UnityEngine.Random.Range(0, newIndexes.Count)];
            T element = list[index];

            indexes.Remove(index);

            result.Add(element);
        }

        list.Clear();
        list.AddRange(result);
    }

    #endregion

    // -------------------------------------------------------------------------------------------------------------------

    #region Physics2D

    /// <summary> Checks for component attached to Rigidbody. </summary>
    public static T CheckForComponent<T>(Vector2 worldPos, int layerMask = ~0) where T : Component
    {
        return CheckForComponent(worldPos, collider => collider.attachedRigidbody?.GetComponent<T>(), layerMask);
    }

    public static T CheckForComponent<T>(Vector2 worldPos, Converter<Collider2D, T> getComponentMethod, int layerMask = ~0) where T : Component
    {
        if (getComponentMethod == null) throw new ArgumentNullException("getComponentMethod cannot be null");

        Collider2D collider = Physics2D.OverlapPoint(worldPos, layerMask);

        if (collider == null) return null;

        T target = getComponentMethod(collider);

        return target;
    }

    public static T CheckForNearestComponent<T>(Vector2 worldPos, int layerMask = ~0, List<T> ignoreList = null) where T : Component
    {
        return CheckForNearestComponent(worldPos, collider => collider.attachedRigidbody?.GetComponent<T>(), layerMask, ignoreList);
    }

    public static T CheckForNearestComponent<T>(Vector2 worldPos, Converter<Collider2D, T> getComponentMethod, int layerMask = ~0, List<T> ignoreList = null) where T : Component
    {
        List<T> components = CheckForComponents(worldPos, getComponentMethod, layerMask);

        T target = null;

        float currentDistance = Mathf.Infinity;

        foreach (T component in components)
        {
            if (ignoreList != null && ignoreList.Contains(component)) continue;

            float distance = ((Vector2)component.transform.position - worldPos).sqrMagnitude;

            if (distance < currentDistance)
            {
                target = component;

                currentDistance = distance;
            }
        }

        return target;
    }

    public static List<T> CheckForComponents<T>(Vector2 worldPos, int layerMask = ~0, List<T> ignoreList = null) where T : Component
    {
        return CheckForComponents(worldPos, collider => collider.attachedRigidbody?.GetComponent<T>(), layerMask, ignoreList);
    }

    public static List<T> CheckForComponents<T>(Vector2 worldPos, Converter<Collider2D, T> getComponentMethod, int layerMask = ~0, List<T> ignoreList = null) where T : Component
    {
        if (getComponentMethod == null) throw new ArgumentNullException("getComponentMethod cannot be null");

        Collider2D[] colliders = Physics2D.OverlapPointAll(worldPos, layerMask);

        List<T> targets = new List<T>();

        foreach (Collider2D collider in colliders)
        {
            T target = getComponentMethod(collider);

            if (target != null)
            {
                if (ignoreList != null && ignoreList.Contains(target)) continue;

                targets.Add(target);
            }
        }

        return targets;
    }

    public static T CheckForComponentRaycast<T>(Vector2 origin, Vector2 direction, int layerMask = ~0) where T : Component
    {
        return CheckForComponentRaycast(origin, direction, collider => collider.attachedRigidbody?.GetComponent<T>(), layerMask);
    }

    public static T CheckForComponentRaycast<T>(Vector2 origin, Vector2 direction, Converter<Collider2D, T> getComponentMethod, int layerMask = ~0) where T : Component
    {
        RaycastHit2D result = Physics2D.Raycast(origin, direction, direction.magnitude, layerMask);

        if (result)
        {
            T target = getComponentMethod(result.collider);

            return target;
        }

        return null;
    }

    #endregion

    // -------------------------------------------------------------------------------------------------------------------

    #region Singleton

    public static T GetSingleton<T>(ref T instance)
        where T : Component
    {
        if (instance == null)
        {
            T sceneResult = UnityEngine.Object.FindObjectOfType<T>();

            if (sceneResult != null)
            {
                instance = sceneResult;
            }
            else
            {
                // Create instance
                GameObject obj = new GameObject($"{typeof(T).Name}_Instance");

                instance = obj.AddComponent<T>();
            }
        }

        return instance;
    }

    public static void OnSingletonAwake<T>(ref T instance, T @this)
        where T : Component
    {
        if (instance == null)
        {
            instance = @this;
        }
        else if (instance != @this)
        {
            UnityEngine.Object.Destroy(@this.gameObject);
        }
    }

    #endregion

    // -------------------------------------------------------------------------------------------------------------------

    #region Other

    public static void LoadScene(string name, bool async = false)
    {
        if (async) SceneManager.LoadSceneAsync(name);
        else SceneManager.LoadScene(name);
    }

    public static void LoadScene(int buildIndex, bool async = false)
    {
        if (async) SceneManager.LoadSceneAsync(buildIndex);
        else SceneManager.LoadScene(buildIndex);
    }

    public static void ReloadScene() => LoadScene(SceneManager.GetActiveScene().buildIndex);

    #endregion

    // -------------------------------------------------------------------------------------------------------------------

    #region Editor

    public static void DestroyInEditor(UnityEngine.Object obj)
    {

#if UNITY_EDITOR

        EditorApplication.delayCall += () => MonoBehaviour.DestroyImmediate(obj);

#endif

    }

    #endregion

}