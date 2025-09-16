using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static event Action NewGame;
    public static event Action GameOver;
    public static event Action GameWon;
    public static event Action newLife;
    public static event Action<int> scoreUpdated;
    public static event Action<int> livesUpdated;
    public static event Action<int> pointsLeftUpdated;

    public static UnityEvent onGameStart;

    int score;
    int lives;
    int pointsLeft;

    public void StartNewGame()
    {
        setScore(0);
        setLives(3);
        NewGame?.Invoke();
        Debug.Log("New Game Started");
    }
    public void EndGame()
    {
        GameOver?.Invoke();
        Debug.Log("Game Over");
    }
    public void gameWon()
    {
        GameWon?.Invoke();
        Debug.Log("Game Over");
    }

    private void OnEnable()
    {

        GridManager.pointsActivated += setPointsLeft;
        point.pointScored += OnPointScored;
        ghost.playerHit += OnLifeLost;
        ghost.ghostHit += OnGhostHit;
    }

    private void OnPointScored()
    {
        incrementScore();
        decrementPointsLeft();
        if (pointsLeft <= 0)
        {
            gameWon();
        }
    }

    void setScore(int newScore)
    {
        score = newScore;
        scoreUpdated?.Invoke(score);
    }
    void incrementScore()
    {
        score++;
        scoreUpdated?.Invoke(score);
    }
    void incrementScore(int plus)
    {
        score+=plus;
        scoreUpdated?.Invoke(score);
    }
    void decrementLives()
    {
        lives--;
        livesUpdated?.Invoke(lives);
    }
    void setLives(int newLives)
    {
        lives = newLives;
        livesUpdated?.Invoke(lives);
    }

    void decrementPointsLeft()
    {
        pointsLeft--;
        pointsLeftUpdated?.Invoke(pointsLeft);
    }

    void setPointsLeft(int count)
    {
        pointsLeft = count;
        pointsLeftUpdated?.Invoke(pointsLeft);
    }
    void OnLifeLost()
    {
        decrementLives();
        if (lives <= 0)
        {
            EndGame();
        }
        else
        {
            newLife?.Invoke();
        }
    }
    void OnGhostHit()
    {
        incrementScore(20);
    }
}
