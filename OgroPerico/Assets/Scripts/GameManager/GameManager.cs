using UnityEngine;

public class SceneMaster : MonoBehaviour
{
    public static SceneMaster Instance { get; private set; }

    [Header("UI Dialogue Popup")]
    public DialoguePopup dialoguePopup;

    [Header("Intro Dialogue Texts")]
    public DialogueLine[] introSentences;      // Escribe aquí las frases del prólogo

    
    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        DialogueLine[] dialogueLines = new DialogueLine[]
                {
                    new DialogueLine { characterName = "Ogro Perico", sentence = "¡¡GRUAAAAARGH!! ¿Quién osa perturbar mis dominios? Mmm… ya veo quién eres, ratilla de oficina." },
                    new DialogueLine { characterName = "Ogro Perico", sentence = "¿Vienes por el Plan Dorado? ¡Qué osadía! No lo conseguirás… y tampoco saldrás de estas mazmorras con vida." },
                    new DialogueLine { characterName = "Ogro Perico", sentence = "Bienvenido a mi mazmorra. Aquí solo sobreviven los que saben obedecer… y tú has llegado demasiado lejos." }
                };

        introSentences = dialogueLines;
        if (dialoguePopup == null)
        {
            Debug.LogError("SceneMaster: dialoguePopup no está asignado en el inspector.");
            return;
        }
        if (introSentences != null && introSentences.Length > 0)
        {
                
                dialoguePopup.ShowDialogue(introSentences);
        }
        else
        {
            Debug.LogWarning("SceneMaster: introSentences está vacío. No se mostrará diálogo de inicio.");
        }
    }

    public void win(DialoguePopup dialoguePopup)
    {
        Debug.Log("win text");
        DialogueLine[] winDialogue = new DialogueLine[]
        {
            new DialogueLine { characterName = "Julián", sentence = "Se acabó, Ogro Perico. El Plan Dorado es mío. Esta empresa va a cambiar, cueste lo que cueste." },
            new DialogueLine { characterName = "Julián", sentence = "Se terminaron las jornadas infinitas, los correos a medianoche y las promesas vacías. Voy a reestructurarlo todo." },
            new DialogueLine { characterName = "Ogro Perico", sentence = "Grrrhh… así que… ¿tú serás el nuevo jefe?" },
            new DialogueLine { characterName = "Ogro Perico", sentence = "Dime… ¿y si dejamos esto atrás y… somos socios?" }
        };

        dialoguePopup.ShowDialogue(winDialogue, "GameWin");
    }
}
