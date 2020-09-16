using System.Collections;
using UnityEngine;

public class SwipeDrawer : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField] private Transform[] linePoints;
    [SerializeField] private float zOffset = 10;
    [SerializeField] private float followSpeed;
    
    private Vector3[] currentPositions;
    private Vector3 tmpPosition;

    private void OnEnable()
    {
        SwipeDetector.OnSwipe += SwipeDetector_OnSwipe;
    }
    
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        currentPositions = new Vector3[2];
    }

    private void SwipeDetector_OnSwipe(SwipeData data)
    {
        gameObject.SetActive(true);
        
        tmpPosition = data.StartPosition;
        tmpPosition.z = zOffset;
        linePoints[0].transform.position = Camera.main.ScreenToWorldPoint(tmpPosition);

        tmpPosition = data.EndPosition;
        tmpPosition.z = zOffset;
        linePoints[1].transform.position = Camera.main.ScreenToWorldPoint(tmpPosition);
        
        //Vector3 positions = new Vector3[2];
        //positions[0] = Camera.main.ScreenToWorldPoint(new Vector3(data.StartPosition.x, data.StartPosition.y, zOffset));
        //positions[1] = Camera.main.ScreenToWorldPoint(new Vector3(data.EndPosition.x, data.EndPosition.y, zOffset));
        //lineRenderer.positionCount = 2;
        //lineRenderer.SetPositions(positions);

        StartCoroutine(DelayedDisable(0.5f));
    }

    private void Update()
    {
        //currentPositions[0] = Vector3.Lerp(currentPositions[0], linePoints[0].position, Time.deltaTime * followSpeed);
        //currentPositions[1] = Vector3.Lerp(currentPositions[1], linePoints[1].position, Time.deltaTime * followSpeed);

        currentPositions[0] = linePoints[0].position;
        currentPositions[1] = linePoints[1].position;
        
        lineRenderer.SetPositions(currentPositions);
    }

    private IEnumerator DelayedDisable(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}