using System;
using System.Collections.Generic;
using Base;
using UnityEngine;

public class Data : MonoBehaviour
{

    //Variables 
    private static Data _instance; //Is the instance of the object that will show up in each scene 
    public Unit player;
    private int _enemyId = 0;
    private int _money = 25;
    private int[] _items = new[] { 0, 0, 0 }; //Health, Mana, Smoke Bomb
    [SerializeField]  private bool[] _goalsCompleted = new bool[13];
    private Vector2 _playerPosition = Vector2.zero;
    public string _enemyParentPath = "";
    public string _enemyPath = "";
    public bool _isIntroDone;

    private string _story = "Your journey began:";
    private bool checkIfPlayerIsDone;

    private string[] _goalText = new[]
    {
        "Explore unknown lands: ", //1
        "Become a Hero by slaying monsters: ", //5
        "Defend against the human invaders by defeating them: ", //5
        "Become rich beyond your wildest dreams by collecting te piece: ", //150
        "Climb the peak of Mount Alena: ", //1
        "Become the most popular person in town by chatting with everyone: ", //10
        "Be known as master escape arist by using smoke bombs: ", //4
        "Show everyone that you got cash by buying up potions: ", //6
        "Relax at the beaches of Lake Vando: ", //1
        "Find over 10 te piece while rummaging through pots: ", //10
        "Seek know from the great sage: ", //1
        "", //1                             
        "", //1
    };

    [SerializeField] private int[] _goal =  new [] {1, 5, 5, 150, 1, 10, 4, 12, 1, 10, 1, 1, 1 };
    [SerializeField] private int _currentGoalScore = 0;

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

    public void SetUnit(Unit unit)
    {
        player.Copy(unit);
    }
    public Unit GetUnit() { return player; }

    public void AddMoney(int money) { _money += money; }
    public void SubMoney(int money) { _money -= money; }
    public int GetMoney() { return _money; }

    public void AddItem(int index) { _items[index]++; }
    public void SubItem(int index) { _items[index]--; }
    public int GetItem(int index) { return _items[index]; }

    public void ResetItems()
    {
        _items[0] = 0;
        _items[1] = 0;
        _items[2] = 1;
    }

    public void SetPosition(Vector2 pos)
    {
        _playerPosition = pos;
    }

    public Vector2 GetPosition()
    {
        return _playerPosition;
    }

    public string GetGoalText()
    {
        if (_goalsCompleted[(int)player.lifeGoal])
        {
            return _goalText[(int)player.lifeGoal] +  _goal[(int) player.lifeGoal] + "/" +  _goal[(int) player.lifeGoal];
        }
        else
        {
            return _goalText[(int) player.lifeGoal] + _currentGoalScore + "/" + _goal[(int) player.lifeGoal];
        }
    }

    public bool GetIsGoalCompleted()
    {
        return _goalsCompleted[(int) player.lifeGoal];
    }

    public int GetCurrentGoal()
    {
        return _currentGoalScore;
    }

    public int GetLifeGoal()
    {
        return (int) player.lifeGoal;
    }

    public void UpdateGoal(int add)
    {
        _currentGoalScore += add;
        if (_currentGoalScore >= _goal[(int) player.lifeGoal])
        {
            _goalsCompleted[(int) player.lifeGoal] = true;
            
        }
    }

    public void ResetGoalState()
    {
        _currentGoalScore = 0;
    }

    public void SetEnemy(string parent, string enemy)
    {
        _enemyParentPath = parent;
        _enemyPath = enemy;
    }

    public void AddToStory(string bit)
    {
        _story += "\n" + bit;
    }

    public string GetStory()
    {
        return _story;
    }

    public void SetPlayerDone(bool done)
    {
        checkIfPlayerIsDone = done;
    }

    public bool GetPlayerDone()
    {
        return checkIfPlayerIsDone;
    }

}
