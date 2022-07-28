using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Singleton;
using TMPro;

public class GameManager : Singleton<GameManager>
{

    [Header("References")]
    public GameObject mainMenu;
    public GameObject uiValues;
    public GameObject cameraCanvas;
    public TextMeshProUGUI scoreText = null;
    public TextMeshProUGUI diceText = null;
    public TextMeshProUGUI maxScoreText = null;

    [Header("Buttons Animation")]
    public GameObject btnContainer;
    public Ease ease;
    public float timeBtnAnim;

    [Header("End Game")]
    public GameObject endGameScreen;
    public int finalScore;
    public int turboScore;
    public int maxScore;
    public bool checkedEndLine = false;

    [Header("Restart Game")]
    public int isRestart; //padrão binário, 0 = não e 1 = sim.

    [Header("Final Stars")]
    int totalScore;
    public List<GameObject> fullStars;

    [Header("Pause Game")]
    public GameObject pauseScreen;
    private bool _isGameStarted;

    [Header("Tutorial")]
    public GameObject[] tutorialImages;
    public int _viewed = 0;

    [Header("Female Animation")]
    public Animator femaleAnim;

    private void OnEnable()
    {
        Actions.startTutorial += StartTutorialCoroutine;
    }

    private void OnDisable()
    {
        Actions.startTutorial -= StartTutorialCoroutine;
    }

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

        if (isRestart == 0)
        {
            mainMenu.SetActive(true);
            AnimationButtons();
            _isGameStarted = false;
        }
        else StartRun();

        TurnAllStarsOff();
    }

    private void Update()
    {
        if (_isGameStarted && Input.GetKeyUp(KeyCode.Escape)) PauseGame();
    }

    void TurnAllStarsOff()
    {
        foreach (var gameObject in fullStars)
        {
            gameObject.SetActive(false);
        }
    }

    public void AnimationButtons()
    {
        btnContainer.transform.DOScale(0, timeBtnAnim).SetEase(ease).From();
    }

    public void StartRun()
    {
        SFXPool.Instance.CreatePool();
        _isGameStarted = true;
        cameraCanvas.SetActive(true);
        uiValues.SetActive(true);
        //StartCoroutine(TutorialCoroutine());
        PlayerController.Instance.InvokeStartRun();
        RollDice.Instance.InvokeStartRoll();
        RollDice.Instance.CallDiceSFX();
        Cursor.visible = false;
    }

    public void PauseGame()
    {
        RollDice.Instance.canMove = false;
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
        AudioListener.pause = true;
        Cursor.visible = true;

        if (Input.GetKeyDown(KeyCode.Escape)) ResumeGame();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        RollDice.Instance.canMove = true;
        pauseScreen.SetActive(false);
        AudioListener.pause = false;
        Cursor.visible = false;
    }

    public void EndGame()
    {
        PlayerController.Instance.canRun = false;
        cameraCanvas.SetActive(false);
        uiValues.SetActive(false);
        femaleAnim.SetTrigger("FemaleWin");
        PlayerController.Instance.animator.SetTrigger("EndGame");
        //RollDice.Instance.DestroyDice();
        UpdateUI();
        Invoke(nameof(ShowEndGameScreen), 5);
    }

    public void ShowEndGameScreen()
    {
        endGameScreen.SetActive(true);
        Cursor.visible = true;
    }

    public void RestartGame(int i)
    {
        PlayerPrefs.SetInt("isRestart", 1);
        SceneManager.LoadScene(i);
    }

    public void GoToMenu(int i)
    {
        PlayerPrefs.SetInt("isRestart", 0);
        SceneManager.LoadScene(i);
    }

    public void ExitApplication()
    {
        PlayerPrefs.SetInt("viewedTutorial", 0);
        PlayerPrefs.SetInt("isRestart", 0);
        Application.Quit();
    }

    public void UpdateUI()
    {
        TurnTurboInPoints();
        totalScore = ItemManager.Instance.dice * turboScore;
        if (checkedEndLine) totalScore += 300;
        SaveMaxScore();
        StarsCalculate();
        scoreText.text = "Score: " + totalScore.ToString("000");
        diceText.text = "Dices: " + ItemManager.Instance.dice.ToString("000");
    }

    void TurnTurboInPoints()
    {
        turboScore = ItemManager.Instance.turbo;

        if (turboScore == 0) turboScore = 1;
    }

    void SaveMaxScore()
    {
        if (totalScore > maxScore)
        {
            maxScore = totalScore;

            maxScoreText.text = ("NEW  " + maxScore).ToString();
            maxScoreText.color = Color.green;

            PlayerPrefs.SetInt("maxScore", maxScore);
        }
        else
        {
            maxScoreText.text = maxScore.ToString();
            maxScoreText.color = Color.yellow;
        }
    }

    void StarsCalculate()
    {
        if (totalScore > 200 && totalScore < 500)
        {
            fullStars[0].SetActive(true);
        }
        else if (totalScore >= 500 && totalScore < 700)
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

    public void StartTutorialCoroutine()
    {
        if (_viewed == 0) StartCoroutine(TutorialCoroutine());
        else return;
    }

    public IEnumerator TutorialCoroutine()
    {
        for (int i = 0; i < tutorialImages.Length; i++)
        {
            tutorialImages[i].SetActive(true);
            yield return new WaitForSeconds(3);
            tutorialImages[i].SetActive(false);
            yield return new WaitForSeconds(1);
        }

        _viewed = 1;
        PlayerPrefs.SetInt("viewedTutorial", 1);
    }
}
