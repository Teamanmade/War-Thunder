using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    //属性值
    public int lifeValue = 3;
    public int playerScored = 0;
    public static bool isDead;
    public bool isDefeat;
    //玩家得分
    public int playerScore = 0;

    //引用
    public GameObject born;
    public Text playerScoreText;
    public Text playerLifeValueText;
    public Text playerFailedText;
    public Text playerSuccessText;

    //单例
    private static PlayerManager instance;

    public static PlayerManager Instance
    {
        get
        {
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    public void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckWinAfterTime(10f));
        playerFailedText.gameObject.SetActive(false); // 初始状态下隐藏失败文本
        playerSuccessText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            Recover();
        }
        playerScoreText.text = playerScore.ToString();
        playerLifeValueText.text = lifeValue.ToString();
    }

    private void Recover()
    {
        if (lifeValue <= 0)
        {
            //游戏失败，返回主界面
            isDefeat = true;
            Time.timeScale = 0; // 暂停游戏
        }
        else
        {
            lifeValue--;
            GameObject go = Instantiate(born, new Vector3(-2, -8, 0), Quaternion.identity);
            go.GetComponent<Born>().isPlayer = true;
            isDead = false;
        }
    }

    IEnumerator CheckWinAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (lifeValue > 0)
        {
            //玩家获胜
            Debug.Log("玩家获胜！");
            ShowPlayerSuccessText();
            Time.timeScale = 0; // 暂停游戏
        }
        else
        {
            //如果生命值小于等于零，则玩家未获胜
            Debug.Log("玩家未获胜！");
            ShowPlayerFailedText();
            Time.timeScale = 0; // 暂停游戏
        }
    }

    private void ShowPlayerFailedText()
    {
        playerFailedText.gameObject.SetActive(true); // 显示失败文本
    }
    private void ShowPlayerSuccessText()
    {
        playerSuccessText.gameObject.SetActive(true); // 显示失败文本
    }
}
