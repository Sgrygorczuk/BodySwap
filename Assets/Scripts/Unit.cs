using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string name = "";
    public Sprite sprite;
    [Range(15,45)] public int maxHealth = 15;
    [Range(15,45)] public int currentHealth = 15;
    [Range(15,45)] public int currentMana = 15;
    [Range(15,45)] public int maxMana = 15;
    [Range(0,1)] public float physicalDefense = 1;
    [Range(0,1)] public float magicalDefense= 1;
    [Range(5,40)]public int moneyDrop = 10;
    public List<Attacks> attacks = new List<Attacks>();
    public bool isHuman;

    public void Copy(Unit unit)
    {
        name = unit.name;
        sprite = unit.sprite;
        maxHealth = unit.maxHealth;
        currentHealth = unit.currentHealth;
        maxMana = unit.maxMana;
        currentMana = unit.currentMana;
        physicalDefense = unit.physicalDefense;
        magicalDefense = unit.magicalDefense;
        moneyDrop = unit.moneyDrop;
        attacks.Clear();
        for (var i = 0; i < unit.attacks.Count;  i++) { attacks.Add(unit.attacks[i]); }
        isHuman = unit.isHuman;
    }

    public void SetName(string name)
    {
        this.name = name;
    }
    
}
