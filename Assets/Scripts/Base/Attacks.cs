using UnityEngine;

namespace Base
{
    /// <summary>
    /// This Script is used to create PreFabs of attacks that will be slotted in Unit scripts
    /// </summary>
    public class Attacks : MonoBehaviour
    {
        public string attackName;  //Name of the attack 
        public int damage;   //How much damage does the attack do
        public int manaCost; //How much does it cost to use the attack
    }
}
