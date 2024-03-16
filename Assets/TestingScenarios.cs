using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        
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
            AIMgr.inst.HandleMove(ownship, new Vector3(3000, 0, 3000));
            added = true;
        }
            

        if (Input.GetKeyUp(KeyCode.F1))
        {
            DestroyAllEntities();
            RandomizedTraffic(numTraffic);
            SpawnOwnship();
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

        Mathf.Log(spawnX, 10);

        DistanceMgr.inst.Initialize();
    }

    void SpawnOwnship()
    {
        Vector3 spawn = new Vector3(0, 0, 0);
        Entity381 ent = EntityMgr.inst.CreateEntity(EntityType.PilotVessel, spawn, Vector3.zero);
        ownship = ent;
        SelectionMgr.inst.selectedEntity = ent;
        DistanceMgr.inst.Initialize();
        added = false;
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
