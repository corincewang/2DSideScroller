using UnityEngine;
using System.Collections;

public class BombSpawner : MonoBehaviour
{
    public GameObject bombPrefab;
    public float spawnInterval = 3f;
    
    void Start()
    {
        StartCoroutine(SpawnBombs());
    }
    
    IEnumerator SpawnBombs()
    {
        while (GameManager.Gary != null && GameManager.Gary.isCountdownActive)
        {
            yield return null;
        }
        
        while (GameManager.Gary != null && !GameManager.Gary.isGameOver)
        {
            float randomX = Random.Range(transform.position.x - transform.localScale.x / 2f, transform.position.x + transform.localScale.x / 2f);
            Vector3 spawnPos = new Vector3(randomX, transform.position.y, transform.position.z);
            Instantiate(bombPrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}

