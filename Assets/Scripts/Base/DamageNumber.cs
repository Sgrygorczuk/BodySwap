using System;
using UnityEngine;
using TMPro;

namespace Base
{
    /// <summary>
    /// This Script is used on the damage numbers, it destroys the object after 2 seconds of it existing 
    /// </summary>
    public class DamageNumber : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        
        private void Start()
        {
            _text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            SetColor();
            Destroy(gameObject, 2.0f);
        }

        /// <summary>
        /// Checks what the value of the text is and sets the color to text between white, yellow and red based on how
        /// high the damage was 
        /// </summary>
        private void SetColor()
        {
            int.TryParse(_text.text, out var number);
            _text.color = number switch
            {
                < 5 => Color.white,
                >= 5 and < 10 => Color.yellow,
                _ => Color.red
            };
        }
    }
}
