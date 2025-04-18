using UnityEngine;

[ExecuteInEditMode]
public class BoxGenerator : MonoBehaviour
{
    public float height = 2f;

    [ContextMenu("Generate Box Collider")]
    public void GenerateBox()
    {
        MeshFilter meshFilter = GetComponentInChildren<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("No MeshFilter found under this object.");
            return;
        }

        Mesh mesh = meshFilter.sharedMesh;
        if (mesh == null)
        {
            Debug.LogError("No Mesh found in MeshFilter.");
            return;
        }

        Vector3 scale = meshFilter.transform.lossyScale;
        Vector3 size = Vector3.Scale(mesh.bounds.size, scale);
        Vector3 worldCenter = meshFilter.transform.TransformPoint(mesh.bounds.center);
        Vector3 localCenter = transform.InverseTransformPoint(worldCenter);

        Transform existingChild = transform.Find("BoxCollider_Child");
        if (existingChild != null)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                Destroy(existingChild.gameObject);
            else
                DestroyImmediate(existingChild.gameObject);
#endif
        }

        GameObject child = new GameObject("BoxCollider_Child");
        child.transform.SetParent(transform);
        child.transform.localPosition = localCenter + new Vector3(0, height / 2f, 0);
        child.transform.localRotation = Quaternion.identity;

        BoxCollider col = child.AddComponent<BoxCollider>();
        col.size = new Vector3(size.x, height, size.z);
        col.center = Vector3.zero;
    }
}
