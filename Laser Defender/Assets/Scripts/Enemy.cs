﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 100f;
    float shotCounter;
    [SerializeField] GameObject EnemyLaserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    float minTimeBetweenShots = .2f;
    float maxTimeBetweenShots = 3f;
    private void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }
    private void Update()
    {
        CountDownAndShoot();
    }
    void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }


    void Fire()
    {
        GameObject laser = Instantiate(
        EnemyLaserPrefab,
        transform.position,
        Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //get damamge dealer from bullets
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
         if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.hit();
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
