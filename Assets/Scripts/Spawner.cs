using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Block _blockTemplate;
    [SerializeField] private Obstacle _obstacleTemplate;
    [SerializeField] private Finish _finishTemplate;
    [SerializeField] private int _towerSize;

    private void Start()
    {
        BildTower();
    }
    private void BildTower()
    {
        GameObject currentSegment = gameObject;

        for (int i = 0; i < _towerSize; i++)
        {
            currentSegment = BuildSegment(currentSegment, _blockTemplate.gameObject);

            currentSegment = BuildSegment(currentSegment, _obstacleTemplate.gameObject);
        }

        currentSegment = BuildSegment(currentSegment, _finishTemplate.gameObject);

    }

    private GameObject BuildSegment(GameObject currentSegnemtent, GameObject nextSegment)
    {
        return Instantiate(nextSegment, GetBuildPoint(currentSegnemtent.transform, nextSegment.transform), Quaternion.identity, transform);
    }

    private Vector3 GetBuildPoint(Transform currentSegnemtent, Transform nextSegment)
    {
        return new Vector3(currentSegnemtent.position.x, currentSegnemtent.position.y + currentSegnemtent.localScale.y / 2f + nextSegment.localScale.y / 2f, currentSegnemtent.position.z);
    }
}
