using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Transition12 transition12script;

    [Header("Phase 1 : Formes (Premier Jeu)")]
    public GameObject game2Container;
    public ShapeMinigame shapeGameScript;

    [Header("Phase 2 : Dossiers (Deuxi�me Jeu)")]
    public GameObject game1Container;
    public FolderMiniGame folderGameScript;
    public float folderGameDuration = 60f;

    private bool isShapePhaseFinished = false;

    void Start()
    {
        if (game2Container != null) game2Container.SetActive(true);
        if (game1Container != null) game1Container.SetActive(false);

        StartCoroutine(GameSequence());
    }

    public void EndShapePhase()
    {
        // CORRECTION ICI : On pr�cise UnityEngine.Debug
        UnityEngine.Debug.Log(">>> SIGNAL RE�U : Phase 1 termin�e par le joueur !");
        isShapePhaseFinished = true;
    }

    IEnumerator GameSequence()
    {
        // --- PHASE 1 ---
        UnityEngine.Debug.Log(">>> Chef d'orchestre : Lancement Phase 1. En attente...");

        isShapePhaseFinished = false;

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

            //lancer scene 2..
            transition12script.ChangementDeSol();
        } 
    }

    //animation chute / transition :

    public void AddPoint() { }
}