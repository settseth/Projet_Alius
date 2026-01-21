using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition12 : MonoBehaviour
{
    [SerializeField] public GameObject solOriginal;
    [SerializeField] public GameObject solNouveau;
    [SerializeField] public GameObject solOklm;

    public string sceneToLoad;

    public void ChangementDeSol()
    {
        solOriginal.SetActive(false);
        solNouveau.SetActive(true);
        solOklm.SetActive(true);

        StartCoroutine(fall());
        StartCoroutine(LoadSceneAfterDelay("Part2"));
    }

    IEnumerator fall()
    {
        float delay = 1.0f;        // délai initial
        float minDelay = 0.05f;    // vitesse max
        float acceleration = 0.85f; // facteur d'accélération

        foreach (Transform child in solNouveau.transform)
        {
            // Ajout du Rigidbody uniquement s'il n'existe pas
            Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = child.gameObject.AddComponent<Rigidbody>();
                rb.useGravity = true;
                rb.drag = 7f;
                rb.angularDrag = 7f;

                // Évite les rotations cheloues
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }

            yield return new WaitForSeconds(delay);

            // Accélération progressive
            delay = Mathf.Max(minDelay, delay * acceleration);
        }
    }

    IEnumerator LoadSceneAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(7f);
        SceneManager.LoadScene(sceneName);
    }
}
