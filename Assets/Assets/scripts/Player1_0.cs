using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
//liu
public class Player : MonoBehaviour
{
    //������ֵ
    public float moveSpeed = 4;

    //�޵�Ч��
    public bool isDefended = true;
    public float Defentime = 3;

    //˫��
    public int double_time = 5;
    public bool double_bullet = false; 

    //����
    public bool speed_up = false;
    public float speed_time = 3f;

    //ʱ���ʱ��
    private float timeValue = 0;

    //������
    private Vector3 bulletEulerAngles;
    private SpriteRenderer sr;
    public Sprite[] tankSprite; // �� �� �� ��
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject defenEffectPrefab;

    private void Awake()
    {

        sr = GetComponent<SpriteRenderer>(); // ����ƴд����
    }

    private void attack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && timeValue >= 0.4f)
        {
            //isPlayerbullet = true;
            Vector3 spawnOffset = Vector3.zero;
            if (bulletEulerAngles.z == 0) spawnOffset = Vector3.up * 1.0f;    // ��
            else if (bulletEulerAngles.z == 180) spawnOffset = Vector3.down * 1.0f; // ��
            else if (bulletEulerAngles.z == -90) spawnOffset = Vector3.right * 1.0f; // ��
            else if (bulletEulerAngles.z == 90) spawnOffset = Vector3.left * 1.0f;  // ��
            Instantiate(bulletPrefab, transform.position + spawnOffset, Quaternion.Euler(bulletEulerAngles));
            if (double_bullet == true && double_time > 0)
            {
                if (bulletEulerAngles.z == 0) spawnOffset = Vector3.up * 1.5f;    // ��
                else if (bulletEulerAngles.z == 180) spawnOffset = Vector3.down * 1.5f; // ��
                else if (bulletEulerAngles.z == -90) spawnOffset = Vector3.right * 1.5f; // ��
                else if (bulletEulerAngles.z == 90) spawnOffset = Vector3.left * 1.5f;  // ��
                Instantiate(bulletPrefab, transform.position + spawnOffset, Quaternion.Euler(bulletEulerAngles));
                double_time -= 1;
            }
            else if (double_time <= 0)
            {
                double_bullet = false;
                double_time = 5;
            }
            timeValue = 0; // ������ȴʱ��
        }
    }

    void Update()
    {
        //�޵�Ч��
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

        //������ȴʱ�䣨�ӵ��������ȴʱ��Ϊ0.4�룩
        if (timeValue < 0.4f)
        {
            timeValue += Time.deltaTime;
        }

        attack(); // ���ù�������

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void FixedUpdate()
    {
        // ��ȡ��������
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // ��������������� Sprite ���ƶ�����
        if (verticalInput != 0 && horizontalInput != 0)
        {
            // б���򣬲�����ӵ�
            if (verticalInput > 0)
            {
                if (horizontalInput > 0)
                {
                    sr.sprite = tankSprite[4]; // ����
                    bulletEulerAngles = new Vector3(0, 0, -45);
                }
                else
                {
                    sr.sprite = tankSprite[5]; // ����
                    bulletEulerAngles = new Vector3(0, 0, 45);
                }
            }
            else
            {
                if (horizontalInput > 0)
                {
                    sr.sprite = tankSprite[7]; // ����
                    bulletEulerAngles = new Vector3(0, 0, -135);
                }
                else
                {
                    sr.sprite = tankSprite[6]; // ����
                    bulletEulerAngles = new Vector3(0, 0, 135);
                }
            }
        }
        else if (verticalInput != 0)
        {
            if (verticalInput > 0)
            {
                sr.sprite = tankSprite[0]; // ��
                bulletEulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                sr.sprite = tankSprite[2]; // ��
                bulletEulerAngles = new Vector3(0, 0, 180);
            }
        }
        else if (horizontalInput != 0)
        {
            if (horizontalInput > 0)
            {
                sr.sprite = tankSprite[1]; // ��
                bulletEulerAngles = new Vector3(0, 0, -90);
            }
            else
            {
                sr.sprite = tankSprite[3]; // ��
                bulletEulerAngles = new Vector3(0, 0, 90);
            }
        }

        // �ƶ��߼�
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
                // �޳�����������ʱ����޵�Ч�������ҵ�����ʧ
                isDefended = true;
                Defentime = 3; // �����޵�ʱ��
                Destroy(collision.gameObject); // ���ٵ���
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
