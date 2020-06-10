using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * @author Rasmus Rosenkjær
 * @version 1.1
 * @date 18/05/2020
 * */
public class SceneLoader
{
    /// <summary>
    /// Loads the next scene
    /// </summary>
    /// @author Rasmus Rosenkjær
    /// @status Done
    /// @date 18/05/2020
    public void LoadScene() 
    {
        SceneManager.LoadScene(1);
    }
}
