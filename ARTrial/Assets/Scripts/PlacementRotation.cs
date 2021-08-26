using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlacementObject))]
public class PlacementRotation : MonoBehaviour
{
    private PlacementObject placementObject;

    [SerializeField]
    private Vector3 rotationSpeed = Vector3.zero;

    void Awake()
    {
        placementObject = GetComponent<PlacementObject>();
    }


    // Update is called once per frame
    void Update()
    {
        if(placementObject.Selected)
        {
            transform.Rotate(rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}
