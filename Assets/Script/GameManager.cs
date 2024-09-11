using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public LineRenderer lineRender;
    public PolygonCollider2D polygonCollider2D;
    public Rigidbody2D lineRigibody2D;
    public float width = 0.1f;
    private List<Vector2> polygonPoints = new List<Vector2>();
    private List<Vector2> points = new List<Vector2>();
    public GameObject bee;
    public List<GameObject> beeList;
    public GameObject beeHive;
    public int levelIndex = 0;
    public LevelManager levelManager;

    public Button nextLevel;
    public Button tryAgain;
    public TextMeshProUGUI announcer;
    public Canvas buttonGroup;
    void Start()
    {
        polygonCollider2D = lineRender.GetComponent<PolygonCollider2D>();
        lineRigibody2D = lineRender.GetComponent<Rigidbody2D>();
        lineRigibody2D.gravityScale = 0;
        points.Clear();
        polygonPoints.Clear();
        lineRender.positionCount = 0;
        polygonCollider2D.pathCount = 0;
        LoadLevel(0);
    }

    // Update is called once per frame
    void Update()
    {
        beeHive = GameObject.Find("Stup");
        if (!levelManager.doneDrawing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                    points.Clear();
                    polygonPoints.Clear();
                    lineRender.positionCount = 0;
                    polygonCollider2D.pathCount = 0;
            }
            if (Input.GetMouseButton(0))
            {
                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (points.Count == 0 || Vector2.Distance(mousePosition, points.Last()) > 0.1f)
                    {
                        AddPoint(mousePosition);
                    }
            }
            if (Input.GetMouseButtonUp(0))
            {
                    EndDrawing();
                   // levelManager.doneDrawing = true;
                    lineRigibody2D.gravityScale = 1;
            }
        }

    }
    void FixedUpdate()
    {
        UpdateLineRenderer();
    }
    void AddPoint(Vector2 newPoint)
    {
        if (newPoint != Vector2.zero)
        {
            if (points.Count > 0)
            {
                Vector2 lastPoint = points[points.Count - 1];
                Vector2 direction = (newPoint - lastPoint).normalized;


                Vector2 perpendicular = new Vector2(-direction.y, direction.x);


                Vector2 offset = perpendicular * (lineRender.startWidth / 2);
                Vector2 point1 = newPoint + offset;
                Vector2 point2 = newPoint - offset;


                polygonPoints.Add(point1);
                polygonPoints.Insert(0, point2);
            }

            points.Add(newPoint);
            lineRender.positionCount = points.Count;
            lineRender.SetPosition(points.Count - 1, newPoint);
        }

    }
    void EndDrawing()
    {
        if (points.Count > 1)
        {
            levelManager.doneDrawing = true;
            polygonCollider2D.pathCount = 1;
            polygonCollider2D.SetPath(0, polygonPoints.ToArray());
             lineRigibody2D.bodyType = RigidbodyType2D.Dynamic;
            StartCoroutine(BeeSpawn());
            StartCoroutine(CountDown());
        }
    }
    void UpdateLineRenderer()
    {
        if (polygonCollider2D != null && polygonCollider2D.pathCount > 0)
        {
            Vector2[] colliderPoints = polygonCollider2D.GetPath(0);
            if (colliderPoints != null)
            {
                lineRender.positionCount = colliderPoints.Length / 2;
                for (int i = 0; i < colliderPoints.Length / 2; i++)
                {
                    Vector3 worldPoint = polygonCollider2D.transform.TransformPoint(colliderPoints[i]);
                    lineRender.SetPosition(i, worldPoint);
                }
            }
        }
    }
    void LoadLevel(int levelIndex)
    {
        levelManager.LoadLevel(levelIndex);
        beeHive = GameObject.Find("Stup");
    }

    public void OnClickNextLevel()
    {
        levelManager.doneDrawing = false;
        levelIndex++;
        levelManager.LoadLevel(levelIndex);
        lineRigibody2D.gravityScale = 0;
        points.Clear();
        polygonPoints.Clear();
        lineRender.positionCount = 0;
        polygonCollider2D.pathCount = 0;
        lineRender.transform.position = Vector3.zero;

        for (int i = 0; i < beeList.Count; i++)
        {
            Destroy(beeList[i]);
        }
        beeList.Clear();
        buttonGroup.gameObject.SetActive(false);
        nextLevel.gameObject.SetActive(false);
        announcer.gameObject.SetActive(false);
        StopAllCoroutines();
    }
    public void OnClickTryAgain()
    {
        levelManager.LoadLevel(levelIndex);
        points.Clear();
        polygonPoints.Clear();
        lineRender.positionCount = 0;
        polygonCollider2D.pathCount = 0;
        lineRigibody2D.gravityScale = 0;
        lineRigibody2D.bodyType = RigidbodyType2D.Kinematic;
        lineRender.transform.position = Vector3.zero;
        lineRigibody2D.velocity = Vector2.zero;
        lineRigibody2D.angularVelocity = 0f;
        lineRender.transform.rotation = Quaternion.identity;
        for (int i = 0; i < beeList.Count; i++)
        {
            Destroy(beeList[i]);
        }
        beeList.Clear();
        buttonGroup.gameObject.SetActive(false);
        tryAgain.gameObject.SetActive(false);
        announcer.gameObject.SetActive(false);
        levelManager.doneDrawing = false;
        StopAllCoroutines();
    }
    IEnumerator BeeSpawn()
    {
        int beeCount = 0;
        while (beeCount < 7)
        {
            GameObject beeSpawn = Instantiate(bee, beeHive.transform.position, Quaternion.identity);
            beeList.Add(beeSpawn);
            beeCount++;
            yield return new WaitForSeconds(0.2f);
        }
    }
    IEnumerator CountDown()
    {
       yield return new WaitForSeconds(10f);
       buttonGroup.gameObject.SetActive(true);
       nextLevel.gameObject.SetActive(true);
    }
}
