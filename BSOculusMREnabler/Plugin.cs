using System;
using System.IO;
using System.Linq;
using IllusionPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace BSOculusMREnabler
{
	public class Plugin : IPlugin
	{
        private bool _init;
        private OVRManagerHack _ovrManagerHack;

        public string Name => "OculusXRHack";

		public string Version => "v1.0";

		public void OnApplicationStart()
		{
            if (_init) return;
            _init = true;
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
        }

		public void OnApplicationQuit()
		{
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;

        }

		public void OnLevelWasLoaded(int level)
		{
		}

		public void OnLevelWasInitialized(int level)
		{
		}

		public void OnUpdate()
		{
            
            
            
            
        }

		public void OnFixedUpdate()
		{
		}

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
        {
            if (scene.buildIndex < 1) return;
            Console.WriteLine("enableMixed reality to false");
            OVRManagerHack.enableMixedReality = false;
            if (_ovrManagerHack != null) Object.Destroy(_ovrManagerHack.gameObject);

            var mainCamera = Object.FindObjectsOfType<Camera>().FirstOrDefault(x => x.CompareTag("MainCamera"));
            if (mainCamera == null) return;

            var gameObj = new GameObject("OVRManagerHackTemp");
            //OVRManagerHack.MainCamera = mainCamera;
            _ovrManagerHack = gameObj.AddComponent<OVRManagerHack>();

            OVRManagerHack.enableMixedReality = true;
            Console.WriteLine("enableMixed reality to true");
        }
    }
}