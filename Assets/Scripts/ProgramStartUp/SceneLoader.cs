using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private int staticBuildIndex;
    [SerializeField]
    private int gameBuildIndex;

    private Dictionary<int, Scene> scenes;
    bool sceneLoaded = false;

    public IEnumerator Initialize()
    {
        scenes = new Dictionary<int, Scene>();

        StartCoroutine(LoadSceneInternal(staticBuildIndex));

        // Wait for scene to finish loading ...
        while(!sceneLoaded)
        {
            yield return null;
        }

        StartCoroutine(LoadSceneInternal(gameBuildIndex));

        // Wait for scene to finish loading ...
        while (!sceneLoaded)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(scenes[gameBuildIndex]);
        EventManager.instance.TriggerEvent(AllEventTypes.EVENT_GAME_SCENE_LOADED);
    }

    // Will set the current null scene variable to its proper value
    private IEnumerator LoadSceneInternal(int buildIndex)
    {
        sceneLoaded = false;
        AsyncOperation operation = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
        operation.allowSceneActivation = true;

        while (operation.progress < 1f)
        {
            yield return null;
        }

        Scene scene = SceneManager.GetSceneByBuildIndex(buildIndex);
        scenes.Add(buildIndex, scene);

        sceneLoaded = true;
    }

    public void Shutdown()
    {
        foreach (Scene scene in scenes.Values)
        {
            StartCoroutine(ShutdownSceneInternal(scene));
        }

        scenes.Clear();
    }

    private IEnumerator ShutdownSceneInternal(Scene scene)
    {
        AsyncOperation operation = SceneManager.UnloadSceneAsync(scene);

        while (operation.progress < 0.9f)
        {
            yield return null;
        }
    }
}
