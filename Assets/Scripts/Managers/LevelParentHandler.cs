using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelParentHandler : MonoBehaviour
{
    public Transform spawnPoint;

    // Start is called before the first frame update
    private void Awake()
    {
        GameManager.instance.levelParent = this.gameObject;
        GameManager.instance.hasKey = false;
        GameManager.instance.character.transform.position = spawnPoint.position;
    }
}
