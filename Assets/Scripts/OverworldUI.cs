using System.Collections;
using System.Collections.Generic;
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
    [HideInInspector] public Image playerHealthImage;
    [HideInInspector] public Image playerManaImage;
    [HideInInspector] public Image playerIcon;

    // Start is called before the first frame update
    void Start()
    {
        _data = GameObject.Find("Data").GetComponent<Data>();
        
        //Grabs the components  
        playerUnit = GameObject.Find("Overworld_Canvas").transform.Find("Player_Stats").GetComponent<Unit>();
        playerUnit.Copy(_data.player);
        
        //Updated once
        playerNameText = GameObject.Find("Overworld_Canvas").transform.Find("Player_Stats").transform.Find("Player_Name").GetComponent<TextMeshProUGUI>();
        
        //Updated through the battle 
        playerHealthText = GameObject.Find("Overworld_Canvas").transform.Find("Player_Stats").transform.Find("HP_Bar_BG").transform.Find("HP_Text").GetComponent<TextMeshProUGUI>();
        playerManaText = GameObject.Find("Overworld_Canvas").transform.Find("Player_Stats").transform.Find("MP_Bar_BG").transform.Find("MP_Text").GetComponent<TextMeshProUGUI>();
        playerHealthImage = GameObject.Find("Overworld_Canvas").transform.Find("Player_Stats").transform.Find("HP_Bar_BG").transform.Find("HP_Bar").GetComponent<Image>();
        playerManaImage = GameObject.Find("Overworld_Canvas").transform.Find("Player_Stats").transform.Find("MP_Bar_BG").transform.Find("MP_Bar").GetComponent<Image>();
        playerIcon = GameObject.Find("Overworld_Canvas").transform.Find("Player_Stats").transform.Find("Player_Icon_Mask")
            .transform.Find("Player_Icon").GetComponent<Image>();

        //Pulls and passes on the information to the components 
        playerNameText.text = playerUnit.name;
        playerIcon.sprite = playerUnit.sprite;
        playerHealthText.text = playerUnit.currentHealth + "/" + playerUnit.maxHealth;
        playerManaText.text = playerUnit.currentMana + "/" + playerUnit.maxMana;
        playerHealthImage.fillAmount = (float) playerUnit.currentHealth / playerUnit.maxHealth;
        playerManaImage.fillAmount =  (float) playerUnit.currentMana / playerUnit.maxMana;
 
    }
    
}
