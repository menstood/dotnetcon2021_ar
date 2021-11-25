using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using  UnityEngine.XR.ARSubsystems;

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject placementIndicator;

    private Pose placementPose;
    private bool placementPoseIsValid = false;
    [SerializeField]
    private ARRaycastManager raycastManager;
    [SerializeField]
    private ARSessionOrigin arSessionOrigin;

private bool alreadyDrop = false;
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && alreadyDrop == false)
        {
            PlaceObject();
            alreadyDrop = true;
        }
    }

    private void PlaceObject()
    {
        var go = Instantiate(objectToPlace, placementPose.position,  placementPose.rotation);
        go.transform.rotation *= Quaternion.Euler(0,180f,0);
    }

    private void UpdatePlacementIndicator()
    {

            placementIndicator.SetActive(!alreadyDrop);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = arSessionOrigin.camera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();

        raycastManager.Raycast(screenCenter, hits,TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = arSessionOrigin.camera.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}
