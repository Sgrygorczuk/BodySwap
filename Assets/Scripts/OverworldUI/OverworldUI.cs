using Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OverWorldUI
{
    /// <summary>
    /// This Script is used in the over world levels, it connects all of the general UI elements that the player will see
    /// including the player icon,name, health, mana, goal, items and money.  
    /// </summary>
    
    public class OverWorldUI : MonoBehaviour
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
        private void Start()
        {
            _data = GameObject.Find("Data").GetComponent<Data>();
        
            //Data
            playerUnit = GameObject.Find("OverWorld_Canvas").transform.Find("Player_Stats").GetComponent<Unit>();
            playerUnit = _data.player;
        
            //Player
            playerNameText = GameObject.Find("OverWorld_Canvas").transform.Find("Player_Stats").transform.Find("Player_Name").GetComponent<TextMeshProUGUI>();
            playerHealthText = GameObject.Find("OverWorld_Canvas").transform.Find("Player_Stats").transform.Find("HP_Bar_BG").transform.Find("HP_Text").GetComponent<TextMeshProUGUI>();
            playerManaText = GameObject.Find("OverWorld_Canvas").transform.Find("Player_Stats").transform.Find("MP_Bar_BG").transform.Find("MP_Text").GetComponent<TextMeshProUGUI>();
            lifeGoalText = GameObject.Find("OverWorld_Canvas").transform.Find("Life_Goal").transform.Find("Life_Goal_Text").GetComponent<TextMeshProUGUI>();
            playerHealthImage = GameObject.Find("OverWorld_Canvas").transform.Find("Player_Stats").transform.Find("HP_Bar_BG").transform.Find("HP_Bar").GetComponent<Image>();
            playerManaImage = GameObject.Find("OverWorld_Canvas").transform.Find("Player_Stats").transform.Find("MP_Bar_BG").transform.Find("MP_Bar").GetComponent<Image>();
            playerIcon = GameObject.Find("OverWorld_Canvas").transform.Find("Player_Stats").transform.Find("Player_Icon_Mask")
                .transform.Find("Player_Icon").GetComponent<Image>();
        
            //Items 
            moneyText = GameObject.Find("OverWorld_Canvas").transform.Find("Items").transform.Find("Money_Text").GetComponent<TextMeshProUGUI>();
            healthPotionText = GameObject.Find("OverWorld_Canvas").transform.Find("Items").transform.Find("Health_Text").GetComponent<TextMeshProUGUI>();
            manaPotionText = GameObject.Find("OverWorld_Canvas").transform.Find("Items").transform.Find("Mana_Text").GetComponent<TextMeshProUGUI>();
            smokeBombText = GameObject.Find("OverWorld_Canvas").transform.Find("Items").transform.Find("Bomb_Text").GetComponent<TextMeshProUGUI>();
        
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
    
        /// <summary>
        /// Updates the current counter of player items and money  
        /// </summary>
        /// <param name="money"></param>
        /// <param name="hp"></param>
        /// <param name="mana"></param>
        /// <param name="bomb"></param>
        public void UpdateItemUI(string money, string hp, string mana, string bomb)
        {
            moneyText.text = money;
            healthPotionText.text = hp;
            manaPotionText.text = mana;
            smokeBombText.text = bomb;
        }
    }
}
