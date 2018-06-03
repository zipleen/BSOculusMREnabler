﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace BSOculusMREnabler
{
    public class OVRManagerHack : MonoBehaviour
    {
        private bool suppressDisableMixedRealityBecauseOfNoMainCameraWarning;
        public static bool enableMixedReality;
        private static bool prevEnableMixedReality = false;
        private bool multipleMainCameraWarningPresented = false;
        private static OVRManager oVRManager;
        private Camera directCamera;

        private Type ovrMixedRealityType;
        private MethodInfo updateMethod;
        private MethodInfo cleanupMethod;

        private Type getOvrMixedRealityType()
        {
            if (ovrMixedRealityType == null)
            {
                Assembly ass = Assembly.Load("Assembly-CSharp-firstpass");
                ovrMixedRealityType = ass.GetType("OVRMixedReality");
            }
            return ovrMixedRealityType;
        }

        public MethodInfo getUpdateMethod()
        {
            if (updateMethod == null)
            {
                Type asmType = getOvrMixedRealityType();

                if (asmType == null)
                {
                    Console.Write("asmtype null");
                    return null;
                }

                updateMethod = asmType.GetMethod("Update", BindingFlags.Public | BindingFlags.Static);
            }
            return updateMethod;
        }

        public MethodInfo getCleanupMethod()
        {
            if (cleanupMethod == null)
            {
                Type asmType = getOvrMixedRealityType();

                if (asmType == null)
                {
                    Console.Write("asmtype null");
                    return null;
                }

                cleanupMethod = asmType.GetMethod("Cleanup", BindingFlags.Public | BindingFlags.Static);
            }
            return cleanupMethod;
        }

        public void Awake()
        {
            this.name = "OVRManagerHack";
            doTheOvrHack();
            //OVRPlugin.occlusionMesh = true;
        }

        public void Update()
        {
            if (!enableMixedReality && !prevEnableMixedReality)
            {
                return;
            }

            if ((UnityEngine.Object)Camera.main != (UnityEngine.Object)null)
            {
                suppressDisableMixedRealityBecauseOfNoMainCameraWarning = false;

                Camera mainCamera = FindMainCamera();
                if ((Camera)mainCamera == (UnityEngine.Object)null)
                {
                    Console.Write("mainCamera null");
                    return;
                }

                if (enableMixedReality)
                {
                    if (base.gameObject == null)
                    {
                        Console.Write("base.gameObject is null");
                        return;
                    }

                    System.Object[] parameters = { base.gameObject, mainCamera, oVRManager.compositionMethod, oVRManager.useDynamicLighting, oVRManager.capturingCameraDevice, oVRManager.depthQuality };
                    getUpdateMethod().Invoke(null, parameters);
                    //OVRMixedReality.Update(base.gameObject, MainCamera, OVRManager.CompositionMethod.Sandwich, false, OVRManager.CameraDevice.WebCamera0, OVRManager.DepthQuality.High);

                    /*
                    OVRDirectComposition ovrComposition = (OVRDirectComposition)getOvrMixedRealityType().GetField("currentComposition", BindingFlags.Static | BindingFlags.Public).GetValue(null);
                    directCamera = ovrComposition.directCompositionCamera;
                    directCamera.allowHDR = false;
                    */
                }
                if (prevEnableMixedReality && !enableMixedReality)
                {
                    Console.WriteLine("Cleanup!");
                    getCleanupMethod().Invoke(null, null);
                    //OVRMixedReality.Cleanup();
                }
                prevEnableMixedReality = enableMixedReality;
            }
            else if (!suppressDisableMixedRealityBecauseOfNoMainCameraWarning)
            {
                Debug.LogWarning("x Main Camera is not set, Mixed Reality disabled");
                suppressDisableMixedRealityBecauseOfNoMainCameraWarning = true;
            }
        }

        public void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Console.Write(".");
            Object.FindObjectsOfType<MainCamera>().FirstOrDefault(x => x.CompareTag("MainCamera")).mainEffect.OnRenderImage(src, dest);
        }

        public void OnDisable()
        {
            Console.WriteLine("OnDisable -> Cleanup!");
            getCleanupMethod().Invoke(null, null);
        }

        private void doTheOvrHack()
        {
            if (oVRManager == null)
            {
                // hack the OVRManager
                oVRManager = new OVRManager();
                typeof(OVRManager).GetProperty("instance").SetValue(null, oVRManager, null);

                
                bool flag = (bool) typeof(OVRManager).GetMethod("LoadMixedRealityCaptureConfigurationFileFromCmd", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
                bool flag2 = (bool) typeof(OVRManager).GetMethod("CreateMixedRealityCaptureConfigurationFileFromCmd", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
                if (flag || flag2)
                {
                    Console.WriteLine("Loading mrc.config...");
                    OVRMixedRealityCaptureSettings oVRMixedRealityCaptureSettings = ScriptableObject.CreateInstance<OVRMixedRealityCaptureSettings>();
                    oVRMixedRealityCaptureSettings.ReadFrom(oVRManager);
                    if (flag)
                    {
                        oVRMixedRealityCaptureSettings.CombineWithConfigurationFile();
                        oVRMixedRealityCaptureSettings.ApplyTo(oVRManager);
                    }
                    if (flag2)
                    {
                        oVRMixedRealityCaptureSettings.WriteToConfigurationFile();
                    }
                    Object.Destroy(oVRMixedRealityCaptureSettings);
                };

                if ((bool)typeof(OVRManager).GetMethod("UseDirectCompositionFromCmd", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null))
                {
                    oVRManager.compositionMethod = OVRManager.CompositionMethod.Direct;
                }
                if ((bool)typeof(OVRManager).GetMethod("UseExternalCompositionFromCmd", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null))
                {
                    oVRManager.compositionMethod = OVRManager.CompositionMethod.External;
                }
                Debug.LogWarning("OVR: CompositionMethod : " + oVRManager.compositionMethod);
            }
        }

        private Camera FindMainCamera()
        {
            return Object.FindObjectsOfType<MainCamera>().FirstOrDefault(x => x.CompareTag("MainCamera")).camera;
            /*
            GameObject[] array = GameObject.FindGameObjectsWithTag("MainCamera");
            List<Camera> list = new List<Camera>(4);
            GameObject[] array2 = array;
            foreach (GameObject gameObject in array2)
            {
                Camera component = gameObject.GetComponent<Camera>();
                if ((Object)component != (Object)null && component.enabled)
                {
                    OVRCameraRig componentInParent = ((Component)component).GetComponentInParent<OVRCameraRig>();
                    if ((Object)componentInParent != (Object)null && (Object)componentInParent.trackingSpace != (Object)null)
                    {
                        list.Add(component);
                    }
                }
            }
            if (list.Count == 0)
            {
                return Camera.main;
            }
            if (list.Count == 1)
            {
                return list[0];
            }
            if (!multipleMainCameraWarningPresented)
            {
                Debug.LogWarning("Multiple MainCamera found. Assume the real MainCamera is the camera with the least depth");
                multipleMainCameraWarningPresented = true;
            }
            list.Sort((Camera c0, Camera c1) => (!(c0.depth < c1.depth)) ? ((c0.depth > c1.depth) ? 1 : 0) : (-1));
            return list[0];
            */
        }
    }
}
