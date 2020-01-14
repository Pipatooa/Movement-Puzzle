using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int colorIndex;
    
    public int colorIndexUp;
    public int colorIndexRight;
    public int colorIndexDown;
    public int colorIndexLeft;

    public int facingDir;

    [HideInInspector] public bool selected;
    [HideInInspector] public int lastMoveDir;

    [HideInInspector] public int posX;
    [HideInInspector] public int posY;

    [HideInInspector] public bool reachedGoal;

    public GameObject arrowPrefab;
    public GameObject needlePrefab;

    public float arrowScale = 0.25f;
    public float needleScale = 0.75f;

    public float needleSpinSpeed = 3f;

    Rigidbody rb;

    GameObject needle;

    GameObject arrowUp;
    GameObject arrowRight;
    GameObject arrowDown;
    GameObject arrowLeft;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        Events.OnLevelUpdate += OnLevelUpdate;
    }

    void Start()
    {
        GameObject CreateArrow(Quaternion rotation, int colorIndex)
        {
            GameObject arrow;

            arrow = Instantiate(arrowPrefab, gameObject.transform.position + rotation * new Vector3(0, 0.25f, 1), Quaternion.identity, gameObject.transform) as GameObject;
            arrow.transform.localScale *= arrowScale;
            arrow.transform.rotation = rotation;
            arrow.GetComponent<Renderer>().material.color = Level.colorScheme.colors[colorIndex].material.color;

            return arrow;
        }
        
        // Create arrows
        if (colorIndexUp != -1) arrowUp = CreateArrow(Quaternion.identity, colorIndexUp);
        if (colorIndexRight != -1) arrowUp = CreateArrow(Quaternion.Euler(0, 90, 0), colorIndexRight);
        if (colorIndexDown != -1) arrowUp = CreateArrow(Quaternion.Euler(0, 180, 0), colorIndexDown);
        if (colorIndexLeft != -1) arrowUp = CreateArrow(Quaternion.Euler(0, 270, 0), colorIndexLeft);

        gameObject.transform.rotation = Quaternion.Euler(0, facingDir * 90, 0);
        gameObject.GetComponent<Renderer>().material = Level.colorScheme.colors[colorIndex].material;

        needle = Instantiate(needlePrefab, gameObject.transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity, gameObject.transform) as GameObject;
        needle.transform.localScale *= needleScale;
    }

    void Update()
    {
        if (selected && !Level.playerManager.resetLocked && !reachedGoal)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && colorIndexUp != -1) Move(0);
            else if (Input.GetKeyDown(KeyCode.RightArrow) && colorIndexRight != -1) Move(1);
            else if (Input.GetKeyDown(KeyCode.DownArrow) && colorIndexDown != -1) Move(2);
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && colorIndexLeft != -1) Move(3);

            if (Input.GetKeyDown(KeyCode.R))
            {
                facingDir += 1;
                facingDir %= 4;

                gameObject.transform.rotation = Quaternion.Euler(0, facingDir * 90, 0);
            }
        }

        // Needle spin
        needle.transform.localRotation = Quaternion.Lerp(needle.transform.localRotation, Quaternion.Euler(0, lastMoveDir * 90, 0), needleSpinSpeed * Time.deltaTime);
    }

    public void LoadState(LevelData.PlayerInfo playerInfo, bool isInitialState)
    {
        posX = playerInfo.posX;
        posY = playerInfo.posY;

        facingDir = playerInfo.facingDir;
        lastMoveDir = playerInfo.lastMoveDir;

        reachedGoal = playerInfo.reachedGoal;

        colorIndex = playerInfo.colorIndex;
        colorIndexUp = playerInfo.colorIndexUp;
        colorIndexRight = playerInfo.colorIndexRight;
        colorIndexDown = playerInfo.colorIndexDown;
        colorIndexLeft = playerInfo.colorIndexLeft;

        if (!isInitialState)
        {
            gameObject.transform.position = new Vector3(posX, 0.5f, posY);
            gameObject.transform.rotation = Quaternion.Euler(0, facingDir * 90, 0);
        }

        gameObject.SetActive(!reachedGoal);

        rb.useGravity = false;
        rb.velocity = Vector3.zero;
    }

    // Move the player in dir
    void Move(int dir)
    {
        Vector3 vector;

        switch (dir)
        {
            case 0:
                vector = new Vector3(0, 0, 1);
                break;
            case 1:
                vector = new Vector3(1, 0, 0);
                break;
            case 2:
                vector = new Vector3(0, 0, -1);
                break;
            case 3:
                vector = new Vector3(-1, 0, 0);
                break;
            default:
                return;
        }

        vector = Quaternion.Euler(0, facingDir * 90, 0) * vector;

        int newPosX = Mathf.RoundToInt(posX + vector.x);
        int newPosY = Mathf.RoundToInt(posY + vector.z);

        if (newPosX < 0 || newPosX >= Level.levelData.tileArray.GetLength(0) || newPosY < 0 || newPosY >= Level.levelData.tileArray.GetLength(1))
        {
            return;
        }

        Player nudgedPlayer = null;
        foreach (Player player in Level.playerManager.players)
        {
            if (player.posX == newPosX && player.posY == newPosY && !player.reachedGoal)
            {
                nudgedPlayer = player;
                break;
            }
        }

        int absDir = (dir + facingDir) % 4;

        if (nudgedPlayer != null) if (!nudgedPlayer.Shift(absDir)) return;

        lastMoveDir = dir;

        posX = newPosX;
        posY = newPosY;

        gameObject.transform.position += vector;

        Level.playerManager.UpdateColorCount();
    }

    bool Shift(int absDir)
    {
        Vector3 vector;

        switch (absDir)
        {
            case 0:
                vector = new Vector3(0, 0, 1);
                break;
            case 1:
                vector = new Vector3(1, 0, 0);
                break;
            case 2:
                vector = new Vector3(0, 0, -1);
                break;
            case 3:
                vector = new Vector3(-1, 0, 0);
                break;
            default:
                return false;
        }

        int newPosX = Mathf.RoundToInt(posX + vector.x);
        int newPosY = Mathf.RoundToInt(posY + vector.z);

        if (newPosX < 0 || newPosX >= Level.levelData.tileArray.GetLength(0) || newPosY < 0 || newPosY >= Level.levelData.tileArray.GetLength(1))
        {
            return false ;
        }

        Player nudgedPlayer = null;
        foreach (Player player in Level.playerManager.players)
        {
            if (player.posX == newPosX && player.posY == newPosY && !player.reachedGoal)
            {
                nudgedPlayer = player;
                break;
            }
        }

        if (nudgedPlayer != null) if (!nudgedPlayer.Shift(absDir)) return false;

        posX = newPosX;
        posY = newPosY;

        gameObject.transform.position += vector;

        return true;
    }

    void OnLevelUpdate()
    {
        // Check if player has died
        if (!Level.levelData.tileArray[posX, posY].exists || !Level.levelData.tileArray[posX, posY].traversable)
        {
            Level.playerManager.resetLocked = true;
            Level.playerManager.resetLockTime = Time.time;

            rb.useGravity = true;
        }

        // Check if reached goal
        if (!reachedGoal && posX == Level.levelData.goalX && posY == Level.levelData.goalY)
        {
            reachedGoal = true;

            gameObject.SetActive(false);

            Events.OnPlayerReachedGoal();
        }
    }
}
