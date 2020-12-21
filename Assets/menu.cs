using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject startPanel;
    public GameObject pausePanel;
    public GameObject endPanel;
    public Text winnerText;
    public GameObject stick;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && gameManager.gameInProgress)
        {
            PauseGame();
        } else if (Input.GetKeyDown(KeyCode.Escape) && gameManager.gameInProgress) {
            EndGame("Game finished earlier");
        }


    }

    public void StartGame()
    {
        gameManager.gameInProgress = true;
        HideCursor();
        stick.SetActive(true);
        startPanel.SetActive(false);
    }

    public void ResumeGame()
    {
        gameManager.gameInProgress = true;
        HideCursor();
        stick.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void PauseGame()
    {
        gameManager.gameInProgress = false;
        ShowCursor();
        stick.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void EndGame(string winner)
    {
        winnerText.text = winner;
        gameManager.gameInProgress = false;
        ShowCursor();
        stick.SetActive(false);
        endPanel.SetActive(true);
    }

    public void ReloadLevel()
    {
       Application.LoadLevel(0);
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
