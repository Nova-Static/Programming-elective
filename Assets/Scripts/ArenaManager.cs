﻿using System;
using UnityEngine;

/// <summary>
/// The Arena manager
/// </summary>
public class ArenaManager : MonoBehaviour
{
    bool BattleStarted = false;

    BaseAI[] AIBots = null;
    MechController[] Bots = null;

    [SerializeField]
    MechController Bot = null;

    SpawnPos[] spawnPositions = null;

    void Start()
    {
        spawnPositions = FindObjectsOfType<SpawnPos>();
        PrepareArena();
    }

    /// <summary>
    /// Prepare the Arena so the battle can start any moment
    /// The AI's are instantiated and assigned to the Bots (BotController)
    /// </summary>
    private void PrepareArena()
    {
        AIBots = new BaseAI[]
                {

            new SzilardAi(),
            new DarwinsAi(),
            new RamjarAI(),
            new StanimirAI()
                };

        Bots = new MechController[spawnPositions.Length];

        CheckConditions();

        int loopCount = Math.Min(spawnPositions.Length, AIBots.Length);
        reshuffle(AIBots);
        for (int i = 0; i < loopCount; i++)
        {
            Transform position = spawnPositions[i].transform;
            Vector3 spawnPos = new Vector3(position.position.x + UnityEngine.Random.Range(-3, 10), position.position.y, position.position.z + UnityEngine.Random.Range(-3, 10));
            
            MechController instance = Instantiate(Bot, spawnPos, Quaternion.identity);
            instance.setAI(AIBots[i]);
            instance.name = AIBots[i].name;
            Bots[i] = instance;
        }
    }

    void reshuffle(BaseAI[] ais)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < ais.Length; t++)
        {
            BaseAI tmp = ais[t];
            int r = UnityEngine.Random.Range(t, ais.Length);
            ais[t] = ais[r];
            ais[r] = tmp;
        }
    }


    private void CheckConditions()
    {
        if (AIBots.Length < spawnPositions.Length)
        {
            Debug.LogWarning("More bot to be spawned than there are spawn locations.");
        }
        else if (spawnPositions.Length < AIBots.Length)
        {
            Debug.LogWarning("There is still a few spawn locations available.");
        }
    }

    private void WinCondition()
    {

    }
    /// <summary>
    /// Start the battle when the space bar is pressed (and the battle has not yet started
    /// </summary>
    void Update()
    {
        if (!BattleStarted && Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var bot in Bots)
            {
                bot.SetActive(true);
            }
            BattleStarted = true;
        }
    }
}
