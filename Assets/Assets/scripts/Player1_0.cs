using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
//liu
public class Player : MonoBehaviour
{
    //基础数值
    public float moveSpeed = 4;

    //无敌效果
    public bool isDefended = true;
    public float Defentime = 3;

    //双发
    public int double_time = 5;
    public bool double_bullet = false; 

    //加速
    public bool speed_up = false;
    public float speed_time = 3f;

    //时间计时器
    private float timeValue = 0;

    //引用类
    private Vector3 bulletEulerAngles;
    private SpriteRenderer sr;
    public Sprite[] tankSprite; // 上 右 下 左
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject defenEffectPrefab;

    private void Awake()
    {

        sr = GetComponent<SpriteRenderer>(); // 修正拼写错误
    }

    private void attack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && timeValue >= 0.4f)
        {
            //isPlayerbullet = true;
            Vector3 spawnOffset = Vector3.zero;
            if (bulletEulerAngles.z == 0) spawnOffset = Vector3.up * 1.0f;    // 上
            else if (bulletEulerAngles.z == 180) spawnOffset = Vector3.down * 1.0f; // 下
            else if (bulletEulerAngles.z == -90) spawnOffset = Vector3.right * 1.0f; // 右
            else if (bulletEulerAngles.z == 90) spawnOffset = Vector3.left * 1.0f;  // 左
            Instantiate(bulletPrefab, transform.position + spawnOffset, Quaternion.Euler(bulletEulerAngles));
            if (double_bullet == true && double_time > 0)
            {
                if (bulletEulerAngles.z == 0) spawnOffset = Vector3.up * 1.5f;    // 上
                else if (bulletEulerAngles.z == 180) spawnOffset = Vector3.down * 1.5f; // 下
                else if (bulletEulerAngles.z == -90) spawnOffset = Vector3.right * 1.5f; // 右
                else if (bulletEulerAngles.z == 90) spawnOffset = Vector3.left * 1.5f;  // 左
                Instantiate(bulletPrefab, transform.position + spawnOffset, Quaternion.Euler(bulletEulerAngles));
                double_time -= 1;
            }
            else if (double_time <= 0)
            {
                double_bullet = false;
                double_time = 5;
            }
            timeValue = 0; // 重置冷却时间
        }
    }

    void Update()
    {
        //无敌效果
        if (isDefended)
        {
            defenEffectPrefab.SetActive(true);
            Defentime -= Time.deltaTime;
            if (Defentime <= 0)
            {
                isDefended = false;
                defenEffectPrefab.SetActive(false);
            }
        }

        //更新冷却时间（子弹射出后，冷却时间为0.4秒）
        if (timeValue < 0.4f)
        {
            timeValue += Time.deltaTime;
        }

        attack(); // 调用攻击方法

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void FixedUpdate()
    {
        // 获取键盘输入
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 根据输入组合设置 Sprite 和移动方向
        if (verticalInput != 0 && horizontalInput != 0)
        {
            // 斜方向，不射出子弹
            if (verticalInput > 0)
            {
                if (horizontalInput > 0)
                {
                    sr.sprite = tankSprite[4]; // 右上
                    bulletEulerAngles = new Vector3(0, 0, -45);
                }
                else
                {
                    sr.sprite = tankSprite[5]; // 左上
                    bulletEulerAngles = new Vector3(0, 0, 45);
                }
            }
            else
            {
                if (horizontalInput > 0)
                {
                    sr.sprite = tankSprite[7]; // 右下
                    bulletEulerAngles = new Vector3(0, 0, -135);
                }
                else
                {
                    sr.sprite = tankSprite[6]; // 左下
                    bulletEulerAngles = new Vector3(0, 0, 135);
                }
            }
        }
        else if (verticalInput != 0)
        {
            if (verticalInput > 0)
            {
                sr.sprite = tankSprite[0]; // 上
                bulletEulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                sr.sprite = tankSprite[2]; // 下
                bulletEulerAngles = new Vector3(0, 0, 180);
            }
        }
        else if (horizontalInput != 0)
        {
            if (horizontalInput > 0)
            {
                sr.sprite = tankSprite[1]; // 右
                bulletEulerAngles = new Vector3(0, 0, -90);
            }
            else
            {
                sr.sprite = tankSprite[3]; // 左
                bulletEulerAngles = new Vector3(0, 0, 90);
            }
        }

        // 移动逻辑
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0);
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }
        chance();
        transform.Translate(movement * moveSpeed * Time.fixedDeltaTime);
    }
    private void chance()
    { 
        
        if (speed_time >= 0 && speed_up == true)
        {

            moveSpeed = 6f;
            speed_time -= Time.deltaTime;
        }
        else if (speed_time < 0)
        {
            speed_up = false;
            moveSpeed = 4f;
            speed_time = 3f;
        }
    }
    private void die()
    {
        if (isDefended)
            return;
        else
        {
            PlayerManager.isDead = true;
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "PowerUp":
                // 罐车触碰到道具时获得无敌效果，并且道具消失
                isDefended = true;
                Defentime = 3; // 重置无敌时间
                Destroy(collision.gameObject); // 销毁道具
                break;
            case "DoubleShot":
                double_bullet = true;
                Destroy(collision.gameObject);
                break;
            case "Speed":
                speed_up = true;
                bullet.isPlayerSpeedup = true;
                Destroy(collision.gameObject);
                break;
            default:
                break;
        }
    }
}
