using Sirenix.OdinInspector;
using UnityEngine.Rendering;

namespace FS2.Environment
{
    public class AmbientModeValueDropdownList
    {
        public static ValueDropdownList<AmbientMode> valueDropdownList = new ValueDropdownList<AmbientMode>()
        {
            {  "Skybox", AmbientMode.Skybox },
            {  "Gradient", AmbientMode.Trilight },
            {  "Color", AmbientMode.Flat },
        };
    }
}