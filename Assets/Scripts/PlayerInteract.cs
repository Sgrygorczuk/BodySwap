using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    private Data _data;

    private Canvas _overWorldUICavnas;
    private OverworldUI _overworldUI;
    private Canvas _talkUI;
    private OverworldTalk _talkUIScript;
    private Canvas _shopUI;
    private OverworldShopUI _overworldShopUI;
    
    private PlayerMovement _playerMovement;

    private AudioSource _talkAudioSource;
    private AudioSource _fightAudioSource;
    private AudioSource _shopAudioSource;
    private AudioSource _moveAudioSource;
    private AudioSource _selectAudioSource; 
    private AudioSource _denyAudioSource;

    private int _itemIndex = 0;
    public GameObject[] itemArrows = new GameObject[4];
    private int[] itemCosts = new[] { 5, 2, 10 };
    private enum TalkState
    {
        noTalking,
        canTalk,
        isTalking,
        canShop,
        inShop,
    }

    private TalkState _currentTalkState = TalkState.noTalking;
    
    // Start is called before the first frame update
    void Start()
    {
        _data = GameObject.Find("Data").GetComponent<Data>();
        transform.position = _data.GetPosition();
        
        _overWorldUICavnas = GameObject.Find("Overworld_Canvas").GetComponent<Canvas>();
        _overworldUI = GameObject.Find("Overworld_Canvas").GetComponent<OverworldUI>();
        
        _talkUI = GameObject.Find("Talk_Canvas").GetComponent<Canvas>();
        _talkUIScript = GameObject.Find("Talk_Canvas").GetComponent<OverworldTalk>();
        _talkUI.enabled = false;

        _shopUI = GameObject.Find("Shop_Canvas").GetComponent<Canvas>();
        _shopUI.enabled = false;
        _overworldShopUI = GameObject.Find("Shop_Canvas").GetComponent<OverworldShopUI>();

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
        
        _playerMovement = GetComponent<PlayerMovement>();
        
        if (_data._enemyParentPath != "")
        {
            var owUnit = GameObject.Find(_data._enemyParentPath).transform.Find(_data._enemyPath).GetComponent<OWUnit>();
            StartCoroutine(owUnit.Incapacitate());
        }
    }

    private void Update()
    {
        switch (_currentTalkState)
        {
            case TalkState.canTalk:
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _overWorldUICavnas.enabled = false;
                    _talkUI.enabled = true;
                    _currentTalkState = TalkState.isTalking;
                    _playerMovement.UpdateMovementState(false);
                    _talkAudioSource.Play();
                }
                break;
            }
            case TalkState.isTalking:
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _overWorldUICavnas.enabled = true;
                    _talkUI.enabled = false;
                    _currentTalkState = TalkState.canTalk;
                    _playerMovement.UpdateMovementState(true);
                    _talkAudioSource.Play();
                }
                break;
            }
            case TalkState.canShop:
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _overWorldUICavnas.enabled = false;
                    _shopUI.enabled = true;
                    _currentTalkState = TalkState.inShop;
                    _overworldShopUI.UpdateItemUI("x" +_data.GetMoney(), "x" +_data.GetItem(0), "x" +_data.GetItem(1), "x" + _data.GetItem(2));
                    _playerMovement.UpdateMovementState(false);
                    _talkAudioSource.Play();
                }
                break;
            }
            case TalkState.inShop:
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
                        _currentTalkState = TalkState.canShop;
                        _overWorldUICavnas.enabled = true;
                        _shopUI.enabled = false;
                        _itemIndex = 0;
                        _selectAudioSource.Play();
                        UpdateItemArrows();
                        _playerMovement.UpdateMovementState(true);
                        _overworldUI.UpdateItemUI("x" +_data.GetMoney(), "x" +_data.GetItem(0), "x" +_data.GetItem(1), "x" + _data.GetItem(2));
                    }
                    else
                    {
                        if (_data.GetMoney() >= itemCosts[_itemIndex])
                        {
                            _selectAudioSource.Play();
                            _data.SubMoney(itemCosts[_itemIndex]);
                            _data.AddItem(_itemIndex);
                            _overworldShopUI.UpdateItemUI("x" +_data.GetMoney(), "x" +_data.GetItem(0), "x" +_data.GetItem(1), "x" + _data.GetItem(2));
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
            SceneManager.LoadScene("BattleScene");
        }
        //Same and is a monster or human 
        if (col.CompareTag(tag) && ((col.CompareTag("Monster")) || col.CompareTag("Human")))
        {
            var owUnit = col.GetComponent<OWUnit>();
            owUnit.SetIcon(true);
            _talkUIScript.UpdateUI(owUnit.unit.sprite, owUnit.gameObject.name, owUnit.talkingText);
            _overworldShopUI.UpdateTalksUI(owUnit.unit.sprite, owUnit.talkingText);
            //If shopkeeper
            _currentTalkState = owUnit.isShopkeeper ? TalkState.canShop : TalkState.canTalk;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        //Same and is a monster or human 
        if (col.CompareTag(tag) && (col.CompareTag("Monster")) || col.CompareTag("Human"))
        {
            col.GetComponent<OWUnit>().SetIcon(false);
            _currentTalkState = TalkState.noTalking;

        }
    }
    
}
