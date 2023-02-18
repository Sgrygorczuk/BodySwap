using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    private Data _data;
    // Start is called before the first frame update
    void Start()
    {
        _data = GameObject.Find("Data").GetComponent<Data>();
        transform.position = _data.GetPosition();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        print(col.tag);
        print(tag);
        if (!col.CompareTag(tag))
        {
            print("In");   
            _data.SetId(col.GetComponent<OWUnit>().unit.enemyId);
            _data.SetPosition(transform.position);
            SceneManager.LoadScene("BattleScene");
        }
    }
}
