using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace FS2.Environment.Weather.Cloud
{
    [Serializable]
    public class EnvironmentCloudSettings
    {
        [LabelText("雲層貼圖")] public Texture cloudTexture;
        [LabelText("雲層參數 (速度, 大小, 覆蓋率, 透明度")] public Vector4 cloudFactor = new Vector4(0.005f, 0.005f, 0.1f, 5.0f);
        [LabelText("陰影強度曲線")] public AnimationCurve shadowStrengthCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [LabelText("陰影時間倍率")] public float shadowTimeMultiply = 1f;
        [LabelText("使用自訂陰影顏色")] public bool useCustomsShadowColor = false;
        [LabelText("自訂陰影顏色"), ShowIf("useCustomsShadowColor"), ColorUsage(false, true)] public Color customShadowColor;
    }
}