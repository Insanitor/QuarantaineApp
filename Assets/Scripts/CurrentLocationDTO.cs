using System;

/**
 * @author Rasmus Rosenkjær
 * @version 1.1
 * @date 19/05/2020
 * */
public class CurrentLocationDTO
{
    private int userID;
    private string status;
    private double latitude;
    private double longitude;
    private string currentTime;
    private bool act;

    /// <summary>
    /// Constuctor
    /// </summary>
    /// @author Rasmus Rosenkjær
    /// @status Done
    /// @date 19/05/2020
    /// <param name="userID"></param>
    /// <param name="status"></param>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <param name="currentTime"></param>
    /// <param name="act"></param>
    public CurrentLocationDTO(int userID, string status, double latitude, double longitude, string currentTime, bool act)
    {
        UserID = userID;
        Status = status;
        Latitude = latitude;
        Longitude = longitude;
        CurrentTime = currentTime;
        Act = act;
    }

    public CurrentLocationDTO() 
    {

    }

    public int UserID
    {
        get
        {
            return userID;
        }

        set
        {
            userID = value;
        }
    }

    public string Status
    {
        get
        {
            return status;
        }

        set
        {
            status = value;
        }
    }

    public double Latitude
    {
        get
        {
            return latitude;
        }

        set
        {
            latitude = value;
        }
    }

    public double Longitude
    {
        get
        {
            return longitude;
        }

        set
        {
            longitude = value;
        }
    }

    public string CurrentTime
    {
        get
        {
            return currentTime;
        }

        set
        {
            currentTime = value;
        }
    }

    public bool Act
    {
        get
        {
            return act;
        }

        set
        {
            act = value;
        }
    }



}
