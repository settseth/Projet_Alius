using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorOpen : MonoBehaviour
{
    public Animator myAnimator;

    void OnTriggerEnter()
    {
        myAnimator.SetBool("isCloseEnough", true);
        StartCoroutine(LoadSceneAfterDelay("SampleScene"));
    }

    IEnumerator LoadSceneAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(sceneName);
    }
}
