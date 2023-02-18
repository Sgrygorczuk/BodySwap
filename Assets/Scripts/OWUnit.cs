using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using UnityEngine;

public class OWUnit : MonoBehaviour
{
    public Unit unit;
    private SpriteRenderer _spriteRenderer;
    public Sprite[] sprites = new Sprite[2];
    public bool isPlayer;
    public BoxCollider2D[] _boxCollider2Ds;
    public string talkingText;
    private SpriteRenderer _spriteRendererIcon;
    public Sprite[] chatIcons;
    public bool isShopkeeper;
    
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
        if (isPlayer) return;
        _spriteRendererIcon = transform.Find("Icon").GetComponent<SpriteRenderer>();
        _spriteRendererIcon.sprite = isShopkeeper ? chatIcons[0] : chatIcons[1];
        _spriteRendererIcon.enabled = false;
    }

    public void SetIcon(bool visible )
    {
        _spriteRendererIcon.enabled = visible;
    }
    
    public IEnumerator Incapacitate()
    {
        print("A");
        _spriteRenderer.enabled = false;
        _boxCollider2Ds[0].enabled = false;
        _boxCollider2Ds[1].enabled = false;
        //Wait for 2 min.
        yield return new WaitForSeconds(120);
        _spriteRenderer.enabled = true;
        _boxCollider2Ds[0].enabled = true;
        _boxCollider2Ds[1].enabled = true;
    }
}
