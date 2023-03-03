using System.Collections.Generic;
using UnityEngine;

namespace Base
{
    /// <summary>
    /// This Script is used throughout the whole game to carry the player data and progress. It's used to update
    /// the UI, player stats and what actions the game should take. 
    /// </summary>
    
    public class Data : MonoBehaviour
    {
        //Variables 
        //============= Data 
        //Is the instance of the object that will show up in each scene 
        private static Data _instance;
        
        //============= Player 
        //The Unit holds all the stats that the player will call upon 
        public Unit player;
        //How much money the player has 
        private int _money = 25;
        //How much of each item the player has 
        private int[] _items = { 0, 0, 0 }; //Health, Mana, Smoke Bomb
        //The location the player was when move to battle screen 
        private Vector2 _playerPosition = Vector2.zero;
        
        //=========== Enemy 
        //The index the enemy is assigned to be read in battle scene 
        private int _enemyId;
        //Where the enemy exits on hierarchy so we can make them disappear after battle  
        public string enemyParentPath = "";
        public string enemyPath = "";

        //=========== Progress 
        //This holds all the actions performed by the player to be displayed at the end of the story
        private List<string> _story = new() { "Your journey began:" };
        //Checks if the player won the game 
        private bool _checkIfPlayerIsDone;
        //Checks if the intro cutscene was played 
        public bool isIntroDone;
        //Checks if the game displayed the intro pop up 
        public bool isIntroPopUpDone;

        //============ Goals 
        //The text that will display when a character chooses a goal 
        private readonly string[] _goalText = {
            "Explore unknown lands: ", //1
            "Become a Hero by slaying monsters: ", //5
            "Defend against the human invaders by defeating them: ", //5
            "Become rich beyond your wildest dreams by collecting te piece: ", //65
            "Be known as master escape arist by using smoke bombs: ", //6
        };
        //The the number of acts the player needs to perform till the goal is reached 
        [SerializeField] private int[] goal =  {1, 5, 5, 65, 6};
        //Counter for how many times the act has bee performed 
        [SerializeField] private int currentGoalScore;
        //Keeps track of which goal is complete 
        [SerializeField]  private bool[] goalsCompleted = new bool[5];

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

        /// <summary>
        /// Used to reset the data when the game ends and loops back to the main menu 
        /// </summary>
        /// <param name="playerBase"></param>
        public void ResetData(Unit playerBase)
        {
            player.Copy(playerBase);
            _enemyId = 0;
            _money = 25;
            _items = new[] { 0, 0, 0 }; //Health, Mana, Smoke Bomb
            for (int i = 0; i < goalsCompleted.Length; i++) { goalsCompleted[i] = false; }
            _playerPosition = Vector2.zero;
            enemyParentPath = "";
            enemyPath = "";
            isIntroDone = false;
            _story.Clear();
            _story.Add( "Your journey began:" );
            _checkIfPlayerIsDone = false;
        }
        
        //==================================================================================================================
        // Enemy Methods 
        //==================================================================================================================

        public void SetId(int id) { _enemyId = id; }
        public int GetId() { return _enemyId; }
        
        public void SetEnemy(string parent, string enemy)
        {
            enemyParentPath = parent;
            enemyPath = enemy;
        }

        //==================================================================================================================
        // Player Methods 
        //==================================================================================================================
        
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
            _items[0] = 1;
            _items[1] = 1;
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

        //==================================================================================================================
        // Goal Methods 
        //==================================================================================================================

        public string GetGoalText()
        {
            if (goalsCompleted[(int)player.lifeGoal])
            {
                return _goalText[(int)player.lifeGoal] +  goal[(int) player.lifeGoal] + "/" +  goal[(int) player.lifeGoal];
            }

            return _goalText[(int) player.lifeGoal] + currentGoalScore + "/" + goal[(int) player.lifeGoal];
        }

        public bool GetIsGoalCompleted()
        {
            return goalsCompleted[(int) player.lifeGoal];
        }

        public int GetCurrentGoal()
        {
            return currentGoalScore;
        }

        public int GetLifeGoal()
        {
            return (int) player.lifeGoal;
        }

        public void UpdateGoal(int add)
        {
            currentGoalScore += add;
            if (currentGoalScore >= goal[(int) player.lifeGoal])
            {
                goalsCompleted[(int) player.lifeGoal] = true;
            
            }
        }

        public void ResetGoalState()
        {
            currentGoalScore = 0;
        }
        
        //==================================================================================================================
        // Progress Methods 
        //==================================================================================================================

        public void AddToStory(string bit)
        {
            _story.Add("\n" + bit);
        }

        public List<string> GetStory()
        {
            return _story;
        }

        public void SetPlayerDone(bool done)
        {
            _checkIfPlayerIsDone = done;
        }

        public bool GetPlayerDone()
        {
            return _checkIfPlayerIsDone;
        }

    }
}
