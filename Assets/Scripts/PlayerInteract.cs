using System;
using System.Collections;
using Base;
using OverWorldUI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    //=========== Data 
    private Data _data;

    //============== UI General Connections 
    private Canvas _overWorldUICanvas;
    private OverWorldUI.OverWorldUI _overWorldUI;
    private Canvas _talkUI;
    private OverWorldTalk _talkUIScript;
    private Canvas _shopUI;
    private OverWorldShopUI _overWorldShopUI;
    private GameObject _popUp;
    private TextMeshProUGUI _popTextTitle;
    private TextMeshProUGUI _popTextBody;
    
    //========= Player Controls 
    private PlayerMovement _playerMovement;

    //=========== SFX 
    private AudioSource _talkAudioSource;
    private AudioSource _fightAudioSource;
    private AudioSource _shopAudioSource;
    private AudioSource _moveAudioSource;
    private AudioSource _selectAudioSource;
    private AudioSource _denyAudioSource;
    
    //============= Items 
    private int _itemIndex;
    public GameObject[] itemArrows = new GameObject[4];
    private readonly int[] _itemCosts = { 5, 2, 10 };

    //=========== State Machine 
    private enum TalkState
    {
        NoTalking,
        CanTalk,
        IsTalking,
        CanShop,
        InShop,
        PopUp,
        Won,
    }

    private TalkState _currentTalkState = TalkState.PopUp;

    //======= Transition Animator 
    private Animator _animator;
    
    //==================================================================================================================
    // Awake Functions 
    //==================================================================================================================
    
    /// <summary>
    /// Sets up the 
    /// </summary>
    private void Awake()
    {
        //Connects the data 
        _data = GameObject.Find("Data").GetComponent<Data>();
        
        //Connects Player Movement and Transition Animator 
        _playerMovement = GetComponent<PlayerMovement>();
        _animator = GameObject.Find("Transition").GetComponent<Animator>();

        //Connects Pop Up 
        _popUp = GameObject.Find("OverWorld_Canvas").transform.Find("PopUp").gameObject;
        _popTextTitle = GameObject.Find("OverWorld_Canvas").transform.Find("PopUp").transform.Find("Title").GetComponent<TextMeshProUGUI>();
        _popTextBody = GameObject.Find("OverWorld_Canvas").transform.Find("PopUp").transform.Find("Body").GetComponent<TextMeshProUGUI>();

        AwakePopUp();
        StartCoroutine(AwakeMove());
    }

    /// <summary>
    /// Makes player wait till fade is done till they can move, 
    /// </summary>
    /// <returns></returns>
    private IEnumerator AwakeMove()
    {
        _playerMovement.UpdateMovementState(false);
        _animator.Play("OverWorldStart");
        yield return new WaitForSeconds(1.5f);
        if (_data.isIntroPopUpDone)
        {
            _playerMovement.UpdateMovementState(true);
        }
    }

    /// <summary>
    /// Checks if there is the first time loading into over world, if it is make the pop up come up else give
    /// controls to player to walk around 
    /// </summary>
    private void AwakePopUp()
    {
        if (!_data.isIntroPopUpDone)
        {
            _currentTalkState = TalkState.PopUp;
            _popUp.SetActive(true);
            _popTextTitle.text = "Welcome to Footnotes";
            _popTextBody.text = "You have just arrived in a land unknown, a new story ready to unfold. " +
                                "Will you become a footnote or will you write the story? \n \n [Use WASD to move and" +
                                " Space to interact]";
            _data.isIntroPopUpDone = true;
        }
        else
        {
            _popUp.SetActive(false);
            _currentTalkState = TalkState.NoTalking;
            _playerMovement.UpdateMovementState(true);
        }
    }
    
    //==================================================================================================================
    // Base Functions 
    //==================================================================================================================

    /// <summary>
    /// Connects all the 
    /// </summary>
    private void Start()
    {
        //Put player where they last were 
        transform.position = _data.GetPosition();
        
        //Connects Game Objects 
        SetUpSfx();
        SetUpUI();
        //Makes enemy NPC disappear  
        IncapacitateNpc();
        //Checks if player won, starts the exit sequence 
        WonGame();
    }

    /// <summary>
    /// Connects all of the SFX in the game 
    /// </summary>
    private void SetUpSfx()
    {
        _talkAudioSource = GameObject.Find("SFX").transform.Find("Talk").GetComponent<AudioSource>();
        _fightAudioSource = GameObject.Find("SFX").transform.Find("Fight").GetComponent<AudioSource>();
        _moveAudioSource = GameObject.Find("SFX").transform.Find("MovingSFX").GetComponent<AudioSource>();
        _selectAudioSource = GameObject.Find("SFX").transform.Find("SelectSFX").GetComponent<AudioSource>();
        _denyAudioSource = GameObject.Find("SFX").transform.Find("DenySFX").GetComponent<AudioSource>();
    }

    /// <summary>
    /// Connects all the UI elements in the over world 
    /// </summary>
    private void SetUpUI()
    {
        _overWorldUICanvas = GameObject.Find("OverWorld_Canvas").GetComponent<Canvas>();
        _overWorldUI = GameObject.Find("OverWorld_Canvas").GetComponent<OverWorldUI.OverWorldUI>();

        _talkUI = GameObject.Find("Talk_Canvas").GetComponent<Canvas>();
        _talkUIScript = GameObject.Find("Talk_Canvas").GetComponent<OverWorldTalk>();
        _talkUI.enabled = false;

        _shopUI = GameObject.Find("Shop_Canvas").GetComponent<Canvas>();
        _shopUI.enabled = false;
        _overWorldShopUI = GameObject.Find("Shop_Canvas").GetComponent<OverWorldShopUI>();
        

        //Connects arrows for the shop 
        for (var i = 0; i < itemArrows.Length; i++)
        {
            var path = "Point_" + i;
            var arrow = GameObject.Find("Shop_Canvas").transform.Find("Shop").Find(path).gameObject;
            itemArrows[i] = arrow;
        }
    }

    /// <summary>
    /// Turns off the NPC that the player fought in last battle 
    /// </summary>
    private void IncapacitateNpc()
    {
        if (_data.enemyParentPath == "") return;
        var owUnit = GameObject.Find(_data.enemyParentPath).transform.Find(_data.enemyPath)
            .GetComponent<OWUnit>();
        StartCoroutine(owUnit.Incapacitate());
    }

    /// <summary>
    /// Sets up the pop up and allows player to exit out of the game to last cut scene 
    /// </summary>
    private void WonGame()
    {
        if (!_data.GetIsGoalCompleted()) return;
        _playerMovement.canMove = false;
        _popUp.SetActive(true);
        _popTextTitle.text = "You've achieved your life goal!";
        _popTextBody.text = "";
        _currentTalkState = TalkState.Won;
    }

    //==================================================================================================================
    // State Machine Methods 
    //==================================================================================================================

    /// <summary>
    /// Controls what the player can do while playing 
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private void Update()
    {
        switch (_currentTalkState)
        {
            case TalkState.CanTalk:
            {
                CanTalk();
                break;
            }
            case TalkState.IsTalking:
            {
                IsTalking();
                break;
            }
            case TalkState.CanShop:
            {
                CanShop();
                break;
            }
            case TalkState.InShop:
            {
                UpdateShopMenu();
                InShop();
                break;
            }
            case TalkState.PopUp:
            {
                PopUp();
                break;
            }
            case TalkState.Won:
            {
                Win();
                break;
            }
            case TalkState.NoTalking:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// Player is near an NPC and can start a conversation with them 
    /// </summary>
    private void CanTalk()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        _overWorldUICanvas.enabled = false;
        _talkUI.enabled = true;
        _currentTalkState = TalkState.IsTalking;
        _playerMovement.UpdateMovementState(false);
        _talkAudioSource.Play();
    }

    /// <summary>
    ///Player is in a conversation with a NPC and can leave to regular actions 
    /// </summary>
    private void IsTalking()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        _overWorldUICanvas.enabled = true;
        _talkUI.enabled = false;
        _currentTalkState = TalkState.CanTalk;
        _playerMovement.UpdateMovementState(true);
        _talkAudioSource.Play();
    }

    /// <summary>
    /// Player is near a Shop Keeper and can open up the Shop Menus 
    /// </summary>
    private void CanShop()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        _overWorldUICanvas.enabled = false;
        _shopUI.enabled = true;
        _currentTalkState = TalkState.InShop;
        _overWorldShopUI.UpdateItemUI("x" +_data.GetMoney(), "x" +_data.GetItem(0), "x" +_data.GetItem(1), "x" + _data.GetItem(2));
        _playerMovement.UpdateMovementState(false);
        _talkAudioSource.Play();
    }

    /// <summary>
    /// Player is in the shop they can navigate up and down the menu 
    /// </summary>
    private void UpdateShopMenu()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _itemIndex--;
            if (_itemIndex < 0) { _itemIndex =itemArrows.Length - 1; }
            _moveAudioSource.Play();
            UpdateItemArrows();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            _itemIndex++;
            if (_itemIndex == (itemArrows).Length) { _itemIndex = 0; }
            _moveAudioSource.Play();
            UpdateItemArrows();
        }
    }

    /// <summary>
    /// Player is in the shop they can try to purchase items or go back to regular actions 
    /// </summary>
    private void InShop()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        //Player is in the last option and goes back to regular actions 
        if (_itemIndex == (itemArrows).Length - 1)
        {
            _currentTalkState = TalkState.CanShop;
            _overWorldUICanvas.enabled = true;
            _shopUI.enabled = false;
            _itemIndex = 0;
            _selectAudioSource.Play();
            UpdateItemArrows();
            _playerMovement.UpdateMovementState(true);
            _overWorldUI.UpdateItemUI("x" +_data.GetMoney(), "x" +_data.GetItem(0), "x" +_data.GetItem(1), "x" + _data.GetItem(2));
        }
        //Player tries to purchase item
        else
        {
            if (_data.GetMoney() >= _itemCosts[_itemIndex])
            {
                _selectAudioSource.Play();
                _data.SubMoney(_itemCosts[_itemIndex]);
                _data.AddItem(_itemIndex);
                _overWorldShopUI.UpdateItemUI("x" +_data.GetMoney(), "x" +_data.GetItem(0), "x" +_data.GetItem(1), "x" + _data.GetItem(2));
            }
            else
            {
                _denyAudioSource.Play();
            }
        }
    }
    
    /// <summary>
    /// Updates the visual elements of where the player is currently looking 
    /// </summary>
    private void UpdateItemArrows()
    {
        foreach (var arrow in itemArrows) { arrow.SetActive(false); }
        itemArrows[_itemIndex].SetActive(true);
    }

    /// <summary>
    /// Shows the intro pop up 
    /// </summary>
    private void PopUp()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        _popUp.SetActive(false);
        _playerMovement.UpdateMovementState(true);
        _currentTalkState = TalkState.NoTalking;
    }

    /// <summary>
    /// Waits for player to end the game 
    /// </summary>
    private void Win()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        StartCoroutine(LoadLevel("CutSceneEnd"));
    }
    
    //==================================================================================================================
    // Trigger Methods  
    //==================================================================================================================


    /// <summary>
    /// Checks if hte player has walked up to an NPC 
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter2D(Collider2D col)
    {
        //Not the same but monster or human 
        if (!col.CompareTag(tag) && ((col.CompareTag("Monster")) || col.CompareTag("Human")))
        {
            _data.SetId(col.GetComponent<OWUnit>().unit.enemyId);
            _data.SetPosition(transform.position);
            _data.SetEnemy(col.transform.parent.name, col.name);
            _fightAudioSource.Play();
            StartCoroutine(LoadLevel("BattleScene"));
        }
        //Same and is a monster or human 
        if (col.CompareTag(tag) && ((col.CompareTag("Monster")) || col.CompareTag("Human")))
        {
            var owUnit = col.GetComponent<OWUnit>();
            owUnit.SetIcon(true);
            _talkUIScript.UpdateUI(owUnit.unit.sprite, owUnit.gameObject.name, owUnit.talkingText);
            _overWorldShopUI.UpdateTalksUI(owUnit.unit.sprite, owUnit.talkingText);
            //If shopkeeper
            _currentTalkState = owUnit.isShopkeeper ? TalkState.CanShop : TalkState.CanTalk;
        }
    }

    /// <summary>
    /// Starts the exit animation and loads player to 
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadLevel(string levelName)
    {
        _playerMovement.UpdateMovementState(false);
        _animator.Play("OverWorldEnd");
        yield return new WaitForSeconds(2.1f);
        SceneManager.LoadScene(levelName);
    }

    /// <summary>
    /// Checks if the player has walked away from the NPC
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerExit2D(Collider2D col)
    {
        //Same and is a monster or human 
        if ((!col.CompareTag(tag) || (!col.CompareTag("Monster"))) && !col.CompareTag("Human")) return;
        col.GetComponent<OWUnit>().SetIcon(false);
        _currentTalkState = TalkState.NoTalking;
    }
    
}
