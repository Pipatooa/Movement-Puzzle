﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace LevelObjects
{
    public class Player : BaseLevelObject
    {
        public int facingDir;
        public int lastMoveDir;

        public int colorIndex;
        public int[] colorIndexes = new int[4];

        public bool reachedGoal;

        float arrowScale = 0.25f;
        float needleScale = 0.75f;
        float needleSpinSpeed = 3f;

        GameObject needle;

        // Set properties of object
        public Player() : base()
        {
            objectID = 0;

            Events.OnLevelUpdate += OnLevelUpdate;
        }

        // Writes additional data about the object given a binary writer
        public override void WriteData(ref BinaryWriter writer)
        {
            base.WriteData(ref writer);

            writer.Write((byte)facingDir);
            writer.Write((byte)lastMoveDir);

            writer.Write((sbyte)colorIndex);
            writer.Write((sbyte)colorIndexes[0]);
            writer.Write((sbyte)colorIndexes[1]);
            writer.Write((sbyte)colorIndexes[2]);
            writer.Write((sbyte)colorIndexes[3]);
        }

        // Reads additional data about the object given a binary reader
        public override void ReadData(ref BinaryReader reader)
        {
            base.ReadData(ref reader);

            facingDir = reader.ReadByte();
            lastMoveDir = reader.ReadByte();

            colorIndex = reader.ReadSByte();
            colorIndexes[0] = reader.ReadSByte();
            colorIndexes[1] = reader.ReadSByte();
            colorIndexes[2] = reader.ReadSByte();
            colorIndexes[3] = reader.ReadSByte();
        }

        // Creates all game objects for level object under parent transform
        public override void CreateGameObjects(Transform parentTransform)
        {
            base.CreateGameObjects(parentTransform);
            
            // Add this script to player manager and list of level objects
            LevelInfo.playerManager.players.Add(this);

         
            // Create player cube
            gameObject = GameObject.Instantiate(LevelInfo.levelAssets.player, new Vector3(posX, 0.5f, posY), Quaternion.identity, parentTransform);
            gameObject.transform.rotation = Quaternion.Euler(0, facingDir * 90, 0);
            gameObject.GetComponent<Renderer>().material = LevelInfo.colorScheme.colors[colorIndex].material;
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;

            // Iterate through directions to create arrows
            for (int dir = 0; dir < 4; dir++)
            {
                // Do not create arrow if no color for direction
                if (colorIndexes[dir] == -1) continue;

                // Otherwise create arrow with given color
                Quaternion rotation = Quaternion.Euler(0, dir * 90, 0);
                GameObject arrow = GameObject.Instantiate(LevelInfo.levelAssets.playerArrow, gameObject.transform.position + rotation * new Vector3(0, 0.25f, 1), Quaternion.identity, gameObject.transform) as GameObject;
                arrow.transform.localScale *= arrowScale;
                arrow.transform.rotation = rotation;
                arrow.GetComponent<Renderer>().material.color = LevelInfo.colorScheme.colors[colorIndexes[dir]].material.color;
            }

            // Create needle
            needle = GameObject.Instantiate(LevelInfo.levelAssets.playerNeedle, gameObject.transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity, gameObject.transform) as GameObject;
            needle.transform.localScale *= needleScale;
        }

        // Makes the player fall out of the level
        public override void Kill()
        {
            base.Kill();

            LevelInfo.playerManager.resetLocked = true;
            LevelInfo.playerManager.resetLockTime = Time.time;
        }

        // Called every frame by player manager
        public void Update()
        {
            // Spin needle towards last move direction
            needle.transform.localRotation = Quaternion.Lerp(needle.transform.localRotation, Quaternion.Euler(0, lastMoveDir * 90, 0), needleSpinSpeed * Time.deltaTime);
        }

        // Move the player in dir, taking direction player is facing into account
        public void Move(int dir)
        {
            int absDir = (dir + facingDir) % 4;
            if (Shift(absDir))
            {
                lastMoveDir = dir;

                Events.PlayerMoved();
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

            public bool oldReachedGoalStatus;

            public PlayerChange(Player player)
            {
                this.player = player;

                oldPosX = player.posX;
                oldPosY = player.posY;

                oldFacingDir = player.facingDir;
                oldLastMoveDir = player.lastMoveDir;

                oldReachedGoalStatus = player.reachedGoal;
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

            reachedGoal = playerChange.oldReachedGoalStatus;

            gameObject.transform.position = new Vector3(posX, 0.5f, posY);
            gameObject.transform.rotation = Quaternion.Euler(0, facingDir * 90, 0);

            gameObject.SetActive(!reachedGoal);

            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }

        // Saves an old state of the player as a change
        public void SavePlayerState()
        {
            PlayerChange playerChange = new PlayerChange(this);
            UndoSystem.AddChange(playerChange);
        }
    }
}