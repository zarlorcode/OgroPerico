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

    private void Start()
    {
        if (myRenderer == null) myRenderer = GetComponent<SpriteRenderer>();
        if (spriteCerrada != null) myRenderer.sprite = spriteCerrada;

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
    }

    private void CerrarPuerta()
    {
        estaAbierta = false;
        Debug.Log("Puerta cerrada (combinación rota).");
        if (myRenderer != null && spriteCerrada != null)
            myRenderer.sprite = spriteCerrada;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radioDeBusqueda);
    }
}