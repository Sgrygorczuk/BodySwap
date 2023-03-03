using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OverWorldUI
{
    /// <summary>
    /// This Script is used in the over world levels to set up the talk segments allows NPC to tell things to the
    /// player 
    /// </summary>
    
    public class OverWorldTalk : MonoBehaviour
    {
        private Image _iconImage;
        [HideInInspector] public TextMeshProUGUI nameText;
        private TextMeshProUGUI _talkText;
    
        /// <summary>
        /// Connects the UI game objects 
        /// </summary>
        private void Start()
        {
            _iconImage = GameObject.Find("Talk_Canvas").transform.Find("Talker_Icon_Mask").transform.Find("Icon_Image").GetComponent<Image>();
            nameText = GameObject.Find("Talk_Canvas").transform.Find("Name_Text").GetComponent<TextMeshProUGUI>();
            _talkText = GameObject.Find("Talk_Canvas").transform.Find("Talk_Text").GetComponent<TextMeshProUGUI>();
        }

        /// <summary>
        /// Updates the data to display what the NPC says 
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="unitName"></param>
        /// <param name="talk"></param>
        public void UpdateUI(Sprite sprite, string unitName, string talk)
        {
            _iconImage.sprite = sprite;
            nameText.text = unitName;
            _talkText.text = talk;
        }

    }
}
