using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OverWorldUI
{
    /// <summary>
    /// This Script is used in the over world levels, it's used to set up the Shop UI and player interactions with it
    /// </summary>
    public class OverWorldShopUI : MonoBehaviour
    {
        //======= Shopkeeper Elements 
        private Image _iconImage;
        private TextMeshProUGUI _talkText;
    
        //========= Items & Money Components
        [HideInInspector] public TextMeshProUGUI moneyText;
        [HideInInspector] public TextMeshProUGUI healthPotionText;
        [HideInInspector] public TextMeshProUGUI manaPotionText;
        [HideInInspector] public TextMeshProUGUI smokeBombText;
    
        // Start is called before the first frame update
        private void Start()
        {
            _iconImage =GameObject.Find("Shop_Canvas").transform.Find("Talks").transform.Find("Talker_Icon_Mask").transform.Find("Icon_Image").GetComponent<Image>();
            _talkText = GameObject.Find("Shop_Canvas").transform.Find("Talks").transform.Find("Talk_Text").GetComponent<TextMeshProUGUI>();
        
            //Items 
            moneyText = GameObject.Find("Shop_Canvas").transform.Find("Items").transform.Find("Money_Text").GetComponent<TextMeshProUGUI>();
            healthPotionText = GameObject.Find("Shop_Canvas").transform.Find("Items").transform.Find("Health_Text").GetComponent<TextMeshProUGUI>();
            manaPotionText = GameObject.Find("Shop_Canvas").transform.Find("Items").transform.Find("Mana_Text").GetComponent<TextMeshProUGUI>();
            smokeBombText = GameObject.Find("Shop_Canvas").transform.Find("Items").transform.Find("Bomb_Text").GetComponent<TextMeshProUGUI>();

        }
        
        /// <summary>
        /// Updates the shop keeper elements  
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="talk"></param>
        public void UpdateTalksUI(Sprite sprite, string talk)
        {
            _iconImage.sprite = sprite;
            _talkText.text = talk;
        }

        /// <summary>
        /// 
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
