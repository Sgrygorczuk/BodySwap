using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Base;
using UnityEngine;
using TMPro;

/// <summary>
/// This Script is used in the last cutscene to display the story of the player 
/// </summary>

public class StoryText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private readonly List<string> _stack = new();
    private List<string> _list = new();
    private const float Timer = 2.5f;

    /// <summary>
    /// Connects the elements 
    /// </summary>
    private void Start()
    {
        _text = GameObject.Find("Canvas").transform.Find("Story").GetComponent<TextMeshProUGUI>();
        var data = GameObject.Find("Data").GetComponent<Data>();
        _list = data.GetStory();
        _stack.Add(_list[0]);
        _stack.Add(_list[1]);
        UpdateText();
        StartCoroutine(UpdateStack());
    }
    
    /// <summary>
    /// For the _list.Count add new lines to the text to show the players full story 
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateStack()
    {
        yield return new WaitForSeconds(6f);
        for (var i = 2; i < _list.Count; i++)
        {
            _stack.Add(_list[i]);
            CheckIfStackOverflow();
            UpdateText();
            yield return new WaitForSeconds(Timer);
        }
    }

    /// <summary>
    /// Checks if the list is 5 or less, if it's larger than 5 remove the first element 
    /// </summary>
    private void CheckIfStackOverflow()
    {
        if (_stack.Count > 5)
        {
            _stack.RemoveAt(0);
        }
    }

    /// <summary>
    /// Collects all the strings from the list into one string that gets added to the UI
    /// </summary>
    private void UpdateText()
    {
        var textString = _stack.Aggregate("", (current, t) => current + (t + "\n"));
        _text.text = textString;
    }
}
