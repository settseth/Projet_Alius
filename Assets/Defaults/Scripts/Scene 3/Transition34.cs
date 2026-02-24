using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition34 : MonoBehaviour
{

    void OnTriggerEnter()
    {
        StartCoroutine(LoadSceneAfterDelay("Part4"));
    }

    IEnumerator LoadSceneAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(sceneName);
    }
}
