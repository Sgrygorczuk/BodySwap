using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OWUnit : MonoBehaviour
{
    public Unit unit;
    private SpriteRenderer _spriteRenderer;
    public Sprite[] sprites = new Sprite[2];
    public bool isPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        if (isPlayer)
        {
            unit = GameObject.Find("Data").GetComponent<Data>().GetUnit();
        }

        tag = unit.isHuman ? "Human" : "Monster";
        _spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = unit.isHuman ? sprites[0] : sprites[1];
    }
}
