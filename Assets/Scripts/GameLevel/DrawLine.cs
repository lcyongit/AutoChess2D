using System;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject linePrefab;

    private List<Vector3> linePointList = new();
    private List<GameObject> lineList = new();

    public void GenerateCurvedLine(List<List<GameLevelNode>> allGameLevelNodeList, int curveVertexCount = 0)
    {
        // 清除現有line
        int index = lineList.Count;

        for (int i = 0; i < index; i++)
        {
            Destroy(lineList[i]);
        }

        lineList.Clear();

        for (int i = 0; i < allGameLevelNodeList.Count - 1; i++)
        {
            foreach (var startNode in allGameLevelNodeList[i])
            {
                Vector3 startNodePos = startNode.transform.position;
                
                foreach (var endNode in startNode.to)
                {
                    linePointList.Clear();

                    Vector3 endNodePos = endNode.transform.position;

                    // 隨機產生彎曲頂點位置
                    float distance = (startNodePos - endNodePos).magnitude;
                    float halfDistance = distance / 2f;
                    Vector3 pathDirection = (endNodePos - startNodePos).normalized;
                    Vector3 curveVertexPos = startNodePos + (pathDirection * halfDistance);

                    //float randomX = UnityEngine.Random.Range(0, 2);
                    //if (randomX == 0)
                    //    randomX = UnityEngine.Random.Range(-(distance / 4f), 0.5f);
                    //else
                    //    randomX = UnityEngine.Random.Range((distance / 4f), -0.5f);

                    //float randomY = UnityEngine.Random.Range(0, 2);
                    //if (randomY == 0)
                    //    randomY = UnityEngine.Random.Range(-(distance / 4f), -0.5f);
                    //else
                    //    randomY = UnityEngine.Random.Range((distance / 4f), 0.5f);

                    float randomX = UnityEngine.Random.Range(-1f, 1f);
                    float randomY = UnityEngine.Random.Range(-1f, 1f);

                    curveVertexPos += new Vector3(randomX, randomY, 0f);

                    curveVertexCount = Mathf.Max(curveVertexCount, 0);

                    for (float t = 0; t <= 1f; t += 1f / (curveVertexCount + 1)) // 彎曲頂點 + 終點
                    {
                        Vector3 posStartToMiddle = Vector3.Lerp(startNodePos, curveVertexPos, t);
                        Vector3 posMiddleToEnd = Vector3.Lerp(curveVertexPos, endNodePos, t);

                        Vector3 curve = Vector3.Lerp(posStartToMiddle, posMiddleToEnd, t);

                        linePointList.Add(curve);

                    }

                    var line = Instantiate(linePrefab).GetComponent<LineRenderer>();
                    line.positionCount = curveVertexCount + 2; // 彎曲頂點 + 起點 + 終點
                    line.SetPositions(linePointList.ToArray());

                    lineList.Add(line.gameObject);

                }

            }

        }

        

    }


}
