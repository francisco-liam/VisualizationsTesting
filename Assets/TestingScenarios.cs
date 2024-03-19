using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestingScenarios : MonoBehaviour
{
    [Header("Traffic Ship Settings")]
    public List<Entity381> generatedEntites = new List<Entity381>();
    public List<Vector3> entPositions = new List<Vector3>();
    public List<bool> letMove = new List<bool>();
    public int numTraffic;

    [Header("Ownship Settings")]
    public Entity381 ownship;
    public bool ownshipMove;
    bool added = true;

    int scenarioID;
    List<Vector3> stationaryShips1 = new List<Vector3>();
    List<Vector3> stationaryShips2 = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        //positions for scenario 2
        stationaryShips2.Add(new Vector3(0.00f, 0.00f, 490.00f));
        stationaryShips2.Add(new Vector3(535.87f, 0.00f, 822.00f));
        stationaryShips2.Add(new Vector3(834.40f, 0.00f, 32.50f));
        stationaryShips2.Add(new Vector3(-225.00f, 0.00f, 135.80f));
        stationaryShips2.Add(new Vector3(-109.60f, 0.00f, -202.60f));
        stationaryShips2.Add(new Vector3(188.70f, 0.00f, -108.20f));
        stationaryShips2.Add(new Vector3(433.00f, 0.00f, -5.85f));
        stationaryShips2.Add(new Vector3(262.90f, 0.00f, 624.00f));
        stationaryShips2.Add(new Vector3(760.40f, 0.00f, 635.60f));
        stationaryShips2.Add(new Vector3(964.80f, 0.00f, 471.00f));

        //positions for scenario 1
        stationaryShips1.Add(new Vector3(0, 0, 803));
        stationaryShips1.Add(new Vector3(200, 0, 412.33f));
        stationaryShips1.Add(new Vector3(400, 0, 463.07f));
        stationaryShips1.Add(new Vector3(600, 0, 1343.37f));
        stationaryShips1.Add(new Vector3(800, 0, 249.66f));
        stationaryShips1.Add(new Vector3(1000, 0, 1940.38f));
        stationaryShips1.Add(new Vector3(1200, 0, 786.68f));
        stationaryShips1.Add(new Vector3(1400, 0, 1949.57f));
        stationaryShips1.Add(new Vector3(1600, 0, 1107.63f));
        stationaryShips1.Add(new Vector3(1800, 0, 1798.60f));
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < generatedEntites.Count; i++)
        {
            if (!letMove[i])
                generatedEntites[i].position = entPositions[i];
        }

        if(!ownshipMove && ownship != null)
            ownship.desiredSpeed = 0;

        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            string positions = "";
            foreach(Vector3 pos in entPositions)
            {
                positions += "\n" + pos;
            }
            Debug.Log(positions);
        }

        if (!added)
        {
            SetWaypoints();
            added = true;
        }
            

        if (Input.GetKeyUp(KeyCode.F1))
        {
            DestroyAllEntities();
            RandomizedTraffic(numTraffic);
            SpawnOwnship();
        }

        if (Input.GetKeyUp(KeyCode.F2))
        {
            DestroyAllEntities();
            InitializeScenarioOne();
            SpawnOwnship();
        }

        if (Input.GetKeyUp(KeyCode.F3))
        {
            DestroyAllEntities();
            InitializeScenarioTwo();
            SpawnOwnship();
        }

        if (Input.GetKeyUp(KeyCode.F4))
        {
            DestroyAllEntities();
            InitialiizeScenario3(numTraffic);
            SpawnOwnship();
        }

        if (Input.GetKeyUp(KeyCode.F5))
        {
            DestroyAllEntities();
            InitialiizeScenario4(numTraffic);
            SpawnOwnship();
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            ownshipMove = !ownshipMove;
        }
    }

    void RandomizedTraffic(int numTraffic)
    {
        float spawnX = 0;

        for (int i = 0; i < numTraffic; i++)
        {
            EntityType vesselType = (EntityType)Random.Range(0, 9);
            float spawnZ = Random.Range(0f, 2000f);

            Vector3 spawn = new Vector3(spawnX, 0, spawnZ);

            Entity381 ent = EntityMgr.inst.CreateEntity(vesselType, spawn, Vector3.zero);

            spawnX += 200;

            generatedEntites.Add(ent);
            entPositions.Add(spawn);
            letMove.Add(false);
        }

        scenarioID = 0;
        DistanceMgr.inst.Initialize();
    }

    void InitializeScenarioOne()
    {
        for (int i = 0; i < 10; i++)
        {
            EntityType vesselType = (EntityType)Random.Range(0, 9);

            Entity381 ent = EntityMgr.inst.CreateEntity(vesselType, stationaryShips1[i], Vector3.zero);

            generatedEntites.Add(ent);
            entPositions.Add(stationaryShips1[i]);
            letMove.Add(false);
        }

        scenarioID = 1;
        DistanceMgr.inst.Initialize();
    }

    void InitializeScenarioTwo()
    {
        for (int i = 0; i < 10; i++)
        {
            EntityType vesselType = (EntityType)Random.Range(0, 9);

            Entity381 ent = EntityMgr.inst.CreateEntity(vesselType, stationaryShips2[i], Vector3.zero);

            generatedEntites.Add(ent);
            entPositions.Add(stationaryShips2[i]);
            letMove.Add(false);
        }

        scenarioID = 2;
        DistanceMgr.inst.Initialize();
    }

    void InitialiizeScenario3(int numTraffic)
    {
        float[] spawnZ = {0f, -400f, 400f};

        float spawnX = -600;

        for (int i = 0; i < numTraffic; i++)
        {
            EntityType vesselType = (EntityType)Random.Range(0, 9);
            int spawnZIndex = (i + 1) % 3;

            Vector3 spawn = new Vector3(spawnX, 0, spawnZ[spawnZIndex]);

            Entity381 ent = EntityMgr.inst.CreateEntity(vesselType, spawn, Vector3.zero);
            ent.desiredHeading = 270f;
            ent.heading = 270f;

            ent.maxSpeed = 15f;
            ent.desiredSpeed = 15f;
            ent.acceleration = 5f;

            spawnX += 300;

            generatedEntites.Add(ent);
            entPositions.Add(spawn);
            letMove.Add(true);
        }

        scenarioID = 3;
        DistanceMgr.inst.Initialize();
    }

    void InitialiizeScenario4(int numTraffic)
    {
        float[] spawnZ = { 0f, -700f, 700f };

        float spawnX = -600;

        for (int i = 0; i < numTraffic; i++)
        {
            EntityType vesselType = (EntityType)Random.Range(0, 9);
            int spawnZIndex = i % 3;

            Vector3 spawn = new Vector3(spawnX, 0, spawnZ[spawnZIndex]);

            Entity381 ent = EntityMgr.inst.CreateEntity(vesselType, spawn, Vector3.zero);
            ent.desiredHeading = 270f;
            ent.heading = 270f;

            ent.maxSpeed = 15f;
            ent.desiredSpeed = 15f;
            ent.acceleration = 5f;

            spawnX += 400;

            generatedEntites.Add(ent);
            entPositions.Add(spawn);
            letMove.Add(true);
        }

        scenarioID = 4;
        DistanceMgr.inst.Initialize();
    }

    void SpawnOwnship()
    {
        Vector3 spawn = new Vector3(0, 0, 0);
        if(scenarioID == 3)
            spawn = new Vector3(300, 0, -700);
        if(scenarioID == 4)
            spawn = new Vector3(-1500, 0, 0);
        Entity381 ent = EntityMgr.inst.CreateEntity(EntityType.PilotVessel, spawn, Vector3.zero);
        ent.heading = 90;
        ownship = ent;
        SelectionMgr.inst.selectedEntity = ent;
        DistanceMgr.inst.Initialize();
        added = false;
    }

    void SetWaypoints()
    {
        if(scenarioID == 1)
        {
            AIMgr.inst.HandleMove(ownship, new Vector3(730, 0, 770));
            AIMgr.inst.HandleMove(ownship, new Vector3(430, 0, 1690));
            AIMgr.inst.HandleMove(ownship, new Vector3(2290, 0, 2100));
            AIMgr.inst.HandleMove(ownship, new Vector3(1040, 0, 480));
            AIMgr.inst.HandleMove(ownship, new Vector3(-480, 0, 370));
        }
        else if(scenarioID == 3)
        {
            AIMgr.inst.HandleMove(ownship, new Vector3(1000, 0, 2000));
            Debug.Log("3");
        }
        else if(scenarioID == 4)
        {
            AIMgr.inst.HandleMove(ownship, new Vector3(4000, 0, 0));
            Debug.Log("4");
        }
            
        else
            AIMgr.inst.HandleMove(ownship, new Vector3(3000, 0, 3000));
    }

    void DestroyAllEntities()
    {
        int count = EntityMgr.inst.entities.Count;
        generatedEntites.Clear();
        entPositions.Clear();
        letMove.Clear();
        for (int i = 0; i < count; i++)
        {
            Entity381 ent = EntityMgr.inst.entities[0];
            EntityMgr.inst.entities.RemoveAt(0);
            Destroy(ent.gameObject);
        }
        LineMgr.inst.DestroyAllLines();
        DistanceMgr.inst.Initialize();
    }
}
