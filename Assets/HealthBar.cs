using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // maxHealth stores the enemy's maximal health points
    // currentHealth tracks how much health remains
    // originalScale remembers healthbar's original size
    public float maxHealth = 100;
    public float currentHealth = 100;
    private float originalScale;
	// Use this for initialization
	void Start ()
    {
        originalScale = gameObject.transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // coppy localScale to a temp variable because you cannot adjust only its X value
        // calculate new x value based on bug's current health then set temp back on localScale
        Vector3 tmpScale = gameObject.transform.localScale;
        tmpScale.x = currentHealth / maxHealth * originalScale;
        gameObject.transform.localScale = tmpScale;
	}
}
