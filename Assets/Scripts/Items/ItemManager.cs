using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using TMPro;

public class ItemManager : Singleton<ItemManager>
{
    public TextMeshProUGUI uiTextDice = null;
    public TextMeshProUGUI uiTextTurbo = null;
    public Transform withoutTurboText;
    public float warningTime;
    public int dice;
    public int turbo;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        dice = 0;
        turbo = PlayerController.Instance.maxTurbos;
    }

    private void Update()
    {
        UpdateUI();
    }

    public void AddCoins(int amount = 1)
    {
        dice += amount;
    }

    public void AddTurbo(int amount = 1)
    {
        turbo += amount;
    }

    public void RemoveTurbo(int amount = 1)
    {
        turbo -= amount;
    }

    public void UpdateUI()
    {
        uiTextDice.text = " x " + dice;
        uiTextTurbo.text = " x " + turbo;
    }

    public void WithoutTurboWarning()
    {
        StartCoroutine(WithoutTurboCoroutine());
    }

    public IEnumerator WithoutTurboCoroutine()
    {
        withoutTurboText.gameObject.SetActive(true);
        yield return new WaitForSeconds(warningTime);
        withoutTurboText.gameObject.SetActive(false);
        StopCoroutine(WithoutTurboCoroutine());
    }
}
