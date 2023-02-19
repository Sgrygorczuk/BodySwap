using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BattleScript : MonoBehaviour
{
    //======== Data
    private Data _data;
    private UnitsAndAttacks _unitsAndAttacksScript;

    //========= State Machine Components 
    private enum BattleStates
    {
        Start,
        PlayerTurn,
        EnemyTurn,
        End
    }
    private BattleStates _currentState = BattleStates.Start;
    
    //======== Set Up
    private EnemyComponents _eC; //Enemy Visual Components and Controls 
    private PlayerComponents _pC; //Player Visual Components and Controls 
    private UIComponent _uC; //UI Visual Components and Controls 

    //======== Controls
    private enum MenuState
    {
        Base,
        Attack,
        Item
    }
    private MenuState _currentMenu = MenuState.Base;
    
    
    private int _baseIndex = 0;       //Which base option is the player looking at 
    private int _attackIndex = 0;     //Which attack is the player looking at
    private int _itemIndex = 0;
    private bool _enemyTurn;
    private Animator _battleAnimator;
    [SerializeField] private GameObject damageNumber;

    private AudioSource _moveAudioSource;
    private AudioSource _selectAudioSource;
    private AudioSource _denyAudioSource;
    private AudioSource _attackAudioSource;

    //======== Consts 
    private const float TIME_BAR_LOWERING = 0.02f;
    private const float TIME_TILL_ENEMY_TURN = 1f;
    
    // Start is called before the first frame update
    private void Start()
    {
        
        SetUpBattle();
        _currentState = BattleStates.PlayerTurn;

        _moveAudioSource = GameObject.Find("SFX").transform.Find("MovingSFX").GetComponent<AudioSource>();
        _selectAudioSource = GameObject.Find("SFX").transform.Find("SelectSFX").GetComponent<AudioSource>();
        _denyAudioSource = GameObject.Find("SFX").transform.Find("DenySFX").GetComponent<AudioSource>();
        _attackAudioSource = GameObject.Find("SFX").transform.Find("AttackSFX").GetComponent<AudioSource>();
    }

    private void SetUpBattle()
    {
        _data = GameObject.Find("Data").GetComponent<Data>();
        _unitsAndAttacksScript = GameObject.Find("BattleScript").GetComponent<UnitsAndAttacks>();
        
        _pC = GameObject.Find("BattleScript").GetComponent<PlayerComponents>();
        _pC.SetUpPlayer(_data.GetUnit());
        
        _eC = GameObject.Find("BattleScript").GetComponent<EnemyComponents>();
        _eC.SetUpEnemy(_unitsAndAttacksScript, _data.GetId());

        _uC = GameObject.Find("BattleScript").GetComponent<UIComponent>();
        _uC.SetUpUI();
        _uC.itemText[0].text = _data.GetItem(0).ToString();
        _uC.itemText[1].text = _data.GetItem(1).ToString();
        _uC.itemText[2].text = _data.GetItem(2).ToString();

        _battleAnimator = GameObject.Find("Canvas").GetComponent<Animator>();
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
            case BattleStates.End:
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SceneManager.LoadScene("StartScene");
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
        switch (_currentMenu)
        {
            case MenuState.Base:
            {
                PlayerBaseActions(); 
                break;
            }
            case MenuState.Attack:
            {
                PlayerAttackActions();
                break;
            }
            case MenuState.Item:
            {
                PlayerItemActions();
                break;
            }
        }
    }

    private void PlayerBaseActions()
    {
        //TODO Connect to Input Manager 
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _baseIndex--;
            if (_baseIndex < 0) { _baseIndex = _uC.baseArrows.Length - 1; }
            _moveAudioSource.Play();
            UpdateBaseArrows();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _baseIndex++;
            if (_baseIndex == _uC.baseArrows.Length) { _baseIndex = 0; }
            _moveAudioSource.Play();
            UpdateBaseArrows();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (_baseIndex)
            {
                case 0:
                {
                    _currentMenu = MenuState.Attack;
                    _uC.attackTab.SetActive(true);
                    _selectAudioSource.Play();
                    break;
                }
                case 1:
                {
                    _currentMenu = MenuState.Item;
                    _uC.itemTab.SetActive(true);
                    _selectAudioSource.Play();
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
        foreach (var arrow in _uC.baseArrows) { arrow.SetActive(false); }
        _uC.baseArrows[_baseIndex].SetActive(true);
    }

    private void PlayerAttackActions()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _attackIndex--;
            if (_attackIndex < 0) { _attackIndex = _uC.attackArrows.Length - 1; }
            _moveAudioSource.Play();
            UpdateAttackArrows();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            _attackIndex++;
            if (_attackIndex == (_uC.attackArrows).Length) { _attackIndex = 0; }
            _moveAudioSource.Play();
            UpdateAttackArrows();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_attackIndex == _uC.attackArrows.Length - 1)
            {
                _currentMenu = MenuState.Base;
                _uC.attackTab.SetActive(false);
                _attackIndex = 0;
                _selectAudioSource.Play();
                UpdateAttackArrows();
            }
            else 
            {
                if (_attackIndex < _pC.maxPlayerAttackIndex &&
                    _pC.playerUnit.currentMana >= _pC.playerUnit.attacks[_attackIndex].manaCost)
                {
                    StartCoroutine(PlayerAttack());
                    _selectAudioSource.Play();
                }
                else
                {
                    _denyAudioSource.Play();
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            _currentMenu = MenuState.Base;
            _uC.attackTab.SetActive(false);
            _attackIndex = 0;
            UpdateAttackArrows();
        }
    }

    private void UpdateAttackArrows()
    {
        foreach (var arrow in _uC.attackArrows) { arrow.SetActive(false); }
        _uC.attackArrows[_attackIndex].SetActive(true);
    }

    private IEnumerator PlayerAttack()
    {
        _battleAnimator.Play("EnemyHurt");
        var damage = (int) Random.Range(_pC.playerUnit.attacks[_attackIndex].damage * 0.6f,
            _pC.playerUnit.attacks[_attackIndex].damage);
        SpawnDamage(_eC.enemyDamageNumberSpawnPoint, damage.ToString());
        var time = TIME_BAR_LOWERING * (_pC.playerUnit.attacks[_attackIndex].damage + 1);
        _currentState = BattleStates.EnemyTurn;
        _currentMenu = MenuState.Base;
        _uC.attackTab.SetActive(false);
        StartCoroutine(LowerMagic());
        StartCoroutine(LowerEnemyHealth(damage));
        PlayerLoseControls();
        _attackAudioSource.Play();
            yield return new WaitForSeconds(time);
        if (_eC.enemyCurrentHealth <= 0)
        {
            yield return new WaitForSeconds(TIME_TILL_ENEMY_TURN / 2f);
            StartCoroutine(WinAction());
            _selectAudioSource.Play();
            yield break;
        }
        _enemyTurn = true;
    }

    private void PlayerLoseControls()
    {
        _baseIndex = 0;
        UpdateAttackArrows();
        _uC.baseArrows[0].SetActive(false);
        
        _attackIndex = 0;
        UpdateAttackArrows();

        foreach (var text in _uC.baseText) { text.color = Color.gray; }
    }
    
    private IEnumerator LowerMagic()
    {
        var goal = _pC.playerUnit.currentMana - _pC.playerUnit.attacks[_attackIndex].manaCost;
        _pC.playerManaText.text = goal + "/" + _pC.playerUnit.maxMana;
        for (int i = _pC.playerUnit.currentMana; i > goal; i--)
        {
            _pC.playerUnit.currentMana--;
            _pC.playerManaImage.fillAmount = (float) _pC.playerUnit.currentMana / _pC.playerUnit.maxMana;
            yield return new WaitForSeconds(TIME_BAR_LOWERING);
        }
    }

    private IEnumerator LowerEnemyHealth(int damage)
    {
        var goal = _eC.enemyCurrentHealth - damage;
        for (int i = _eC.enemyCurrentHealth; i > goal; i--)
        {
            _eC.enemyCurrentHealth--;
            _eC.enemyHealthImage.fillAmount = (float) _eC.enemyCurrentHealth / _eC.enemyUnit.maxHealth;
            yield return new WaitForSeconds(TIME_BAR_LOWERING);
        }
    }

    private void SpawnDamage(GameObject point, string damage)
    {
        var clone = Instantiate(damageNumber, point.transform.position, Quaternion.identity);
        clone.transform.SetParent(_uC.canvas.transform);
        clone.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = damage;
    }
    
    //==================================================================================================================
    //Items Actions 
    //==================================================================================================================
    
    private void PlayerItemActions()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _itemIndex--;
            if (_itemIndex < 0) { _itemIndex = _uC.itemArrows.Length - 1; }
            _moveAudioSource.Play();
            UpdateItemArrows();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            _itemIndex++;
            if (_itemIndex == (_uC.itemArrows).Length) { _itemIndex = 0; }
            _moveAudioSource.Play();
            UpdateItemArrows();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if(_itemIndex ==  _uC.itemArrows.Length - 1)
            {
                _currentMenu = MenuState.Base;
                _uC.itemTab.SetActive(false);
                _itemIndex = 0;
                UpdateItemArrows();
                _selectAudioSource.Play();
            }
            else 
            {
                if (_data.GetItem(_itemIndex) >= 1){
                    _data.SubItem(_itemIndex);
                    _uC.itemText[_itemIndex].text = _data.GetItem(_itemIndex).ToString();
                    switch (_itemIndex)
                    {
                        case 0:
                        {
                            StartCoroutine(IncreasePlayerHealth());
                            break;
                        }
                        case 1:
                        {
                            StartCoroutine(IncreaseMagic());
                            break;
                        }
                        case 2:
                        {
                            StartCoroutine(SetUpEscapeScreen());
                            break;
                        }
                    }
                    _currentMenu = MenuState.Base;
                    _uC.itemTab.SetActive(false);
                    _itemIndex = 0;
                    UpdateItemArrows();
                    _selectAudioSource.Play();
                }
                else
                {
                    _denyAudioSource.Play();
                }   
            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            _currentMenu = MenuState.Base;
            _uC.itemTab.SetActive(false);
            _itemIndex = 0;
            UpdateItemArrows();
        }
    }

    private void UpdateItemArrows()
    {
        foreach (var arrow in _uC.itemArrows) { arrow.SetActive(false); }
        _uC.itemArrows[_itemIndex].SetActive(true);
    }
    
    private IEnumerator IncreaseMagic()
    {
        for (int i = _pC.playerUnit.currentMana; i < _pC.playerUnit.maxMana; i++)
        {
            _pC.playerUnit.currentMana++;
            _pC.playerManaImage.fillAmount = (float) _pC.playerUnit.currentMana / _pC.playerUnit.maxMana;
            yield return new WaitForSeconds(TIME_BAR_LOWERING);
        }
        _pC.playerManaText.text = _pC.playerUnit.currentMana + "/" + _pC.playerUnit.maxMana;
    }
    
    private IEnumerator IncreasePlayerHealth()
    {
        for (int i = _pC.playerUnit.currentHealth; i < _pC.playerUnit.maxHealth; i++)
        {
            _pC.playerUnit.currentHealth++;
            _pC.playerHealthImage.fillAmount = (float) _pC.playerUnit.currentHealth / _pC.playerUnit.maxHealth;
            yield return new WaitForSeconds(TIME_BAR_LOWERING);
        }
        _pC.playerHealthText.text = _pC.playerUnit.currentHealth + "/" + _pC.playerUnit.maxHealth;
    }

    //==================================================================================================================
    //Enemy Actions 
    //==================================================================================================================

    private IEnumerator EnemyAction()
    {
        var rollAttack = Random.Range(0, _eC.maxEnemyAttackIndex);
        var time = TIME_BAR_LOWERING * (_eC.enemyUnit.attacks[rollAttack].damage + 1);
        var damage = (int) Random.Range(_eC.enemyUnit.attacks[rollAttack].damage * 0.6f,
            _eC.enemyUnit.attacks[rollAttack].damage);
        yield return new WaitForSeconds(TIME_TILL_ENEMY_TURN);
        StartCoroutine(LowerPlayerHealth(damage));
        _battleAnimator.Play("Shake");
        SpawnDamage(_pC.playerDamageNumberSpawnPoint, damage.ToString());
        yield return new WaitForSeconds(time);
        if (_pC.playerUnit.currentHealth <= 0)
        {
            StartCoroutine(LoseAction());
            yield break;
        }
        PlayerRegainControls();
        _currentState = BattleStates.PlayerTurn;

    }

    private IEnumerator LowerPlayerHealth(int damage)
    {
        var goal = _pC.playerUnit.currentHealth - damage;
        _pC.playerHealthText.text = goal + "/" + _pC.playerUnit.maxHealth;
        for (int i = _pC.playerUnit.currentHealth; i > goal; i--)
        {
            _pC.playerUnit.currentHealth--;
            _pC.playerHealthImage.fillAmount = (float) _pC.playerUnit.currentHealth / _pC.playerUnit.maxHealth;
            yield return new WaitForSeconds(TIME_BAR_LOWERING);
        }
    }

    private void PlayerRegainControls()
    {
        _uC.baseArrows[0].SetActive(true);
        foreach (var text in _uC.baseText) { text.color = Color.black; }
    }
    
    //==================================================================================================================
    //End Actions 
    //==================================================================================================================

    private IEnumerator WinAction()
    {
        _battleAnimator.Play("EnemyFade");
        if (_data.GetLifeGoal() == 1 || _data.GetLifeGoal() == 2)
        {
            _data.UpdateGoal(1);
        }
        yield return new WaitForSeconds(TIME_TILL_ENEMY_TURN * 2);
        SetUpVictoryScreen(true);
        _data.SetUnit(_pC.playerUnit);
        yield return new WaitForSeconds(TIME_TILL_ENEMY_TURN);
        _currentState = BattleStates.End;
    }

    private IEnumerator LoseAction()
    {
        _battleAnimator.Play("Swap");
        _data.ResetGoalState();
        yield return new WaitForSeconds(0.25f);
        SetUpVictoryScreen(false);
        _eC.enemyImage.sprite = _pC.playerUnit.sprite;
        _pC.playerIcon.sprite = _eC.enemyUnit.sprite;
        UpdatePlayerData();
        _eC.enemyUnit.currentHealth = _eC.enemyCurrentHealth;
        _data.SetUnit(_eC.enemyUnit);
        _data.ResetItems();
        yield return new WaitForSeconds(TIME_TILL_ENEMY_TURN);
        _battleAnimator.Play("EnemyFade");
        yield return new WaitForSeconds(TIME_TILL_ENEMY_TURN);
        _currentState = BattleStates.End;
    }
    
    private void UpdatePlayerData()
    {
        _pC.playerNameText.text = _eC.enemyUnit.unitName;
        _pC.playerHealthImage.fillAmount = (float) _eC.enemyCurrentHealth / _eC.enemyUnit.maxHealth;
        _eC.enemyHealthImage.fillAmount = (float) _pC.playerUnit.currentHealth / _pC.playerUnit.maxHealth;
        _pC.playerManaImage.fillAmount =  (float) _eC.enemyUnit.currentMana / _eC.enemyUnit.currentMana;
        _pC.playerHealthText.text = _eC.enemyCurrentHealth + "/" + _eC.enemyUnit.maxHealth;
        _pC.playerManaText.text = _eC.enemyUnit.currentMana+ "/" + _eC.enemyUnit.currentMana;

    }

    private void SetUpVictoryScreen(bool playerWon)
    {
        if (playerWon)
        {
            _uC.defeated.text = "You have defeated " + _eC.enemyUnit.unitName + " may they rest in peace.";
            _uC.gold.text = "You gather " +  _eC.enemyUnit.moneyDrop + " gold pieces.";
            if (_data.GetLifeGoal() == 3)
            {
                _data.UpdateGoal(_eC.enemyUnit.moneyDrop);
            }
            _data.AddToStory(_pC.playerUnit.unitName + " have defeated " + _eC.enemyUnit.unitName + " may they rest in peace.");
            if (_data.GetIsGoalCompleted())
            {
                _data.AddToStory(_pC.playerUnit.unitName + " completed their life goal of: " + _data.GetGoalText());
                _data.SetPlayerDone(true);
            }
        }
        else
        {
            _uC.defeated.text = "You have defeated " + _pC.playerUnit.unitName + " may they rest in peace.";
            _uC.gold.text = "You gather " +  _data.GetMoney() + " gold pieces.";
            if (!_data.GetIsGoalCompleted())
            {
                _data.AddToStory(_pC.playerUnit.unitName + " failed their life goal of: " + _data.GetGoalText());
            }
        }
        _uC.victoryTab.SetActive(true);
        if (playerWon) { _data.AddMoney(_eC.enemyUnit.moneyDrop); }
    }
    
    private IEnumerator SetUpEscapeScreen()
    {
        _uC.defeated.text = "You have escaped from " + _eC.enemyUnit.unitName + " in a cloud of smoke.";
        _data.AddToStory(_pC.playerUnit.unitName + " escaped from " + _eC.enemyUnit.unitName + ".");
        if (_data.GetLifeGoal() == 6)
        {
            _data.UpdateGoal(1);
        }
        if (_data.GetIsGoalCompleted())
        {
            _data.AddToStory(_pC.playerUnit.unitName + " completed their life goal of: " + _data.GetGoalText());
            _data.SetPlayerDone(true);
        }
        _uC.gold.text = "You get to live another day.";
        yield return new WaitForSeconds(TIME_TILL_ENEMY_TURN);
        _currentState = BattleStates.End;
        _uC.victoryTab.SetActive(true);
    }


    

}