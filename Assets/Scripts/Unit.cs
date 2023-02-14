using UnityEngine;
using UnityEngine.UI;
public class Unit : MonoBehaviour
{
    public string name = "";
    public Sprite sprite;
    public int health = 100;
    public int mana = 100;
    public int physicalDefense = 100;
    public int magicalDefense= 100;
    public int moneyDrop = 10;
    public Attack[] attacks;

    [System.Serializable]
    public struct Attack
    {
        public int damage;   //How much damage does the attack do
        public int manaCost; //How much does it cost to use the attack
        public bool type;    //Defines if the attack is magic or physical
        public string name;  //Name of the attack 
    }
    

}
