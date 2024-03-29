using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : MonoBehaviour
{
    public float distance = 10;
    public float angle = 30;
    public float height = 1.0f;
    public Color meshColor = Color.red;
    public int scanFrequency = 30;
    public LayerMask layers;
    public List<GameObject> Objects = new List<GameObject>();
    public LayerMask occlusionLayers;
    private GameObject playerObj = null;
    private bool spotted = false;

    Collider[] colliders = new Collider[50];
    Mesh mesh;
    int count;
    float scanInterval;
    float scanTimer;

    public delegate void MonsterSpotted();
    public static event MonsterSpotted WhenMonsterSpotted;

    public delegate void MonsterNotSpotted();
    public static event MonsterNotSpotted WhenMonsterNotSpotted;

    // Start is called before the first frame update
    void Start()
    {
        scanInterval = 1.0f / scanFrequency;
        if (playerObj == null)
            playerObj = GameObject.Find("PlayerCapsule");
    }

    // Update is called once per frame
    void Update()
    {
        
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer = .05f;
            Scan();
        }
        
        //Scan();
    }

    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layers, QueryTriggerInteraction.Collide);

        Objects.Clear();
        for (int i = 0; i < count; ++i)
        {
            GameObject obj = colliders[i].gameObject;
            if (IsInSight(obj))
            {
                Objects.Add(obj);
                //print("playSound!!");
                Spotted();
            }
            else
            {
                spotted = false;
                if (WhenMonsterNotSpotted != null)
                    WhenMonsterNotSpotted();
            }
        }
    }

    private void Spotted()
    {
        //print("playSound!!");
        if (!spotted)
        {
            spotted = true;
            if (WhenMonsterSpotted != null)
                WhenMonsterSpotted();
        }
    }

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;
        if (direction.y < 0 || direction.y > height)
        {
            return false;
        }

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > angle)
        {
            return false;
        }

        origin.y += height / 2;
        dest.y = origin.y;
        if (Physics.Linecast(origin, dest, occlusionLayers))
        {
            return false;
        }

        return true;
    }

    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int numTriangles = 8;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

        int vert = 0;

        //left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        //far side
        vertices[vert++] = bottomLeft;
        vertices[vert++] = bottomRight;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = topLeft;
        vertices[vert++] = bottomLeft;

        //top
        vertices[vert++] = topCenter;
        vertices[vert++] = topLeft;
        vertices[vert++] = topRight;

        //bottom
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomLeft;

        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
        scanInterval = 1.0f / scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }


    }

}
