using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int hearts;
    public CharacterHandler character;
    public bool hasKey;
    public GameObject levelParent;
    public int currentLevel;

    public void loadLevel(int i)
    {
        if (levelParent)
        {
            Destroy(levelParent);
        }

        SceneManager.LoadScene("Level" + i, LoadSceneMode.Additive);
        currentLevel = i;
    }


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        hasKey = false;
        loadLevel(1);
    }


    public void GetKey()
    {
        hasKey = true;
    }

    public void HitHazard()
    {
        hearts--;
        character.transform.position = levelParent.transform.Find("SpawnPoint").position;
    }

    public void ReachTheDoor()
    {
        if (hasKey)
        {
            hasKey = false;
            loadLevel(currentLevel + 1);
        }
    }
}
