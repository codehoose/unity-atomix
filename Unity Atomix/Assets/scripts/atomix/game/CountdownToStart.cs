using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CountdownToStart : MonoBehaviour
{
    private bool _countDownActive = true;

    public TMPro.TextMeshProUGUI text;

    public Button startGame;
    
    IEnumerator Start()
    {
        // On button click ignore stuff...
        startGame.onClick.AddListener(new UnityAction(() => {
            _countDownActive = false;
        }));

        // While loop, so while running, update the counter
        // when the counter expires start the game
        var seconds = 5f;
        while (_countDownActive && seconds >= 0)
        {
            var intSeconds = Mathf.Ceil(seconds);
            text.text = intSeconds.ToString();
            seconds -= Time.deltaTime;
            yield return null;
        }

        text.text = "";
        var canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
    }
}
