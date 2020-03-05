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

    void OnDestroy()
    {
        Events.OnLevelUpdate -= OnLevelUpdate;
    }

    void Start()
    {
        // Function to create arrows around the player
        GameObject CreateArrow(Quaternion rotation, int colorIndex)
        {
            GameObject arrow;

            arrow = Instantiate(arrowPrefab, gameObject.transform.position + rotation * new Vector3(0, 0.25f, 1), Quaternion.identity, gameObject.transform) as GameObject;
            arrow.transform.localScale *= arrowScale;
            arrow.transform.rotation = rotation;
            arrow.GetComponent<Renderer>().material.color = LevelInfo.colorScheme.colors[colorIndex].material.color;

            return arrow;
        }
        
        // Create arrows
        if (colorIndexUp != -1) arrowUp = CreateArrow(Quaternion.identity, colorIndexUp);
        if (colorIndexRight != -1) arrowUp = CreateArrow(Quaternion.Euler(0, 90, 0), colorIndexRight);
        if (colorIndexDown != -1) arrowUp = CreateArrow(Quaternion.Euler(0, 180, 0), colorIndexDown);
        if (colorIndexLeft != -1) arrowUp = CreateArrow(Quaternion.Euler(0, 270, 0), colorIndexLeft);

        gameObject.transform.rotation = Quaternion.Euler(0, facingDir * 90, 0);
        gameObject.GetComponent<Renderer>().material = LevelInfo.colorScheme.colors[colorIndex].material;

        needle = Instantiate(needlePrefab, gameObject.transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity, gameObject.transform) as GameObject;
        needle.transform.localScale *= needleScale;
    }

    void Update()
    {
        // Needle spin
        needle.transform.localRotation = Quaternion.Lerp(needle.transform.localRotation, Quaternion.Euler(0, lastMoveDir * 90, 0), needleSpinSpeed * Time.deltaTime);
    }

    // Loads a previous player state
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

    // Move the player in dir, taking direction player is facing into account
    public void Move(int dir)
    {
        int absDir = (dir + facingDir) % 4;
        if (Shift(absDir))
        {
            lastMoveDir = dir;
            
            ColorManager.CalculateColors();
        }

        SavePlayerState();
    }

    // Nudge the player in dir
    // Returns true if successful, otherwise, false
    public bool Shift(int absDir)
    {
        // Calculate the position the player will end up in
        Vector3 vector = Utils.DirectionToVector(absDir);

        int newPosX = Mathf.RoundToInt(posX + vector.x);
        int newPosY = Mathf.RoundToInt(posY + vector.z);

        // Check whether new position is within level bounds
        if (newPosX < 0 || newPosX >= LevelInfo.levelData.tileArray.GetLength(0) || newPosY < 0 || newPosY >= LevelInfo.levelData.tileArray.GetLength(1))
        {
            return false;
        }

        // Check whether a player to be nudged is present in new position
        Player nudgedPlayer = null;
        foreach (Player player in LevelInfo.playerManager.players)
        {
            if (player.posX == newPosX && player.posY == newPosY && !player.reachedGoal)
            {
                nudgedPlayer = player;
                break;
            }
        }

        // If player to nudge, and player did not move when nudged, don't move player
        if (nudgedPlayer != null) if (!nudgedPlayer.Shift(absDir)) return false;

        SavePlayerState();

        // Otherwise, update player's position
        posX = newPosX;
        posY = newPosY;

        gameObject.transform.position += vector;

        var thisPlayer = this;
        LevelInfo.levelData.tileArray[posX, posY].ProcessPlayer(ref thisPlayer);

        return true;
    }

    void OnLevelUpdate()
    {
        // Check if player has died
        if (LevelInfo.levelData.tileArray[posX, posY].objectID == 0 || !LevelInfo.levelData.tileArray[posX, posY].traversable)
        {
            LevelInfo.playerManager.resetLocked = true;
            LevelInfo.playerManager.resetLockTime = Time.time;

            rb.useGravity = true;
        }
    }

    // Information about a change that has occured to the player
    class PlayerChange : UndoSystem.Change
    {
        // Info about change
        Player player;

        public int oldPosX;
        public int oldPosY;

        public int oldFacingDir;
        public int oldLastMoveDir;

        public PlayerChange(Player player, int oldPosX, int oldPosY, int oldFacingDir, int oldLastMoveDir)
        {
            this.player = player;

            this.oldPosX = oldPosX;
            this.oldPosY = oldPosY;

            this.oldFacingDir = oldFacingDir;
            this.oldLastMoveDir = oldLastMoveDir;
        }

        public override void UndoChange()
        {
            player.UndoPlayerChange(this);
        }
    }

    // Undo a change that has occured to the player
    void UndoPlayerChange(PlayerChange playerChange)
    {
        posX = playerChange.oldPosX;
        posY = playerChange.oldPosY;
        
        facingDir = playerChange.oldFacingDir;
        lastMoveDir = playerChange.oldLastMoveDir;

        gameObject.transform.position = new Vector3(posX, 0.5f, posY);
        gameObject.transform.rotation = Quaternion.Euler(0, facingDir * 90, 0);
    }

    // Saves an old state of the player as a change
    void SavePlayerState()
    {
        PlayerChange playerChange = new PlayerChange(this, posX, posY, facingDir, lastMoveDir);
        UndoSystem.AddChange(playerChange);
    }
}
