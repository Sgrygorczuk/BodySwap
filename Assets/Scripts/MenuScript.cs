using Base;
using UnityEngine;

/// <summary>
/// This Script is used in the Menu Scene, it inherits most of it's functionality from Cut Scene Script.
/// It's only addition is to look for Data and reset it. 
/// </summary>

public class MenuScript : CutSceneScript
{
    //==================================================================================================================
    // Base Functions 
    //================================================================================================================== 
    
    /// <summary>
    /// Connects the data and resets it for when player has beaten the game 
    /// </summary>
    protected override void Start()
    {
        base.Start();

        //Gets the Start Unit Data and copies it into the Data for a reset 
        var player = GameObject.Find("Controls").transform.Find("Start_Unit").GetComponent<Unit>();
        var data = GameObject.Find("Data").transform.GetComponent<Data>();
        data.ResetData(player);
    }
    
}
