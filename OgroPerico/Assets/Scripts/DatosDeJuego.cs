using UnityEngine;

// Clase estática para guardar datos entre escenas
public static class DatosDeJuego
{
    // Guardamos la posición aquí. El "?" permite que sea nulo (null) si no hay posición definida.
    public static Vector2? PosicionInicialJugador = null;

    public static int VidaJugador = -1;

    public static int MaxCorazones = 5;       
    public static float VelocidadMovimiento = 5f; 
    public static float MultiplicadorAtaque = 1f;
}
