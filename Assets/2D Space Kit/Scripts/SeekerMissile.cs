using UnityEngine;
using System.Collections;

public class SeekerMissile : MonoBehaviour {
	public GameObject shoot_effect;
	public GameObject hit_effect;
	public GameObject firing_ship;
    public Transform player;
    private Rigidbody2D rb;
    public float acceleration_amount;
    public float rotation_speed;
	
	// Use this for initialization
	void Start () {
		GameObject obj = (GameObject) Instantiate(shoot_effect, transform.position  - new Vector3(0,0,5), Quaternion.identity); //Spawn muzzle flash
		obj.transform.parent = firing_ship.transform;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
		gameObject.tag = firing_ship.tag;
		Destroy(gameObject, 10f); //Bullet will despawn after 5 seconds
	}
	
	// Update is called once per frame
	void Update () {
        player = getPlayerPos();
        Vector3 direction = player.position - transform.position;
        Vector2 beforeVel = rb.velocity;
        Vector2 steer = new Vector2(direction.normalized.x,direction.normalized.y) - rb.velocity; 
        rb.AddForce(steer * acceleration_amount* Time.deltaTime);
        transform.rotation = Quaternion.Euler (new Vector3(0, 0, Mathf.LerpAngle(transform.rotation.eulerAngles.z, (Mathf.Atan2 (rb.velocity.y,rb.velocity.x) * Mathf.Rad2Deg) - 90f, rotation_speed*Time.deltaTime)));
        rb.AddForce(transform.up * acceleration_amount*1.5f* Time.deltaTime);
	}
	
	
	void OnTriggerEnter2D(Collider2D col) {

		if( col == null)
			return;
		//Don't want to collide with the ship that's shooting this thing, nor another projectile.
		if (
			col.gameObject != firing_ship 
		&& col.gameObject.tag != "Projectile" 
		&& col.gameObject.tag != firing_ship.tag 
		&& (firing_ship.tag != "Player" || col.gameObject.tag != "Minion")
		&& (firing_ship.tag != "Minion" || col.gameObject.tag != "Player")
		) {
			Instantiate(hit_effect, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}

    public Transform getPlayerPos()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
	
	
	
}
