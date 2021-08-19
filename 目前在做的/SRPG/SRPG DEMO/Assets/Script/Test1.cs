using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test1 : MonoBehaviour
{
    [SerializeField] GameObject testObject;

    [SerializeField] PathFinder pathFinder;


    public void Test()
    {
        /*
        SrpgClass testS = testObject.GetComponent<SrpgClass>();
        testS.InitClass();
        Vector3Int targetPosition = new Vector3Int((int)gameObject.transform.position.x, (int)gameObject.transform.position.y, 0);
        StartCoroutine(testS.MoveToPosition(targetPosition));\
        */
        testObject.GetComponent<SrpgClassUnit>().UpdatePosition(new Vector3Int((int)testObject.transform.position.x, (int)testObject.transform.position.y, 0));
        pathFinder.CreatMoveRenge(testObject.GetComponent<SrpgClassUnit>());

        var queue = pathFinder.AstarCreatMovePath(testObject.GetComponent<SrpgClassUnit>().m_Position, new Vector3Int(4, 0, 0));
        Debug.Log(queue.Count);
        foreach(var cD in queue)
        {
            Debug.Log($"x = {cD.m_Position.x} , y = {cD.m_Position.y}");
        }
        var srpgClass = testObject.GetComponent<SrpgClassUnit>();
        srpgClass.InitClass();

        StartCoroutine(srpgClass.StartPathMove(queue));

    }



}
