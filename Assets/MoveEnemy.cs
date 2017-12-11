using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    // prevents change to the field
    [HideInInspector]
    public GameObject[] waypoints;
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;
    public float speed = 5.0f;

    // Use this for initialization
    void Start ()
    {
        // periodically checks for time at position
        lastWaypointSwitchTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // From the waypoints array, you retrieve the start and end position for the current path segment.
        Vector3 startPosition = waypoints[currentWaypoint].transform.position;
        Vector3 endPosition = waypoints[currentWaypoint + 1].transform.position;

        // Calculate the time needed for the whole distance with the formula time = distance / speed, 
        // then determine the current time on the path. Using Vector3.Lerp, 
        // you interpolate the current position of the enemy between the segment's start and end positions.
        float pathLength = Vector3.Distance(startPosition, endPosition);
        float totalTimeForPath = pathLength / speed;
        float currentTimeOnPath = Time.time - lastWaypointSwitchTime;
        gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);

        // Check whether the enemy has reached the endPosition. If yes, handle these two possible scenarios:
        if (gameObject.transform.position.Equals(endPosition))
        {
            // The enemy is not yet at the last waypoint, 
            // so increase currentWaypoint and update lastWaypointSwitchTime. 
            // Later, you'll add code to rotate the enemy so it points in the direction it's moving, too.
            if (currentWaypoint < waypoints.Length - 2)
            {
                currentWaypoint++;
                lastWaypointSwitchTime = Time.time;
                RotateIntoMoveDirection();
            }
            // The enemy reached the last waypoint, so this destroys it and triggers a sound effect. 
            // Later you'll add code to decrease the player's health, too.
            else
            {
                Destroy(gameObject);

                AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
                //TODO deduct health

                GameManagerBehavior gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehavior>();
                gameManager.Health -= 1;
            }
        }
	}

    private void RotateIntoMoveDirection()
    {
        // It calculates the bug’s current movement direction by 
        // subtracting the current waypoint’s position from that of the next waypoint.
        Vector3 newStartPosition = waypoints[currentWaypoint].transform.position;
        Vector3 newEndPosition = waypoints[currentWaypoint + 1].transform.position;
        Vector3 newDirection = (newEndPosition - newStartPosition);

        // It uses Mathf.Atan2 to determine the angle toward which newDirection points, in radians, 
        // assuming zero points to the right. 
        // Multiplying the result by 180 / Mathf.PI converts the angle to degrees.
        float x = newDirection.x;
        float y = newDirection.y;
        float rotationAngle = Mathf.Atan2(y, x) * 180 / Mathf.PI;

        // Finally, it retrieves the child named Sprite and rotates it rotationAngle degrees along the z-axis.
        // Note that you rotate the child instead of the parent so the health bar — you’ll add it soon — remains horizontal.
        GameObject sprite = (GameObject)gameObject.transform.Find("Sprite").gameObject;
        sprite.transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
    }

    public float DistanceToGoal()
    {
        float distance = 0;
        distance += Vector2.Distance(gameObject.transform.position, waypoints[currentWaypoint + 1].transform.position);
        for (int i = currentWaypoint + 1; i < waypoints.Length - 1; i++)
        {
            Vector3 startPosition = waypoints[i].transform.position;
            Vector3 endPosition = waypoints[i + 1].transform.position;
            distance += Vector2.Distance(startPosition, endPosition);
        }
        return distance;
    }
}
