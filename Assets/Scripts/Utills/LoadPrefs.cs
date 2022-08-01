using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPrefs : MonoBehaviour
{
    [Header("Volume Settings")]
    public AudioSource musicSource;
    public Slider volumeSlider;
    public TextMeshProUGUI sliderValue;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            float localVolume = PlayerPrefs.GetFloat("masterVolume");

            sliderValue.text = localVolume.ToString("0.0");
            volumeSlider.value = localVolume;
            musicSource.volume = localVolume;
        }

        if (PlayerPrefs.HasKey("viewedTutorial"))
        {
            int localValue = PlayerPrefs.GetInt("viewedTutorial");

            GameManager.Instance._viewed = localValue;
        }

        if (PlayerPrefs.HasKey("isRestart"))
        {
            int localValue = PlayerPrefs.GetInt("isRestart");

            GameManager.Instance.isRestart = localValue;
        }

        if (PlayerPrefs.HasKey("maxScore"))
        {
            int localValue = PlayerPrefs.GetInt("maxScore");

            GameManager.Instance.maxScore = localValue;
        }

        if (PlayerPrefs.HasKey("level"))
        {
            int localLevel = PlayerPrefs.GetInt("level");

            LevelManager.Instance.level = localLevel;
        }

        if (PlayerPrefs.HasKey("piecesNumber"))
        {
            int localValue = PlayerPrefs.GetInt("piecesNumber");

            LevelManager.Instance.numberOfPieces = localValue;
        }
    }
}
