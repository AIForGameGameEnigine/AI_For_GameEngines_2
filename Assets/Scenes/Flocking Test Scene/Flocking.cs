using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : MonoBehaviour
{
  Vector3 position;
  Vector3 rotation;
  Vector3 velocity;
  Vector3 acceleration = new Vector3(0, 0, 0);

  float maxSteeringForce = 0.01f;
  float maxSpeed = 0.01f;
  float desiredSeparation = 2.0f;
  float neighborDist = 10;

  GameObject[] minions;

  void Start()
  { 
    //Create a unit vector with random direction
    velocity = Random.insideUnitCircle.normalized;
    position = this.transform.position;

    this.transform.LookAt(velocity);
  }

  void Update()
  {
    minions = GameObject.FindGameObjectsWithTag("Enemy");

    flock(minions);
    updateSpeed();
    move();
  }

  void applyForce(Vector3 force) 
  {
    acceleration += force;
  }

  // Perform calcualations that affect the flock
  void flock(GameObject[] minions) 
  {
    Vector3 separation = separate(minions);
    Vector3 alignment = align(minions);
    Vector3 coheshion = cohesion(minions);

    separation *= 1.5f;
    alignment *= 1.0f;
    coheshion *= 1.0f;

    // Add the force vectors to acceleration
    applyForce(separation);
    applyForce(alignment);
    applyForce(coheshion);
  }

  // Update and limit speed
  void updateSpeed() 
  {
    velocity += acceleration;

    velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
    position += velocity;

    acceleration *= 0;
  }

  // Apply a steering force towards a target
  Vector3 seek(Vector3 target) 
  {
    Vector3 desiredDirection = target - position;

    desiredDirection.Normalize();
    desiredDirection *= maxSpeed;

    Vector3 steer = desiredDirection - velocity;
    steer = Vector3.ClampMagnitude(steer, maxSteeringForce);

    rotation = steer;

    return steer;
  }

  // Move the minion
  void move() 
  {

    this.transform.rotation = Quaternion.FromToRotation(this.transform.position, position);
    this.transform.Translate(position * Time.deltaTime);
    // this.transform.Rotate(rotation * Time.deltaTime);
    //this.transform.LookAt(rotation * Time.deltaTime);
  }

  // Check for nearby minions and steers away if neccessary
  Vector3 separate (GameObject[] minions) 
  {
    Vector3 steer = new Vector3(0, 0, 0);
    int count = 0;

    // Check if minions are too close
    foreach (GameObject minion in minions) 
    {
      float dist = Vector3.Distance(this.transform.position, minion.transform.position);

      if (dist > 0 && dist < desiredSeparation) 
      {
        // Calculate vector pointing away from neighbor
        Vector3 diff = position - minion.transform.position;
        diff.Normalize();
        diff /= dist;
        
        steer += diff;
        count++;
      }
    }

    // Find the average
    if (count > 0) 
    {
      steer /= (float)count;
    }

    if (steer.magnitude > 0) 
    {
      steer.Normalize();
      steer *= maxSpeed;
      steer -= velocity;
      steer = Vector3.ClampMagnitude(steer, maxSteeringForce);
    }

    return steer;
  }

  // Calculate the average velocity
  Vector3 align (GameObject[] minions) 
  {
    Vector3 sum = new Vector3(0, 0, 0);
    int count = 0;

    foreach (GameObject minion in minions) 
    {
      float dist = Vector3.Distance(this.transform.position, minion.transform.position);

      if (dist > 0 && dist < neighborDist) 
      {
        sum += minion.GetComponent<Flocking>().velocity;
        count++;
      }
    }

    if (count > 0) 
    {
      sum /= (float)count;

      sum.Normalize();
      sum *= maxSpeed;

      Vector3 steer = sum - velocity;
      steer = Vector3.ClampMagnitude(steer, maxSteeringForce);

      return steer;
    } 
    else 
    {
      return new Vector3(0, 0, 0);
    }
  }

  // Calculate steering vector towards the average position
  Vector3 cohesion (GameObject[] minions) 
  {
    Vector3 sum = new Vector3(0, 0, 0);
    int count = 0;

    foreach (GameObject minion in minions) 
    {
      float dist = Vector3.Distance(this.transform.position, minion.transform.position);

      if (dist > 0 && dist < neighborDist) 
      {
        sum += minion.transform.position;
        count++;
      }
    }
    
    // Steer towards the position
    if (count > 0) 
    {
      sum /= count;
      return seek(sum);  
    } 
    else 
    {
      return new Vector3(0, 0, 0);
    }
  }
}
