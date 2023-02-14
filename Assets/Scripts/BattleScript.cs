using System.Collections;
using TMPro;
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
    private TextMeshProUGUI _playerHealthText;
    private TextMeshProUGUI _playerManaText;
    private TextMeshProUGUI _playerNameText;
    private Image _playerHealthImage;
    private Image _playerManaImage;
    private Image _playerIcon;
    private GameObject _playerDamageNumberSpawnPoint;
    
    //========= Enemy Components 
    private Unit _enemyUnit;
    private Image _enemyHealthImage;
    private Image _enemyImage;
    private GameObject _enemyDamageNumberSpawnPoint;
    
    //========= Menu Components 
    private GameObject[] _baseArrows = new GameObject[3];
    private TextMeshProUGUI[] _baseText = new TextMeshProUGUI[3];
    private GameObject _attackTab;
    private GameObject[] _attackArrows = new GameObject[4];
    private GameObject _canvas;
    private GameObject _vitoryTab;
    private TextMeshProUGUI _defeated;
    private TextMeshProUGUI _gold;
    
    //======== Controls
    private bool _baseState = true;
    private int _baseIndex = 0;       //Which base option is the player looking at 
    private int _attackIndex = 0;     //Which attack is the player looking at
    private int _maxPlayerAttackIndex = 0;  //How many attacks the player has access to 
    private int _maxEnemyAttackIndex = 0;  //How many attacks the player has access to 
    private int _playerCurrentHealth;
    private int _playerCurrentMana;
    private int _enemyCurrentHealth;
    private bool _enemyTurn;
    private Animator _battleAnimator;
    [SerializeField] private GameObject damageNumber;
        
    //======== Consts 
    private const float TIME_BAR_LOWERING = 0.02f;
    private const float TIME_TILL_ENEMY_TURN = 1f;
    
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
        SetUpUI();

        _battleAnimator = GameObject.Find("Canvas").GetComponent<Animator>();

    }

    //===== Sets Up Player =============================================================================================
    
    private void SetUpPlayer()
    {
        //Grabs the components  
        _playerUnit = GameObject.Find("Canvas").transform.Find("Player_Stats").GetComponent<Unit>();
        //Updated once
        _playerNameText = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("Player_Name").GetComponent<TextMeshProUGUI>();
        SetUpPlayerAttacks();
        //Updated through the battle 
        _playerHealthText = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("HP_Bar_BG").transform.Find("HP_Text").GetComponent<TextMeshProUGUI>();
        _playerManaText = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("MP_Bar_BG").transform.Find("MP_Text").GetComponent<TextMeshProUGUI>();
        _playerHealthImage = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("HP_Bar_BG").transform.Find("HP_Bar").GetComponent<Image>();
        _playerManaImage = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("MP_Bar_BG").transform.Find("MP_Bar").GetComponent<Image>();
        _playerIcon = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("Player_Icon_Mask")
            .transform.Find("Player_Icon").GetComponent<Image>();
        _playerDamageNumberSpawnPoint = GameObject.Find("Canvas").transform.Find("Player_Stats").transform.Find("Point").gameObject;

        //Pulls and passes on the information to the components 
        //TODO Copy Unit Data from OverWorld to Battle Scene Place Holder 
        _playerNameText.text = _playerUnit.name;
        _playerIcon.sprite = _playerUnit.sprite;
        _playerCurrentHealth = _playerUnit.health;
        _playerHealthText.text = _playerCurrentHealth + "/" + _playerUnit.health;
        _playerCurrentMana = _playerUnit.mana;
        _playerManaText.text = _playerCurrentMana + "/" + _playerUnit.mana;
    }

    private void SetUpPlayerAttacks()
    {
        _maxPlayerAttackIndex = _playerUnit.attacks.Length;
        //Copies the data for all of the attacks the player has
        for (int i = 0; i < _maxPlayerAttackIndex; i++)
        {
            var path = "Attack_" + i;
            var attackName = GameObject.Find("Canvas").transform.Find("Attack_Player_Actions").transform.Find(path).transform.Find("Attack_Name").GetComponent<TextMeshProUGUI>();
            var attackType = GameObject.Find("Canvas").transform.Find("Attack_Player_Actions").transform.Find(path).transform.Find("Attack_Type").GetComponent<Image>();
            var attackManaCost = GameObject.Find("Canvas").transform.Find("Attack_Player_Actions").transform.Find(path).transform.Find("Attack_MP_Cost").GetComponent<TextMeshProUGUI>();
            
            attackName.text = _playerUnit.attacks[i].name;
            //TODO Have the type select image 
            attackManaCost.text = "MP: " + _playerUnit.attacks[i].manaCost;
        }
        
        //If the player has less than 4 attacks it fills out the rest of the list with blanks 
        if (_maxPlayerAttackIndex >= 4) return;
        for(int i = _maxPlayerAttackIndex; i < 4; i++)
        {
            var path = "Attack_" + i;
            var attackName = GameObject.Find("Canvas").transform.Find("Attack_Player_Actions").transform.Find(path).transform.Find("Attack_Name").GetComponent<TextMeshProUGUI>();
            var attackType = GameObject.Find("Canvas").transform.Find("Attack_Player_Actions").transform.Find(path).transform.Find("Attack_Type").GetComponent<Image>();
            var attackManaCost = GameObject.Find("Canvas").transform.Find("Attack_Player_Actions").transform.Find(path).transform.Find("Attack_MP_Cost").GetComponent<TextMeshProUGUI>();

            attackName.text = "---";
            //TODO Have the type select image 
            attackManaCost.text = "MP: ---";
        }
    }

    //===== Sets Up Enemy ==============================================================================================
    private void SetUpEnemy()
    {
        _enemyUnit = GameObject.Find("Canvas").transform.Find("Enemy").GetComponent<Unit>();
        _maxEnemyAttackIndex = _enemyUnit.attacks.Length;
        //TODO Copy Unit Data from OverWorld to Battle Scene Place Holder 
        _enemyCurrentHealth = _enemyUnit.health;
        _enemyHealthImage = GameObject.Find("Canvas").transform.Find("Enemy").transform.Find("Enemy_Bar_BG").transform.Find("Enemy_Bar").GetComponent<Image>();
        _enemyImage = GameObject.Find("Canvas").transform.Find("Enemy").transform.Find("Enemy_Sprite")
            .GetComponent<Image>();
        _enemyImage.sprite = _enemyUnit.sprite;
        _enemyDamageNumberSpawnPoint = GameObject.Find("Canvas").transform.Find("Enemy").transform.Find("Point").gameObject;
    }
    
    //===== Sets Up UI =================================================================================================

    private void SetUpUI()
    {
        _canvas = GameObject.Find("Canvas");
        _vitoryTab = GameObject.Find("Canvas").transform.Find("Victory_Pop_Up").gameObject;
        _defeated = GameObject.Find("Canvas").transform.Find("Victory_Pop_Up").transform.Find("Defeated")
            .GetComponent<TextMeshProUGUI>();
        _gold = GameObject.Find("Canvas").transform.Find("Victory_Pop_Up").transform.Find("Gold")
            .GetComponent<TextMeshProUGUI>();
        
        //Connects the Base Point Arrow
        for (int i = 0; i < _baseArrows.Length; i++)
        {
            var path = "Point_" + i;
            var arrow = GameObject.Find("Canvas").transform.Find("Base_Player_Actions").Find(path).gameObject;
            _baseArrows[i] = arrow;
        }

        for (int i = 0; i < _baseText.Length; i++)
        {
            var path = "Text_" + i;
            var text = GameObject.Find("Canvas").transform.Find("Base_Player_Actions").Find(path).GetComponent<TextMeshProUGUI>();
            _baseText[i] = text;
        }

        _attackTab = GameObject.Find("Canvas").transform.Find("Attack_Player_Actions").gameObject;
        _attackTab.SetActive(false);
        
        //Connects the Attack Point Arrows 
        for (int i = 0; i < _attackArrows.Length; i++)
        {
            var path = "Point_" + i;
            var arrow = GameObject.Find("Canvas").transform.Find("Attack_Player_Actions").Find(path).gameObject;
            _attackArrows[i] = arrow;
        }
    }
    
    //==================================================================================================================
    
    //==================================================================================================================
    
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
                if (_enemyTurn)
                {
                    _enemyTurn = false;
                    StartCoroutine(EnemyAction());
                }
                break;
            }
        }
    }
    
    
    //==================================================================================================================
    //Player Actions 
    //==================================================================================================================

    private void PlayerActions()
    {
        if (_baseState) { PlayerBaseActions(); }
        else { PlayerAttackActions(); }
    }

    private void PlayerBaseActions()
    {
        //TODO Connect to Input Manager 
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _baseIndex--;
            if (_baseIndex < 0) { _baseIndex = _baseArrows.Length - 1; }
            UpdateBaseArrows();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _baseIndex++;
            if (_baseIndex == _baseArrows.Length) { _baseIndex = 0; }
            UpdateBaseArrows();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (_baseIndex)
            {
                case 0:
                {
                    _baseState = false;
                    _attackTab.SetActive(true);
                    break;
                }
                case 1:
                {

                    break;
                }
                case 2:
                {

                    break;
                }
            }
        }
    }

    private void UpdateBaseArrows()
    {
        foreach (var arrow in _baseArrows) { arrow.SetActive(false); }
        _baseArrows[_baseIndex].SetActive(true);
    }

    private void PlayerAttackActions()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _attackIndex--;
            if (_attackIndex < 0) { _attackIndex = _attackArrows.Length - 1; }
            UpdateAttackArrows();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            _attackIndex++;
            if (_attackIndex == (_attackArrows).Length) { _attackIndex = 0; }
            UpdateAttackArrows();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_attackIndex < _maxPlayerAttackIndex && _playerCurrentMana >= _playerUnit.attacks[_attackIndex].manaCost){
                StartCoroutine(PlayerAttack());
            }
            else
            {
                //TODO play negative SFX
            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            _baseState = true;
            _attackTab.SetActive(false);
        }
    }

    private void UpdateAttackArrows()
    {
        foreach (var arrow in _attackArrows) { arrow.SetActive(false); }
        _attackArrows[_attackIndex].SetActive(true);
    }

    private IEnumerator PlayerAttack()
    {
        _battleAnimator.Play("EnemyHurt");
        SpawnDamage(_enemyDamageNumberSpawnPoint, _playerUnit.attacks[_attackIndex].damage.ToString());
        var time = TIME_BAR_LOWERING * (_playerUnit.attacks[_attackIndex].damage + 1);
        _currentState = BattleStates.EnemyTurn;
        _baseState = true;
        _attackTab.SetActive(false);
        StartCoroutine(LowerMagic());
        StartCoroutine(LowerEnemyHealth());
        PlayerLoseControls();
        yield return new WaitForSeconds(time);
        if (_enemyCurrentHealth <= 0)
        {
            yield return new WaitForSeconds(TIME_TILL_ENEMY_TURN / 2f);
            StartCoroutine(WinAction());
            yield break;
        }
        _enemyTurn = true;
    }

    private void PlayerLoseControls()
    {
        _baseIndex = 0;
        UpdateAttackArrows();
        _baseArrows[0].SetActive(false);
        
        _attackIndex = 0;
        UpdateAttackArrows();

        foreach (var text in _baseText) { text.color = Color.gray; }
    }

    private IEnumerator LowerMagic()
    {
        var goal = _playerCurrentMana - _playerUnit.attacks[_attackIndex].manaCost;
        _playerManaText.text = goal + "/" + _playerUnit.mana;
        for (int i = _playerCurrentMana; i > goal; i--)
        {
            _playerCurrentMana--;
            _playerManaImage.fillAmount = (float) _playerCurrentMana / _playerUnit.mana;
            yield return new WaitForSeconds(TIME_BAR_LOWERING);
        }
    }
    
    private IEnumerator LowerEnemyHealth()
    {
        //TODO make a better damage system based on physical vs magic 
        var goal = _enemyCurrentHealth - _playerUnit.attacks[_attackIndex].damage;
        for (int i = _enemyCurrentHealth; i > goal; i--)
        {
            _enemyCurrentHealth--;
            _enemyHealthImage.fillAmount = (float) _enemyCurrentHealth / _enemyUnit.health;
            yield return new WaitForSeconds(TIME_BAR_LOWERING);
        }
    }

    private void SpawnDamage(GameObject point, string damage)
    {
        var clone = Instantiate(damageNumber, point.transform.position, Quaternion.identity);
        clone.transform.SetParent(_canvas.transform);
        clone.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = damage;
    }
    
    //==================================================================================================================
    //Enemy Actions 
    //==================================================================================================================

    private IEnumerator EnemyAction()
    {
        var rollAttack = Random.Range(0, _maxEnemyAttackIndex);
        var time = TIME_BAR_LOWERING * (_enemyUnit.attacks[rollAttack].damage + 1);
        yield return new WaitForSeconds(TIME_TILL_ENEMY_TURN);
        StartCoroutine(LowerPlayerHealth(rollAttack));
        _battleAnimator.Play("Shake");
        SpawnDamage(_playerDamageNumberSpawnPoint, _enemyUnit.attacks[rollAttack].damage.ToString());
        yield return new WaitForSeconds(time);
        if (_playerCurrentHealth <= 0)
        {
            StartCoroutine(LoseAction());
            yield break;
        }
        PlayerRegainControls();
        _currentState = BattleStates.PlayerTurn;

    }

    private IEnumerator LowerPlayerHealth(int rollAttack)
    {
        var goal = _playerCurrentHealth - _enemyUnit.attacks[rollAttack].damage;
        _playerHealthText.text = goal + "/" + _playerUnit.health;
        for (int i = _playerCurrentHealth; i > goal; i--)
        {
            _playerCurrentHealth--;
            _playerHealthImage.fillAmount = (float) _playerCurrentHealth / _playerUnit.health;
            yield return new WaitForSeconds(TIME_BAR_LOWERING);
        }
    }

    private void PlayerRegainControls()
    {
        _baseArrows[0].SetActive(true);
        foreach (var text in _baseText) { text.color = Color.black; }
    }
    
    //==================================================================================================================
    //Win Actions 
    //==================================================================================================================

    private IEnumerator WinAction()
    {
        _currentState = BattleStates.Won;
        _battleAnimator.Play("EnemyFade"); 
        yield return new WaitForSeconds(TIME_TILL_ENEMY_TURN * 2);
        SetUpVictoryScreen();
        yield return new WaitForSeconds(TIME_TILL_ENEMY_TURN);
    }
    
    //==================================================================================================================
    //Lose Actions 
    //==================================================================================================================

    private IEnumerator LoseAction()
    {
        _currentState = BattleStates.Lost;
        _battleAnimator.Play("Swap");
        yield return new WaitForSeconds(0.25f);
        _enemyImage.sprite = _playerUnit.sprite;
        _playerIcon.sprite = _enemyUnit.sprite;
        UpdatePlayerData();
        SwapData();
        yield return new WaitForSeconds(TIME_TILL_ENEMY_TURN);
        _battleAnimator.Play("EnemyFade"); 
        SetUpVictoryScreen();
        yield return new WaitForSeconds(TIME_TILL_ENEMY_TURN);
    }

    private void UpdatePlayerData()
    {
        _playerNameText.text = _enemyUnit.name;
        _playerHealthImage.fillAmount = (float) _enemyCurrentHealth / _enemyUnit.health;
        _enemyHealthImage.fillAmount = (float) _playerCurrentHealth / _playerUnit.health;
        _playerManaImage.fillAmount =  (float) _enemyUnit.mana / _enemyUnit.mana;
        _playerHealthText.text = _enemyCurrentHealth + "/" + _enemyUnit.health;
        _playerManaText.text =_enemyUnit.mana+ "/" + _enemyUnit.mana;

    }
    private void SwapData()
    {
        //TODO Put data in cross scene script 
    }

    private void SetUpVictoryScreen()
    {
        //TODO use crsoss scene data to input names 
        _defeated.text = "You have defeated " + _playerUnit.name + " may they rest in peace.";
        _gold.text = "You gather " +  _playerUnit.moneyDrop + " gold pieces.";
        _vitoryTab.SetActive(true);
    }


    

}
