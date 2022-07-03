using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarNPC : MonoBehaviour
{
    public Slider h_slider;
    public Slider s_slider;
    public GameObject target;

    private void Start() {
        target = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        gameObject.transform.position = target.transform.position + new Vector3(0.0f,0.3f,0.0f);
    }

    private void Update() {
        if (target == null)
        {
            Destroy(gameObject);
        }
        gameObject.transform.position = target.transform.position + new Vector3(0.0f,0.3f,0.0f);
        gameObject.transform.rotation = Quaternion.Euler (0.0f, 0.0f, gameObject.transform.rotation.z * -1.0f);
        
    }

    public void SetMaxHealth(int health)
    {
        h_slider.maxValue = health;
        h_slider.value = health;
    }

    public void SetHealth(int health)
    {
        h_slider.value = health;
    }
}
