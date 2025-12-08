using UnityEngine;

public class Puerta : MonoBehaviour
{
    [Header("Detección Automática")]
    [SerializeField] private float radioDeBusqueda = 10f;
    [SerializeField] private LayerMask capaArchivadores;

    private int totalArchivadores = 0;
    private int aciertosActuales = 0; // Cuántos están en el estado correcto
    private bool estaAbierta = false;

    [Header("Visuales")]
    [SerializeField] private SpriteRenderer myRenderer;
    [SerializeField] private Sprite spriteAbierta;
    [SerializeField] private Sprite spriteCerrada;

    [SerializeField] private Vector3 cambioPosicionCamara;
    [SerializeField] private Vector3 cambioPosicionJugador;

    private CamController camControl;
    private Collider2D miCollider;

    private void Start()
    {
        if (myRenderer == null) myRenderer = GetComponent<SpriteRenderer>();
        if (spriteCerrada != null) myRenderer.sprite = spriteCerrada;

        miCollider = GetComponent<Collider2D>();
        
        camControl = Camera.main.GetComponent<CamController>();

        DetectarArchivadores();
    }

    private void DetectarArchivadores()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radioDeBusqueda, capaArchivadores);

        foreach (var hit in hits)
        {
            Archivador archivador = hit.GetComponent<Archivador>();
            if (archivador != null)
            {
                archivador.AsignarPuerta(this);
                totalArchivadores++;
            }
        }
        Debug.Log($"Puzle iniciado: Se necesitan {totalArchivadores} archivadores correctos.");
    }

    // Este método ahora recibe +1 (acierto) o -1 (fallo)
    public void ModificarContador(int cambio)
    {
        aciertosActuales += cambio;
        
        Debug.Log($"Progreso: {aciertosActuales} / {totalArchivadores}");

        // Comprobamos la condición de victoria
        if (aciertosActuales >= totalArchivadores && !estaAbierta)
        {
            AbrirPuerta();
        }
        else if (aciertosActuales < totalArchivadores && estaAbierta)
        {
            CerrarPuerta(); // ¡El jugador deshizo el puzle!
        }
    }

    private void AbrirPuerta()
    {
        estaAbierta = true;
        Debug.Log("¡PUERTA ABIERTA!");
        if (myRenderer != null && spriteAbierta != null)
            myRenderer.sprite = spriteAbierta;

        if (miCollider != null) miCollider.isTrigger = true;
    }

    private void CerrarPuerta()
    {
        estaAbierta = false;
        Debug.Log("Puerta cerrada (combinación rota).");
        if (myRenderer != null && spriteCerrada != null)
            myRenderer.sprite = spriteCerrada;

        if (miCollider != null) miCollider.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Comprobamos si es el Player
        // 2. Comprobamos si la puerta ESTÁ ABIERTA
        if (other.CompareTag("Player") && estaAbierta)
        {
            if (camControl != null)
            {
                // Actualizamos los límites de la cámara (Min y Max)
                camControl.minPos += cambioPosicionCamara;
                camControl.maxPos += cambioPosicionCamara;
            }

            // Movemos al jugador a la nueva posición
            other.transform.position += cambioPosicionJugador;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radioDeBusqueda);
    }
}