using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace FS2.Environment.Weather.Lightning
{
    [Serializable]
    public class EnvironmentLightningSettings
    {
        [LabelText("閃電時間")] public Vector2 lightningTime = new Vector2(5f, 10f);
        [LabelText("閃電顏色")] public Color lightningColor = new Color(180f / 255f, 240f / 255f, 240f / 255f);
        [LabelText("閃電垂直角度")] public Vector2 lightningVerticalAngle = new Vector2(30f, 75f);
        [LabelText("閃電次數")] public Vector2Int lightningCount = new Vector2Int(5, 10);
        [LabelText("閃電強度")] public Vector2 lightningIntensity = new Vector2(1.25f, 1.75f);
        [LabelText("閃電間隔")] public Vector2 lightningInterval = new Vector2(0.05f, 0.15f);
    }
}