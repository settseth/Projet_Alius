using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Phase 1 : Formes (Premier Jeu)")]
    public GameObject game2Container;         // Conteneur des Formes
    public ShapeMinigame shapeGameScript;     // Script des Formes
    public float shapeGameDuration = 60f;     // Durée du jeu des formes

    [Header("Phase 2 : Dossiers (Deuxième Jeu)")]
    public GameObject game1Container;         // Conteneur des Dossiers
    public FolderMiniGame folderGameScript;   // Script des Dossiers
    public float folderGameDuration = 60f;    // Durée du jeu des dossiers

    void Start()
    {
        // Initialisation : On active les FORMES, on cache les DOSSIERS
        if (game2Container != null) game2Container.SetActive(true);
        if (game1Container != null) game1Container.SetActive(false);

        StartCoroutine(GameSequence());
    }

    IEnumerator GameSequence()
    {
        // ==========================================
        // --- PHASE 1 : FORMES ---
        // ==========================================
        UnityEngine.Debug.Log($">>> Chef d'orchestre : Lancement Phase 1 (Formes) pour {shapeGameDuration} secondes.");

        if (shapeGameScript != null) shapeGameScript.StartGame();

        // On attend la fin du timer des formes
        yield return new WaitForSeconds(shapeGameDuration);

        // ==========================================
        // --- TRANSITION ---
        // ==========================================
        UnityEngine.Debug.Log(">>> Chef d'orchestre : Fin Phase 1. Transition vers Dossiers...");

        // On cache le jeu des formes
        if (game2Container != null) game2Container.SetActive(false);

        // ==========================================
        // --- PHASE 2 : DOSSIERS ---
        // ==========================================
        UnityEngine.Debug.Log($">>> Chef d'orchestre : Lancement Phase 2 (Dossiers) pour {folderGameDuration} secondes.");

        // On affiche le jeu des dossiers
        if (game1Container != null) game1Container.SetActive(true);

        // On lance la logique du jeu des dossiers
        if (folderGameScript != null) folderGameScript.StartFolderGame();

        // On attend la fin du timer des dossiers
        yield return new WaitForSeconds(folderGameDuration);

        // ==========================================
        // --- FIN TOTALE ---
        // ==========================================
        UnityEngine.Debug.Log(">>> Chef d'orchestre : Fin de la Phase 2. Arrêt du jeu.");

        // On arrête proprement le jeu des dossiers
        if (folderGameScript != null) folderGameScript.StopFolderGame();

        // Optionnel : Désactiver le conteneur ou afficher un écran de fin
        // if (game1Container != null) game1Container.SetActive(false);
    }

    // Fonction vide pour compatibilité si d'autres scripts l'appellent
    public void AddPoint() { }
}