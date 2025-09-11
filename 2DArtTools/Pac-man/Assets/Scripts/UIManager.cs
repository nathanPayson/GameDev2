using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject MainMenuUI;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject gameWonUI;

    private void OnEnable()
    {
        GameManager.NewGame += ShowGameUI;
        GameManager.GameOver += ShowGameOverUI;
        GameManager.GameWon += ShowGameWonUI;
    }

    private void OnDisable()
    {
        GameManager.NewGame -= ShowGameUI;
        GameManager.GameOver -= ShowGameOverUI;
        GameManager.GameWon -= ShowGameWonUI;
    }
    void ShowGameUI()
    {
        Debug.Log("Show Game UI");
        // Implement UI logic to show game HUD
        gameUI.SetActive(true);
        gameOverUI.SetActive(false);
        gameWonUI.SetActive(false);
        MainMenuUI.SetActive(false);
    }
    void ShowGameOverUI()
    {
        Debug.Log("Show Game Over UI");
        // Implement UI logic to show game over screen
        gameUI.SetActive(false);
        gameOverUI.SetActive(true);
        gameWonUI.SetActive(false);
        MainMenuUI.SetActive(false);
    }
    void ShowGameWonUI()
    {
        Debug.Log("Show Game Won UI");
        // Implement UI logic to show game won screen
        gameUI.SetActive(false);
        gameOverUI.SetActive(false);
        gameWonUI.SetActive(true);
        MainMenuUI.SetActive(false);
    }
}
