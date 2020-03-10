using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelAssets", menuName = "Level Assets")]
public class LevelAssets : ScriptableObject
{
    public GameObject player;
    public GameObject playerArrow;
    public GameObject playerNeedle;
    
    public GameObject tile;
    public GameObject goal;
}
