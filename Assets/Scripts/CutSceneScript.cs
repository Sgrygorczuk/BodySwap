using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is used in all of the cut scenes and is inherited by the Menu Script. It allows dev to tell what
/// the next scene will be loaded and prompts the player to move to the next scene. 
/// </summary>

public class CutSceneScript : MonoBehaviour
{
    //The animator that will fade away the scene
    private Animator _animator;
    //The scene that will be loaded into 
    public string scenePath = "StartScene";

    //==================================================================================================================
    // Base Functions 
    //================================================================================================================== 
  
    /// <summary>
   /// Connects the animator 
   /// </summary>
    protected virtual void Start()
    {
        _animator = GameObject.Find("Transition").transform.GetComponent<Animator>();
    }
    
    /// <summary>
    /// Waits for player to press space to start loading sequence 
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ToLevel());
        }
    }
    
    //==================================================================================================================
    // Transition Functions 
    //================================================================================================================== 
    
    /// <summary>
    /// Plays the transition animations, waits for it to pass then loads next scene. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator ToLevel()
    {
        _animator.Play("Transition");
        yield return new WaitForSeconds(1.75f);
        SceneManager.LoadScene(scenePath);
    }
}
