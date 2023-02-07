using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Takes control of player's collected data and its interreaction with the rules of the game
/// </summary>
public class GameManager : MonoBehaviour
{
    private Action onBonusTaken;
    private Action onTimeTurnerTaken;

    public static GameManager Instance { get; set; } //Singleton pattern

    public bool isBonusTaken;
    public bool isTimeTurnerTaken;
    public bool isTimeTicking;
    public bool isTimeOver; // shows if timer hit zero
    public int levelReached; // controls what level we are at
    public float totalTime; // overall time spent playing
    [SerializeField] public float maxTime; // max time given for each level
    [SerializeField] public Button reloadSceneButton;
    [SerializeField] private Image timerImage;
    [SerializeField] private TMP_Text bonusTakenText;
    [SerializeField] private AudioSource bonusSound;
    [SerializeField] private AudioSource timeTurnerSound;
    private int bonusPoints;
    private float currentTime; // time left before gaveOver
    
    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);

        currentTime = maxTime;
        isBonusTaken = false;
        isTimeTurnerTaken = false;
        isTimeTicking = true;
        isTimeOver = false;
        bonusPoints = 0;
        levelReached = 1;

        onBonusTaken += IncrementBonusPoints;
        onBonusTaken += ProduceBonusSound;
        onBonusTaken += DisableReloadLevelButton;

        onTimeTurnerTaken += ProduceTimeTurnerSound;
        onTimeTurnerTaken += TurnTimeBack;
    }

    public int GetBonusPoints() => bonusPoints;

    public void TurnTimeBack()
    {
        currentTime = maxTime;
    }

    private void IncrementBonusPoints()
    {
        bonusPoints += 1;
    }

    private void ProduceBonusSound()
    {
        bonusSound.Play();
    }

    private void ProduceTimeTurnerSound()
    {
        timeTurnerSound.Play();
    }

    private void DisableReloadLevelButton()
    {
        reloadSceneButton.interactable = false;
    }

    private void Update()
    {
        if(isBonusTaken)
        {
            onBonusTaken?.Invoke();
            isBonusTaken = false;
        }
        if(isTimeTurnerTaken)
        {
            onTimeTurnerTaken?.Invoke();
            isTimeTurnerTaken = false;
        }
        if (isTimeTicking)
        {
            currentTime -= Time.deltaTime;
            totalTime += Time.deltaTime;
        } 
        timerImage.fillAmount = currentTime / maxTime;
        bonusTakenText.text = $"{bonusPoints} / 7";
        if(currentTime <= 0)
        {
            isTimeOver = true;
        }
    }
}
