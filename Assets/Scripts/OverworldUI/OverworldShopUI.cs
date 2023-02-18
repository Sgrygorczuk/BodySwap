using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverworldShopUI : MonoBehaviour
{
    [HideInInspector] private Image iconImage;
    [HideInInspector] private TextMeshProUGUI talkText;
    
    //========= Items & Money Components
    [HideInInspector] public TextMeshProUGUI moneyText;
    [HideInInspector] public TextMeshProUGUI healthPotionText;
    [HideInInspector] public TextMeshProUGUI manaPotionText;
    [HideInInspector] public TextMeshProUGUI smokeBombText;
    
    // Start is called before the first frame update
    void Start()
    {
        iconImage =GameObject.Find("Shop_Canvas").transform.Find("Talks").transform.Find("Talker_Icon_Mask").transform.Find("Icon_Image").GetComponent<Image>();
        talkText = GameObject.Find("Shop_Canvas").transform.Find("Talks").transform.Find("Talk_Text").GetComponent<TextMeshProUGUI>();
        
        //Items 
        moneyText = GameObject.Find("Shop_Canvas").transform.Find("Items").transform.Find("Money_Text").GetComponent<TextMeshProUGUI>();
        healthPotionText = GameObject.Find("Shop_Canvas").transform.Find("Items").transform.Find("Health_Text").GetComponent<TextMeshProUGUI>();
        manaPotionText = GameObject.Find("Shop_Canvas").transform.Find("Items").transform.Find("Mana_Text").GetComponent<TextMeshProUGUI>();
        smokeBombText = GameObject.Find("Shop_Canvas").transform.Find("Items").transform.Find("Bomb_Text").GetComponent<TextMeshProUGUI>();

    }

    public void UpdateTalksUI(Sprite sprite, string talk)
    {
        iconImage.sprite = sprite;
        talkText.text = talk;
    }

    public void UpdateItemUI(string money, string hp, string mana, string bomb)
    {
        moneyText.text = money;
        healthPotionText.text = hp;
        manaPotionText.text = mana;
        smokeBombText.text = bomb;
    }
}
