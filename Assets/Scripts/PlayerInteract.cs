using System.Collections;
using Base;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    private Data _data;

    private Canvas _overWorldUICanvas;
    private OverworldUI _overWorldUI;
    private Canvas _talkUI;
    private OverworldTalk _talkUIScript;
    private Canvas _shopUI;
    private OverworldShopUI _overWorldShopUI;
    
    private GameObject _winBox;
    private GameObject _blackBox;
    private TextMeshProUGUI _endText;

    private PlayerMovement _playerMovement;

    private AudioSource _talkAudioSource;
    private AudioSource _fightAudioSource;
    private AudioSource _shopAudioSource;
    private AudioSource _moveAudioSource;
    private AudioSource _selectAudioSource;
    private AudioSource _denyAudioSource;
    
    private int _itemIndex;
    public GameObject[] itemArrows = new GameObject[4];
    private readonly int[] _itemCosts = { 5, 2, 10 };

    private enum TalkState
    {
        NoTalking,
        CanTalk,
        IsTalking,
        CanShop,
        InShop,
        PopUp,
    }

    private TalkState _currentTalkState = TalkState.NoTalking;
    
    private Animator _animator;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _animator = GameObject.Find("Transition").GetComponent<Animator>();
        
        StartCoroutine(AwakeMove());

    }

    private IEnumerator AwakeMove()
    {
        _playerMovement.UpdateMovementState(false);
        _animator.Play("OverWorldStart");
        yield return new WaitForSeconds(1.5f);
        _playerMovement.UpdateMovementState(true);
    }

    // Start is called before the first frame update
    private void Start()
    {
        _data = GameObject.Find("Data").GetComponent<Data>();
        transform.position = _data.GetPosition();

        _overWorldUICanvas = GameObject.Find("OverWorld_Canvas").GetComponent<Canvas>();
        _overWorldUI = GameObject.Find("OverWorld_Canvas").GetComponent<OverworldUI>();

        _talkUI = GameObject.Find("Talk_Canvas").GetComponent<Canvas>();
        _talkUIScript = GameObject.Find("Talk_Canvas").GetComponent<OverworldTalk>();
        _talkUI.enabled = false;

        _shopUI = GameObject.Find("Shop_Canvas").GetComponent<Canvas>();
        _shopUI.enabled = false;
        _overWorldShopUI = GameObject.Find("Shop_Canvas").GetComponent<OverworldShopUI>();

        _talkAudioSource = GameObject.Find("SFX").transform.Find("Talk").GetComponent<AudioSource>();
        _fightAudioSource = GameObject.Find("SFX").transform.Find("Fight").GetComponent<AudioSource>();
        _moveAudioSource = GameObject.Find("SFX").transform.Find("MovingSFX").GetComponent<AudioSource>();
        _selectAudioSource = GameObject.Find("SFX").transform.Find("SelectSFX").GetComponent<AudioSource>();
        _denyAudioSource = GameObject.Find("SFX").transform.Find("DenySFX").GetComponent<AudioSource>();

        for (int i = 0; i < itemArrows.Length; i++)
        {
            var path = "Point_" + i;
            var arrow = GameObject.Find("Shop_Canvas").transform.Find("Shop").Find(path).gameObject;
            itemArrows[i] = arrow;
        }

        if (_data.enemyParentPath != "")
        {
            var owUnit = GameObject.Find(_data.enemyParentPath).transform.Find(_data.enemyPath)
                .GetComponent<OWUnit>();
            StartCoroutine(owUnit.Incapacitate());
        }

        _winBox = GameObject.Find("OverWorld_Canvas").transform.Find("GameWon").gameObject;
        _winBox.SetActive(false);
        
        _blackBox = GameObject.Find("OverWorld_Canvas").transform.Find("GameWonBlack").gameObject;
        _blackBox.SetActive(false);
        _endText = GameObject.Find("OverWorld_Canvas").transform.Find("GameWonBlack").transform.Find("EndText")
            .GetComponent<TextMeshProUGUI>();
        
        if (_data.GetIsGoalCompleted())
        {
            StartCoroutine(WonGame());
        }
    }

    IEnumerator WonGame()
    {
        _playerMovement.canMove = false;
        _winBox.SetActive(true);
        yield return new WaitForSeconds(5f);
        _winBox.SetActive(false);
        _endText.text = _data.GetStory();
        _blackBox.SetActive(true);
        yield return new WaitForSeconds(30f);
        SceneManager.LoadScene("MainMenu");
    }

    private void Update()
    {
        switch (_currentTalkState)
        {
            case TalkState.CanTalk:
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _overWorldUICanvas.enabled = false;
                    _talkUI.enabled = true;
                    _currentTalkState = TalkState.IsTalking;
                    _playerMovement.UpdateMovementState(false);
                    _talkAudioSource.Play();
                }
                break;
            }
            case TalkState.IsTalking:
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _overWorldUICanvas.enabled = true;
                    _talkUI.enabled = false;
                    _currentTalkState = TalkState.CanTalk;
                    _playerMovement.UpdateMovementState(true);
                    _talkAudioSource.Play();
                }
                break;
            }
            case TalkState.CanShop:
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _overWorldUICanvas.enabled = false;
                    _shopUI.enabled = true;
                    _currentTalkState = TalkState.InShop;
                    _overWorldShopUI.UpdateItemUI("x" +_data.GetMoney(), "x" +_data.GetItem(0), "x" +_data.GetItem(1), "x" + _data.GetItem(2));
                    _playerMovement.UpdateMovementState(false);
                    _talkAudioSource.Play();
                }
                break;
            }
            case TalkState.InShop:
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
                else if (Input.GetKeyDown(KeyCode.Space))
                {
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
                break;
            }
        }
    }
    
    private void UpdateItemArrows()
    {
        foreach (var arrow in itemArrows) { arrow.SetActive(false); }
        itemArrows[_itemIndex].SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Not the same but monster or human 
        if (!col.CompareTag(tag) && ((col.CompareTag("Monster")) || col.CompareTag("Human")))
        {
            _data.SetId(col.GetComponent<OWUnit>().unit.enemyId);
            _data.SetPosition(transform.position);
            _data.SetEnemy(col.transform.parent.name, col.name);
            _fightAudioSource.Play();
            StartCoroutine(LoadToBattle());
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

    private IEnumerator LoadToBattle()
    {
        _playerMovement.UpdateMovementState(false);
        _animator.Play("OverWorldEnd");
        yield return new WaitForSeconds(2.1f);
        SceneManager.LoadScene("BattleScene");
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        //Same and is a monster or human 
        if ((!col.CompareTag(tag) || (!col.CompareTag("Monster"))) && !col.CompareTag("Human")) return;
        col.GetComponent<OWUnit>().SetIcon(false);
        _currentTalkState = TalkState.NoTalking;
    }
    
}
