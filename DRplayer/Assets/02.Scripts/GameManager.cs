using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void Startgame()
    {
        
        SceneManager.LoadScene(1);
       
    }

    public void HomeMenu()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void Exitgame()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Application.Quit();
        }
    }


}
