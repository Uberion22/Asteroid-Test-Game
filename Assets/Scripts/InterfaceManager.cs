using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] GameObject buttons;
    [SerializeField] GameObject continueBtn;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text livesText;
    [SerializeField] TMP_Text settingsButtonText;
    private float currentTimeScale;

    // Start is called before the first frame update
    void Start()
    {
        currentTimeScale = Time.timeScale;
        Time.timeScale = 0;
        ShowButtons(GameManager.IsNewGame);
        SetSettingsButtonText();
    }

    void OnEnable()
    {
        GameManager.UpdateScore += UpdateScorePoints;
        GameManager.UpdateLives += UpdateLives;
    }

    void OnDisable()
    {
        GameManager.UpdateScore -= UpdateScorePoints;
        GameManager.UpdateLives -= UpdateLives;
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
        GameManager.KeyboardOnly = !GameManager.KeyboardOnly;
        SetSettingsButtonText();
    }

    private void UpdateScorePoints(long currentScore)
    {
        scoreText.text = $"Score: {currentScore}";
    }

    private void UpdateLives(int currentLives)
    {
        if (currentLives == 0)
        {
            ShowButtons();
            continueBtn.SetActive(false);
        }

        livesText.text = $"Lives: {currentLives}";
    }

    private void SetSettingsButtonText()
    {
        settingsButtonText.text = GameManager.KeyboardOnly ? "Controls:\nkeyboard" : "Controls:\nkeyboard + mouse";
    }
}
