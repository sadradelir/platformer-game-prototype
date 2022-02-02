using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColideHandler : MonoBehaviour
{
    public Collider2D collider;

    private void FixedUpdate()
    {
        var contactFilter2D = new ContactFilter2D();
        Collider2D[] colides = new Collider2D[3];
        Physics2D.OverlapCollider(collider, contactFilter2D, colides);
        for (int i = 0; i < 3; i++)
        {
            if (colides[i])
            {
                if (colides[i].tag.Equals("Hazard"))
                {
                    GameManager.instance.HitHazard();
                }
                if (colides[i].tag.Equals("Door"))
                {
                    if (GameManager.instance.hasKey)
                    {
                        GameManager.instance.ReachTheDoor();
                    }
                }
                if (colides[i].tag.Equals("Spring"))
                {
                   // GameManager.instance.character.SpringJump();
                }
                if (colides[i].tag.Equals("Key"))
                {
                    GameManager.instance.GetKey();
                    Destroy(colides[i].gameObject);
                }
            }
        }
    }
}