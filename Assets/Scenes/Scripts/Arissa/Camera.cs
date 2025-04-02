using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 400f;
    public Transform playerBody;
    public Vector3 thirdPersonOffset = new Vector3(0, 2f, -1.95f); // Posición en tercera persona
    public Vector3 firstPersonOffset = new Vector3(0, 1.6f, 0); // Posición en primera persona

    private float xRotation = 0f;
    private bool isFirstPerson = false; // Estado de la cámara

    public float rayDistance = 5f; // Distancia máxima del rayo
    public LayerMask interactableLayer;
    public LayerMask ignoreLayer; // Capa a ignorar


    public KeyCode interactionKey = KeyCode.E; // tecla de acciones

    private GameObject lastHighlightedObject = null;
    private Color lastHighlightedObjectColor;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Cambio de cámara al presionar "C"
        if (Input.GetKeyDown(KeyCode.C))
        {
            isFirstPerson = !isFirstPerson; // Alternar entre primera y tercera persona
        }

        if (isFirstPerson)
        {
            // Primera persona: Cámara fija al cuerpo del jugador
            transform.position = Vector3.Lerp(transform.position, playerBody.position + firstPersonOffset, Time.deltaTime * 10f);
            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);


        }
        else
        {
            // Tercera persona: Mantener la cámara detrás del jugador
            Quaternion rotation = Quaternion.Euler(xRotation, playerBody.eulerAngles.y, 0);
            Vector3 targetPosition = playerBody.position + rotation * thirdPersonOffset;

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);
            transform.LookAt(playerBody.position + Vector3.up * 1.5f);
        }

        // Rotar el jugador con el mouse
        playerBody.Rotate(Vector3.up * mouseX);


        // Creamos un rayo desde la cámara en la dirección que está mirando
        RaycastHit hit;

        Ray ray = new Ray(transform.position, transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);

        // Comprobamos si el rayo impacta con algún objeto
        if (Physics.Raycast(ray, out hit, rayDistance, ~ignoreLayer))
        {
            // Verifica si el objeto que tocó el rayo es interactuable

            if (hit.collider.CompareTag("Interactuable"))
            {
                // Resalta el objeto si aún no lo está resaltando
                if (lastHighlightedObject != hit.transform.gameObject)
                {
                    // Restauramos el color original del objeto anterior
                    if (lastHighlightedObject != null)
                    {
                        lastHighlightedObject.GetComponent<MeshRenderer>().material.color = lastHighlightedObjectColor;
                    }

                    // Almacenamos el objeto actual y su color original
                    lastHighlightedObject = hit.transform.gameObject;
                    lastHighlightedObjectColor = lastHighlightedObject.GetComponent<MeshRenderer>().material.color;

                    // Cambiamos el color del objeto actual
                    lastHighlightedObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
                }

                // Si el usuario presiona la tecla de interacción
                if (Input.GetKeyDown(interactionKey))
                {
                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                    if (interactable != null)
                    {
                        interactable.Interact(); // Ejecuta el método según el objeto detectado
                    }
                }
            }
        }
        else
        {
            // Si el rayo no toca ningún objeto, restauramos el color del objeto resaltado previamente
            if (lastHighlightedObject != null)
            {
                lastHighlightedObject.GetComponent<MeshRenderer>().material.color = lastHighlightedObjectColor;
                lastHighlightedObject = null;
            }
        }
    }
}
