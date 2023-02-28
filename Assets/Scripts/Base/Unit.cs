using System.Collections.Generic;
using UnityEngine;

namespace Base
{
    /// <summary>
    /// This Script is used in PreFabs of Units that will be fighting the player and used by the player 
    /// </summary>
    public class Unit : MonoBehaviour
    {
        //==== UI 
        //Units Name and Sprite 
        public string unitName = "";
        public Sprite sprite;
        
        //==== Combat Stats 
        //Max Health and Mana
        [Range(10,45)] public int maxHealth = 15;
        [Range(10,45)] public int maxMana = 15;
        //Current Health and Mana
        [Range(10,45)] public int currentHealth = 15;
        [Range(10,45)] public int currentMana = 15;
        //How much money they drop on death 
        [Range(5,40)]public int moneyDrop = 10;
        //List of attacks that is available to the Unit 
        public List<Attacks> attacks = new();
        
        //==== Identification
        //Tells us if it's a human or monster 
        public bool isHuman;
        //Tells us which Unit data to pull into Battle Scene 
        public int enemyId;

        //======= Goals 
        //List of possible goals 
        public enum LifeGoals
        {
            ExploreUnknown = 0,     //Boy
            BeatMonsters = 1,       //Human Viking 
            BeatHumans = 2,         //Goblin Boss
            CollectGold = 3,        //Goblin 
            Escape = 4,             //Human Shooter 
        }
        //The goal that the Unit works towards 
        public LifeGoals lifeGoal;
        
        //==================================================================================================================
        // Base Functions 
        //==================================================================================================================

        /// <summary>
        /// Used by player to copy the data when switching characters 
        /// </summary>
        /// <param name="unit"></param>
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
        
        /// <summary>
        /// Used to update the players name 
        /// </summary>
        /// <param name="nameCopy"></param>
        public void SetName(string nameCopy)
        {
            unitName = nameCopy;
        }
    
    }
}
