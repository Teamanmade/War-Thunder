using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public static bool isPlayerSpeedup = false;
    public float Speed_up_time = 6f;
    public bool isPlayerBullet = true; // �Ƿ�Ϊ����ӵ�
    public float lifeTime = 1; // �ӵ����ʱ�䣬��λΪ��

    private float timer; // ��ʱ������

    // Start is called before the first frame update
    void Start()
    {
        timer = lifeTime; // ��ʼ����ʱ��
    }

    // Update is called once per frame
    void Update()
    {
        // �ƶ��ӵ�
        if (isPlayerSpeedup == true && Speed_up_time >= 0)
        {
            lifeTime = 2f;
            Speed_up_time -= Time.deltaTime;
        }
        else if (Speed_up_time < 0)
        {
            lifeTime = 1f;
            isPlayerSpeedup = false;
            Speed_up_time = 6f;
        }
        transform.Translate(transform.up * Time.deltaTime * 10, Space.World);

        // ���¼�ʱ��
        timer -= Time.deltaTime;

        // �����ʱ���ﵽ0�������ӵ�
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == gameObject) return; // �����ӵ���������ײ

        switch (collision.tag)
        {
            case "Tank":
                if (!isPlayerBullet)
                {
                    collision.SendMessage("die");

                }
                Destroy(gameObject);
                // ������̹�˵���ײ
                break;
            case "Heart":
                collision.SendMessage("Break");
                Destroy(gameObject);
                break;
            //case "Enemy":
            //    if (isPlayerBullet)
            //    collision.SendMessage("Enemydie");
            //    Destroy(gameObject);
            //    break;
            case "Wall":
                Destroy(collision.gameObject);
                Destroy(gameObject);
                break;
            case "Barrier":
                Destroy(gameObject);
                break;
            case "river":
                break;
            default:
                break;
        }
    }
}
