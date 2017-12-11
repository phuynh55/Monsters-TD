using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float speed = 10;
    public int damage;
    public GameObject target;
    public Vector3 startPosition;
    public Vector3 targetPosition;

    private float distance;
    private float startTime;

    private GameManagerBehavior gameManager;
	// Use this for initialization
	void Start ()
    {
        startTime = Time.time;
        distance = Vector2.Distance(startPosition, targetPosition);
        GameObject gm = GameObject.Find("GameManager");
        gameManager = gm.GetComponent<GameManagerBehavior>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // You calculate the new bullet position using Vector3.Lerp to interpolate between start and end positions.
        float timeInterval = Time.time - startTime;
        gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, timeInterval * speed / distance);

        // If the bullet reaches the targetPosition, you verify that target still exists.
        if (gameObject.transform.position.Equals(targetPosition))
        {
            if (target != null)
            {
                // You retrieve the target's HealthBar component and reduce its health by the bullet's damage.
                Transform healthBarTransform = target.transform.Find("HealthBar");
                HealthBar healthBar = healthBarTransform.gameObject.GetComponent<HealthBar>();
                healthBar.currentHealth -= Mathf.Max(damage, 0);

                // If the health of the enemy falls to zero, you destroy it, play a sound effect and reward the player for marksmanship.
                if (healthBar.currentHealth <= 0)
                {
                    Destroy(target);
                    AudioSource audioSource = target.GetComponent<AudioSource>();
                    AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);

                    gameManager.Gold += 50;
                }
            }
            Destroy(gameObject);
        }
	}
}
