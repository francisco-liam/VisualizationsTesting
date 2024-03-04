using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityStandardAssets.Utility.TimedObjectActivator;

public class GameMgr : MonoBehaviour
{
    public static GameMgr inst;
    private GameInputs input;

    private void Awake()
    {
        inst = this;
        input = new GameInputs();
        input.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = Vector3.zero;
        foreach(GameObject go in EntityMgr.inst.entityPrefabs) {
            Entity381 ent = EntityMgr.inst.CreateEntity(go.GetComponent<Entity381>().entityType, position, Vector3.zero);
            position.x += 200;
        }
    }

    public Vector3 position;
    public float spread = 20;
    public float colNum = 10;
    // Update is called once per frame
    void Update()
    {
        if (input.Entities.Create100.triggered) {
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++) {
                    Entity381 ent = EntityMgr.inst.CreateEntity(EntityType.PilotVessel, position, Vector3.zero);
                    position.z += spread;
                }
                position.x += spread;
                position.z = 0;
            }
            DistanceMgr.inst.Initialize();
        }

        if (Input.GetKeyUp(KeyCode.F1))
        {
            DestroyAllEntities();
            RandomizedTraffic();
        }
    }

    void DestroyAllEntities()
    {
        int count = EntityMgr.inst.entities.Count;
        for (int i = 0; i < count; i++)
        {
            Entity381 ent = EntityMgr.inst.entities[0];
            EntityMgr.inst.entities.RemoveAt(0);
            Destroy(ent.gameObject);
        }
        DistanceMgr.inst.Initialize();
    }

    void RandomizedTraffic()
    {
        float spawnX = 0;

        for (int i = 0; i < 10; i++)
        {
            EntityType vesselType = (EntityType) Random.Range(0, 9);
            float spawnZ = Random.Range(0f, 2000f);

            Vector3 spawn = new Vector3(spawnX, 0 , spawnZ);

            Entity381 ent = EntityMgr.inst.CreateEntity(vesselType, spawn, Vector3.zero);

            spawnX += 200;
        }

        Mathf.Log(spawnX, 10);

        DistanceMgr.inst.Initialize();
    }
}
