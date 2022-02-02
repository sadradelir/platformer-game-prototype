using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

 public class MovingObjectHandler : MonoBehaviour
{
     public PathCreator path;
     public float speed;
     private float distance;
     public bool mirror = true; 
    
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            path.transform.SetParent(GameManager.instance.levelParent.transform);
        }
        catch (Exception e)
        {
            path.transform.SetParent(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        distance += speed * Time.deltaTime; 
        this.transform.position =
            path.path.GetPointAtDistance(distance,mirror ? EndOfPathInstruction.Reverse : EndOfPathInstruction.Loop);
    }
}
