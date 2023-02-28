using UnityEngine;

/// <summary>
/// This Script is used in the Menu Scene, it inherits most of it's functionality from Cut Scene Script.
/// It's only addition is to look for Data and reset it. 
/// </summary>

public class MenuScript : CutSceneScript
{
    //The data game object 
    private Data _data;

    //==================================================================================================================
    // Base Functions 
    //================================================================================================================== 
    
    /// <summary>
    /// Connects the data and resets it for when player has beaten the game 
    /// </summary>
    protected override void Start()
    {
        base.Start();
        
        _data = GameObject.Find("Data").transform.GetComponent<Data>();
        _data.Reset();
    }
    
}
