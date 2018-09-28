using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boot : MonoBehaviour
{
    [SerializeField]
    private SceneLoader sceneLoader;

    private bool shuttingDown = false;

    public void Shutdown()
    {
        sceneLoader.Shutdown();
        shuttingDown = true;
    }

    private void Start ()
    {
        StartCoroutine(sceneLoader.Initialize());
	}

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Shutdown();
        }

        if (!shuttingDown)
        {
            return;
        }

        // If scenecount is equal to 1 it means that all other scenes succesfully shut down.
        if (SceneManager.sceneCount == 1)
        {
            // Perfect place to check for leaks etc.
            Application.Quit();
        }
    }
}
