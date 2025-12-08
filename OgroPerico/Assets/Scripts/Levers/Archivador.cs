using UnityEngine;

public class Archivador : MonoBehaviour
{
    // Definimos los estados posibles para mayor claridad
    public enum Estado { Desactivado = 0, Estado1 = 1, Estado2 = 2 }

    [Header("Configuración del Puzle")]
    public float interactionRadius = 1.0f;
    [SerializeField] private Estado estadoActual = Estado.Desactivado; // Estado inicial
    [SerializeField] private Estado estadoRequerido; // ¿Cuál es la solución correcta? (Configurar en Inspector)

    [Header("Visuales del Archivador")]
    [SerializeField] private SpriteRenderer myRenderer;
    [SerializeField] private Sprite spriteDesactivado;
    [SerializeField] private Sprite spriteEstado1;
    [SerializeField] private Sprite spriteEstado2;

    [Header("Visuales del Receptor (Luz/Cable)")]
    [SerializeField] private SpriteRenderer receptorRenderer;
    [SerializeField] private Sprite receptorEncendido; // Sprite cuando el estado es CORRECTO
    [SerializeField] private Sprite receptorApagado;   // Sprite cuando es INCORRECTO

    private Puerta puertaControladora;

    private void Start()
    {
        if (myRenderer == null) myRenderer = GetComponent<SpriteRenderer>();
        
        // Inicializar visuales al arrancar
        ActualizarVisuales();
    }

    public void AsignarPuerta(Puerta puerta)
    {
        puertaControladora = puerta;
        // Si por casualidad empezamos ya en el estado correcto, avisamos a la puerta
        if (estadoActual == estadoRequerido)
        {
            puertaControladora.ModificarContador(1);
        }
    }

    // Llamado por InteractionManager
    public void TryActivate()
    {
        // 1. Guardamos si ANTES del cambio era correcto
        bool eraCorrecto = (estadoActual == estadoRequerido);

        // 2. Cambiar al siguiente estado (Ciclo 0 -> 1 -> 2 -> 0)
        int siguienteEstado = (int)estadoActual + 1;
        if (siguienteEstado > 2) siguienteEstado = 0;
        
        estadoActual = (Estado)siguienteEstado;

        // 3. Comprobamos si AHORA es correcto
        bool esCorrecto = (estadoActual == estadoRequerido);

        // 4. Actualizar gráficos
        ActualizarVisuales();

        // 5. Avisar a la puerta solo si hubo un cambio en la validez
        // Si antes estaba mal y ahora bien: Sumamos 1
        // Si antes estaba bien y ahora mal: Restamos 1
        if (puertaControladora != null)
        {
            if (!eraCorrecto && esCorrecto)
            {
                puertaControladora.ModificarContador(1); // Sumar acierto
            }
            else if (eraCorrecto && !esCorrecto)
            {
                puertaControladora.ModificarContador(-1); // Restar acierto (el jugador la lió)
            }
        }
    }

    private void ActualizarVisuales()
    {
        // A. Cambiar sprite del propio archivador
        switch (estadoActual)
        {
            case Estado.Desactivado:
                myRenderer.sprite = spriteDesactivado;
                break;
            case Estado.Estado1:
                myRenderer.sprite = spriteEstado1;
                break;
            case Estado.Estado2:
                myRenderer.sprite = spriteEstado2;
                break;
        }

        // B. Cambiar sprite del receptor (solo se enciende si es el estado correcto)
        if (receptorRenderer != null)
        {
            if (estadoActual == estadoRequerido)
            {
                receptorRenderer.sprite = receptorEncendido;
            }
            else
            {
                receptorRenderer.sprite = receptorApagado;
            }
        }
    }
}