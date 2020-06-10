
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;

public enum ResponseMessage
{
    OK,
    Failed
}

/**
 * @author Rasmus Rosenkjær
 * @version 1.1
 * @date 25/05/2020
 * */
public static class DatabaseManager
{
    public static int loginID;
    public static int userID;

    /// <summary>
    /// Sends a query to UserLogin Database
    /// Checks if the user input is correct
    /// </summary>
    /// @author Rasmus Rosenkjær
    /// @status Done
    /// @date 25/05/2020
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns> Returns a ResponeMessage of eiter OK or failed </returns>
    public static string QueryDatabaseForLogin(string username, string password)
    {
        using (MySqlConnection conn = new MySqlConnection("Server=172.16.21.168;port=3306;Database=UserLogin;UiD=root;Pwd=rd2020"))
        {
            try
            {

                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                string query = $"SELECT Username, AES_DECRYPT(AES_ENCRYPT('Kode123', 'keystring'), 'keystring'), LoginID FROM Login WHERE Username = '{username}' AND AES_DECRYPT(AES_ENCRYPT('Kode123','keystring'),'keystring') = '{password}';";


                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    loginID = reader.GetInt32(2);
                    userID = GetUserID();
                    return "OK";
                }
                else
                {
                    return "FAILED!";
                }
            }
            catch (System.Exception e)
            {
                return e.ToString();
            }
        }
    }

    /// <summary>
    /// Sends a query to UserLogin Database
    /// </summary>
    /// @author Rasmus Rosenkjær
    /// @status Done
    /// @date 25/05/2020
    /// <returns> Returns the UserID </returns>
    public static int GetUserID()
    {
        using (MySqlConnection conn = new MySqlConnection("Server=172.16.21.168;port=3306;Database=UserLogin;UiD=root;Pwd=rd2020"))
        {
            try
            {

                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                string query = $"SELECT UserID FROM User WHERE LoginID = '{loginID}';";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return reader.GetInt32(0);
                }

                return -1;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Sends a query to AppData database
    /// and sends the entries of location
    /// this is done every 2 hours
    /// </summary>
    /// @author Rasmus Rosenkjær
    /// @status Done
    /// @date 25/05/2020
    /// <param name="currentLocationDTO"></param>
    public static void SendBulkData(CurrentLocationDTO[] currentLocationDTO)
    {
        using (MySqlConnection conn = new MySqlConnection("Server=172.16.21.169;port=3306;Database=AppData;UiD=root;Pwd=rd2020"))
        {
            try
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                string query = "INSERT INTO CurrentLocation(UserID, status, latitude, longitude, currentTime, act) VALUES (";


                for (int i = 0; i < currentLocationDTO.Length; i++)
                {
                    query += currentLocationDTO[i].UserID + ",'" +
                        currentLocationDTO[i].Status + "'," +
                        currentLocationDTO[i].Latitude + "," +
                        currentLocationDTO[i].Longitude + ",'" +
                        currentLocationDTO[i].CurrentTime + "'," +
                        currentLocationDTO[i].Act + ")";
                    if (i != currentLocationDTO.Length - 1)
                    {
                        query += ",(";
                    }
                }
                query += ";";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();
            }
            catch (System.Exception e)
            {
            }

        }

    }

    /// <summary>
    /// Sends a query to insert an entry
    /// this is done if a status of the user is red
    /// </summary>
    /// @author Rasmus Rosenkjær
    /// @status Done
    /// @date 25/05/2020
    /// <param name="currentLocationDTO"></param>
    public static string SendRedEntry(CurrentLocationDTO currentLocationDTO)
    {
        using (MySqlConnection conn = new MySqlConnection("Server=172.16.21.169;port=3306;Database=AppData;UiD=root;Pwd=rd2020"))
        {
            try
            {

                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                string query = "INSERT INTO CurrentLocation(UserID, status, latitude, longitude, currentTime, act) VALUES (";

                query += currentLocationDTO.UserID + ",'" +
                    currentLocationDTO.Status + "'," +
                    currentLocationDTO.Latitude + "," +
                    currentLocationDTO.Longitude + ",'" +
                    currentLocationDTO.CurrentTime + "'," +
                    currentLocationDTO.Act + ")";
                query += ";";


                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();

                return "Red Entry was sent";

            }
            catch (System.Exception e)
            {
                return e.ToString();
            }


        }

    }
}



