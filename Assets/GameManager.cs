using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Paramètres de Spawn")]
    public GameObject[] folderPrefabs;    // Glisse tes différents types de dossiers ici
    public Transform[] spawnPoints;      // Glisse tes différents points de spawn ici
    public Transform folderParent;       // L'objet "Conteneur" pour ranger les dossiers

    [Header("Paramètres de Jeu")]
    public float gameDuration = 120f;
    public float initialSpawnDelay = 3f;
    public float minSpawnDelay = 0.5f;

    private int sortedCount = 0;
    private bool isGameActive = true;

    void Start()
    {
        // Sécurité : on vérifie que les listes ne sont pas vides
        if (folderPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            UnityEngine.Debug.LogError("Attention : folderPrefabs ou spawnPoints est vide !");
            return;
        }

        StartCoroutine(GameTimer());
        StartCoroutine(SpawnFolders());
    }

    IEnumerator SpawnFolders()
    {
        float currentDelay = initialSpawnDelay;

        while (isGameActive)
        {
            // 1. Choisir un dossier au hasard
            GameObject randomPrefab = folderPrefabs[UnityEngine.Random.Range(0, folderPrefabs.Length)];
            // 2. Choisir un point de spawn au hasard
            Transform randomPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
            // 3. Créer le dossier en tant qu'enfant de folderParent
            // On utilise la position et la rotation du point de spawn
            GameObject newFolder = Instantiate(randomPrefab, randomPoint.position, randomPoint.rotation, folderParent);

            // On s'assure que l'échelle reste correcte (1,1,1) si le parent a une échelle bizarre
            newFolder.transform.localScale = randomPrefab.transform.localScale;

            // 4. Difficulté progressive
            currentDelay = Mathf.Max(minSpawnDelay, currentDelay * 0.95f);

            yield return new WaitForSeconds(currentDelay);
        }
    }

    public void AddPoint()
    {
        if (isGameActive)
        {
            sortedCount++;
            UnityEngine.Debug.Log("Point marqué ! Total : " + sortedCount);
        }
    }

    IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(gameDuration);
        EndGame();
    }

    void EndGame()
    {
        isGameActive = false;
        UnityEngine.Debug.Log("--- TEMPS ÉCOULÉ ---");
        UnityEngine.Debug.Log("Nombre total de dossiers triés : " + sortedCount);

        // Optionnel : Détruire tous les dossiers restants à la fin
        if (folderParent != null)
        {
            foreach (Transform child in folderParent)
            {
                Destroy(child.gameObject);
            }
        }
    }
}