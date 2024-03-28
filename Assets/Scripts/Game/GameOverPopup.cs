using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] GameObject gameOverPopup;
    [SerializeField] GameObject loosePopup;
    [SerializeField] GameObject newBestScorePopup;

    private void OnEnable()
    {
        GameEvents.GameOver += OnGameOver;
    }

    private void OnDestroy()
    {
        GameEvents.GameOver -= OnGameOver;
    }

    private void OnGameOver(bool newBestScore)
    {
        gameOverPopup.SetActive(true);
        loosePopup.SetActive(false); 
        newBestScorePopup.SetActive(true);
    }
}
