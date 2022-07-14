using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject weapon_prefab;
    public GameObject missile_prefab;
    public Transform player;
    private Rigidbody2D rb;
    public float shot_speed;
    public float fireRate;
    public int maxHealth = 100;
	public int currentHealth;
    private float nextFireTime = 0.0f;
    public GameObject statusBar;
    public float acceleration_amount = 1f;
    public float rotation_speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        statusBar = gameObject.transform.Find("NPC Canvas(Clone)").gameObject.transform.Find("Status Bar NPC").gameObject;
        currentHealth = maxHealth;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        statusBar.GetComponent<StatusBarNPC>().SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        statusBar.GetComponent<StatusBarNPC>().SetHealth(currentHealth);
        if(currentHealth <= 0)
		{
			Destroy(gameObject);
            player.gameObject.GetComponent<ExampleShipControl>().addFunds(10);
		}

        
        player = getPlayerPos();
        Vector3 direction = player.position - transform.position;
        transform.rotation = Quaternion.Euler (new Vector3(0, 0, Mathf.LerpAngle(transform.rotation.eulerAngles.z, (Mathf.Atan2 (direction.y,direction.x) * Mathf.Rad2Deg) - 90f, rotation_speed*Time.deltaTime)));
        if (Vector3.Distance(player.position, transform.position) > 8f)
        {
            rb.AddForce(transform.up * acceleration_amount * Time.deltaTime);
            Fire(direction);
        } else {
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
        int bullets = 41;
        for (int i = 0; i < bullets; i++)
        {
            Vector3 offset = new Vector3(Mathf.Cos(360/bullets * i)* 2.5f,Mathf.Sin(360/bullets * i)* 2.5f,0);
            Quaternion dir = Quaternion.Euler(0,0,(Mathf.Atan2 (offset.y,offset.x) * Mathf.Rad2Deg));
            GameObject bullet = (GameObject) Instantiate(weapon_prefab, transform.position, dir);
            bullet.GetComponent<Rigidbody2D>().velocity = rb.velocity;
            bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up * shot_speed);
            bullet.GetComponent<Projectile>().firing_ship = this.gameObject;
        }

        GameObject missile1 = (GameObject) Instantiate(missile_prefab, transform.position, transform.rotation);
		missile1.GetComponent<Rigidbody2D>().velocity = rb.velocity;
        missile1.GetComponent<Rigidbody2D>().AddForce(missile1.transform.up * shot_speed);
        missile1.GetComponent<SeekerMissile>().firing_ship = this.gameObject;
        
        GameObject missile2 = (GameObject) Instantiate(missile_prefab, transform.position, transform.rotation);
		missile2.GetComponent<Rigidbody2D>().velocity = rb.velocity;
        missile2.GetComponent<Rigidbody2D>().AddForce(missile2.transform.up * shot_speed);
        missile2.GetComponent<SeekerMissile>().firing_ship = this.gameObject;
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
