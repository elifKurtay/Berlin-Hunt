using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class DistanceCalculator : MonoBehaviour
{
    private float deviceLatitude = 0f;
    private float deviceLongtitude = 0f;

    //first 10 chest, rest arrows
    private float[,] allLocations = new float[,] { { 52.516306f, 13.378211f}, { 52.514517f, 13.377618f}, { 52.516233f, 13.377018f },
                                                    { 52.518451f, 13.374166f}, { 52.512567f, 13.380830f}, { 52.509653f, 13.375916f},
                                                    { 52.513403f, 13.392091f}, {52.518852f, 13.400339f}, { 52.520303f, 13.399900f},
                                                    { 52.508246f, 13.334560f},
                                                    { 52.515225f, 13.377719f}, { 52.516355f, 13.378683f}, {52.517183f, 13.37742f },
                                                    { 52.51603f, 13.373376f}, { 52.517864f, 13.370896f}, { 52.519493f, 13.373513f},
                                                    { 52.512976f, 13.377366f}, {52.510941f, 13.377118f}, { 52.510125f, 13.383741f},
                                                    { 52.510576f, 13.389212f}, { 52.512192f, 13.38972f}, { 52.515986f, 13.39296f},
                                                     {52.517705f, 13.398089f}, { 52.520822f, 13.400794f}};
    private string[] allLinks = new string[] { "https://www.youtube.com/watch?v=QtDmK1vRIFU&ab_channel=TheTimeTravelArtist",
                                                "https://www.youtube.com/watch?v=ezrWvlTHpMM&t=3s&ab_channel=RickSteves%27Europe",
                                                "https://www.youtube.com/watch?v=YcKSN8c-Sjg&ab_channel=FramingReality",
                                                "https://www.youtube.com/watch?v=yZ866HxCyHo&ab_channel=DWNews",
                                                "https://www.youtube.com/watch?v=6mHhHSgiJrk&ab_channel=DWNews",
                                                "https://www.youtube.com/watch?v=v36rp-F0Ntg&ab_channel=DWNews",
                                                "https://www.youtube.com/watch?v=vi55XA1TP4g&ab_channel=Howcast",
                                                "https://www.youtube.com/watch?v=_wf9IRUJ8qI&ab_channel=Panorama-bSightseeingBerlin",
                                                "https://www.youtube.com/watch?v=SCBoJLyTCOo&ab_channel=DWNews",
                                                "https://www.youtube.com/watch?v=lvxDJx8ku3k&ab_channel=Trogain"};

    private int LOC_SIZE = 24;

    public Text distanceTextObject;
    public GameObject openChest;

    // Start is called before the first frame update
    void Awake()
    {
        //start GetCoordinate() function 
        StartCoroutine("GetCoordinates");
    }

    // Update is called once per frame
    void Update()
    {
        int i = findClosestDistance();
        printDistance(i);

    }

    public void ButtonClicked()
    {
        int i = findClosestDistance();
        if(i < 10)
            Application.OpenURL(allLinks[i]);
    }

    //locate the closest game object to device in the array of locations
    // returns the index of the closest object 
    private int findClosestDistance()
    {
        int index = 0;
        float dist = 0f;
        float min = Calc(allLocations[0,0], allLocations[0,1], deviceLatitude, deviceLongtitude);
        for (int i = 1; i < LOC_SIZE; i++)
        {
            dist = Calc(allLocations[i,0], allLocations[i,1], deviceLatitude, deviceLongtitude);
            if(dist < min)
            {
                min = dist;
                index = i;
            }
        }
        if(min < 5f)
        {
            openChest.SetActive(true);
        } else
            openChest.SetActive(false);

        Debug.Log("In find");
        return index;
    }

    private void printDistance(int index)
    {
        float distance = Calc(allLocations[index,0], allLocations[index,1], deviceLatitude, deviceLongtitude);
        
        distanceTextObject.text = "Distance to Next Checkpoint: " + distance + "m";
    }

    //calculates distance between two sets of coordinates, taking into account the curvature of the earth.
    private float Calc(float lat1, float lon1, float lat2, float lon2)
    {

        var R = 6378.137; // Radius of earth in KM
        var dLat = lat2 * Mathf.PI / 180 - lat1 * Mathf.PI / 180;
        var dLon = lon2 * Mathf.PI / 180 - lon1 * Mathf.PI / 180;
        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
          Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180) *
          Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
        var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        var distance = R * c;
        distance = distance * 1000f; // meters
                                     //set the distance text on the canvas
        

        //convert distance from double to float
        return (float)distance;
    }

    IEnumerator GetCoordinates()
    {
        while (true)
        {

            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Permission.RequestUserPermission(Permission.FineLocation);
                Permission.RequestUserPermission(Permission.CoarseLocation);
            }
            // First, check if user has location service enabled
            if (!Input.location.isEnabledByUser)
                yield return new WaitForSeconds(10);

            // Start service before querying location
            Input.location.Start();

            // Wait until service initializes
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            // Service didn't initialize in 20 seconds
            if (maxWait < 1)
            {
                Debug.Log("Timed out");
                print("Timed out");
                yield break;
            }

            // Connection has failed
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.Log("Unable to determine device location");
                print("Unable to determine device location");
                yield break;
            }
            else
            {
                // Access granted and location value could be retrieved
                print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

                //overwrite current lat and lon everytime
                deviceLatitude = Input.location.lastData.latitude;
                deviceLongtitude = Input.location.lastData.longitude;
            }
            Input.location.Stop();
        }
    }
}
