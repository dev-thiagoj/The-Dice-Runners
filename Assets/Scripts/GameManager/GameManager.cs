using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Singleton;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    public GameObject mainMenu;
    public GameObject uiValues;
    public GameObject cameraCanvas;
    public TextMeshProUGUI scoreText = null;
    public TextMeshProUGUI diceText = null;

    [Header("Buttons Animation")]
    public GameObject btnContainer;
    public Ease ease;
    public float timeBtnAnim;

    [Header("End Game")]
    public GameObject endGameScreen;
    public int finalScore;
    public int turboScore;
    public bool checkedEndLine = false;

    [Header("Final Stars")]
    int totalScore;
    public List<GameObject> fullStars;

    [Header("Pause Game")]
    public GameObject pauseScreen;
    private bool _isGameStarted;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        cameraCanvas.SetActive(false);
        uiValues.SetActive(false);
        mainMenu.SetActive(true);
        AnimationButtons();
        _isGameStarted = false;
        TurnAllStarsOff();
    }

    private void Update()
    {
        if (_isGameStarted && Input.GetKeyDown(KeyCode.Escape)) PauseGame();
    }

    void TurnAllStarsOff()
    {
        foreach (var gameObject in fullStars)
        {
            gameObject.SetActive(false);
        }
    }

    public void PlayCameraAnimation()
    {


        StartRun();
    }

    public void AnimationButtons()
    {
        btnContainer.transform.DOScale(0, timeBtnAnim).SetEase(ease).From();
    }

    #region === DEBUG ===
    [NaughtyAttributes.Button]
    public void StartRun()
    {
        SFXPool.Instance.CreatePool();
        _isGameStarted = true;
        cameraCanvas.SetActive(true);
        uiValues.SetActive(true);
        PlayerController.Instance.canRun = true;
        RollDice.Instance.canMove = true;
        RollDice.Instance.CallDiceSFX();
    }
    #endregion

    [NaughtyAttributes.Button]
    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
        AudioListener.pause = true;
    }

    [NaughtyAttributes.Button]
    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
        AudioListener.pause = false;
    }

    [NaughtyAttributes.Button]
    public void EndGame()
    {
        PlayerController.Instance.canRun = false;
        cameraCanvas.SetActive(false);
        uiValues.SetActive(false);
        RollDice.Instance.DestroyDice();
        UpdateUI();
        endGameScreen.SetActive(true);
    }

    public void RestartGame(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void UpdateUI()
    {
        TurnTurboInPoints();
        totalScore = ItemManager.Instance.dice * turboScore;
        if (checkedEndLine) totalScore += 300;
        StarsCalculate();
        scoreText.text = "Score: " + totalScore.ToString("000");
        diceText.text = "Dices: " + ItemManager.Instance.dice.ToString("000");
    }

    void TurnTurboInPoints()
    {
        turboScore = ItemManager.Instance.turbo;

        if (turboScore == 0) turboScore = 1;
    }

    void StarsCalculate()
    {
        if(totalScore > 200 && totalScore < 500)
        {
            fullStars[0].SetActive(true);
        }
        else if(totalScore >= 500 && totalScore < 700)
        {
            fullStars[0].SetActive(true);
            fullStars[1].SetActive(true);
        }
        else if (totalScore >= 700)
        {
            foreach (var star in fullStars)
            {
                star.SetActive(true);
            }
        }
    }
}
