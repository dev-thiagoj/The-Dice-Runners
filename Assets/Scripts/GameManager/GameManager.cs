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
    //[Header("Music Manager")]

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
        AnimationButtons();
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
        PlayerController.Instance.canRun = true;
        RollDice.Instance.canRoll = true;
        RollDice.Instance.CallDiceSFX();
    }
    #endregion

    public void EndGame()
    {
        PlayerController.Instance.canRun = false;
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
        scoreText.text = "Score: " + (ItemManager.Instance.dice * turboScore).ToString("000");
        diceText.text = "Dices: " + ItemManager.Instance.dice.ToString("000");
    }

    void TurnTurboInPoints()
    {
        turboScore = ItemManager.Instance.turbo;

        if (turboScore == 0) turboScore = 1;
    }
}
