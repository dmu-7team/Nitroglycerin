using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public float spawnRadius = 3f;
    public float spawnInterval = 5f;
    public int maxMonsters = 3;

    private float lastSpawnTime;
    private int currentMonsterCount;

    void Update()
    {
        if (Time.time - lastSpawnTime >= spawnInterval && currentMonsterCount < maxMonsters)
        {
            SpawnMonster();
            lastSpawnTime = Time.time;
        }
    }

    void SpawnMonster()
    {
        if (monsterPrefab == null)
        {
            Debug.LogError("monsterPrefab이 설정되지 않았습니다!", this);
            return;
        }

        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        spawnPosition.y = 1.0f;
        Debug.Log("몬스터 생성 위치: " + spawnPosition);
        GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        currentMonsterCount++;
        monster.GetComponent<Monster>().OnMonsterDestroy += () => currentMonsterCount--;
    }
}