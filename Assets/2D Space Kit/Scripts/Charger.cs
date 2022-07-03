using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : MonoBehaviour
{
    public Transform player;
    private Rigidbody2D rb;
    public GameObject death_anim;
    public int maxHealth = 100;
	public int currentHealth;
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

        rb.AddForce(transform.up * 50f * Time.deltaTime);
        
            
    }

    void OnTriggerEnter2D(Collider2D col) {
        DeathAnimation();
		Destroy(gameObject);
	}

    void DeathAnimation() {
        Instantiate(death_anim, transform.position, Quaternion.identity);
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
	

}
