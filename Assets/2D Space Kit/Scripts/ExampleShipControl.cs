using UnityEngine;
using System.Collections;

public class ExampleShipControl : MonoBehaviour {

	public float acceleration_amount = 1f;
	public float rotation_speed = 1f;
	public GameObject turret;
	public GameObject fighter2;
	public GameObject gameOverUI;
	public float turret_rotation_speed = 3f;
	public int maxHealth = 1000;
	public int currentHealth;
	public int startingMoney = 0;
	public int fighterCost = 20;
	public int currentMoney;

	public StatusBar statusBar;

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		currentMoney = startingMoney;
		statusBar.SetMaxHealth(maxHealth);
		statusBar.SetFunds(startingMoney);
	}
	
	// Update is called once per frame
	void Update () {
		
		statusBar.SetHealth(currentHealth);
		statusBar.SetFunds(currentMoney);
		if(currentHealth <= 0)
		{
			Debug.Log("Game Over");
			gameOverUI.SetActive(true);
			Time.timeScale = 0f;
		}
		if (Input.GetKeyDown(KeyCode.LeftAlt))
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
			//SpawnFighter();
		}	
		
		
		
		
	}

	public void addFunds(int amount) {
		currentMoney += amount;
	}

	public void SpawnFighter ()
	{
		if(currentMoney - fighterCost >= 0)
		{
			currentMoney -= fighterCost;
			Debug.Log("Spawned");
			float angle = Random.Range(0,360); 
			Vector3 offset = new Vector3(Mathf.Cos(angle)* 2.5f,Mathf.Sin(angle)* 2.5f,0);

			GameObject friendly = (GameObject) Instantiate(fighter2, transform.position + offset, transform.rotation, gameObject.transform);
		} else {
			Debug.Log("Not enough money");
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
