using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScenarios : MonoBehaviour
{
    public List<Entity381> generatedEntites = new List<Entity381>();
    public List<Vector3> entPositions = new List<Vector3>();
    public List<bool> letMove = new List<bool>();

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

        if (Input.GetKeyUp(KeyCode.F1))
        {
            DestroyAllEntities();
            RandomizedTraffic();
        }
    }

    void RandomizedTraffic()
    {
        float spawnX = 0;

        for (int i = 0; i < 10; i++)
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
        DistanceMgr.inst.Initialize();
    }
}
