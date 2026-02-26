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

    [Header("Animations VR")]
    public Animator BureauTourne; // <-- La nouvelle variable est ici

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
        }
    }

    IEnumerator Transition()
{
    float temps = 0f;
    float dureeTransition = 2.0f; 

    if (BureauTourne != null) BureauTourne.SetBool("Tourne", true);
    if (audioSource != null && sonTransition != null) audioSource.PlayOneShot(sonTransition);
    if (transitionAudioSource != null) transitionAudioSource.Play();
    if (transitionVideoPlayer != null) transitionVideoPlayer.Play();

    SetPhysiqueActive(jeuFormes, false);
    SetPhysiqueActive(jeuDossiers, false);
    
  
Vector3 startRotFormes = jeuFormes.transform.localEulerAngles;
Vector3 startPosFormes = jeuFormes.transform.position; 
Vector3 targetRotFormes = new Vector3(268.6f, startRotFormes.y, startRotFormes.z);
Vector3 targetPosFormes = new Vector3(startPosFormes.x, startPosFormes.y - 0.150f, startPosFormes.z);

while (temps < dureeTransition)
{
    temps += Time.deltaTime;
    float progression = Mathf.SmoothStep(0f, 1f, temps / dureeTransition);
    float angleXFormes = Mathf.LerpAngle(startRotFormes.x, targetRotFormes.x, progression);
    jeuFormes.transform.localRotation = Quaternion.Euler(angleXFormes, startRotFormes.y, startRotFormes.z);
    
    jeuFormes.transform.position = Vector3.Lerp(startPosFormes, targetPosFormes, progression);

    yield return null;
}

jeuFormes.transform.localRotation = Quaternion.Euler(targetRotFormes);
jeuFormes.transform.position = targetPosFormes;

    jeuDossiers.SetActive(false);
    jeuFormes.SetActive(false); 
    
    SetPhysiqueActive(jeuDossiers, true);
    
    Debug.Log("Transition terminée proprement.");
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