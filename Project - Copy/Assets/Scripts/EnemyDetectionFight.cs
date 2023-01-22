using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyDetectionFight : MonoBehaviour
{
    public bool playerInDetectionRange = false;
    public DateTime nextDamage;
    public float fightAfterTime;
    

    public Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (playerInDetectionRange == true)
        {
            FightInDetectionRange();
        }
    }

    void Awake()
    {
        nextDamage = DateTime.Now;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInDetectionRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInDetectionRange = false;
        }
    }

    public void FightInDetectionRange()
    {
        if (nextDamage <= DateTime.Now)
        {
            enemy.AttackPlayer();
            nextDamage = DateTime.Now.AddSeconds(System.Convert.ToDouble(fightAfterTime));
        }
    }
}
