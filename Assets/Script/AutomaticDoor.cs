using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    public Transform doorPanel;
    public float openHeight = 3f;
    public float openSpeed = 2f;
    public AudioSource doorSound;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpening = false;
    private bool isClosing = false;

    void Start()
    {
        if (doorPanel == null)
        {
            doorPanel = transform;
        }

        closedPosition = doorPanel.position;
        openPosition = closedPosition + Vector3.up * openHeight;
    }

    void Update()
    {
        if (isOpening)
        {
            doorPanel.position = Vector3.MoveTowards(doorPanel.position, openPosition, openSpeed * Time.deltaTime);
            
            if (Vector3.Distance(doorPanel.position, openPosition) < 0.01f)
            {
                doorPanel.position = openPosition;
                isOpening = false;
            }
        }
        
        if (isClosing)
        {
            doorPanel.position = Vector3.MoveTowards(doorPanel.position, closedPosition, openSpeed * Time.deltaTime);
            
            if (Vector3.Distance(doorPanel.position, closedPosition) < 0.01f)
            {
                doorPanel.position = closedPosition;
                isClosing = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpening = true;
            isClosing = false;
            
            if (doorSound != null)
            {
                doorSound.Play();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpening = false;
            isClosing = true;
            
            if (doorSound != null)
            {
                doorSound.Play();
            }
        }
    }
}
