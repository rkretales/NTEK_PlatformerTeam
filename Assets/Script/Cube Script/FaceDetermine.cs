using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDetermine : MonoBehaviour
{
    public Transform F;
    public Transform B;
    public Transform U;
    public Transform D;
    public Transform L;
    public Transform R;
    private List<GameObject> Fs = new List<GameObject>();
    private List<GameObject> Bs = new List<GameObject>();
    private List<GameObject> Us = new List<GameObject>();
    private List<GameObject> Ds = new List<GameObject>();
    private List<GameObject> Ls = new List<GameObject>();
    private List<GameObject> Rs = new List<GameObject>();
    private int layerMask = 1 << 0; // This layermask is for the faces of the cube only
    public bool shouldRevertSize = false;
    public GameObject emptyGO;

    // Start is called before the first frame update
    private void Start()
    {
        SetBuildFace();
    }

    // Update is called once per frame
    private void Update()
    {
        FaceReading(Fs, F);
        SetFaceRevert();
    }
    
    public List<GameObject> FaceReading(List<GameObject> rayStarts, Transform rayTransform)
    {
        List<GameObject> facesHit = new List<GameObject>();
        RaycastHit hit; // Declare the hit variable outside the loop

        foreach (GameObject rayStart in rayStarts)
        {
            Vector3 ray = rayStart.transform.position;
            GameObject objectHit = null;

            // Does the ray intersect any objects in the layerMask?
            if (Physics.Raycast(ray, rayTransform.forward, out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(ray, rayTransform.forward * hit.distance, Color.yellow);
                objectHit = hit.collider.gameObject;
                facesHit.Add(objectHit);

                BoxCollider boxCollider = objectHit.GetComponent<BoxCollider>();
                boxCollider.size = new Vector3(1.1f, 1.1f, 1.1f);
                // Debug.Log("Smaller");
            }
        }
        return facesHit;
    }

    public List<GameObject> FaceReverting(List<GameObject> rayStarts, Transform rayTransform)
    {
        List<GameObject> facesHit = new List<GameObject>();
        RaycastHit hit; // Declare the hit variable outside the loop

        foreach (GameObject rayStart in rayStarts)
        {
            Vector3 ray = rayStart.transform.position;
            GameObject objectHit = null;

            // Does the ray intersect any objects in the layerMask?
            if (Physics.Raycast(ray, rayTransform.forward, out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(ray, rayTransform.forward * hit.distance, Color.yellow);
                objectHit = hit.collider.gameObject;
                facesHit.Add(objectHit);

                BoxCollider boxCollider = objectHit.GetComponent<BoxCollider>();
                string tag = boxCollider.tag;
                switch (tag)
                {
                    case "White":
                    case "Yellow":
                        boxCollider.size = new Vector3(9, 1.1f, 1.1f);
                        // Debug.Log("Reverted!");
                        break;
                    case "Orange":
                    case "Red":
                        boxCollider.size = new Vector3(1.1f, 9, 1.1f);
                        break;
                    case "Green":
                    case "Blue":
                        boxCollider.size = new Vector3(1.1f, 1.1f, 9);
                        break;
                }
                // Debug.Log("Bigger");
            }
        }
        return facesHit;
    }


    List<GameObject> BuildFace(Transform rayTransform, Vector3 direction)
    {
        int rayCount = 0;
        List<GameObject> rays = new List<GameObject>();
        // This creates 9 rays in the shape of the sidde of the cube with
        // ray 0 at the top left and ray 8 aat the bottom right
        // 0|1|2
        // 3|4|5
        // 6|7|8

        for (int y = 1; y > -2; y--)
        {
            for (int x = -1; x < 2; x++)
            {
                Vector3 startPos = new Vector3(rayTransform.localPosition.x + x, 
                    rayTransform.localPosition.y + y, 
                    rayTransform.localPosition.z);
                GameObject rayStart = Instantiate(emptyGO, startPos, Quaternion.identity, rayTransform);
                rayStart.name = rayCount.ToString();
                rays.Add(rayStart);
                rayCount++;
            }
        }
        rayTransform.localRotation = Quaternion.Euler(direction);
        return rays;
    }

    private void SetFaceRevert()
    {
        FaceReverting(Bs, B);
        FaceReverting(Us, U);
        FaceReverting(Ds, D);
        FaceReverting(Ls, L);
        FaceReverting(Rs, R);
    }

    private void SetBuildFace()
    {
        Fs = BuildFace(F, new Vector3(0, 90, 0));
        Bs = BuildFace(B, new Vector3(0, 270, 0));
        Us = BuildFace(U, new Vector3(90, 90, 0));
        Ds = BuildFace(D, new Vector3(270, 90, 0));
        Ls = BuildFace(L, new Vector3(0, 180, 0));
        Rs = BuildFace(R, new Vector3(0, 90, 0));
    }
    
}
