using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverworldUI : MonoBehaviour
{
    private Data _data;
    
    //========= Player Components 
    [HideInInspector] public Unit playerUnit;
    [HideInInspector] public TextMeshProUGUI playerHealthText;
    [HideInInspector] public TextMeshProUGUI playerManaText;
    [HideInInspector] public TextMeshProUGUI playerNameText;
    [HideInInspector] public TextMeshProUGUI lifeGoalText;
    [HideInInspector] public Image playerHealthImage;
    [HideInInspector] public Image playerManaImage;
    [HideInInspector] public Image playerIcon;
    
    
    //========= Items & Money Components
    [HideInInspector] public TextMeshProUGUI moneyText;
    [HideInInspector] public TextMeshProUGUI healthPotionText;
    [HideInInspector] public TextMeshProUGUI manaPotionText;
    [HideInInspector] public TextMeshProUGUI smokeBombText;
    

    // Start is called before the first frame update
    void Start()
    {
        _data = GameObject.Find("Data").GetComponent<Data>();
        
        //Data
        playerUnit = GameObject.Find("Overworld_Canvas").transform.Find("Player_Stats").GetComponent<Unit>();
        playerUnit = _data.player;
        
        //Player
        playerNameText = GameObject.Find("Overworld_Canvas").transform.Find("Player_Stats").transform.Find("Player_Name").GetComponent<TextMeshProUGUI>();
        playerHealthText = GameObject.Find("Overworld_Canvas").transform.Find("Player_Stats").transform.Find("HP_Bar_BG").transform.Find("HP_Text").GetComponent<TextMeshProUGUI>();
        playerManaText = GameObject.Find("Overworld_Canvas").transform.Find("Player_Stats").transform.Find("MP_Bar_BG").transform.Find("MP_Text").GetComponent<TextMeshProUGUI>();
        lifeGoalText = GameObject.Find("Overworld_Canvas").transform.Find("Life_Goal").transform.Find("Life_Goal_Text").GetComponent<TextMeshProUGUI>();
        playerHealthImage = GameObject.Find("Overworld_Canvas").transform.Find("Player_Stats").transform.Find("HP_Bar_BG").transform.Find("HP_Bar").GetComponent<Image>();
        playerManaImage = GameObject.Find("Overworld_Canvas").transform.Find("Player_Stats").transform.Find("MP_Bar_BG").transform.Find("MP_Bar").GetComponent<Image>();
        playerIcon = GameObject.Find("Overworld_Canvas").transform.Find("Player_Stats").transform.Find("Player_Icon_Mask")
            .transform.Find("Player_Icon").GetComponent<Image>();
        
        //Items 
        moneyText = GameObject.Find("Overworld_Canvas").transform.Find("Items").transform.Find("Money_Text").GetComponent<TextMeshProUGUI>();
        healthPotionText = GameObject.Find("Overworld_Canvas").transform.Find("Items").transform.Find("Health_Text").GetComponent<TextMeshProUGUI>();
        manaPotionText = GameObject.Find("Overworld_Canvas").transform.Find("Items").transform.Find("Mana_Text").GetComponent<TextMeshProUGUI>();
        smokeBombText = GameObject.Find("Overworld_Canvas").transform.Find("Items").transform.Find("Bomb_Text").GetComponent<TextMeshProUGUI>();
        
        //Pulls and passes on the information to the components 
        
        //Player 
        playerNameText.text = playerUnit.unitName;
        playerIcon.sprite = playerUnit.sprite;
        lifeGoalText.text = _data.GetGoalText();
        if (_data.GetIsGoalCompleted())
        {
            lifeGoalText.fontStyle = FontStyles.Strikethrough;
        }
        playerHealthText.text = playerUnit.currentHealth + "/" + playerUnit.maxHealth;
        playerManaText.text = playerUnit.currentMana + "/" + playerUnit.maxMana;
        playerHealthImage.fillAmount = (float) playerUnit.currentHealth / playerUnit.maxHealth;
        playerManaImage.fillAmount =  (float) playerUnit.currentMana / playerUnit.maxMana;
        
        //Items 
        moneyText.text = "x" + _data.GetMoney();
        healthPotionText.text = "x" + _data.GetItem(0);
        manaPotionText.text = "x" + _data.GetItem(1);
        smokeBombText.text = "x" + _data.GetItem(2);


    }
    public void UpdateItemUI(string money, string hp, string mana, string bomb)
    {
        moneyText.text = money;
        healthPotionText.text = hp;
        manaPotionText.text = mana;
        smokeBombText.text = bomb;
    }
}
