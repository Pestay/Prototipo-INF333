using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public GameObject weapon_prefab;
    public Transform player;
    private Rigidbody2D rb;
    public float shot_speed;
    public float fireRate;
    public int maxHealth = 100;
	public int currentHealth;
    private float nextFireTime = 0.0f;
    public GameObject statusBar;

    // Start is called before the first frame update
    void Start()
    {
        statusBar = gameObject.transform.Find("NPC Canvas(Clone)").gameObject.transform.Find("Status Bar NPC").gameObject;
        currentHealth = maxHealth;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0.0f, 0.0f);
        rb.inertia = 0.0f;
        statusBar.GetComponent<StatusBarNPC>().SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        statusBar.GetComponent<StatusBarNPC>().SetHealth(currentHealth);
        if(currentHealth == 0)
		{
			Destroy(gameObject);
		}

        
        player = getPlayerPos();
        Vector3 direction = player.position - transform.position;
        transform.rotation = Quaternion.Euler (new Vector3(0, 0, Mathf.LerpAngle(transform.rotation.eulerAngles.z, (Mathf.Atan2 (direction.y,direction.x) * Mathf.Rad2Deg) - 90f, 100f*Time.deltaTime)));
        if (Vector3.Distance(player.position, transform.position) > 7f)
        {
            rb.AddForce(transform.up * 50f * Time.deltaTime);
        } else {
            rb.velocity = new Vector2(0.0f,0.0f);
            Fire(direction);
        }
            
            
            
            
        
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

    public Transform getPlayerPos()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void OnCollisionEnter2D (Collision2D collision) {
 
        if (collision.gameObject.tag == "Enemy") {
            
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    
    }

    void OnTriggerEnter2D(Collider2D col) 
    {
        if(col == null)
            return;
        if(col.gameObject.tag != gameObject.tag)
            TakeDamage(10);
    }

    public void TakeDamage(int damage)
	{
		currentHealth -= damage;
	}
}
