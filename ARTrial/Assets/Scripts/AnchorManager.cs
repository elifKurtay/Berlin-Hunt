using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class AnchorManager : MonoBehaviour
{
    [SerializeField]
    private Text debugLog;
    [SerializeField]
    private Text anchorCount;
    [SerializeField]
    private Button toggleButton;
    [SerializeField]
    private Button clearButton;

    private ARRaycastManager aRRaycastManager;
    private ARAnchorManager aRAnchorManager;
    private ARPlaneManager aRPlaneManager;

    private List<ARAnchor> anchors = new List<ARAnchor>();
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    
    void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRAnchorManager = GetComponent<ARAnchorManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();

        toggleButton.onClick.AddListener(TogglePlaneDetection);
        clearButton.onClick.AddListener(ClearAnchors);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);
        if(touch.phase != TouchPhase.Began)
            return;

        if(aRRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            ARAnchor anchor = aRAnchorManager.AddAnchor(hitPose);

            if (anchor == null)
            {
                string errorText = "There was an error creating an anchor. \n";
                Debug.Log(errorText);
                debugLog.text += errorText;
            }
            else
            {
                anchors.Add(anchor);
                Debug.Log("Anchor added to the list of anchors. ");
                debugLog.text = $"Anchor's coordinates: ({anchor.transform.position.x}," +
                    $"{anchor.transform.position.y}, {anchor.transform.position.z})";
                anchorCount.text = $"Anchor Count: {anchors.Count}";
            }
        }
    }

    private void TogglePlaneDetection()
    {
        aRPlaneManager.enabled = !aRPlaneManager.enabled;

        foreach (ARPlane plane in aRPlaneManager.trackables)
            plane.gameObject.SetActive(aRPlaneManager.enabled);

        toggleButton.GetComponentInChildren<Text>().text = aRPlaneManager.enabled ?
            "AR Plane Off" : "AR Plane On";
    }

    private void ClearAnchors()
    {

        foreach (var anchor in anchors)
            aRAnchorManager.RemoveAnchor(anchor);

        anchors.Clear();
        anchorCount.text = $"Anchor Count: 0";
    }
}
