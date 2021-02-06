using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameCoordinator : MonoBehaviour
{
    public static GameCoordinator instance;

    public Text enemyChaseT ;
    public Text playerSafeT;
    public GameObject pauseMenu;
    public GameObject deathScreen;
    public bool isPaused = false;

   

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (isPaused)
            {
                resumeGame();
                return;
            }
            else if (!isPaused)
            {
                pauseGame();
            }
        }
       
    }

    public void playerIsBeingChased()
    {
        playerSafeT.enabled = false;
        enemyChaseT.enabled = true;
    }

    public void playerIsSafe()
    {
        enemyChaseT.enabled = false;
        playerSafeT.enabled = true; 
    }

   public void pauseGame()
    {
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);

    }

    public void resumeGame()
    {

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;

    }

    public void restartLevel()
    {
      
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        resumeGame();
    
    }

    public void quitGame()
    {
        Application.Quit();
    }


    public void playerDied()
    {
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        deathScreen.SetActive(true);

    }
}
