using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceMonster : MonoBehaviour
{
    public GameObject monsterPrefab;
    private GameObject monster;
    private GameManagerBehavior gameManager;
    // Use this for initialization
    void Start ()
    {
        // returns the game object it finds with the given name
        // then retrieves the component and store it for later
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehavior>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    
    private bool canPlaceMonster ()
    {
        // check whether a monster can be placed
        // return monster == null;

        // check whether a monster can be placed and the player has enough gold
        int cost = monsterPrefab.GetComponent<MonsterData>().levels[0].cost;
        return monster == null && gameManager.Gold >= cost;
    }

    // Unity automatically calls OnMouseUp when a player taps a GameObject’s physics collider.
    private void OnMouseUp()
    {
        // When called, this method places a new monster if canPlaceMonster() returns true.
        if (canPlaceMonster())
        {
            // You create the monster with Instantiate, 
            // a method that creates an instance of a given prefab 
            // with the specified position and rotation.
            // In this case, you copy monsterPrefab, 
            // give it the current GameObject’s position and no rotation, 
            // cast the result to a GameObject and store it in monster.

            monster = (GameObject)Instantiate(monsterPrefab, transform.position, Quaternion.identity);
           
            // Finally, you call PlayOneShot to play the sound effect attached to the object’s AudioSource componen
           AudioSource audioSource = gameObject.GetComponent<AudioSource>();
           audioSource.PlayOneShot(audioSource.clip);
           gameManager.Gold -= monster.GetComponent<MonsterData>().CurrentLevel.cost;
        }
        // check whehter it's possible to upgrade, if yes, access the MonsterData component with GetComponent() and
        // call increaseLevel which increases the level of the monster
        else if (canUpgradeMonster())
        {
            monster.GetComponent<MonsterData>().increaseLevel();
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioSource.clip);
            gameManager.Gold -= monster.GetComponent<MonsterData>().CurrentLevel.cost;
        }
    }

    // check whether there is a monster you cna upgrade by checking monster variable
    // for null
    // test whether higher level is available, which is when getNextLevel() doesn't
    // return null
    private bool canUpgradeMonster()
    {
        if(monster != null)
        {
            MonsterData monsterData = monster.GetComponent<MonsterData>();
            MonsterLevel nextLevel = monsterData.getNextLevel();
            if (nextLevel != null)
            {
                // check whether player has enough gold to upgrade
                return gameManager.Gold >= nextLevel.cost;
            }
        }
        return false;
        
    }
}
