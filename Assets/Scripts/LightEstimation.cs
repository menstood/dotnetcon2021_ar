using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class LightEstimation : MonoBehaviour
{
    [SerializeField]
    private ARCameraManager arCameraManager;
    [SerializeField]
    private Light targetLight;

    private void OnEnable()
    {
        arCameraManager.frameReceived += FrameUpdated;
    }

    private void OnDisable()
    {
        arCameraManager.frameReceived -= FrameUpdated;
    }

    private void FrameUpdated(ARCameraFrameEventArgs args)
    {
        if (args.lightEstimation.averageBrightness.HasValue)
            targetLight.intensity = args.lightEstimation.averageBrightness.Value;

        if (args.lightEstimation.averageColorTemperature.HasValue)
            targetLight.colorTemperature = args.lightEstimation.averageColorTemperature.Value;

        if (args.lightEstimation.mainLightColor.HasValue)
            targetLight.color = args.lightEstimation.mainLightColor.Value;

        if (args.lightEstimation.mainLightDirection.HasValue)
            targetLight.transform.rotation = Quaternion.Euler(args.lightEstimation.mainLightDirection.Value);
    }

}