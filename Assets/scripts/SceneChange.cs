using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public bool gamePaused;
    // Start is called before the first frame update
    void Start()
    {
        gamePaused = false;
    }

    // Update is called once per frame
    /*void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            PauseGame();

        }
    }*/

    public void loadlevel(string level)
    {
        LoadingManager.LoadInstance.LoadScene(level);

    }


    public void pauseButton()
    {
        gamePaused = !gamePaused;
        if (gamePaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void exitbutton()
    {
        Application.Quit();
    }

}
