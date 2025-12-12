using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro.Examples;

[System.Serializable]
public class DialogueLine
{
    public string characterName;      // Nombre del que habla
    public string sentence;           // Texto de diálogo
}

public class Intro : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI characterName;

    private DialogueLine[] dialogueLines = new DialogueLine[]
    {
        new DialogueLine { characterName = "Julián", sentence =
            "¡¡¿Qué ha pasado?!! ¿Dónde estoy? Esto no es mi oficina… Respira, calma… Todo está oscuro y silencioso. Demasiado silencioso." },

        new DialogueLine { characterName = "Julián", sentence =
            "Estaba en mi escritorio, otra jornada agotadora, cuando apareció un archivo nuevo: “Plan Dorado”. Jamás lo había visto antes." },

        new DialogueLine { characterName = "Julián", sentence =
            "Había escuchado rumores… una leyenda entre empleados. Un documento capaz de reestructurar toda la empresa. Tonterías de pasillo… o eso creía." },

        new DialogueLine { characterName = "Julián", sentence =
            "Decían que el jefe pactó con el Ogro Perico para multiplicar beneficios, y que el documento fue sellado en las mazmorras administrativas para que nadie lo consiguiera jamás." },

        new DialogueLine { characterName = "Julián", sentence =
            "Abrí el archivo. Dentro había un protocolo secreto… y un enlace sin remitente. No sé por qué hice clic. Todo se volvió negro. Y ahora estoy aquí… donde sea que 'aquí' sea." },

        new DialogueLine { characterName = "Julián", sentence =
            "No veo salida. Solo sombras… y este lugar parece observarme. Sea lo que sea este Plan Dorado, alguien quería que lo encontrara. Necesito descubrir qué es este sitio y sobrevivir lo suficiente para salir." },

        new DialogueLine { characterName = "Ogro Perico", sentence =
            "RRAAAAAARGHHH!! ¿Quién es el insensato que se atreve a entrar en mis mazmorras?" }
    };

    /*private DialogueLine[] extendedDialogueLines = new DialogueLine[]
    {
        new DialogueLine { characterName = "Julián", sentence = "¡¡¿Qué ha pasado?!! ¿Dónde estoy? Esto… esto no es mi oficina. No puede ser. Respira… calma… Tiene que haber una explicación. Todo está oscuro, silencioso… demasiado silencioso." },
        new DialogueLine { characterName = "Julián", sentence = "Recuerdo estar en mi escritorio, revisando informes y correos urgentes. Otra jornada agotadora en Perico’s Corporation, como siempre. Y entonces lo vi: un archivo nuevo, justo en medio del escritorio digital. Se llamaba “Plan Dorado”. Jamás lo había visto antes." },
        new DialogueLine { characterName = "Julián", sentence = "Había escuchado rumores: el Plan Dorado, una especie de leyenda en la empresa. Pero, ¿qué tontería es esa? Nadie la toma en serio…" },
        new DialogueLine { characterName = "Julián", sentence = "Según las historias, el jefe de la compañía —obsesionado con el poder, la productividad eterna y el beneficio personal— hizo un pacto con el Ogro Perico para multiplicar los beneficios a costa de los empleados." },
        new DialogueLine { characterName = "Julián", sentence = "pero, a cambio, tuvo que crear un documento secreto capaz de reestructurar toda la empresa por aquel que logre obtenerlo. Dicho acta, según se dice, permitiría modificar toda la estructura y condiciones de la organización." },
        new DialogueLine { characterName = "Julián", sentence = "Pero, dado el poder de este documento y temiendo perder el poder de la empresa, el Ogro Perico selló el documento en las profundidades de sus mazmorras administrativas, donde nadie ha regresado jamás."},
        new DialogueLine { characterName = "Julián", sentence = "Me pareció extraño… demasiado extraño. Al abrirlo, aparecía un protocolo secreto capaz de ‘reestructurar la empresa’ y cambiar las reglas a quien lo poseyera." },
        new DialogueLine { characterName = "Julián", sentence = "Pensé que era una broma interna del departamento de sistemas. O quizá un archivo olvidado que nunca debería haber llegado a mí. Dentro del archivo había un enlace. Sin remitente, sin fecha, sin nada. Solo una URL cruda que no reconocí." },
        new DialogueLine { characterName = "Julián", sentence = "No sé por qué lo hice… pero hice clic. Tal vez por curiosidad, tal vez por cansancio. Y de repente, todo se volvió negro. No recuerdo nada después. Ni sonido, ni imagen. Como si me hubieran arrancado de la realidad." },
        new DialogueLine { characterName = "Julián", sentence = "Y ahora… estoy aquí. Donde sea que ‘aquí’ sea. Todo está oscuro, como si la luz estuviera prohibida, siento el espacio cerrándose. Como si respirara, o me observara." },
        new DialogueLine { characterName = "Julián", sentence = "Siento además un peso en los hombros, como si llevara algo metálico clavado en la espalda; no es mi traje de oficinista. Me cuesta respirar con normalidad. Intento moverme, pero el suelo cruje y suena extraño bajo mis pasos… como piedra húmeda. Esto no es una ilusión." },
        new DialogueLine { characterName = "Julián", sentence = "No veo a nadie. Ni compañeros, ni señales de salida. Solo sombras que quizá se mueven… o quizá estoy perdiendo la cabeza. Julián, concéntrate. Tienes que mantener la cordura. Si esto es algún tipo de entorno digital, tiene que haber un punto de control." },
        new DialogueLine { characterName = "Julián", sentence = "Sea lo que sea este Plan Dorado, estoy seguro de que no era un simple archivo mal colocado. Alguien quería que lo viera. Debe haber una salida… una forma de apagar lo que sea esto. Pero algo me dice que no va a ser tan simple." },
        new DialogueLine { characterName = "Julián", sentence = "Primero averiguaré qué es este sitio. Espero sobrevivir el tiempo suficiente para encontrar el camino de vuelta..." },
        new DialogueLine { characterName = "Ogro Perico", sentence = "RRAAAARGH!! ¿Qué… qué es ese ruido? ¿Quién osa entrar en mis mazmorras?" }
    };*/

    public CanvasGroup fadeGroup;
    public float fadeDuration = 1f;

    private int index = 0;
    private bool dialogueReady = false;

    void Start()
    {
        fadeGroup.alpha = 0f;
        StartCoroutine(FadeInPanel());
    }

    IEnumerator FadeInPanel()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = Mathf.Clamp01(t / fadeDuration); // 0 a 1
            yield return null;
        }

        // Ahora ya se puede avanzar en el diálogo
        dialogueReady = true;
        ShowLine();
    }

    void ShowLine()
    {
        DialogueLine line = dialogueLines[index];
        dialogueText.text = line.sentence;
        characterName.text = line.characterName;
    }

    void Update()
    {
        if (!dialogueReady) return;

        if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
        {
            NextSentence();
        }
    }

    void NextSentence()
    {
        index++;
        if (index < dialogueLines.Length)
        {
            ShowLine();
        }
        else
        {
            StartCoroutine(FadeOutAndLoad());
        }
    }

    public void SkipIntro()
    {
        // stop all routines
        StopAllCoroutines(); 
        StartCoroutine(FadeOutAndLoad());
    }

    IEnumerator FadeOutAndLoad()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = 1f - (t / fadeDuration);
            yield return null;
        }
        Debug.Log("Loading Scene");

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.reproducirMusicaJuego();
        }

        SceneManager.LoadScene("Game");
    }
}

