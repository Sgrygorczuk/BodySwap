using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverworldTalk : MonoBehaviour
{
    [HideInInspector] private Image iconImage;
    [HideInInspector] public TextMeshProUGUI nameText;
    [HideInInspector] private TextMeshProUGUI talkText;
    
    // Start is called before the first frame update
    void Start()
    {
        iconImage = GameObject.Find("Talk_Canvas").transform.Find("Talker_Icon_Mask").transform.Find("Icon_Image").GetComponent<Image>();
        nameText = GameObject.Find("Talk_Canvas").transform.Find("Name_Text").GetComponent<TextMeshProUGUI>();
        talkText = GameObject.Find("Talk_Canvas").transform.Find("Talk_Text").GetComponent<TextMeshProUGUI>();
    }

    public void UpdateUI(Sprite sprite, string unitName, string talk)
    {
        iconImage.sprite = sprite;
        nameText.text = unitName;
        talkText.text = talk;
    }

}
