using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Transition12 transition12script;

    [Header("Phase 0 : Introduction (Audio)")]
    public AudioSource introAudioSource;
    public VideoPlayer marwanVideo;

    [Header("Phase 1 : Formes (Premier Jeu)")]
    public GameObject jeuFormes;
    public GameObject game2Container;
    public ShapeMinigame shapeGameScript;

    [Header("Phase 2 : Dossiers (Deuxième Jeu)")]
    public GameObject jeuDossiers;
    public GameObject game1Container;
    public FolderMiniGame folderGameScript;
    public float folderGameDuration = 60f;

    [Header("Transition")]
    public AudioSource audioSource;
    public AudioClip sonTransition;
    public AudioSource transitionAudioSource;
    public VideoPlayer transitionVideoPlayer;

    public GameObject lightsManager;

    private bool isShapePhaseFinished = false;

    void Start()
    {
        if (lightsManager != null) lightsManager.SetActive(true);
        if (jeuFormes != null) jeuFormes.SetActive(true);
        if (jeuDossiers != null) jeuDossiers.SetActive(false);
        if (game2Container != null) game2Container.SetActive(false);
        if (game1Container != null) game1Container.SetActive(false);

        StartCoroutine(GrabInfo());
    }

    public void EndShapePhase()
    {
        Debug.Log(">>> SIGNAL RECU : Phase 1 terminée par le joueur !");
        isShapePhaseFinished = true;
    }

    IEnumerator GrabInfo()
    {
        yield return new WaitForSeconds(5);
        StartCoroutine(GameSequence());
    }

    IEnumerator GameSequence()
    {
        yield return new WaitForSeconds(4);

        Debug.Log(">>> Chef d'orchestre : Lancement Phase 0 (Audio). En attente...");
        if (introAudioSource != null)
        {
            Debug.Log(">>> Lecture de l'audio d'introduction...");
            introAudioSource.Play();
            marwanVideo.Play();
            yield return new WaitForSeconds(introAudioSource.clip.length);
        }

        Debug.Log(">>> Chef d'orchestre : Lancement Phase 1. En attente...");
        isShapePhaseFinished = false;
        if (game2Container != null) game2Container.SetActive(true);
        if (shapeGameScript != null) shapeGameScript.StartGame();

        yield return new WaitUntil(() => isShapePhaseFinished == true);

        Debug.Log(">>> Chef d'orchestre : Transition...");
        yield return StartCoroutine(Transition());

        Debug.Log(">>> Chef d'orchestre : Lancement Phase 2 (Dossiers).");
        if (game1Container != null) game1Container.SetActive(true);
        if (folderGameScript != null) folderGameScript.StartFolderGame();

        yield return new WaitForSeconds(folderGameDuration);


        Debug.Log(">>> Chef d'orchestre : Fin de la Phase 2.");
        if (folderGameScript != null)
        {
            folderGameScript.StopFolderGame();
            DesactivateAllWaits();
        }
    }

    IEnumerator Transition()
    {
        if (audioSource != null && sonTransition != null)
        {
            audioSource.PlayOneShot(sonTransition);
        }

        if (transitionAudioSource != null)
        {
            transitionAudioSource.Play();
        }

        if (transitionVideoPlayer != null)
        {
            transitionVideoPlayer.Play();
        }

        float temps = 0;
        float dureeTransition = 2f;
        float distanceDeplacement = 0.4f;

        Vector3 posInitialeFormes = jeuFormes.transform.position;
        Vector3 posCacheeFormes = posInitialeFormes - new Vector3(0, distanceDeplacement, 0);

        jeuDossiers.SetActive(true);
        Vector3 posFinaleDossiers = jeuDossiers.transform.position;
        Vector3 posCacheeDossiers = posFinaleDossiers - new Vector3(0, distanceDeplacement, 0);
        jeuDossiers.transform.position = posCacheeDossiers;

        SetPhysiqueActive(jeuFormes, false);
        SetPhysiqueActive(jeuDossiers, false);

        while (temps < dureeTransition)
        {
            temps += Time.deltaTime;
            float progression = Mathf.SmoothStep(0f, 1f, temps / dureeTransition);

            jeuFormes.transform.position = Vector3.Lerp(posInitialeFormes, posCacheeFormes, progression);
            jeuDossiers.transform.position = Vector3.Lerp(posCacheeDossiers, posFinaleDossiers, progression);

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        jeuFormes.SetActive(false);
        SetPhysiqueActive(jeuDossiers, true);
        Debug.Log("Transition terminée.");
    }

    void DesactivateAllWaits()
    {
        WaitActive[] all = FindObjectsOfType<WaitActive>();
        foreach (var sa in all)
        {
            sa.DesactivateWait();
        }
    }

    private void SetPhysiqueActive(GameObject parent, bool activer)
    {
        Collider[] colliders = parent.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = activer;
        }
        Rigidbody[] rigidbodies = parent.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = !activer;
        }
    }

    public void AddPoint() { }
}
