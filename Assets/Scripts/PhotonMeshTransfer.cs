using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class PhotonMeshTransfer : MonoBehaviourPun
{
    public float testLastSendGametime = 0;
    public GameObject meshPrefab = null;
    public Material meshMaterial = null;
    // Start is called before the first frame update
    void Start()
    {
        testLastSendGametime = Time.realtimeSinceStartup;
        if(!meshPrefab)
        {
            Debug.LogError("You screwed up when setting up this gameobject. meshPrefab is null.");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float timeDif = Time.realtimeSinceStartup - testLastSendGametime;
        if(timeDif> 0.1f)
        {
            testLastSendGametime = Time.realtimeSinceStartup;

            //generate arbitrary mesh data, in this case a triangle

            Vector3 pos = Random.insideUnitSphere * Random.Range(0, 100.0f);
            Vector3[] verts = { Random.insideUnitSphere, Random.insideUnitSphere, Random.insideUnitSphere };
            int[] indexors = { 0, 1, 2, 1, 0, 2 };
            PhotonView pv = this.photonView;
            pv.RPC("receiveMeshData", RpcTarget.All, pos, (object)verts, (object)indexors);
        }
    }

    [PunRPC]
    void receiveMeshData(Vector3 pos, Vector3[] verts, int[] indecieies)
    {
        Debug.Log(pos);
        Debug.Log(verts[2]);
        Debug.Log(indecieies[3]);

        GameObject newMesh = GameObject.Instantiate(meshPrefab);
        newMesh.transform.position = pos;
        Mesh meesh = new Mesh();
        meesh.vertices = verts;
        meesh.SetIndices(indecieies, MeshTopology.Triangles,0);
        meesh.RecalculateBounds();
        meesh.RecalculateNormals();
        meesh.RecalculateTangents();
        newMesh.GetComponent<MeshFilter>().mesh = meesh;
        newMesh.GetComponent<MeshRenderer>().material = meshMaterial;
    }
}
