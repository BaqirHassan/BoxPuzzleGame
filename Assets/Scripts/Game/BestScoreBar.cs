using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BestScoreBar : MonoBehaviour
{
    [SerializeField] Image FillinImage;
    [SerializeField] TextMeshProUGUI BestScoreText;


    private void OnEnable()
    {
        GameEvents.UpdateBestScoreBar += UpdateBestScoreBar;
    }

    private void OnDisable()
    {
        GameEvents.UpdateBestScoreBar -= UpdateBestScoreBar;
    }

    private void UpdateBestScoreBar(int currentScore, int bestScore)
    {
        float currenPercentage = (float)currentScore / (float)bestScore;
        FillinImage.fillAmount = currenPercentage;
        BestScoreText.text = bestScore.ToString();
    }
}
