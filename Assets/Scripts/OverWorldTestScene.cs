using UnityEngine;
using UnityEngine.SceneManagement;

public class OverWorldTestScene : MonoBehaviour
{
    public int enemyIndex;
    public Unit playerUnit;
    private Data _data;

    // Start is called before the first frame update
    void Start()
    {
        _data = GameObject.Find("Data").GetComponent<Data>();
        enemyIndex = Random.Range(0, 11);
        _data.SetId(enemyIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SceneManager.LoadScene("BattleScene");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            _data.SetUnit(playerUnit);
            _data.SetId(enemyIndex);
        }
    }
}
