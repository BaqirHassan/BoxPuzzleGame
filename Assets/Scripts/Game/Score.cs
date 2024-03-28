using System.Collections;
using TMPro;
using UnityEngine;

[System.Serializable]
public class BestScoreData
{
    public int score = 0;
}


public class Score : MonoBehaviour
{
    [SerializeField] private SquareTextureData squareTextureData;
    [SerializeField] private TextMeshProUGUI scoreText;


    private bool newBestScore = false;
    private BestScoreData bestScores = new BestScoreData();
    private int currentScores = 0;
    private string bestScoreKey_ = "BestScoreData";

    private void Awake()
    {
        if(BinaryDataStream.Exist(bestScoreKey_))
        {
            StartCoroutine(ReadDataFile());
        }
    }

    private IEnumerator ReadDataFile()
    {
        bestScores = BinaryDataStream.Read<BestScoreData>(bestScoreKey_);
        yield return new WaitForEndOfFrame();
        GameEvents.UpdateBestScoreBar?.Invoke(currentScores, bestScores.score);
    }

    void Start()
    {
        currentScores = 0;
        newBestScore = false;
        squareTextureData.SetStartColor();
        GameEvents.AddScores?.Invoke(0);
        UpdateScoreText();
    }

    private void OnEnable()
    {
        GameEvents.AddScores += AddScores;
        GameEvents.GameOver += SaveBestScore;
    }

    private void OnDisable()
    {
        GameEvents.AddScores -= AddScores;
        GameEvents.GameOver -= SaveBestScore;
    }

    private void SaveBestScore(bool newBestScore)
    {
        BinaryDataStream.Save<BestScoreData>(bestScores, bestScoreKey_);
    }

    private void AddScores(int scoreToAdd)
    {
        currentScores += scoreToAdd;
        
        if(currentScores > bestScores.score)
        {
            newBestScore = true;
            bestScores.score = currentScores;
        }

        UpdateSquareColor();
        GameEvents.UpdateBestScoreBar?.Invoke(currentScores, bestScores.score);
        UpdateScoreText();
        SaveBestScore(true);
    }

    private void UpdateSquareColor()
    {
        if(GameEvents.UpdateSquareColor != null && currentScores  >= squareTextureData.tresholdVal)
        {
            squareTextureData.UpdateColors(currentScores);
            GameEvents.UpdateSquareColor(squareTextureData.currentColor);
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = currentScores.ToString();
    }
}
