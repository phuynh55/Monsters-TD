using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerBehavior : MonoBehaviour
{ 
    public Text goldLabel;
    private int gold;
    public int Gold
    {
        get
        {
            return gold;
        }
        set
        {
            gold = value;
            goldLabel.GetComponent<Text>().text = "GOLD: " + gold;
        }
    }

    // stores a reference to the wave readout
    public Text waveLabel;
    public GameObject[] nextWaveLabels;

    // store whether the player has lost the game
    public bool gameOver = false;

    private int wave;
    public int Wave
    {
        get
        {
            return wave;
        }
        set
        {
            wave = value;
            if (!gameOver)
            {
                for (int i = 0; i < nextWaveLabels.Length; i++)
                {
                    nextWaveLabels[i].GetComponent<Animator>().SetTrigger("nextWave");
                }
            }
            waveLabel.text = "WAVE: " + (wave + 1);
        }
    }

    // stores reference to player's health
    public Text healthLabel;
    public GameObject[] healthIndicator;

    public int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            // If you're reducing the player's health, use the CameraShake component to create a nice shake effect. This script is included with the project and not covered here.
            if (value < health)
            {
                Camera.main.GetComponent<CameraShake>().Shake();
            }
            // Update the private variable and the health label in the top left corner of the screen.
            health = value;
            healthLabel.text = "HEALTH: " + health;
            // If health drops to 0 and it's not yet game over, set gameOver to true and trigger the GameOver animation.
            if (health <= 0 && !gameOver)
            {
                gameOver = true;
                GameObject gameOverText = GameObject.FindGameObjectWithTag("GameOver");
                gameOverText.GetComponent<Animator>().SetBool("gameOver", true);
            }
            // Remove one of the monsters from the cookie. If it just disabled them, this bit could be written more simply, but it also supports re-enabling them when you add health.
            for (int i = 0; i < healthIndicator.Length; i++)
            {
                if (i < Health)
                {
                    healthIndicator[i].SetActive(true);
                }
                else
                {
                    healthIndicator[i].SetActive(false);
                }
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        Gold = 1000;
        Wave = 0;
        Health = 5;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
