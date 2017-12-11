using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemies : MonoBehaviour
{
    public List<GameObject> enemiesInRange;

    private float lastShotTime;
    private MonsterData monsterData;

    // In OnEnemyDestroy, you remove the enemy from enemiesInRange. When an enemy walks on the trigger around your monster OnTriggerEnter2D is called.
    void OnEnemyDestroy(GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
    }

    // You then add the enemy to the list of enemiesInRange and add OnEnemyDestroy to the EnemyDestructionDelegate. This makes sure that OnEnemyDestroy is called when the enemy is destroyed. You don't want monsters to waste ammo on dead enemies now -- do you?
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
            EnemyDestructionDelegate del = other.gameObject.GetComponent<EnemyDestructionDelegate>();
            del.enemyDelegate += OnEnemyDestroy;
        }
    }

    // In OnTriggerExit2D you remove the enemy from the list and unregister your delegate. Now you know which enemies are in range.
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
            EnemyDestructionDelegate del = other.gameObject.GetComponent<EnemyDestructionDelegate>();
            del.enemyDelegate -= OnEnemyDestroy;
        }
    }

    void Shoot (Collider2D target)
    {
        GameObject bulletPrefab = monsterData.CurrentLevel.bullet;

        // Get the start and target positions of the bullet. Set the z-Position to that of bulletPrefab. Earlier, you set the bullet prefab's z position value to make sure the bullet appears behind the monster firing it, but in front of the enemies.
        Vector3 startPosition = gameObject.transform.position;
        Vector3 targetPosition = target.transform.position;
        startPosition.z = bulletPrefab.transform.position.z;
        targetPosition.z = bulletPrefab.transform.position.z;

        // Instantiate a new bullet using the bulletPrefab for MonsterLevel. Assign the startPosition and targetPosition of the bullet.
        GameObject newBullet = (GameObject)Instantiate(bulletPrefab);
        newBullet.transform.position = startPosition;
        BulletBehavior bulletComp = newBullet.GetComponent<BulletBehavior>();
        bulletComp.target = target.gameObject;
        bulletComp.startPosition = startPosition;
        bulletComp.targetPosition = targetPosition;

        // Make the game juicier: Run a shoot animation and play a laser sound whenever the monster shoots.
        Animator animator = monsterData.CurrentLevel.visualization.GetComponent<Animator>();
        animator.SetTrigger("fireShot");
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);
    }
	// Use this for initialization
	void Start ()
    {
        enemiesInRange = new List<GameObject>();
        lastShotTime = Time.time;
        monsterData = gameObject.GetComponentInChildren<MonsterData>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        GameObject target = null;
        // Determine the target of the monster. Start with the maximum possible distance in the minimalEnemyDistance. Iterate over all enemies in range and make an enemy the new target if its distance to the cookie is smaller than the current minimum.
        float minimalEnemyDistance = float.MaxValue;
        foreach (GameObject enemy in enemiesInRange)
        {
            float distanceToGoal = enemy.GetComponent<MoveEnemy>().DistanceToGoal();
            if (distanceToGoal < minimalEnemyDistance)
            {
                target = enemy;
                minimalEnemyDistance = distanceToGoal;
            }
        }
        // Call Shoot if the time passed is greater than the fire rate of your monster and set lastShotTime to the current time.
        if (target != null)
        {
            if (Time.time - lastShotTime > monsterData.CurrentLevel.fireRate)
            {
                Shoot(target.GetComponent<Collider2D>());
                lastShotTime = Time.time;
            }
            // Calculate the rotation angle between the monster and its target. You set the rotation of the monster to this angle. Now it always faces the target.
            Vector3 direction = gameObject.transform.position - target.transform.position;
            gameObject.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI, new Vector3(0, 0, 1));
        }
	}
}
