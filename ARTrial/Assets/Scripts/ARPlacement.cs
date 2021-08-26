using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

public class ARPlacement : MonoBehaviour
{
    public GameObject[] arObjectToSpawn;
    public GameObject placementIndicator;

    private GameObject spawnedObject;
    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;
    private System.Random rnd;

    // Start is called before the first frame update
    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnedObject == null && placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began )
        {
            ARPlaceObject();
        } 
        else if (placementPoseIsValid && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began ) {
            ARRemoveObject();
        }
            
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if(placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;
        }
    }

    void UpdatePlacementIndicator() 
    {
        if(spawnedObject == null && placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    
    }

    void ARPlaceObject()
    {
        rnd = new System.Random();
        int random = rnd.Next(0, 2);
        spawnedObject = Instantiate(arObjectToSpawn[random], PlacementPose.position, PlacementPose.rotation);
    }

    void ARRemoveObject()
    {
        Destroy(spawnedObject);
        spawnedObject = null;
    }
}
