using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class PlayerData
{
    public SleepingArea sleepingArea;
    public GameMaster gameMaster;
    public PlayableDirector director;
    public InventorySystem inventorySystem;
    public float[] position;

    public PlayerData(Player player)
    { 
        gameMaster = player.gameMaster; 
        director = player.director; 
        inventorySystem = player.inventorySystem;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }

}
