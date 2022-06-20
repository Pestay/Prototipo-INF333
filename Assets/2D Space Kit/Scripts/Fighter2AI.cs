using UnityEngine;
using System.Collections;



public class Fighter2AI : MonoBehaviour {

    private GameObject[] multipleEnemies;
    public Transform player;
    public Transform closestEnemy;
    public GameObject weapon_prefab;
    public float shot_speed;
    public float fireRate;
    private Rigidbody2D rb;
    private float nextFireTime = 0.0f;

    void Start() {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        closestEnemy = null;
    }

    void Update() {
        closestEnemy = getClosestEnemy();
        Vector3 direction = closestEnemy.position - transform.position;
        transform.rotation = Quaternion.Euler (new Vector3(0, 0, Mathf.LerpAngle(transform.rotation.eulerAngles.z, (Mathf.Atan2 (direction.y,direction.x) * Mathf.Rad2Deg) - 90f, 100f*Time.deltaTime)));
        if (Vector3.Distance(closestEnemy.position, transform.position) > 5f)
        {
            GetComponent<Rigidbody2D>().AddForce(transform.up * 20f * Time.deltaTime);
        } else {
            print(rb.velocity.magnitude);
            
            if( rb.velocity.magnitude > 0.01 )
            {
                GetComponent<Rigidbody2D>().AddForce(-transform.up * 20f * Time.deltaTime);
            }
            
            Fire(direction);
        }
    }

    public Transform getClosestEnemy()
    {
        multipleEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        Transform trans = null;

        foreach (GameObject go in multipleEnemies)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, go.transform.position);
            if(currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                trans = go.transform;
            }
        }

        return trans;
    }

    void Fire (Vector3 dir) {
        // If there is more than one bullet between the last and this frame
        // Reset the nextFireTime
        if (Time.time - fireRate > nextFireTime)
            nextFireTime = Time.time - Time.deltaTime;
    
        // Keep firing until we used up the fire time
        while( nextFireTime < Time.time) {
            FireOneShot(dir);
            nextFireTime += fireRate;
        }
    }

    void FireOneShot(Vector3 direction) {
        GameObject bullet = (GameObject) Instantiate(weapon_prefab, transform.position + new Vector3(direction.x,direction.y,0f)*0.06f, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up * shot_speed);
        bullet.GetComponent<Projectile>().firing_ship = this.gameObject;
        
    }

    void OnCollisionEnter2D (Collision2D collision) {
 
        if (collision.gameObject.tag == "Player") {
            
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    
    }
}

