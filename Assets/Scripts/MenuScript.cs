using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private int _index;
    private SpriteRenderer _blackSquare;
    private TextMeshPro _quote;
    private TextMeshPro _credits;
    public SpriteRenderer[] _points = new SpriteRenderer[2];

    public bool canMove = true;
    
    // Start is called before the first frame update
    void Start()
    {
        _blackSquare = GameObject.Find("Black").GetComponent<SpriteRenderer>();
        _blackSquare.enabled = false;

        _quote = GameObject.Find("Quote").GetComponent<TextMeshPro>();
        _quote.enabled = false;
        
        _credits = GameObject.Find("Credits").GetComponent<TextMeshPro>();
        _credits.enabled = false;

        _points[0].enabled = true;
        _points[1].enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                _index++;
                if (_index < _points.Length) return;
                _index = 0;
                UpdatePoints();
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                _index--;
                if (_index < 0)
                {
                    _index = _points.Length - 1;
                    UpdatePoints();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space) && _index == 0)
            {
                StartCoroutine(Level());
                canMove = false;
            }
            else if (Input.GetKeyDown(KeyCode.Space) && _index == 1)
            {
                StartCoroutine(Credits());
                canMove = false;
            }   
        }
    }

    private IEnumerator Level()
    {
        _quote.enabled = true;
        _blackSquare.enabled = true;
        yield return new WaitForSeconds(5f);
        _quote.enabled = false;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("StartScene");
    }
    
    private IEnumerator Credits()
    {
        _credits.enabled = true;
        _blackSquare.enabled = true;
        yield return new WaitForSeconds(5f);
        _credits.enabled = false;
        _blackSquare.enabled = false;
        canMove = true;
    }

    void UpdatePoints()
    {
        foreach (var point in _points) { point.enabled = false; }
        _points[_index].enabled = true;
    }
}
