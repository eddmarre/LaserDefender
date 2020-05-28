using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] int health = 200;



    [Header("Projectile")]
    [SerializeField] float projectileSpeed = 10f;

    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileFiringPeriod = .1f;
    [Header("Explosion")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;

    [Header("Sounds")]
    [SerializeField] AudioClip laserSFX;
    [Range(0, 1)] [SerializeField] float laserSoundVolume = .25f;
    [SerializeField] AudioClip deathSFX;
    [Range(0, 1)] [SerializeField] float deathSoundVolume = .7f;


    Coroutine firingCoroutine;
    float xMin;
    float xMax;
    float ymin;
    float yMax;
    void Start()
    {
        SetUpMoveBoundaries();
    }


    void Update()
    {
        Move();
        Fire();

    }


    void Move()
    {

        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);

        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var nexYPos = Mathf.Clamp(transform.position.y + deltaY, ymin, yMax);
        transform.position = new Vector2(newXPos, nexYPos);
    }
    void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinously()
    {
        while (true)
        {
            GameObject laser = Instantiate(
        laserPrefab,
        transform.position,
        Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position, laserSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }
    void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        ymin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
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
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSoundVolume);
        FindObjectOfType<GameManager>().LoadGameOver();

        //gameManager.LoadGameOver();
    }
}
