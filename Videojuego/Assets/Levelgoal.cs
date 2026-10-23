using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Ponlo en un objeto con Collider2D marcado como "Is Trigger"
public class LevelGoal : MonoBehaviour
{
    public string winMessage = "¡NIVEL COMPLETADO!";
    private bool won;
    private GUIStyle titleStyle;
    private GUIStyle subStyle;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (won) return;
        playermovement player = other.GetComponent<playermovement>();
        if (player != null)
        {
            won = true;
            player.Win();
        }
    }

    void Update()
    {
        // Reiniciar con R una vez ganado
        if (won && Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void OnGUI()
    {
        if (!won) return;

        if (titleStyle == null)
        {
            titleStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 56, fontStyle = FontStyle.Bold };
            titleStyle.normal.textColor = Color.yellow;
            subStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 24 };
            subStyle.normal.textColor = Color.white;
        }

        GUI.color = new Color(0, 0, 0, 0.6f);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        GUI.color = Color.white;

        GUI.Label(new Rect(0, Screen.height * 0.35f, Screen.width, 90), winMessage, titleStyle);
        GUI.Label(new Rect(0, Screen.height * 0.35f + 100, Screen.width, 40), "Presiona R para jugar de nuevo", subStyle);
    }
}