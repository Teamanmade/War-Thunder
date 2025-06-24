using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Born : MonoBehaviour
{
    public GameObject tank;
    public GameObject[] enemyTanks;
    public bool isPlayer;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnTank", 1f);
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SpawnTank()
    {
        if (isPlayer)
        {
            Instantiate(tank, transform.position, transform.rotation);
        }
        else
        {
            int index = Random.Range(0, enemyTanks.Length);
            Instantiate(enemyTanks[index], transform.position, transform.rotation);
        }
    }
}
