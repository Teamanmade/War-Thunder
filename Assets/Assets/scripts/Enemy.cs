using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 4;
    private Vector3 bulletEulerAngles;
    private float timeValue = 0;
    private SpriteRenderer sr;
    public Sprite[] tankSprite; // �� �� �� ��
    public int[] spriteIndex = { 0, 1, 2, 3 }; // 0 �� 1 �� 2 �� 3 ��
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    private Vector3 currentDirection;
    private float directionChangeTime = 0;
    private float directionChangeInterval = 2; // ÿ2��ı�һ�η���
    
    // ���Ѫ������
    public int health = 2;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>(); // ����ƴд����
        currentDirection = RandomMoveDirection(); // ��ʼ��һ���������
    }

    private void attack()
    {
        if (timeValue >= 3)
        {
            Vector3 spawnOffset = Vector3.zero;
            if (bulletEulerAngles.z == 0) spawnOffset = Vector3.up * 1.0f;    // ��
            else if (bulletEulerAngles.z == 180) spawnOffset = Vector3.down * 1.0f; // ��
            else if (bulletEulerAngles.z == -90) spawnOffset = Vector3.right * 1.0f; // ��
            else if (bulletEulerAngles.z == 90) spawnOffset = Vector3.left * 1.0f;  // ��
            Instantiate(bulletPrefab, transform.position + spawnOffset, Quaternion.Euler(bulletEulerAngles));
            timeValue = 0; // ������ȴʱ��
        }
    }

    void Update()
    {
        // ������ȴʱ��
        if (timeValue < 3)
        {
            timeValue += Time.deltaTime;
        }
        attack(); // ���ù�������
        // ���·���ı�ʱ��
        if (directionChangeTime < directionChangeInterval)
        {
            directionChangeTime += Time.deltaTime;
        }
        else
        {
            currentDirection = RandomMoveDirection(); // �ı䷽��
            directionChangeTime = 0; // ���÷���ı�ʱ��
        }
    }

    private void FixedUpdate()
    {
        // �ƶ��߼�
        transform.Translate(currentDirection * moveSpeed * Time.fixedDeltaTime);
        // �����ƶ��������� Sprite ���ƶ�����
        if (currentDirection == Vector3.up)
        {
            sr.sprite = tankSprite[spriteIndex[0]]; // ��
            bulletEulerAngles = new Vector3(0, 0, 0);
        }
        else if (currentDirection == Vector3.down)
        {
            sr.sprite = tankSprite[spriteIndex[2]]; // ��
            bulletEulerAngles = new Vector3(0, 0, 180);
        }
        else if (currentDirection == Vector3.right)
        {
            sr.sprite = tankSprite[spriteIndex[1]]; // ��
            bulletEulerAngles = new Vector3(0, 0, -90);
        }
        else if (currentDirection == Vector3.left)
        {
            sr.sprite = tankSprite[spriteIndex[3]]; // ��
            bulletEulerAngles = new Vector3(0, 0, 90);
        }
    }

    private Vector3 RandomMoveDirection()
    {
        int directionIndex = Random.Range(0, 4);
        switch (directionIndex)
        {
            case 0:
                return Vector3.up; // ��
            case 1:
                return Vector3.right; // ��
            case 2:
                return Vector3.down; // ��
            case 3:
                return Vector3.left; // ��
            default:
                return Vector3.zero;
        }
    }

    private void Enemydie()
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    // �����ײ��ⷽ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet")) // �����ӵ��� "Bullet" ��ǩ
        {
            health--; // ����Ѫ��
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            spriteIndex[0] = 4;
            spriteIndex[1] = 5;
            spriteIndex[2] = 6;
            spriteIndex[3] = 7;
            Destroy(collision.gameObject);
            if (health <= 0) // ���Ѫ��С�ڵ���0
            {
                Enemydie(); // �õ�����ʧ
            }
        }
    }
}
