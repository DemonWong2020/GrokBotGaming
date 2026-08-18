using UnityEngine;

public class GameState : MonoBehaviour
{
    int total;
    int collected;
    bool won;

    public void RegisterTotal(int count)
    {
        total = count;
        collected = 0;
        won = false;
    }

    public void AddCoin()
    {
        collected++;
        if (total > 0 && collected >= total)
            won = true;
    }

    void OnGUI()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.fontSize = 22;
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(24, 20, 640, 40), $"Coins  {collected}/{total}", style);
        GUI.Label(new Rect(24, 52, 640, 40), "WASD move  Space jump  R restart", style);
        if (won)
        {
            var win = new GUIStyle(style);
            win.fontSize = 36;
            GUI.Label(new Rect(24, 100, 800, 60), "All coins collected!", win);
        }
    }
}
