﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    int startingWave = 0;

    private void Start()
    {
        var currentWave = waveConfigs[startingWave];
        StartCoroutine(SpawnAllEnemiesInWave(currentWave));
    }
    IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            Instantiate(waveConfig.GetEnemyPrefab(),
            waveConfig.GetWaypoints()[0].transform.position,
            Quaternion.identity);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }

    }
}
