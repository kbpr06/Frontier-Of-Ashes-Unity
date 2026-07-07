using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;

    [Header("Camera Settings")]
    [SerializeField] private float smoothSpeed = 5f;

    private void LateUpdate()
    {
        // Si no hay objetivo asignado, la cámara no hace nada.
        // Esto evita errores si olvidamos asignar el Player en el Inspector.
        if (target == null)
        {
            return;
        }

        // Creamos una nueva posición para la cámara.
        // Tomamos X e Y del Player, pero mantenemos el Z actual de la cámara.
        Vector3 targetPosition = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );

        // Movemos la cámara suavemente hacia la posición del Player.
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}