using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WallGenerator : MonoBehaviour
{
    public float wallHeight = 2f;
    public float wallThickness = 0.1f;

    [ContextMenu("Build Walls")]
    public void BuildWalls()
    {
        ClearOldWalls();

        Vector3 floorSize = GetFloorSize(out Vector3 floorCenter);
        float width = floorSize.x;
        float depth = floorSize.z;

        float halfWidth = width / 2f;
        float halfDepth = depth / 2f;

        BuildWallOnSide("North", Vector3.forward, floorCenter + Vector3.forward * halfDepth, width, Vector3.right);
        BuildWallOnSide("South", Vector3.back, floorCenter + Vector3.back * halfDepth, width, Vector3.right);
        BuildWallOnSide("East", Vector3.right, floorCenter + Vector3.right * halfWidth, depth, Vector3.forward);
        BuildWallOnSide("West", Vector3.left, floorCenter + Vector3.left * halfWidth, depth, Vector3.forward);
    }

    void BuildWallOnSide(string sideName, Vector3 dir, Vector3 wallCenterWorld, float fullLength, Vector3 alongDir)
    {
        List<BoxCollider> gates = new List<BoxCollider>();

        foreach (Transform child in transform)
        {
            if (!child.name.StartsWith("Gate")) continue;

            BoxCollider[] childColliders = child.GetComponentsInChildren<BoxCollider>();
            foreach (var gateCollider in childColliders)
            {
                Vector3 gateCenter = gateCollider.bounds.center;
                Vector3 localToWall = gateCenter - wallCenterWorld;

                float posOnNormalAxis = Vector3.Dot(localToWall, dir);
                if (Mathf.Abs(posOnNormalAxis) < 0.2f) // small tolerance
                {
                    gates.Add(gateCollider);
                    Debug.Log($"Gate {gateCollider.name} added to {sideName} wall.");
                }
            }
        }

        if (gates.Count == 0)
        {
            CreateWallPiece($"Wall_{sideName}_Full", wallCenterWorld, fullLength, alongDir);
            return;
        }

        gates.Sort((a, b) =>
        {
            float aPos = Vector3.Dot(a.bounds.center - wallCenterWorld, alongDir);
            float bPos = Vector3.Dot(b.bounds.center - wallCenterWorld, alongDir);
            return aPos.CompareTo(bPos);
        });

        float halfLength = fullLength / 2f;
        float prev = -halfLength;

        foreach (var gate in gates)
        {
            Vector3 gateCenter = gate.bounds.center;
            float gateSize = Vector3.Dot(gate.size, alongDir.normalized);
            float gatePos = Vector3.Dot(gateCenter - wallCenterWorld, alongDir);

            float gateMin = gatePos - gateSize / 2f;
            float gateMax = gatePos + gateSize / 2f;

            float leftLength = gateMin - prev;
            if (leftLength > 0.01f)
            {
                Vector3 segmentCenter = wallCenterWorld + alongDir * (prev + leftLength / 2f);
                CreateWallPiece($"Wall_{sideName}_Segment", segmentCenter, leftLength, alongDir);
            }

            prev = gateMax;
        }

        float rightLength = halfLength - prev;
        if (rightLength > 0.01f)
        {
            Vector3 segmentCenter = wallCenterWorld + alongDir * (prev + rightLength / 2f);
            CreateWallPiece($"Wall_{sideName}_Segment", segmentCenter, rightLength, alongDir);
        }
    }

    void CreateWallPiece(string name, Vector3 worldPos, float length, Vector3 alongDir)
    {
        GameObject wall = new GameObject(name);
        wall.transform.SetParent(transform);
        wall.transform.position = worldPos;
        wall.transform.rotation = Quaternion.identity;

        BoxCollider col = wall.AddComponent<BoxCollider>();
        col.size = new Vector3(
            Mathf.Abs(alongDir.x) > 0 ? length : wallThickness,
            wallHeight,
            Mathf.Abs(alongDir.z) > 0 ? length : wallThickness
        );
        col.center = new Vector3(0, wallHeight / 2f, 0);
    }

    Vector3 GetFloorSize(out Vector3 center)
    {
        MeshFilter meshFilter = GetComponentInChildren<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("No MeshFilter found for floor.");
            center = transform.position;
            return new Vector3(10, 1, 10);
        }

        Mesh mesh = meshFilter.sharedMesh;
        Vector3 size = Vector3.Scale(mesh.bounds.size, meshFilter.transform.lossyScale);
        center = meshFilter.transform.position + Vector3.Scale(mesh.bounds.center, meshFilter.transform.lossyScale);
        return size;
    }

    void ClearOldWalls()
    {
        List<Transform> toDestroy = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("Wall_"))
                toDestroy.Add(child);
        }

#if UNITY_EDITOR
        foreach (var t in toDestroy)
        {
            if (Application.isPlaying) Destroy(t.gameObject);
            else DestroyImmediate(t.gameObject);
        }
#endif
    }
}
