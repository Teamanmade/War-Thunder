using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed = 3;

    private SpriteRenderer sr;
    public Sprite[] tankSprite;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal < 0)
        {
            sr.sprite = tankSprite[3];
        }
        float vetical = Input.GetAxis("Vertical");

        transform.Translate(horizontal * speed * Time.deltaTime, vetical * speed * Time.deltaTime, 0);
    }
}
