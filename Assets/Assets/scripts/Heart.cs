using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Heart : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Broken;
    public GameObject Explosion;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Break()
    {
        
        Instantiate(Explosion, transform.position, transform.rotation);
        Instantiate(Broken, transform.position, transform.rotation);
        Destroy(gameObject);
        Time.timeScale = 0;
    }
}

