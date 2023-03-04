using Base;
using UnityEngine;

public class UnitsAndAttacks : MonoBehaviour
{
    
    //Names from
    //https://www.imagineforest.com/blog/fantasy-character-names/
    
    private string[] _names = new[]
    {
        "Akibrus", "Angun", "Balrus", "Bulruk", "Caldor", "Dagen", "Darvyn", "Delvin", "Dracyian",
        "Dray", "Eldar", "Engar", "Fabien", "Farkas", "Galdor", "Igor", "Jai-Blynn", "Klayden", "Laimus",
        "Malfas", "Norok", "Orion", "Pindious", "Quintus", "Rammir", "Remus", "Rorik",
        "Sabir", "Séverin", "Sirius", "Soril", "Sulfu", "Syfas", "Viktas", "Vyn", "Wilkass", "Yagul", "Zakkas",
        "Zarek", "Zorion", "Aleera", "Alva", "Amara", "Anya", "Asralyn", "Azura", "Breya", "Brina",
        "Caelia", "Ciscra", "Dezaral", "Dorath", "Drusila", "Elda", "Esmeralla", "Freya", "Gelda", "Hadena",
        "Kyla", "Kyra", "Lavinia", "Lunarex", "Lyra", "Mireille", "Nyssa", "Olwyn", "Ophelia", "Peregrine",
        "Reyda", "Sarielle", "Shikta", "Sybella", "Syfyn", "Thalia", "Turilla", "Vasha", "Vixen",
        "Yvanna", "Zaria", "Zeniya",
    };
    
    public Unit[] unitsStats;

    public string SetRandomName(string playerName)
    {
        while (true)
        {
            var setRandomName = _names[Random.Range(0, _names.Length)];
            if (playerName != setRandomName)
            {
                return setRandomName;
            }
        }
    }
}
