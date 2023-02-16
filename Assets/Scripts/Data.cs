using UnityEngine;

public class Data : MonoBehaviour
{
    
    //Variables 
    private static Data _instance; //Is the instance of the object that will show up in each scene 
    public Unit player;
    private int _enemyId = 0;
    private int _money = 0;
    private int[] _items = new[] { 1, 1, 1 }; //Health, Mana, Smoke Bomb
    
    //==================================================================================================================
    // Base Functions 
    //==================================================================================================================
    
    //Creates the object, if one already has been created in another scene destroy this one and the make a new one 
    private void Awake()
    {
        //Checks if there already exits a copy of this, if does destroy it and let the new one be created 
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
            
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //==================================================================================================================
    // Data Update Methods 
    //==================================================================================================================

    public void SetId(int id) { _enemyId = id; }
    public int GetId() { return _enemyId; }

    public void SetUnit(Unit unit) { player = unit; }
    public void UpdateUnit(Unit unit) {player.Copy(unit);}
    public Unit GetUnit() { return player; }

    public void AddMoney(int money) { _money += money; }
    public void SubMoney(int money) { _money -= money; }
    public int GetMoney() { return _money; }

    public void AddItem(int index) { _items[index]++; }
    public void SubItem(int index) { _items[index]--; }
    public int GetItem(int index) { return _items[index]; }
    

    
}
