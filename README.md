# BSOculusMREnabler

Beat Saber doesn't work with the native Oculus MR support - the OVRManager is not used by the game.

Liv is the official way to have MR, but unfortunately it doesn't work with the Oculus Store version - only the Steam version with OpenVR. This means everyone who has bought Beat Saber from the Oculus store will not have MR support at all.

This is an attempt to add that support back into Beat Saber.

Unfortunately this is not fully working - there's something wrong in the final render and there seems to be a missing shader of some sort - so all the "special effects" are gone from the render!

# How to use:

## Oculus Camera Tool
- First you need to setup your camera with Oculus camera tool, you should find it somewhere around "C:\Program Files\Oculus\Support\oculus-diagnostics\CameraCalibrationTool\CameraTool.exe"
- Run it and calibrate your camera. In the end click the button that says "Save Camera to OVRServer"
- If you're lost, follow the Oculus guide: https://support.oculus.com/guides/rift/latest/concepts/mr-camera/
- You'll need to do this on every reboot.

### 1. command line parameter
- Run BeatSaber.exe with "-directcomposition" or "-externalcomposition"

### 2. Use a mrc.config file 
- You can also use a mrc.config file which can override some of the settings set by the OVRServer. Save this info inside Beat Saber_Data folder. For default Oculus it should be something inside "D:\Oculus Apps\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data"
Here's an example mrc.config file that you can use
```
{
    "enableMixedReality": true,
    "extraHiddenLayers": {
        "serializedVersion": "2",
        "m_Bits": 0
    },
    "compositionMethod": 2,
    "capturingCameraDevice": 0,
    "flipCameraFrameHorizontally": false,
    "flipCameraFrameVertically": false,
    "handPoseStateLatency": 0.0,
    "sandwichCompositionRenderLatency": 110.0,
    "sandwichCompositionBufferedFrames": 5,
    "chromaKeyColor": {
        "r": 0.0,
        "g": 1.0,
        "b": 0.0,
        "a": 1.0
    },
    "chromaKeySimilarity": 0.23400000238418579,
    "chromaKeySmoothRange": 1.0,
    "chromaKeySpillRange": 0.56299999865889549,
    "useDynamicLighting": false,
    "depthQuality": 1,
    "dynamicLightingSmoothFactor": 8.0,
    "dynamicLightingDepthVariationClampingValue": 0.0010000000474974514,
    "virtualGreenScreenType": 0,
    "virtualGreenScreenTopY": 10.0,
    "virtualGreenScreenBottomY": -10.0,
    "virtualGreenScreenApplyDepthCulling": false,
    "virtualGreenScreenDepthTolerance": 0.20000000298023225
}
```
You can find some specification for each of these values in https://developer.oculus.com/documentation/unity/latest/concepts/unity-mrc/


Based on CameraPlus.