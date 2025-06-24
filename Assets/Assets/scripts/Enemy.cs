using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 4;
    private Vector3 bulletEulerAngles;
    private float timeValue = 0;
    private SpriteRenderer sr;
    public Sprite[] tankSprite; // 上 右 下 左
    public int[] spriteIndex = { 0, 1, 2, 3 }; // 0 上 1 右 2 下 3 左
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    private Vector3 currentDirection;
    private float directionChangeTime = 0;
    private float directionChangeInterval = 2; // 每2秒改变一次方向
    
    // 添加血量变量
    public int health = 2;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>(); // 修正拼写错误
        currentDirection = RandomMoveDirection(); // 初始化一个随机方向
    }

    private void attack()
    {
        if (timeValue >= 3)
        {
            Vector3 spawnOffset = Vector3.zero;
            if (bulletEulerAngles.z == 0) spawnOffset = Vector3.up * 1.0f;    // 上
            else if (bulletEulerAngles.z == 180) spawnOffset = Vector3.down * 1.0f; // 下
            else if (bulletEulerAngles.z == -90) spawnOffset = Vector3.right * 1.0f; // 右
            else if (bulletEulerAngles.z == 90) spawnOffset = Vector3.left * 1.0f;  // 左
            Instantiate(bulletPrefab, transform.position + spawnOffset, Quaternion.Euler(bulletEulerAngles));
            timeValue = 0; // 重置冷却时间
        }
    }

    void Update()
    {
        // 更新冷却时间
        if (timeValue < 3)
        {
            timeValue += Time.deltaTime;
        }
        attack(); // 调用攻击方法
        // 更新方向改变时间
        if (directionChangeTime < directionChangeInterval)
        {
            directionChangeTime += Time.deltaTime;
        }
        else
        {
            currentDirection = RandomMoveDirection(); // 改变方向
            directionChangeTime = 0; // 重置方向改变时间
        }
    }

    private void FixedUpdate()
    {
        // 移动逻辑
        transform.Translate(currentDirection * moveSpeed * Time.fixedDeltaTime);
        // 根据移动方向设置 Sprite 和移动方向
        if (currentDirection == Vector3.up)
        {
            sr.sprite = tankSprite[spriteIndex[0]]; // 上
            bulletEulerAngles = new Vector3(0, 0, 0);
        }
        else if (currentDirection == Vector3.down)
        {
            sr.sprite = tankSprite[spriteIndex[2]]; // 下
            bulletEulerAngles = new Vector3(0, 0, 180);
        }
        else if (currentDirection == Vector3.right)
        {
            sr.sprite = tankSprite[spriteIndex[1]]; // 右
            bulletEulerAngles = new Vector3(0, 0, -90);
        }
        else if (currentDirection == Vector3.left)
        {
            sr.sprite = tankSprite[spriteIndex[3]]; // 左
            bulletEulerAngles = new Vector3(0, 0, 90);
        }
    }

    private Vector3 RandomMoveDirection()
    {
        int directionIndex = Random.Range(0, 4);
        switch (directionIndex)
        {
            case 0:
                return Vector3.up; // 上
            case 1:
                return Vector3.right; // 右
            case 2:
                return Vector3.down; // 下
            case 3:
                return Vector3.left; // 左
            default:
                return Vector3.zero;
        }
    }

    private void Enemydie()
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    // 添加碰撞检测方法
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet")) // 假设子弹有 "Bullet" 标签
        {
            health--; // 减少血量
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            spriteIndex[0] = 4;
            spriteIndex[1] = 5;
            spriteIndex[2] = 6;
            spriteIndex[3] = 7;
            Destroy(collision.gameObject);
            if (health <= 0) // 如果血量小于等于0
            {
                Enemydie(); // 让敌人消失
            }
        }
    }
}
