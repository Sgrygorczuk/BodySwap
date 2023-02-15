using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerComponents : MonoBehaviour
{
    //========= Player Components 
    [HideInInspector] public Unit playerUnit;
    [HideInInspector] public TextMeshProUGUI playerHealthText;
    [HideInInspector] public TextMeshProUGUI playerManaText;
    [HideInInspector] public TextMeshProUGUI playerNameText;
    [HideInInspector] public Image playerHealthImage;
    [HideInInspector] public Image playerManaImage;
    [HideInInspector] public Image playerIcon;
    [HideInInspector] public GameObject playerDamageNumberSpawnPoint;
    
    //======== Player Controls 
    [HideInInspector] public int playerCurrentHealth;
    [HideInInspector] public int playerCurrentMana;
    [HideInInspector] public int maxPlayerAttackIndex = 0;  //How many attacks the player has access to 

    //===== Sets Up Player =============================================================================================
    
    public void SetUpPlayer(Unit unit)
    {
        //Grabs the components  
        playerUnit = GameObject.Find("Canvas").transform.Find("Player_Stats").GetComponent<Unit>();
        playerUnit.Copy(unit);
        
        //Updated once
        playerNameText = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("Player_Name").GetComponent<TextMeshProUGUI>();
        SetUpPlayerAttacks();
        
        //Updated through the battle 
        playerHealthText = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("HP_Bar_BG").transform.Find("HP_Text").GetComponent<TextMeshProUGUI>();
        playerManaText = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("MP_Bar_BG").transform.Find("MP_Text").GetComponent<TextMeshProUGUI>();
        playerHealthImage = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("HP_Bar_BG").transform.Find("HP_Bar").GetComponent<Image>();
        playerManaImage = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("MP_Bar_BG").transform.Find("MP_Bar").GetComponent<Image>();
        playerIcon = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("Player_Icon_Mask")
            .transform.Find("Player_Icon").GetComponent<Image>();
        playerDamageNumberSpawnPoint = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("Point").gameObject;

        //Pulls and passes on the information to the components 
        playerNameText.text = playerUnit.name;
        playerIcon.sprite = playerUnit.sprite;
        playerCurrentHealth = playerUnit.health;
        playerHealthText.text = playerCurrentHealth + "/" + playerUnit.health;
        playerCurrentMana = playerUnit.mana;
        playerManaText.text = playerCurrentMana + "/" + playerUnit.mana;
    }

    private void SetUpPlayerAttacks()
    {
        maxPlayerAttackIndex = playerUnit.attacks.Count;
        //Copies the data for all of the attacks the player has
        for (int i = 0; i < maxPlayerAttackIndex; i++)
        {
            var path = "Attack_" + i;
            var attackName = GameObject.Find("Canvas").transform.Find("Attack_Player_Actions").transform.Find(path).transform.Find("Attack_Name").GetComponent<TextMeshProUGUI>();
            var attackType = GameObject.Find("Canvas").transform.Find("Attack_Player_Actions").transform.Find(path).transform.Find("Attack_Type").GetComponent<Image>();
            var attackManaCost = GameObject.Find("Canvas").transform.Find("Attack_Player_Actions").transform.Find(path).transform.Find("Attack_MP_Cost").GetComponent<TextMeshProUGUI>();
            
            attackName.text = playerUnit.attacks[i].name;
            //TODO Have the type select image 
            attackManaCost.text = "MP: " + playerUnit.attacks[i].manaCost;
        }
        
        //If the player has less than 4 attacks it fills out the rest of the list with blanks 
        if (maxPlayerAttackIndex >= 4) return;
        for(int i = maxPlayerAttackIndex; i < 4; i++)
        {
            var path = "Attack_" + i;
            var attackName = GameObject.Find("Canvas").transform.Find("Attack_Player_Actions").transform.Find(path).transform.Find("Attack_Name").GetComponent<TextMeshProUGUI>();
            var attackType = GameObject.Find("Canvas").transform.Find("Attack_Player_Actions").transform.Find(path).transform.Find("Attack_Type").GetComponent<Image>();
            var attackManaCost = GameObject.Find("Canvas").transform.Find("Attack_Player_Actions").transform.Find(path).transform.Find("Attack_MP_Cost").GetComponent<TextMeshProUGUI>();

            attackName.text = "---";
            //TODO Have the type select image 
            attackManaCost.text = "MP: ---";
        }
    }
}
