using System.Collections;
using System.Collections.Generic;  // gives access to generic data structures like <list>
using UnityEngine;


// creates monster level in monster data
// serializable makes instances editable in inspector, allows editing while game is running
[System.Serializable]
public class MonsterLevel
{
    public int cost;
    public GameObject visualization;
    public GameObject bullet;
    public float fireRate;
}


public class MonsterData : MonoBehaviour
{
    public List<MonsterLevel> levels;

    private MonsterLevel currentLevel;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    // for c# syntax this is not a constructor so do not include "()"
    // Define a property for the private variable currentLevel.
    //With a property defined, you can call just like any other variable: 
    // either as CurrentLevel (from inside the class) or as monster.CurrentLevel (from outside it). 
    // You can define custom behavior in a property's getter or setter method, and by supplying only a getter, 
    // a setter or both, you can control whether a property is read-only, write-only or read/write.
    public MonsterLevel CurrentLevel
    {
        // In the getter, you return the value of currentLevel
        get
        {
            return currentLevel;
        }

        // In the setter, you assign the new value to currentLevel.
        // Next you get the index of the current level.
        // Finally you iterate over all the levels and set the visualization to active or inactive, 
        // depending on the currentLevelIndex. 
        // This is great because it means that whenever someone sets currentLevel, the sprite updates automatically. Properties sure do come handy!

        set
        { 
            currentLevel = value;
            int currentLevelIndex = levels.IndexOf(currentLevel);

            GameObject levelVisualization = levels[currentLevelIndex].visualization;
            for (int i = 0; i < levels.Count; i++)
            {
                if (levelVisualization != null)
                {
                    if (i == currentLevelIndex)
                    {
                        levels[i].visualization.SetActive(true);
                    }
                    else
                    {
                        levels[i].visualization.SetActive(false);
                    }
                }
            }
        }
    }

    // upon placement, makes sure that it will show the correct sprite
   void OnEnable()
    {
        CurrentLevel = levels[0];
    }

    // get index of currentLevel and index of highest level provided that monster
    // did not reach the max level to return next level, otherwise return null
    // also check if upgrading is possible
    public MonsterLevel getNextLevel()
    {
        int currentLevelIndex = levels.IndexOf(currentLevel);
        int maxLevelIndex = levels.Count - 1;
        if (currentLevelIndex < maxLevelIndex)
        {
            return levels[currentLevelIndex + 1];
        }
        else
        {
            return null;
        }
    }

    // gets the index of hte current level and make sure it's not the max level
    public void increaseLevel()
    {
        int currentLevelIndex = levels.IndexOf(currentLevel);
        if (currentLevelIndex < levels.Count - 1)
        {
            CurrentLevel = levels[currentLevelIndex + 1];
        }
    }
}
