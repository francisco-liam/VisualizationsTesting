using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Move : Command
{
    public Vector3 movePosition;
    public Move(Entity381 ent, Vector3 pos) : base(ent)
    {
        movePosition = pos;
    }

    public LineRenderer potentialLine;
    public override void Init()
    {
        //Debug.Log("MoveInit:\tMoving to: " + movePosition);
        line = LineMgr.inst.CreateMoveLine(entity.position, movePosition);
        line.gameObject.SetActive(false);
        potentialLine = LineMgr.inst.CreatePotentialLine(entity.position);
        line.gameObject.SetActive(true);
    }

    public override void Tick()
    {
        DHDS dhds;
        if (AIMgr.inst.isPotentialFieldsMovement)
            dhds = ComputePotentialDHDS();
        else
            dhds = ComputeDHDS();

        entity.desiredHeading = dhds.dh;
        entity.desiredSpeed = dhds.ds;
        line.SetPosition(1, movePosition);
    }

    public Vector3 diff = Vector3.positiveInfinity;
    public float dhRadians;
    public float dhDegrees;
    public DHDS ComputeDHDS()
    {
        diff = movePosition - entity.position;
        dhRadians = Mathf.Atan2(diff.x, diff.z);
        dhDegrees = Utils.Degrees360(Mathf.Rad2Deg * dhRadians);
        return new DHDS(dhDegrees, entity.maxSpeed);

    }

    public DHDS ComputePotentialDHDS()
    {
        Vector3 potentialSum = ComputePotentials(entity.position);

        dh = Utils.Degrees360(Mathf.Rad2Deg * Mathf.Atan2(potentialSum.x, potentialSum.z));

        angleDiff = Utils.Degrees360(Utils.AngleDiffPosNeg(dh, entity.heading));
        cosValue = (Mathf.Cos(angleDiff * Mathf.Deg2Rad) + 1) / 2.0f; // makes it between 0 and 1
        ds = entity.maxSpeed * cosValue;

        return new DHDS(dh, ds);
    }

    public Vector3 ComputePotentials(Vector3 pos)
    {
        float waypointCoefficient = AIMgr.inst.waypointCoefficient;
        float waypointExponent = AIMgr.inst.waypointExponent;
        float attractiveCoefficient = AIMgr.inst.attractiveCoefficient;
        float attractiveExponent = AIMgr.inst.attractiveExponent;
        float repulsiveCoefficient = AIMgr.inst.repulsiveCoefficient;
        float repulsiveExponent = AIMgr.inst.repulsiveExponent;
        float bearingAngle = AIMgr.inst.bearingAngle;
        float bearingAngleExp = AIMgr.inst.bearingAngleExp;
        float bearingCoefficient = AIMgr.inst.bearingCoefficient;
        float bearingExponent = AIMgr.inst.bearingExponent;
        float taAngle = AIMgr.inst.taAngle;
        float taAngleExp = AIMgr.inst.taAngleExp;
        float taCoefficient = AIMgr.inst.taCoefficient;
        float taExponent = AIMgr.inst.taExponent;

        Potential p;

        Vector3 waypointDist = movePosition - pos;
        Vector3 waypointField =  waypointDist.normalized * waypointCoefficient * Mathf.Pow(waypointDist.magnitude, waypointExponent);

        Vector3 repulsiveField = Vector3.zero;
        Vector3 attractiveField = Vector3.zero;
        Vector3 bearingField = Vector3.zero;
        Vector3 taField = Vector3.zero;

        foreach (Entity381 ent in EntityMgr.inst.entities)
        {
            if (ent == entity) continue;

            p = DistanceMgr.inst.GetPotential(entity, ent);

            Vector3 posDiff = ent.position - pos;

            Vector3 starboard = Vector3.Normalize(Vector3.Cross(entity.velocity, Vector3.down));

            float relBearing = Mathf.Atan2((ent.position - entity.position).x, (ent.position - entity.position).z) * Mathf.Rad2Deg - entity.heading;
            float targetAngle = Mathf.Atan2((entity.position - ent.position).x, (entity.position - ent.position).z) * Mathf.Rad2Deg - ent.heading;

            float bAngle = Mathf.Sin((relBearing + bearingAngle) * Mathf.Deg2Rad);
            float tAngle = Mathf.Sin((targetAngle + taAngle) * Mathf.Deg2Rad);

            Vector3 repField = Mathf.Pow(posDiff.magnitude, repulsiveExponent) * repulsiveCoefficient * -posDiff.normalized;
            Vector3 attField = Mathf.Pow(posDiff.magnitude, attractiveExponent) * attractiveCoefficient * posDiff.normalized;
            Vector3 crossPosField = Mathf.Pow(0.5f * (bAngle + 1), bearingAngleExp) * Mathf.Pow(posDiff.magnitude, bearingExponent) * bearingCoefficient * starboard;
            Vector3 crossVelField = Mathf.Pow(0.5f * (tAngle + 1), taAngleExp) * Mathf.Pow(posDiff.magnitude, taExponent) * taCoefficient * starboard;

            repulsiveField += repField;
            attractiveField += attField;
            bearingField += crossPosField;
            taField += crossVelField;
        }

        Vector3 totalPotential = waypointField + repulsiveField + attractiveField + bearingField + taField;

        return totalPotential;
    }

    public Vector3 attractivePotential = Vector3.zero;
    public Vector3 potentialSum = Vector3.zero;
    public Vector3 repulsivePotential = Vector3.zero;
    public float dh;
    public float angleDiff;
    public float cosValue;
    public float ds;



    public float doneDistanceSq = 1000;
    public override bool IsDone()
    {

        return ((entity.position - movePosition).sqrMagnitude < doneDistanceSq);
    }

    public override void Stop()
    {
        entity.desiredSpeed = 0;
        LineMgr.inst.DestroyLR(line);
        LineMgr.inst.DestroyLR(potentialLine);
    }
}
