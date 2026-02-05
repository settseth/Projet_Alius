using System.Collections;
using UnityEngine; // On garde uniquement UnityEngine et System.Collections

public class FolderMiniGame : MonoBehaviour
{
    [Header("Param�tres de Spawn")]
    public GameObject[] folderPrefabs;
    public Transform[] spawnPoints;

    // Si tu laisses vide, �a prendra l'objet lui-m�me comme parent
    public Transform folderParent;

    [Header("Difficult�")]
    public float initialSpawnDelay = 3f;
    public float minSpawnDelay = 0.5f;

    private bool isRunning = false;

    // Cette fonction est appel�e par le GameManager
    public void StartFolderGame()
    {
        // 1. V�rifications de s�curit�
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
        UnityEngine.Debug.Log(">>> D�marrage du script FolderMiniGame !");
        isRunning = true;
        StartCoroutine(SpawnRoutine());
    }

    public void StopFolderGame()
    {
        UnityEngine.Debug.Log("<<< Arr�t du script FolderMiniGame.");
        isRunning = false;
        StopAllCoroutines();

        // Nettoyage des dossiers restants
        if (folderParent != null)
        {
            foreach (Transform child in folderParent)
            {
                //Destroy(child.gameObject);
            }
        }
    }

    IEnumerator SpawnRoutine()
    {
        float currentDelay = initialSpawnDelay;

        while (isRunning)
        {
            SpawnOneFolder();

            // Augmente la difficult� (r�duit le d�lai)
            currentDelay = Mathf.Max(minSpawnDelay, currentDelay * 0.95f);

            yield return new WaitForSeconds(currentDelay);
        }
    }

    void SpawnOneFolder()
    {
        // Choix al�atoire s�curis�
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