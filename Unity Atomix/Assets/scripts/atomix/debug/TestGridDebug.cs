using UnityEngine;

public class TestGridDebug : MonoBehaviour
{
    public AtomicFileLoader fileLoader;

    public TMPro.TextMeshProUGUI text;

    private void Update()
    {
        var arena = fileLoader.GridDebug;
        if (!string.IsNullOrEmpty(arena))
        {
            text.text = arena;
        }
    }
}
