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
    public GameObject dogHead;
    public GameObject dogHead2;
    public Rigidbody2D dogHeadRigidbody;
    public Rigidbody2D dogHeadRigidbody2;
    public List<GameObject> beeList;
    public GameObject beeHive;
    public GameObject beeHive2;
    public int levelIndex = 0;
    public LevelManager levelManager;

    public Button nextLevel;
    public Button tryAgain;
    public TextMeshProUGUI announcer;
    public Canvas buttonGroup;
    private bool isBlocked = false;
    private Vector2 lastValidPoint;
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
        beeHive2 = GameObject.Find("Stup2");
        dogHead = GameObject.Find("Doghead");
        dogHead2 = GameObject.Find("Doghead2");

        dogHeadRigidbody = dogHead.GetComponent<Rigidbody2D>();
        if(dogHead2 !=null){
            dogHeadRigidbody2 = dogHead2.GetComponent<Rigidbody2D>();
        }
        if (!levelManager.doneDrawing)
        {
            if (Input.GetMouseButtonDown(0))
            {  
                    points.Clear();
                    polygonPoints.Clear();
                    lineRender.positionCount = 0;
                    polygonCollider2D.pathCount = 0;
                    isBlocked = false;
            }
            if (Input.GetMouseButton(0))
            {       
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
              
            if (isBlocked) 
            {
                if (CanResumeDrawing(mousePosition)) 
                {
                    isBlocked = false;
                    AddPoint(lastValidPoint); 
                }
            }
            else 
            {
                if (CanDraw(mousePosition))
                {
                    if (points.Count == 0 || Vector2.Distance(mousePosition, points.Last()) > 0.1f)
                    {
                        AddPoint(mousePosition);
                    }
                }
                else
                {
                    isBlocked = true; 
                    lastValidPoint = points.Last(); 
                }
            }
            }
            if (Input.GetMouseButtonUp(0))
            {
                    EndDrawing();
                    lineRigibody2D.gravityScale = 1;
                    if(dogHead2==null){
                        dogHeadRigidbody.gravityScale = 1;
                    }else
                    {
                    dogHeadRigidbody.gravityScale = 1;
                    dogHeadRigidbody2.gravityScale = 1;
                    }
                    
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
        beeHive2 = GameObject.Find("Stup2");
        dogHead = GameObject.Find("Doghead");
        dogHeadRigidbody = dogHead.GetComponent<Rigidbody2D>();
        dogHeadRigidbody.gravityScale = 0;
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
        announcer.gameObject.SetActive(false);
        tryAgain.gameObject.SetActive(false);
        nextLevel.gameObject.SetActive(false);
        StopAllCoroutines();
    }
    public void OnClickTryAgain()
    {
        levelManager.doneDrawing = false;
        levelManager.LoadLevel(levelIndex);
        lineRigibody2D.gravityScale = 0;
        points.Clear();
        polygonPoints.Clear();
        lineRender.positionCount = 0;
        polygonCollider2D.pathCount = 0;
        lineRigibody2D.bodyType = RigidbodyType2D.Kinematic;
        lineRender.transform.position = Vector3.zero;
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
        announcer.gameObject.SetActive(false);
        tryAgain.gameObject.SetActive(false);
        StopAllCoroutines();
    }
    public void OnClickPreviouLevel(){
        levelManager.doneDrawing = false;
        levelIndex--;
        levelManager.LoadLevel(levelIndex);
        lineRigibody2D.gravityScale = 0;
        points.Clear();
        polygonPoints.Clear();
        lineRender.positionCount = 0;
        polygonCollider2D.pathCount = 0;
        lineRigibody2D.bodyType = RigidbodyType2D.Kinematic;
        lineRender.transform.position = Vector3.zero;
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
        nextLevel.gameObject.SetActive(false);
        announcer.gameObject.SetActive(false);
        StopAllCoroutines();
    }
    IEnumerator BeeSpawn()
    {
        int beeCount = 0;
        while (beeCount < 7)
        {
            if(beeHive2 == null){
            GameObject beeSpawn1 = Instantiate(bee, beeHive.transform.position, Quaternion.identity);
            beeList.Add(beeSpawn1);
            beeCount++;
            yield return new WaitForSeconds(0.2f);
            }
            else
            {
            GameObject beeSpawn = Instantiate(bee, beeHive.transform.position, Quaternion.identity);
            beeCount++;
            yield return new WaitForSeconds(0.2f);
            GameObject beeSpawn2 = Instantiate(bee, beeHive2.transform.position, Quaternion.identity);
            beeCount++;
            beeList.Add(beeSpawn);
            beeList.Add(beeSpawn2);
            }
        }
    }
   bool CanDraw(Vector2 mousePosition)
{
    if (points.Count > 0)
    {
        Vector2 lastPoint = points.Last();
        RaycastHit2D hit = Physics2D.Linecast(lastPoint, mousePosition);

        if (hit.collider != null && hit.collider.CompareTag("Obstacle") || hit.collider != null && hit.collider.CompareTag("Dog") || hit.collider != null && hit.collider.CompareTag("Water") || hit.collider != null && hit.collider.CompareTag("ToxicWater"))
        {
            isBlocked = true; 
            return false;
        }
    }

    isBlocked = false; 
    return true;
}
bool CanResumeDrawing(Vector2 mousePosition)
{
    RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
    return hit.collider == null || !hit.collider.CompareTag("Obstacle") || !hit.collider.CompareTag("Dog") || !hit.collider.CompareTag("Water") || !hit.collider.CompareTag("ToxicWater") ;
}
    IEnumerator CountDown()
    {
       yield return new WaitForSeconds(10f);
       buttonGroup.gameObject.SetActive(true);
       nextLevel.gameObject.SetActive(true);
    }
}
