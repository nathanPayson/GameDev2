using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ghost : MonoBehaviour
{
    public GameObject baseSprite;
    public GameObject powerSprite;
    bool powerActive = false;
    public static Action playerHit;
    public NavMeshAgent agent;
    bool gameOn = false;
    int baseDelay = 2;
    [SerializeField] int startingDelay;
    public static Action ghostHit;
    bool allowedToMove = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && powerActive)
        {
            ghostHit?.Invoke();
            respawn();
        }
        if (other.CompareTag("Player") && !powerActive)
        {
            playerHit?.Invoke();
        }
    }

    private void Start()
    {

    }
    void respawn()
    {
        allowedToMove = false;
        transform.position = new Vector3(14.5f, 0, 12.5f);
        transform.rotation = Quaternion.Euler(90, 0, 0);
        StartCoroutine(respawnDelay(baseDelay));
    }
    void newGameRespawn()
    {
        allowedToMove = false;
        transform.position = new Vector3(14.5f, 0, 12.5f);
        transform.rotation = Quaternion.Euler(90, 0, 0);
        StartCoroutine(respawnDelay(startingDelay));
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
        if (gameOn && !powerActive && allowedToMove)
        {
            agent.enabled = true;
            MoveTowardsTarget();
        }
        else
        {
            agent.enabled = false;
        }
    }

    private void OnEnable()
    {
        PowerPoint.powerActivated += onPowerUp;
        playerController.poweredDown += onPowerDown;
        GameManager.NewGame += onNewGame;
        GameManager.GameOver += onGameOver;
        GameManager.GameWon += onGameOver;
        ghost.playerHit += () => respawn();

    }
    private void OnDisable()
    {
        PowerPoint.powerActivated -= onPowerUp;
        playerController.poweredDown -= onPowerDown;
        GameManager.NewGame -= onNewGame;
        GameManager.GameOver -= onGameOver;
        GameManager.GameWon -= onGameOver;
        ghost.playerHit -= () => respawn();
    }

    void MoveTowardsTarget()
    {
        agent.SetDestination(playerController.player.GridToWorld(playerController.player.gridPos));
    }

    void onNewGame()
    {
        
        gameOn = true;
        newGameRespawn(); 
    }

    IEnumerator respawnDelay(int delay)
    {
        yield return new WaitForSeconds(delay);
        agent.enabled = true;
        allowedToMove = true;
    }

    void onGameOver()
    {
        respawn();
        gameOn = false;
    }
    void onPowerUp()
    {
        powerActive = true;
        agent.enabled = false;
        powerSprite.SetActive(true);
        baseSprite.SetActive(false);
    }
    void onPowerDown()
    {
        powerActive = false;
        agent.enabled = true;
        powerSprite.SetActive(false);
        baseSprite.SetActive(true);
    }
}
