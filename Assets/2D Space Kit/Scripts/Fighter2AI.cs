using UnityEngine;
using System.Collections;



public class Fighter2AI : MonoBehaviour {

    private GameObject[] multipleEnemies;
    public Transform player;
    public Transform closestEnemy;
    public GameObject weapon_prefab;
    public float shot_speed;
    public float fireRate;
    public int maxHealth = 100;
	public int currentHealth;
    private Rigidbody2D rb;
    private float nextFireTime = 0.0f;

    enum STATES : ushort{
        NULL_STATE, // used for return null
        SEARCH_ENEMY,
        GO_ATTACK_POS,
        ATTACK
    }

    STATES current_state = STATES.SEARCH_ENEMY;
    Rigidbody2D current_target = null;
    Vector3 attack_position = Vector3.zero;
    Vector3 random_vec = Vector3.zero; // <-- FIX
    
    void UpdateFSM(){ // called each frame
        STATES new_state = GetTransitions(current_state);
        if(new_state != STATES.NULL_STATE){
            OnEnterState(current_state, new_state);
            //OnExitState(new_state, current_state);
            current_state = new_state;
        }
        DoActions(current_state);
    }


    STATES GetTransitions(STATES state){ // check transition for selected state 
        switch (state){
            case STATES.SEARCH_ENEMY:
                if(TargetExist()){
                    return STATES.GO_ATTACK_POS;
                }
                break;
            case STATES.GO_ATTACK_POS:
                if(!TargetExist()){
                    return STATES.SEARCH_ENEMY;
                }
                else if( Vector3.Distance( this.transform.position, attack_position) < 5.0f ){
                    return STATES.ATTACK;
                }
                break;
            case STATES.ATTACK:
                if(!TargetExist()){
                    return STATES.SEARCH_ENEMY;
                }
                else if( Vector3.Distance( this.transform.position, attack_position) > 20.0f ){
                    return STATES.GO_ATTACK_POS;
                }
                break;
        }
        return STATES.NULL_STATE;
    }


    void OnEnterState(STATES last_state, STATES new_state){
        switch(last_state){
            case STATES.SEARCH_ENEMY:
                Debug.Log("SEARCH ENEMY");
                break;
            case STATES.GO_ATTACK_POS:
                Debug.Log("ATTACK POS");
                break;
            case STATES.ATTACK:
                Debug.Log("ATTACK");
                break;
        }
    }



    void DoActions(STATES state){ // Events while in state
        switch(state){
            case STATES.SEARCH_ENEMY:
                FindTarget();
                break;
            case STATES.GO_ATTACK_POS:
                GoToAttackPosition();
                break;
            case STATES.ATTACK:
                DoAttack();
                break;
        }
    }

    //------- Checkers
    bool TargetExist(){
        if(current_target != null){
            return true;
        }
        return false;
    }


    

    //------- Actions, this methods change global variables

    void FindTarget(){
        //rb.velocity = new Vector2(0.0f,0.0f);
        GameObject[] total_enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(total_enemies.Length > 0){
            current_target = total_enemies[GetClosestTargetIndex(total_enemies)].GetComponent<Rigidbody2D>();
        }
    }
    
    
    
    void GoToAttackPosition(){
        attack_position = current_target.transform.position + random_vec;
        MoveToPos(attack_position);
    }
    
    
    void DoAttack(){
        //rb.velocity = new Vector2(0.0f,0.0f);
        Vector3 dir = Vector3.Normalize(this.transform.position - current_target.transform.position);
        transform.rotation = Quaternion.Euler (new Vector3(0, 0, Mathf.LerpAngle(transform.rotation.eulerAngles.z, (Mathf.Atan2 (dir.y,dir.x) * Mathf.Rad2Deg) + 90f, 100f*Time.deltaTime))); 
        Fire(dir);
    }





    //-------------- Utility
    



    //Return a index of the closest target
    int GetClosestTargetIndex(GameObject[] target_list){ 
        float closestDistance = Mathf.Infinity;
        int r_index = 0;
        for(int i = 0; i < target_list.Length; i++){
            float distance = Vector3.Distance(this.transform.position, target_list[i].transform.position);
            if(distance < closestDistance){
                closestDistance = distance;
                r_index = i;
            }
        }

        return r_index;
    }


    void MoveToPos(Vector3 pos){ // Move target to position
        Vector3 direction = Vector3.Normalize(pos - transform.position);
        Vector3 dir = Vector3.Normalize(current_target.transform.position - transform.position);
        //transform.position = pos;
        GetComponent<Rigidbody2D>().AddForce(transform.up * 100f * Time.deltaTime);
        transform.rotation = Quaternion.Euler (new Vector3(0, 0, Mathf.LerpAngle(transform.rotation.eulerAngles.z, (Mathf.Atan2 (dir.y,dir.x) * Mathf.Rad2Deg) - 90f, 100f*Time.deltaTime)));
    }



    
    void Start() {
        currentHealth = maxHealth;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        closestEnemy = null;
        random_vec = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        random_vec = Vector3.ClampMagnitude(random_vec, 1.0f);
        
    }

    void Update() {
        if(currentHealth <= 0){
			Destroy(gameObject);
		}
        UpdateFSM();

        /*
        if (totalEnemies.Length == 0)
        {
            rb.velocity = new Vector2(0.0f,0.0f);
        } else {
            closestEnemy = getClosestEnemy();


            Vector3 direction = closestEnemy.position - transform.position;
            transform.rotation = Quaternion.Euler (new Vector3(0, 0, Mathf.LerpAngle(transform.rotation.eulerAngles.z, (Mathf.Atan2 (direction.y,direction.x) * Mathf.Rad2Deg) - 90f, 100f*Time.deltaTime)));
            if (Vector3.Distance(closestEnemy.position, transform.position) > 5f)
            {
                GetComponent<Rigidbody2D>().AddForce(transform.up * 20f * Time.deltaTime);
            } else {
                
                rb.velocity = new Vector2(0.0f,0.0f);
                
                Fire(direction);
            }
        }
        */
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

    void OnCollisionEnter2D (Collision2D collision) 
    {
 
        if (collision.gameObject.tag == "Player" ||  collision.gameObject.tag == "Minion") {
            
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    
    }

    void OnTriggerEnter2D(Collider2D col) 
    {
        if(col == null)
            return;
        if(col.gameObject.tag == "Enemy") 
            TakeDamage(10);
    }

    public void TakeDamage(int damage)
	{
		currentHealth -= damage;
	}
}