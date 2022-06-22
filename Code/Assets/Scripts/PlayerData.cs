using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int cherries;
    public int highscore;

    public PlayerData(World world)
    {
        cherries = world.totalCherries;
        highscore = world.GetScore();
    }
}
