using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    //����ֵ
    public int lifeValue = 3;
    public int playerScored = 0;
    public static bool isDead;
    public bool isDefeat;
    //��ҵ÷�
    public int playerScore = 0;

    //����
    public GameObject born;
    public Text playerScoreText;
    public Text playerLifeValueText;
    public Text playerFailedText;
    public Text playerSuccessText;

    //����
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
        playerFailedText.gameObject.SetActive(false); // ��ʼ״̬������ʧ���ı�
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
            //��Ϸʧ�ܣ�����������
            isDefeat = true;
            Time.timeScale = 0; // ��ͣ��Ϸ
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
            //��һ�ʤ
            Debug.Log("��һ�ʤ��");
            ShowPlayerSuccessText();
            Time.timeScale = 0; // ��ͣ��Ϸ
        }
        else
        {
            //�������ֵС�ڵ����㣬�����δ��ʤ
            Debug.Log("���δ��ʤ��");
            ShowPlayerFailedText();
            Time.timeScale = 0; // ��ͣ��Ϸ
        }
    }

    private void ShowPlayerFailedText()
    {
        playerFailedText.gameObject.SetActive(true); // ��ʾʧ���ı�
    }
    private void ShowPlayerSuccessText()
    {
        playerSuccessText.gameObject.SetActive(true); // ��ʾʧ���ı�
    }
}
