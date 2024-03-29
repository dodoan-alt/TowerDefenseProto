﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    public GameObject[] waypoints;
    int currentWaypoint = 0;
    float lastWaypointSwitchTime;
    public float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        lastWaypointSwitchTime = Time.time;        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 startPosition = waypoints[currentWaypoint].transform.position;
        Vector3 endPosition = waypoints[currentWaypoint + 1].transform.position;

        float pathLength = Vector3.Distance(startPosition, endPosition);
        float totalTimeForPath = pathLength / speed;
        float currentTimeOnPath = Time.time - lastWaypointSwitchTime;
        gameObject.transform.position = Vector2.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);

        if (gameObject.transform.position.Equals(endPosition))
        {
            if(currentWaypoint < waypoints.Length - 2)
            {
                currentWaypoint++;
                lastWaypointSwitchTime = Time.time;

                RotateIntoMoveDirection();
            }
            else
            {
                Destroy(gameObject);

                AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                audioSource.PlayOneShot(audioSource.clip);

                GameObject.Find("GameManager").GetComponent<GameManagerBehavior>().Health--;
            }
        }
    }

    void RotateIntoMoveDirection()
    {
        Vector3 startPosition = waypoints[currentWaypoint].transform.position;
        Vector3 endPosition = waypoints[currentWaypoint + 1].transform.position;
        Vector3 direction = endPosition - startPosition;

        // It uses Mathf.Atan2 to determine the angle toward which direction points, in radians, assuming zero points to the right.
        // Multiplying the result by 180 / Mathf.PI converts the angle to degrees.
        float rotation = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;

        gameObject.transform.Find("Sprite").transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
    }

    public float DistanceToGold()
    {
        float distance = 0;
        distance += Vector2.Distance(gameObject.transform.position, waypoints[currentWaypoint].transform.position);

        for(int i = currentWaypoint + 1; i < waypoints.Length - 1; i++)
        {
            distance += Vector2.Distance(waypoints[i].transform.position, waypoints[i+1].transform.position);
        }

        return distance;
    }
}
