using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleScript : MonoBehaviour
{

    //========= State Machine Components 
    private enum BattleStates
    {
        Start,
        PlayerTurn,
        EnemyTurn,
        Won,
        Lost
    }
    private BattleStates _currentState = BattleStates.Start;
    
    //========= Player Components 
    private Unit _playerUnit;
    private Text _playerHealthText;
    private Text _playerManaText;
    private Image _playerHealthImage;
    private Image _playerManaImage;

    //========= Enemy Components 
    private Unit _enemyUnit;
    
    //======== Controls
    private int _baseIndex = 0;       //Which base option is the player looking at 
    private int _attackIndex = 0;     //Which attack is the player looking at
    private int _maxAttackIndex = 0;  //How many attacks the player has access to 
    private int _playerCurrentHealth;
    private int _playerCurrentMana;
    private int _enemyCurrentHealth;
    
    // Start is called before the first frame update
    private void Start()
    {
        SetUpBattle();
        _currentState = BattleStates.PlayerTurn;
    }

    private void SetUpBattle()
    {
        //TODO pull info from the over world player and enemy and pass them to the place holders  
        SetUpPlayer();
        SetUpEnemy();
    }

    private void SetUpPlayer()
    {
        //Grabs the components  
        _playerUnit = GameObject.Find("Canvas").transform.Find("Player_Stats").GetComponent<Unit>();
        //Updated once
        var playerName = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("Player_Name").GetComponent<Text>();
        //Updated through the battle 
        _playerHealthText = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("HP_Bar_BG").transform.Find("HP_Text").GetComponent<Text>();
        _playerManaText = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("MP_Bar_BG").transform.Find("MP_Text").GetComponent<Text>();
        _playerHealthImage = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("HP_Bar_BG").transform.Find("HP_Bar").GetComponent<Image>();
        _playerManaImage = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("MP_Bar_BG").transform.Find("MP_Bar").GetComponent<Image>();
        
        //Pulls and passes on the information to the components 
        //TODO Copy Unit Data from OverWorld to Battle Scene Place Holder 
        playerName.text = _playerUnit.name;
        _playerCurrentHealth = _playerUnit.health;
        _playerHealthText.text = _playerCurrentHealth + "/" + _playerUnit.health;
        _playerCurrentMana = _playerUnit.mana;
        _playerManaText.text = _playerCurrentMana + "/" + _playerUnit.mana;
        
        _maxAttackIndex = _playerUnit.attacks.Length;
    }

    private void SetUpPlayerAttacks()
    {
        var attackOneName = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("Player_Name").GetComponent<Text>();
        var attackOneType = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("Player_Name").GetComponent<Text>();
        var attackOne = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("Player_Name").GetComponent<Text>();

    }

    private void SetUpEnemy()
    {
        _enemyUnit = GameObject.Find("Canvas").transform.Find("Enemy").GetComponent<Unit>();
        //TODO Copy Unit Data from OverWorld to Battle Scene Place Holder 
    }

    private void Update()
    {
        switch (_currentState)
        {
            case BattleStates.PlayerTurn:
            {
                PlayerActions();
                break;
            }
            case BattleStates.EnemyTurn:
            {
                break;
            }
        }
    }

    private void PlayerActions()
    {
        
    }
}
