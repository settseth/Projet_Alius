using System.Collections;
using UnityEngine;

public class StartAnimation : MonoBehaviour
{
    [Header("References")]
    public Camera targetCamera;

    [Header("Camera Distance")]
    public float maxFarClip = 100f;
    public float changeInterval = 1f;
    public float farClipStep = 10f;

    [Header("Colors")]
    public Color backgroundColor = Color.black;
    public Color fogColor = Color.gray;

    public Light Spotlight;

    public AudioSource lights2;


    private void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        if (targetCamera == null)
        {
            Debug.LogError("Aucune caméra trouvée ! Assigne targetCamera dans l'inspecteur.");
            enabled = false;
            return;
        }


        StartCoroutine(ChangeFarClip());
    }

    IEnumerator ChangeFarClip()
    {
        while (targetCamera.farClipPlane < maxFarClip)
        {
            targetCamera.farClipPlane += farClipStep;
            lights2.Play();
            changeInterval = changeInterval / 1.1f;
            yield return new WaitForSeconds(changeInterval);
        }

        // Changement des couleurs
        targetCamera.clearFlags = CameraClearFlags.SolidColor;
        targetCamera.backgroundColor = backgroundColor;

        RenderSettings.fog = true;
        RenderSettings.fogColor = fogColor;

        // Activation de tous les Wait
        ActivateAllWaits();
    }

    void ActivateAllWaits()
    {
        WaitActive[] all = FindObjectsOfType<WaitActive>();
        foreach (var sa in all)
        {
            sa.ActivateWait();
        }
        Spotlight.gameObject.SetActive(true);
    }
}
