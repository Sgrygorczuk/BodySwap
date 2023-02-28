using System.Collections.Generic;
using UnityEngine;

public class MenuControls : MonoBehaviour
{
    private int _index = 0;
    private int _max = 0; 
    private List<SpriteRenderer> _points = new List<SpriteRenderer>();
    
    public MenuControls(int max, GameObject points)
    {
        _max = max; 
        AttachPoints(points);
        UpdatePoints();
    }

    private void AttachPoints(GameObject points)
    {
        for (var i = 0; i < points.transform.childCount; i++)
        {
            _points.Add(points.transform.GetChild(i).GetComponent<SpriteRenderer>());
        }
    }
    
    public void UpdateMenuIndex(int length)
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
                _index++;
                if (_index < length) return;
                _index = 0;
                UpdatePoints();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
                _index--;
                if (_index < 0)
                {
                    _index = length - 1;
                    UpdatePoints();
                }
        }
    }

    private  void UpdatePoints()
    {
        foreach (var point in _points) { point.enabled = false; }
        _points[_index].enabled = true;
    }
    
}
