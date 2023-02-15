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
    private bool _baseState = true;
    private int _baseIndex = 0;       //Which base option is the player looking at 
    private int _attackIndex = 0;     //Which attack is the player looking at
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
        _data = GameObject.Find("Data").GetComponent<Data>();
        _unitsAndAttacksScript = GameObject.Find("BattleScript").GetComponent<UnitsAndAttacks>();
        
        _pC = GameObject.Find("BattleScript").GetComponent<PlayerComponents>();
        _pC.SetUpPlayer(_data.GetUnit());
        
        _eC = GameObject.Find("BattleScript").GetComponent<EnemyComponents>();
        _eC.SetUpEnemy(_unitsAndAttacksScript, _data.GetId());

        _uC = GameObject.Find("BattleScript").GetComponent<UIComponent>();
        _uC.SetUpUI();

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
                    SceneManager.LoadScene("SampleScene");
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
            if (_baseIndex < 0) { _baseIndex = _uC.baseArrows.Length - 1; }
            UpdateBaseArrows();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _baseIndex++;
            if (_baseIndex == _uC.baseArrows.Length) { _baseIndex = 0; }
            UpdateBaseArrows();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (_baseIndex)
            {
                case 0:
                {
                    _baseState = false;
                    _uC.attackTab.SetActive(true);
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
        foreach (var arrow in _uC.baseArrows) { arrow.SetActive(false); }
        _uC.baseArrows[_baseIndex].SetActive(true);
    }

    private void PlayerAttackActions()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _attackIndex--;
            if (_attackIndex < 0) { _attackIndex = _uC.attackArrows.Length - 1; }
            UpdateAttackArrows();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            _attackIndex++;
            if (_attackIndex == (_uC.attackArrows).Length) { _attackIndex = 0; }
            UpdateAttackArrows();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_attackIndex < _pC.maxPlayerAttackIndex && _pC.playerCurrentMana >= _pC.playerUnit.attacks[_attackIndex].manaCost){
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
            _uC.attackTab.SetActive(false);
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
        SpawnDamage(_eC.enemyDamageNumberSpawnPoint, _pC.playerUnit.attacks[_attackIndex].damage.ToString());
        var time = TIME_BAR_LOWERING * (_pC.playerUnit.attacks[_attackIndex].damage + 1);
        _currentState = BattleStates.EnemyTurn;
        _baseState = true;
        _uC.attackTab.SetActive(false);
        StartCoroutine(LowerMagic());
        StartCoroutine(LowerEnemyHealth());
        PlayerLoseControls();
        yield return new WaitForSeconds(time);
        if (_eC.enemyCurrentHealth <= 0)
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
        _uC.baseArrows[0].SetActive(false);
        
        _attackIndex = 0;
        UpdateAttackArrows();

        foreach (var text in _uC.baseText) { text.color = Color.gray; }
    }
    
    private IEnumerator LowerMagic()
    {
        var goal = _pC.playerCurrentMana - _pC.playerUnit.attacks[_attackIndex].manaCost;
        _pC.playerManaText.text = goal + "/" + _pC.playerUnit.mana;
        for (int i = _pC.playerCurrentMana; i > goal; i--)
        {
            _pC.playerCurrentMana--;
            _pC.playerManaImage.fillAmount = (float) _pC.playerCurrentMana / _pC.playerUnit.mana;
            yield return new WaitForSeconds(TIME_BAR_LOWERING);
        }
    }
    
    private IEnumerator LowerEnemyHealth()
    {
        //TODO make a better damage system based on physical vs magic 
        var goal = _eC.enemyCurrentHealth - _pC.playerUnit.attacks[_attackIndex].damage;
        for (int i = _eC.enemyCurrentHealth; i > goal; i--)
        {
            _eC.enemyCurrentHealth--;
            _eC.enemyHealthImage.fillAmount = (float) _eC.enemyCurrentHealth / _eC.enemyUnit.health;
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
    //Enemy Actions 
    //==================================================================================================================

    private IEnumerator EnemyAction()
    {
        var rollAttack = Random.Range(0, _eC.maxEnemyAttackIndex);
        var time = TIME_BAR_LOWERING * (_eC.enemyUnit.attacks[rollAttack].damage + 1);
        yield return new WaitForSeconds(TIME_TILL_ENEMY_TURN);
        StartCoroutine(LowerPlayerHealth(rollAttack));
        _battleAnimator.Play("Shake");
        SpawnDamage(_pC.playerDamageNumberSpawnPoint, _eC.enemyUnit.attacks[rollAttack].damage.ToString());
        yield return new WaitForSeconds(time);
        if (_pC.playerCurrentHealth <= 0)
        {
            StartCoroutine(LoseAction());
            yield break;
        }
        PlayerRegainControls();
        _currentState = BattleStates.PlayerTurn;

    }

    private IEnumerator LowerPlayerHealth(int rollAttack)
    {
        var goal = _pC.playerCurrentHealth - _eC.enemyUnit.attacks[rollAttack].damage;
        _pC.playerHealthText.text = goal + "/" + _pC.playerUnit.health;
        for (int i = _pC.playerCurrentHealth; i > goal; i--)
        {
            _pC.playerCurrentHealth--;
            _pC.playerHealthImage.fillAmount = (float) _pC.playerCurrentHealth / _pC.playerUnit.health;
            yield return new WaitForSeconds(TIME_BAR_LOWERING);
        }
    }

    private void PlayerRegainControls()
    {
        _uC.baseArrows[0].SetActive(true);
        foreach (var text in _uC.baseText) { text.color = Color.black; }
    }
    
    //==================================================================================================================
    //Win Actions 
    //==================================================================================================================

    private IEnumerator WinAction()
    {
        _battleAnimator.Play("EnemyFade"); 
        yield return new WaitForSeconds(TIME_TILL_ENEMY_TURN * 2);
        SetUpVictoryScreen(true);
        yield return new WaitForSeconds(TIME_TILL_ENEMY_TURN);
        _currentState = BattleStates.End;
    }
    
    //==================================================================================================================
    //Lose Actions 
    //==================================================================================================================

    private IEnumerator LoseAction()
    {
        _battleAnimator.Play("Swap");
        yield return new WaitForSeconds(0.25f);
        _eC.enemyImage.sprite = _pC.playerUnit.sprite;
        _pC.playerIcon.sprite = _eC.enemyUnit.sprite;
        UpdatePlayerData();
        SwapData();
        yield return new WaitForSeconds(TIME_TILL_ENEMY_TURN);
        _battleAnimator.Play("EnemyFade"); 
        SetUpVictoryScreen(false);
        yield return new WaitForSeconds(TIME_TILL_ENEMY_TURN);
        _currentState = BattleStates.End;
    }
    
    private void UpdatePlayerData()
    {
        _pC.playerNameText.text = _eC.enemyUnit.name;
        _pC.playerHealthImage.fillAmount = (float) _eC.enemyCurrentHealth / _eC.enemyUnit.health;
        _eC.enemyHealthImage.fillAmount = (float) _pC.playerCurrentHealth / _pC.playerUnit.health;
        _pC.playerManaImage.fillAmount =  (float) _eC.enemyUnit.mana / _eC.enemyUnit.mana;
        _pC.playerHealthText.text = _eC.enemyCurrentHealth + "/" + _eC.enemyUnit.health;
        _pC.playerManaText.text = _eC.enemyUnit.mana+ "/" + _eC.enemyUnit.mana;

    }
    
    private void SwapData()
    {
        _data.SetUnit(_unitsAndAttacksScript.unitsStats[_data.GetId()]);
        _data.UpdateUnit(_eC.enemyUnit);
    }

    private void SetUpVictoryScreen(bool playerWon)
    {
        if (playerWon)
        {
            _uC.defeated.text = "You have defeated " + _eC.enemyUnit.name + " may they rest in peace.";
            _uC.gold.text = "You gather " +  _eC.enemyUnit.moneyDrop + " gold pieces.";
        }
        else
        {
            _uC.defeated.text = "You have defeated " + _pC.playerUnit.name + " may they rest in peace.";
            _uC.gold.text = "You gather " +  _pC.playerUnit.moneyDrop + " gold pieces.";   
        }
        _uC.victoryTab.SetActive(true);
    }


    

}