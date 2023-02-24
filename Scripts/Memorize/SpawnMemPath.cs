using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMemPath : MonoBehaviour
{
    public static SpawnMemPath instance;




    [SerializeField] private ColorType color;
    [SerializeField] private GameObject[] memPathPrefabs; //0 = blue, 1 = orange
    [SerializeField] private Transform memPathSpawnPoint;
    [SerializeField] private float waitTimeBeforeSpawn;


    private void Awake()
    {
        instance = this;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var weAreNotRecording = Recordings.instance.recordings[0].IsRecording == false && Recordings.instance.recordings[1].IsRecording == false;
            if (weAreNotRecording)
            {
                switch (color)
                {
                    case ColorType.BLUE:
                        if (Recordings.instance.recordings[0].RecordedDatas.Count > 0)
                        {
                            Invoke(nameof(InstantiateBlueMempath), waitTimeBeforeSpawn);
                        }
                        break;
                    case ColorType.ORANGE:
                        if (Recordings.instance.recordings[1].RecordedDatas.Count > 0)
                        {
                            Invoke(nameof(InstantiateOrangeMempath), waitTimeBeforeSpawn);
                        }
                        break;
                }
            }
        }
    }

    private void InstantiateBlueMempath()
    {
        var memPath = Instantiate(memPathPrefabs[0], memPathSpawnPoint);
        memPath.transform.parent = null; //otherwise it is stretched in a bad way
        memPath.transform.localScale = new Vector3(1, 1, 1);
    }

    private void InstantiateOrangeMempath()
    {
        var memPath = Instantiate(memPathPrefabs[1], memPathSpawnPoint);
        memPath.transform.parent = null; //otherwise it is stretched in a bad way
        memPath.transform.localScale = new Vector3(1, 1, 1);
    }





    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DestroyAllMemPaths();
        }

    }

    public void DestroyAllMemPaths()
    {
        var allMemPaths = FindObjectsOfType<MemPath>();
        foreach (MemPath memPath in allMemPaths)
        {
            Destroy(memPath.gameObject);
        }
    }

    public enum ColorType
    {
        BLUE,
        ORANGE
    }
}
