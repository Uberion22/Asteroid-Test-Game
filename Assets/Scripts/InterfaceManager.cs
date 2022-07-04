using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    private float currentTimeScale;
    [SerializeField] GameObject buttons;
    [SerializeField] GameObject continueBtn;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text livesText;
    [SerializeField] private TMP_Text settingsButtonText;
    
    private bool isMouseUsed;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.UpdateScore += UpdateScorePoints;
        GameManager.UpdateLives += UpdateLives;
        currentTimeScale = Time.timeScale;
        Time.timeScale = 0;
        ShowButtons(GameManager.IsNewGame);
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape))
       {
           ShowButtons(!buttons.activeSelf);
       }
    }

    public void OnContinuePress()
    {
        Time.timeScale = currentTimeScale;
        ShowButtons(false);
    }

    public void OnRestartPress()
    {
        if (!GameManager.IsNewGame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Debug.Log("Reset");
        }
        ShowButtons(false);
        GameManager.IsNewGame = false;
        Time.timeScale = currentTimeScale;
    }

    public void ShowButtons(bool show = true)
    {
        buttons.SetActive(show);
        Time.timeScale = show ? 0 : currentTimeScale;
        continueBtn.SetActive(!GameManager.IsNewGame);
    }

    public void OnExitPress()
    {
        Application.Quit();
    }

    public void OnSettingsPress()
    {
        isMouseUsed = !isMouseUsed;
        settingsButtonText.text = isMouseUsed ? "Controls:\nkeyboard + mouse" : "Controls:\nkeyboard";
    }

    private void UpdateScorePoints(object currentScore, EventArgs e)
    {
        scoreText.text = $"Score: {currentScore}";
    }

    private void UpdateLives(object currentLives, EventArgs e)
    {
        if ((int) currentLives == 0)
        {
            ShowButtons();
            continueBtn.SetActive(false);
        }

        Debug.Log("Dameget");
        livesText.text = $"Lives: {currentLives}";
    }
}
