using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * @author Rasmus Rosenkjær
 * @version 1.1
 * @date 25/05/2020
 * */

public class LoginManager : MonoBehaviour
{
    [SerializeField]
    TMP_InputField username;
    [SerializeField]
    TMP_InputField password;
    [SerializeField]
    TextMeshProUGUI errorText;

    /// <summary>
    /// Tells the Database manager to send a query
    /// for login, with the input username and password
    /// and checks for the response.
    /// </summary>
    /// @author Rasmus Rosenkjær
    /// @status Done
    /// @date 25/05/2020
    public void Login()
    {
        SceneLoader sceneLoader = new SceneLoader();

        string reply = DatabaseManager.QueryDatabaseForLogin(username.text, password.text);
        if (reply == "OK")
        {
            sceneLoader.LoadScene();
            errorText.text = "Logging in";
        }
        else
        {
            errorText.text = reply;
        }

    }

}
