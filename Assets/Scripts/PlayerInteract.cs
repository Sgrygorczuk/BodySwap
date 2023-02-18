using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    private Data _data;

    private Canvas _overWorldUI;
    private Canvas _talkUI;
    private OverworldTalk _talkUIScript;

    private GameObject _shopUI;
    private PlayerMovement _playerMovement;

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
        
        _overWorldUI = GameObject.Find("Overworld_Canvas").GetComponent<Canvas>();
        _talkUI = GameObject.Find("Talk_Canvas").GetComponent<Canvas>();
        _talkUIScript = GameObject.Find("Talk_Canvas").GetComponent<OverworldTalk>();
        _talkUI.enabled = false;

        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        switch (_currentTalkState)
        {
            case TalkState.canTalk:
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _overWorldUI.enabled = false;
                    _talkUI.enabled = true;
                    _currentTalkState = TalkState.isTalking;
                    _playerMovement.UpdateMovementState(false);
                }
                break;
            }
            case TalkState.isTalking:
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _overWorldUI.enabled = true;
                    _talkUI.enabled = false;
                    _currentTalkState = TalkState.canTalk;
                    _playerMovement.UpdateMovementState(true);
                }
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Not the same but monster or human 
        if (!col.CompareTag(tag) && (col.CompareTag("Monster")) || col.CompareTag("Human"))
        {
            _data.SetId(col.GetComponent<OWUnit>().unit.enemyId);
            _data.SetPosition(transform.position);
            SceneManager.LoadScene("BattleScene");
        }
        //Same and is a monster or human 
        if (col.CompareTag(tag) && (col.CompareTag("Monster")) || col.CompareTag("Human"))
        {
            var owUnit = col.GetComponent<OWUnit>();
            owUnit.SetIcon(true);
            _talkUIScript.UpdateUI(owUnit.unit.sprite, owUnit.gameObject.name, owUnit.talkingText);
            //If shopkeeper
            if(false){}
            else
            {
                _currentTalkState = TalkState.canTalk;
            }
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
