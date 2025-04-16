using UnityEngine;
using UnityEngine.UI;

namespace HotFix.ui
{
    public class EmptyRaycast : MaskableGraphic, ICanvasRaycastFilter
    {
        public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            return true;
        }
    }
}