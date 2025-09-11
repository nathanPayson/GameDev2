using TMPro;
using UnityEngine;

public class gameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI pointsLeftText;
    [SerializeField] TextMeshProUGUI livesText;

    private void OnEnable()
    {
        GameManager.pointsLeftUpdated += UpdatePointsLeft;
        GameManager.scoreUpdated += UpdateScore;
        GameManager.livesUpdated += UpdateLives;
    }
    private void OnDisable()
    {
        GameManager.pointsLeftUpdated -= UpdatePointsLeft;
        GameManager.scoreUpdated -= UpdateScore;
    }
    void UpdatePointsLeft(int pointsLeft)
    {
        pointsLeftText.text = "Points Left: " + pointsLeft;
    }
    void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }
    void UpdateLives(int lives)
    {
        livesText.text = "Lives: " + lives;
    }
}
