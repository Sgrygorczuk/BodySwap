using Base;
using UnityEngine;
using UnityEngine.UI;

namespace BattleScripts
{
    /// <summary>
    /// This Script is used inside of Battle Script, this is used to connect all of the UI elements in the enemy
    /// to the Data passed in and well as to keep track of the enemy stats throughout the battle 
    /// </summary>
    public class EnemyComponents : MonoBehaviour
    {
        //========= Enemy Components 
        [HideInInspector] public Unit enemyUnit;
        [HideInInspector] public Image enemyHealthImage;
        [HideInInspector] public Image enemyImage;
        [HideInInspector] public GameObject enemyDamageNumberSpawnPoint;
    
        //======== Enemy Controls 
        [HideInInspector] public int maxEnemyAttackIndex = 0;  //How many attacks the player has access to 
        [HideInInspector] public int enemyCurrentHealth;

        public void SetUpEnemy(UnitsAndAttacks unit, int enemyId)
        {

            enemyUnit = GameObject.Find("Canvas").transform.Find("Enemy").GetComponent<Unit>();
            enemyUnit = unit.unitsStats[enemyId];
            enemyUnit.SetName(unit.SetRandomName());
        
            enemyHealthImage = GameObject.Find("Canvas").transform.Find("Enemy").transform.Find("Enemy_Bar_BG").transform.Find("Enemy_Bar").GetComponent<Image>();
            enemyImage = GameObject.Find("Canvas").transform.Find("Enemy").transform.Find("Enemy_Sprite")
                .GetComponent<Image>();
            enemyImage.sprite = enemyUnit.sprite;
            enemyDamageNumberSpawnPoint = GameObject.Find("Canvas").transform.Find("Enemy").transform.Find("Point").gameObject;
        
            maxEnemyAttackIndex = enemyUnit.attacks.Count;
            enemyCurrentHealth = enemyUnit.currentHealth;
            enemyHealthImage.fillAmount = (float) enemyCurrentHealth / enemyUnit.maxHealth;
        }

    }
}
