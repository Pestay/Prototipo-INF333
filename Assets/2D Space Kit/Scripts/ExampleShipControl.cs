using UnityEngine;
using System.Collections;

public class ExampleShipControl : MonoBehaviour {

	public float acceleration_amount = 1f;
	public float rotation_speed = 1f;
	public GameObject turret;
	public GameObject fighter2;
	public float turret_rotation_speed = 3f;
	public int maxHealth = 1000;
	public int currentHealth;

	public StatusBar statusBar;

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		statusBar.SetMaxHealth(maxHealth);
	}
	
	// Update is called once per frame
	void Update () {
		
		statusBar.SetHealth(currentHealth);
		if(currentHealth <= 0)
		{
			Debug.Log("Game Over");
		}
		if (Input.GetKeyDown(KeyCode.Escape))
			Screen.lockCursor = !Screen.lockCursor;	
	
	
	
		if (Input.GetKey(KeyCode.W)) {
			GetComponent<Rigidbody2D>().AddForce(transform.up * acceleration_amount * Time.deltaTime);
		
		}
		if (Input.GetKey(KeyCode.S)) {
			GetComponent<Rigidbody2D>().AddForce((-transform.up) * acceleration_amount * Time.deltaTime);
			
		}
		
		if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift)) {
			GetComponent<Rigidbody2D>().AddForce((-transform.right) * acceleration_amount * 0.6f  * Time.deltaTime);
			//print ("strafeing");
		}
		if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift)) {
			GetComponent<Rigidbody2D>().AddForce((transform.right) * acceleration_amount * 0.6f  * Time.deltaTime);
			
		}
		
		if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.LeftShift)) {
			GetComponent<Rigidbody2D>().AddTorque(-rotation_speed  * Time.deltaTime);
			
		}
		if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftShift)) {
			GetComponent<Rigidbody2D>().AddTorque(rotation_speed  * Time.deltaTime);
			
		}	
		if (Input.GetKey(KeyCode.C)) {
			GetComponent<Rigidbody2D>().angularVelocity = Mathf.Lerp(GetComponent<Rigidbody2D>().angularVelocity, 0, rotation_speed * 0.06f * Time.deltaTime);
			GetComponent<Rigidbody2D>().velocity = Vector2.Lerp(GetComponent<Rigidbody2D>().velocity, Vector2.zero, acceleration_amount * 0.06f * Time.deltaTime);
		}	
		
		
		if (Input.GetKey(KeyCode.H)) {
			transform.position = new Vector3(0,0,0);
		}

		if (Input.GetKey(KeyCode.F)) {
			GameObject friendly = (GameObject) Instantiate(fighter2, transform.position, transform.rotation, gameObject.transform);
		}	
		
		
		
		
	}

	void OnTriggerEnter2D(Collider2D col) 
    {
		if(col == null)
            return;
		if(col.gameObject.tag == "Enemy")
		{
        	TakeDamage(10);
		}

    }


	public void TakeDamage(int damage)
	{
		currentHealth -= damage;
	}
}
