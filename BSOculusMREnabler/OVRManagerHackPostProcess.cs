using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BSOculusMREnabler
{
    class OVRManagerHackPostProcess : MonoBehaviour
    {
        public void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Object.FindObjectsOfType<MainCamera>().FirstOrDefault(x => x.CompareTag("MainCamera")).mainEffect.OnRenderImage(src, dest);
        }
    }
}
