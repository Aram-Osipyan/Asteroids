using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] private InputController inputController;

    private CanvasGroup _canvasGroup;
    private void Awake()
    {
        inputController.OnGamePause += PauseGame;
        inputController.OnGameRevival += ReviveGame;
    }

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }

    private void PauseGame()
    {
        _canvasGroup.alpha = 1;
        Time.timeScale = 0;        
    }
    private void ReviveGame()
    {
        _canvasGroup.alpha = 0;
        Time.timeScale = 1;
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        ReviveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ContinueGame()
    {
        ReviveGame();
    }
}
