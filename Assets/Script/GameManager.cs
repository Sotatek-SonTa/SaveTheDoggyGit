using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SaveTheDoggyLevelManager;
using SaveTheDogUIManager;
using SaveTheDogSoundManager;

namespace SaveTheDoggyGamemanager
{
    public class GameManager : MonoBehaviour
    {
        public LineRenderer lineRender;
        public PolygonCollider2D polygonCollider2D;
        public Rigidbody2D lineRigibody2D;
        public float width = 0.1f;

        private List<Vector2> polygonPoints = new List<Vector2>();
        private List<Vector2> points = new List<Vector2>();
        private Vector2 lastValidPoint;
        public int levelIndex ;
        private bool isBlocked = false;

        public LevelManager levelManager;
        public UIManager uIManager;
        public bool isLoose;


        void Start()
        {
            Input.multiTouchEnabled = false;
            polygonCollider2D = lineRender.GetComponent<PolygonCollider2D>();
            lineRigibody2D = lineRender.GetComponent<Rigidbody2D>();
            lineRigibody2D.gravityScale = 0;
            lineRender.startColor = Color.black;
            lineRender.endColor = Color.black;
            points.Clear();
            polygonPoints.Clear();
            lineRender.positionCount = 0;
            polygonCollider2D.pathCount = 0;
            lineRender.widthMultiplier = 0.12f;
            levelManager.DogDead += AnnouceDogDead;
            LoadLevel(levelIndex);
        }

        private void AnnouceDogDead()
        {
            if(isLoose)
            {
                return;
            }
            uIManager.ShowingLostUI();
            uIManager.ShutDownCurrentTween();
            SoundManager.Instance.PlaySound(SoundName.LOOSE,0.5f);
            SoundManager.Instance.StopBGM();
            StopAllCoroutines();
            isLoose = true;
        }

        // Update is called once per frame
        void Update()
        {
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
                                uIManager.AdjustSlider(CanDraw(mousePosition));
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
                }
            }
        }
        void FixedUpdate()
        {
            UpdateLineRenderer();
        }
        void AddPoint(Vector2 newPoint)
        {
            Vector3 newPointwithZ = new Vector3(newPoint.x, newPoint.y, -1f);
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
                    SoundManager.Instance.PlaySound(SoundName.DRAWLINE,0.2f);
                }

                points.Add(newPoint);
                lineRender.positionCount = points.Count;
                lineRender.SetPosition(points.Count - 1, newPointwithZ);
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
                uIManager.ActiveTimer();
                uIManager.ProgressBarEffect();
                levelManager.StartLevel();
                SoundManager.Instance.ChangeBGMSound(SoundName.COUNTDOWN,0.7f);
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
                        worldPoint.z = -1f;
                        lineRender.SetPosition(i, worldPoint);
                    }
                }
            }
        }
        void LoadLevel(int levelIndex)
        {
            levelManager.LoadLevel(levelIndex);
            uIManager.SetUpCountDownTime();
        }
        public void OnClickTryAgain()
        {
            Refresh();
        }
        public void Refresh()
        {
            levelManager.doneDrawing = false;
            isLoose = false;
            levelManager.LoadLevel(levelIndex);
            uIManager.SetUpCountDownTime();
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
            uIManager.DisappearUI();
            uIManager.SetUpCountDownTime();
            SoundManager.Instance.ChangeBGMSound(SoundName.BGM,0.3f);
            StopAllCoroutines();
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
            return hit.collider == null || !hit.collider.CompareTag("Obstacle") || !hit.collider.CompareTag("Dog") || !hit.collider.CompareTag("Water") || !hit.collider.CompareTag("ToxicWater");
        }
        public IEnumerator CountDown()
        {
            yield return new WaitForSeconds(10f);
            uIManager.ShowingWinUI();
            SoundManager.Instance.PlaySound(SoundName.WIN,0.5f);
            SoundManager.Instance.StopBGM();
        }
          public void OnClickNextLevel()
        {
            levelIndex++;
            Refresh();
        }
    }
}

