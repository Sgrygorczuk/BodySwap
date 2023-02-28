using UnityEngine;

namespace Base
{
    /// <summary>
    /// This Script is used on the damage numbers, it destroys the object after 2 seconds of it existing 
    /// </summary>
    public class DamageNumber : MonoBehaviour
    {
        private void Start() { Destroy(gameObject, 2.0f); }
    }
}
