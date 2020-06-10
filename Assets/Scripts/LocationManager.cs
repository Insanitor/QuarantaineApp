using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Android;
using System;

/**
 * @author Rasmus Rosenkjær
 * @version 1.1
 * @date 20/05/2020
 * */

public class LocationManager : MonoBehaviour
{
    public float longitude, latitude;
    public TextMeshProUGUI text;
    public TextMeshProUGUI status;
    public GameObject camera;
    public GameObject orangeCircle;
    public string locationStatus = "Green";

    public CurrentLocationDTO[] currentLocationDTOs = new CurrentLocationDTO[60];

    /// <summary>
    /// Checks if the user as accepted GPS location
    /// then starts 3 coroutines
    /// </summary>
    /// @author Rasmus Rosenkjær
    /// @status Done
    /// @date 20/05/2020
    private void Start()
    {

        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        if (Input.location.isEnabledByUser)
        {
            StartCoroutine(GetLocation());
            StartCoroutine(SaveCurrentLocation());
            StartCoroutine(SendBulkData());
        }

        Input.compass.enabled = true;
    }

    /// <summary>
    /// Sets the text UI element to show
    /// the Longitude and Latitude of the user
    /// </summary>
    /// @author Rasmus Rosenkjær
    /// @status Done
    /// @date 20/05/2020
    void FixedUpdate()
    {
        //status.text = Input.location.status.ToString();
        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;

        text.text = "Longtitude: " + longitude + "\n" +
            "Latitude: " + latitude + "\n" +
            "Player x:" + transform.position.x + "\nPlayer y:" + transform.position.y;
        this.transform.position = new Vector3(longitude, latitude, 0);
        //this.transform.position = new Vector3((((longtitude * 10000) % 10f) / 10), (((latitude * 10000) % 10f) / 10), 0);
    }

    /// <summary>
    /// A Enumerator to get the new position of the user.
    /// </summary>
    /// @author Rasmus Rosenkjær
    /// @status Done
    /// @date 20/05/2020
    /// <returns></returns>
    IEnumerator GetLocation()
    {
        Input.location.Start(5f, 2f);
        while (Input.location.status == LocationServiceStatus.Initializing)
        {
            yield return new WaitForSeconds(0.5f);
        }
        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;
        camera.transform.position = new Vector3(longitude, latitude, -10);
        orangeCircle.transform.position = new Vector3(longitude, latitude, 0);
        this.transform.position = new Vector3(longitude, latitude, 0);
        yield break;
    }

    public int entryCounter = 0;
    public int yellowCounter = 0;

    /// <summary>
    /// A Enumerator to save the location of the user
    /// this is done every 2 minutes
    /// </summary>
    /// @author Rasmus Rosenkjær
    /// @status Done
    /// @date 20/05/2020
    /// <returns></returns>
    IEnumerator SaveCurrentLocation()
    {
        yield return new WaitForSecondsRealtime(120);

        if (locationStatus.Equals("Yellow"))
        {
            yellowCounter++;
            if (yellowCounter >= 3)
            {
                locationStatus = "Red";
                this.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            }
        }

        CurrentLocationDTO location = new CurrentLocationDTO();

        location.UserID = DatabaseManager.userID;

        location.Status = locationStatus;

        location.Latitude = latitude;

        location.Longitude = longitude;

        location.CurrentTime = DateTime.Now.ToString();

        location.Act = false;

        currentLocationDTOs[entryCounter] = location;

        if (locationStatus.Equals("Red"))
        {

            status.text = DatabaseManager.SendRedEntry(currentLocationDTOs[entryCounter]);
        }


        entryCounter++;

        StartCoroutine(SaveCurrentLocation());

    }

    /// <summary>
    /// An Enumerator to send the saved locations
    /// of the user, this is done every 2 hours
    /// </summary>
    /// @author Rasmus Rosenkjær
    /// @status Done
    /// @date 20/05/2020
    /// <returns></returns>
    IEnumerator SendBulkData()
    {
        yield return new WaitForSecondsRealtime(7200);
        DatabaseManager.SendBulkData(currentLocationDTOs);

        StartCoroutine(SendBulkData());
    }

    /// <summary>
    /// Unity method to check if the user left his area
    /// </summary>
    /// @author Rasmus Rosenkjær
    /// @status Done
    /// @date 20/05/2020
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        locationStatus = "Yellow";
        yellowCounter++;
        this.GetComponent<SpriteRenderer>().color = new Color(255, 255, 0);
    }

    /// <summary>
    /// Unity method to check if the user entered his area
    /// </summary>
    /// @author Rasmus Rosenkjær
    /// @status Done
    /// @date 20/05/2020
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        locationStatus = "Green";
        yellowCounter = 0;
        this.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
    }
}
