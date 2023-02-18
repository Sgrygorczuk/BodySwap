using System.Collections.Generic;
using UnityEngine;

namespace Base
{
    public class Unit : MonoBehaviour
    {
        public string unitName = "";
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
        public int enemyId = 0;
        [HideInInspector] public enum LifeGoals
        {
            ExploreUnknown = 0,     //Boy
            BeatMonsters = 1,       //Human Viking 
            BeatHumans = 2,         //Goblin Boss
            CollectGold = 3,        //Goblin 
            MountAlena = 4,         //Human Deerman
            Chatter = 5,            //Toad 
            Escape = 6,             //Human Shooter 
            Spender = 7,            //Human Bringan 
            LakeVando = 8,          //Toad Mage
            TrashPicker = 9,        //Human Graverobber 
            Sage = 10,               //Goblin Mage
            B = 11,                  //Human Axman
            C = 12,                  //Toad Big
        }

        public LifeGoals lifeGoal;

        public void Copy(Unit unit)
        {
            unitName = unit.unitName;
            sprite = unit.sprite;
            maxHealth = unit.maxHealth;
            currentHealth = unit.currentHealth;
            maxMana = unit.maxMana;
            currentMana = unit.currentMana;
            attacks = unit.attacks;
            isHuman = unit.isHuman;
            lifeGoal = unit.lifeGoal;
            tag = unit.tag;
        }
        
        public void SetName(string name)
        {
            this.unitName = name;
        }
    
    }
}
