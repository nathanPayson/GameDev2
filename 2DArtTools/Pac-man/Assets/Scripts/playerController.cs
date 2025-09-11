
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    public float moveSpeed = 5f;   // Units per second
    public GridManager gridLoader; // Reference to the GridManager

    public Vector2Int gridPos;    // Current position in grid coordinates
    private Vector2Int targetPos;  // The tile we’re moving toward
    private bool isMoving = false;

    [SerializeField] InputAction inputActions;
    [SerializeField] GameObject baseSprite;
    [SerializeField] GameObject powerSprite;
    public static playerController player;
    private Vector2 moveInput;     // From InputSystem
    bool gameOn = false;

    public static Action poweredDown;

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.performed += OnMove;
        inputActions.canceled += OnMove;
        GameManager.NewGame += onNewGame;
        GameManager.GameWon += () => gameOn = false;
        GameManager.GameOver += () => gameOn = false;
        PowerPoint.powerActivated += OnPowerUp;
        ghost.playerHit += () => onNewGame();

    }

    void OnDisable()
    {
        inputActions.performed -= OnMove;
        inputActions.canceled -= OnMove;
        inputActions.Disable();
        GameManager.NewGame -= onNewGame;
        GameManager.GameWon -= () => gameOn = false;
        GameManager.GameOver -= () => gameOn = false;
        ghost.playerHit -= () => onNewGame();
    }
    void Start()
    {
        player = this;
        gridLoader = GridManager.sharedInstance;
        gridPos = new Vector2Int(2, 2);
        targetPos = gridPos;

        // Place the character in world space
        transform.position = GridToWorld(gridPos);
    }

    void Update()
    {
        if (!isMoving && gameOn)
        {
            TryMove();
        }
        else if (gameOn)
        {
            MoveTowardsTarget();
        }
    }
    void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    void TryMove()
    {
        Vector2Int dir = Vector2Int.zero;

        // Snap analog/keyboard input to grid directions
        if (moveInput.y > 0.5f)
        {
            dir = Vector2Int.up;
            gameObject.transform.eulerAngles = new Vector3(90, -90, 0);
        }
        else if (moveInput.y < -0.5f)
        {
            dir = Vector2Int.down;
            gameObject.transform.eulerAngles = new Vector3(90, 90, 0);
        }
        else if (moveInput.x < -0.5f)
        {
            dir = Vector2Int.left;
            gameObject.transform.eulerAngles = new Vector3(90, 180, 0);
        }
        else if (moveInput.x > 0.5f)
        {
            dir = Vector2Int.right;
            gameObject.transform.eulerAngles = new Vector3(90, 0, 0);
        }

        if (dir != Vector2Int.zero)
        {
            Vector2Int nextPos = gridPos + dir;

            if (IsWalkable(nextPos))
            {
                targetPos = nextPos;
                isMoving = true;
            }
        }
    }
    public void MoveTowardsTarget()
    {
        Vector3 targetWorld = GridToWorld(targetPos);

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetWorld,
            moveSpeed * Time.deltaTime
        );

        // Arrived at target?
        if (Vector3.Distance(transform.position, targetWorld) < 0.01f)
        {
            transform.position = targetWorld;
            gridPos = targetPos;
            isMoving = false;
        }
    }

    bool IsWalkable(Vector2Int pos)
    {
        // Out of bounds check
        if (pos.x < 0 || pos.x >= gridLoader.Grid.GetLength(0) ||
            pos.y < 0 || pos.y >= gridLoader.Grid.GetLength(1))
            return false;

        TileType tile = gridLoader.Grid[pos.x,pos.y];
        return tile == TileType.Path || tile == TileType.Power;  // Path is walkable
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x, 0, gridPos.y);
    }

    private void onNewGame()
    {
        gameOn = true;
        gridPos = new Vector2Int(2, 2);
        targetPos = gridPos;
        OnPowerDown();

        // Place the character in world space
        transform.position = GridToWorld(gridPos);
    }

    private void OnPowerUp()
    {
        powerSprite.SetActive(true);
        baseSprite.SetActive(false);
        StartCoroutine(PowerDownAfterDelay(5f));

    }

    private void OnPowerDown()
    {
        poweredDown?.Invoke();
        baseSprite.SetActive(true);
        powerSprite.SetActive(false);
    }

    IEnumerator PowerDownAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnPowerDown();
    }
}
