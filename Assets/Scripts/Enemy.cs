using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void EnemyEvent();
    public static event EnemyEvent OnEnemyDie;

    public int hp = 6;
    public float speed = 1f;
    public float minMoveWaitTime = 3f;
    public float maxMoveWaitTime = 6f;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float minShootWaitTime = 5f;
    public float maxShootWaitTime = 10f;
    public int bulletsInBurst = 3;
    public AudioClip shootSound;
    public bool stayStill = false;
    public GameObject explosionEffect;

    private Vector3 targetLocation;
    private Transform playerHead;

    private BoxCollider movementArea;
    private Rigidbody rb;
    
    void Awake()
    {
        movementArea = GameObject.FindWithTag("EnemyMovementArea").GetComponent<BoxCollider>();
        playerHead = GameObject.FindWithTag("MainCamera").transform;

        rb = GetComponent<Rigidbody>();

        if(!stayStill)
        {
            StartCoroutine("RandomMovement");
        }        
        StartCoroutine("RandomShooting");
    }

    void FixedUpdate()
    {
        if(!stayStill)
        {
            transform.position = Vector3.Lerp(transform.position, targetLocation, Time.deltaTime * speed);
        }        

        //Quaternion targetRotation = Quaternion.LookRotation(playerHead.position - transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1f);
        transform.LookAt(playerHead);
    }

    public void GetShot(Transform gun, RaycastHit hitInfo)
    {
        rb.AddForceAtPosition((transform.position - gun.position).normalized * 3f, hitInfo.point, ForceMode.Impulse);
        --hp;
        if(hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // @TODO: Particles, sound
        GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(explosion, 2f);
                
        if(OnEnemyDie != null)
        {
            OnEnemyDie();
        }

        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private IEnumerator RandomMovement()
    {
        while(true)
        {
            Vector3 min = movementArea.bounds.min;
            Vector3 max = movementArea.bounds.max;

            targetLocation = new Vector3(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y),
                Random.Range(min.z, max.z));

            yield return new WaitForSeconds(Random.Range(minMoveWaitTime, maxMoveWaitTime));
        }        
    }

    private IEnumerator RandomShooting()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(minShootWaitTime, maxShootWaitTime));

            AudioSource.PlayClipAtPoint(shootSound, transform.position);

            for (int i = 0; i < bulletsInBurst; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                bullet.transform.LookAt(playerHead);
                bullet.GetComponent<Bullet>().SetTarget(playerHead.position);
                yield return new WaitForSeconds(.25f);
            }
            
        }
    }    
}
