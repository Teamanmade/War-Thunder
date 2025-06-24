using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public static bool isPlayerSpeedup = false;
    public float Speed_up_time = 6f;
    public bool isPlayerBullet = true; // 是否为玩家子弹
    public float lifeTime = 1; // 子弹存活时间，单位为秒

    private float timer; // 计时器变量

    // Start is called before the first frame update
    void Start()
    {
        timer = lifeTime; // 初始化计时器
    }

    // Update is called once per frame
    void Update()
    {
        // 移动子弹
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

        // 更新计时器
        timer -= Time.deltaTime;

        // 如果计时器达到0，销毁子弹
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == gameObject) return; // 避免子弹与自身碰撞

        switch (collision.tag)
        {
            case "Tank":
                if (!isPlayerBullet)
                {
                    collision.SendMessage("die");

                }
                Destroy(gameObject);
                // 忽略与坦克的碰撞
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
