using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public Transform spawnPoint;       // Le spawn point (vide)
    public GameObject spherePrefab;    // Référence du prefab sphère
    public GameObject cubePrefab;      // Référence du prefab cube

    public float spawnInterval = 5f;   // Intervalle initial
    public float acceleration = 0.1f;  // Combien réduire l'intervalle
    public float minInterval = 0.5f;   // Intervalle minimum

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // Choix aléatoire entre cube et sphère
            GameObject prefabToSpawn = (Random.value > 0.5f) ? spherePrefab : cubePrefab;

            // Instanciation en enfant du spawnPoint
            GameObject spawned = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity, spawnPoint);

            // Renommer pour enlever "(Clone)"
            spawned.name = prefabToSpawn.name;

            // Attente avant le prochain spawn
            yield return new WaitForSeconds(spawnInterval);

            // Réduire l'intervalle progressivement
            spawnInterval -= acceleration;
            if (spawnInterval < minInterval)
                spawnInterval = minInterval;
        }
    }
}
