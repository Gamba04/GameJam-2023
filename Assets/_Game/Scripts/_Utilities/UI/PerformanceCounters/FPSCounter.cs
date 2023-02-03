using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : PerformanceCounter
{
    protected override void UpdateText()
    {
        int fps = Mathf.RoundToInt(1f / Time.unscaledDeltaTime);

        text.text = fps.ToString();
    }
}
