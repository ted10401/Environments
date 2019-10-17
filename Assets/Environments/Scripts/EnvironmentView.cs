using UnityEngine;
using FS2.Environment.Lighting;
using FS2.Environment.Weather.Cloud;
using FS2.Environment.Weather.Rain;
using FS2.Environment.Weather.Lightning;

namespace FS2.Environment
{
    public class EnvironmentView : MonoBehaviour
    {
        public EnvironmentLightingView environmentLightingView;
        public EnvironmentCloudView environmentCloudView;
        public EnvironmentRainView environmentRainView;
        public EnvironmentLightningView environmentLightningView;
    }
}