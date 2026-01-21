using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Transition12 transition12script;

    [Header("Phase 0 : Introduction (Audio)")]
    public AudioSource introAudioSource; 

    [Header("Phase 1 : Formes (Premier Jeu)")]
    public GameObject game2Container;
    public ShapeMinigame shapeGameScript;

    [Header("Phase 2 : Dossiers (Deuxieme Jeu)")]
    public GameObject game1Container;
    public FolderMiniGame folderGameScript;
    public float folderGameDuration = 60f;

    private bool isShapePhaseFinished = false;

    void Start()
    {
        if (game2Container != null) game2Container.SetActive(false);
        if (game1Container != null) game1Container.SetActive(false);

        StartCoroutine(GameSequence());
    }

    public void EndShapePhase()
    {
        UnityEngine.Debug.Log(">>> SIGNAL RECU : Phase 1 terminée par le joueur !");
        isShapePhaseFinished = true;
    }

    IEnumerator GameSequence()
    {
        // --- PHASE 0 : INTRODUCTION ---
        UnityEngine.Debug.Log(">>> Chef d'orchestre : Lancement Phase 0 (Audio). En attente...");
        if (introAudioSource != null)
        {
            UnityEngine.Debug.Log(">>> Lecture de l'audio d'introduction...");
            introAudioSource.Play();

            // On attend la fin précise du morceau MP3
            yield return new WaitForSeconds(introAudioSource.clip.length);
        }

        // --- PHASE 1 ---
        UnityEngine.Debug.Log(">>> Chef d'orchestre : Lancement Phase 1. En attente...");

        isShapePhaseFinished = false;

        if (game2Container != null) game2Container.SetActive(true); // On active le visuel du jeu 1
        if (shapeGameScript != null) shapeGameScript.StartGame();

        // On attend le signal
        yield return new WaitUntil(() => isShapePhaseFinished == true);

        // --- TRANSITION ---
        UnityEngine.Debug.Log(">>> Chef d'orchestre : Transition...");

        if (game2Container != null) game2Container.SetActive(false);
        yield return null;

        // --- PHASE 2 ---
        UnityEngine.Debug.Log($">>> Chef d'orchestre : Lancement Phase 2 (Dossiers).");

        if (game1Container != null) game1Container.SetActive(true);
        yield return null;

        if (folderGameScript != null) folderGameScript.StartFolderGame();

        yield return new WaitForSeconds(folderGameDuration);

        // --- FIN ---
        UnityEngine.Debug.Log(">>> Chef d'orchestre : Fin de la Phase 2.");

        if (folderGameScript != null)
        {
            folderGameScript.StopFolderGame();
            transition12script.ChangementDeSol();
        }
    }

    public void AddPoint() { }
}