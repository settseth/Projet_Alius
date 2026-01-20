using System.Collections;
using UnityEngine; // On garde uniquement UnityEngine et System.Collections

public class FolderMiniGame : MonoBehaviour
{
    [Header("Paramètres de Spawn")]
    public GameObject[] folderPrefabs;
    public Transform[] spawnPoints;

    // Si tu laisses vide, ça prendra l'objet lui-même comme parent
    public Transform folderParent;

    [Header("Difficulté")]
    public float initialSpawnDelay = 3f;
    public float minSpawnDelay = 0.5f;

    private bool isRunning = false;

    // Cette fonction est appelée par le GameManager
    public void StartFolderGame()
    {
        // 1. Vérifications de sécurité
        if (folderPrefabs == null || folderPrefabs.Length == 0)
        {
            UnityEngine.Debug.LogError("ERREUR FolderMiniGame : La liste 'Folder Prefabs' est vide !");
            return;
        }
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            UnityEngine.Debug.LogError("ERREUR FolderMiniGame : La liste 'Spawn Points' est vide !");
            return;
        }

        // 2. Initialisation du parent si vide
        if (folderParent == null) folderParent = this.transform;

        // 3. Lancement
        UnityEngine.Debug.Log(">>> Démarrage du script FolderMiniGame !");
        isRunning = true;
        StartCoroutine(SpawnRoutine());
    }

    public void StopFolderGame()
    {
        UnityEngine.Debug.Log("<<< Arrêt du script FolderMiniGame.");
        isRunning = false;
        StopAllCoroutines();

        // Nettoyage des dossiers restants
        if (folderParent != null)
        {
            foreach (Transform child in folderParent)
            {
                Destroy(child.gameObject);
            }
        }
    }

    IEnumerator SpawnRoutine()
    {
        float currentDelay = initialSpawnDelay;

        while (isRunning)
        {
            SpawnOneFolder();

            // Augmente la difficulté (réduit le délai)
            currentDelay = Mathf.Max(minSpawnDelay, currentDelay * 0.95f);

            yield return new WaitForSeconds(currentDelay);
        }
    }

    void SpawnOneFolder()
    {
        // Choix aléatoire sécurisé
        int randPrefab = UnityEngine.Random.Range(0, folderPrefabs.Length);
        int randPoint = UnityEngine.Random.Range(0, spawnPoints.Length);

        GameObject prefab = folderPrefabs[randPrefab];
        Transform point = spawnPoints[randPoint];

        if (prefab != null && point != null)
        {
            GameObject newObj = Instantiate(prefab, point.position, point.rotation, folderParent);
        }
    }
}