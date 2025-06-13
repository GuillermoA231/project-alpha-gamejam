using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class WaveManagerUI : MonoBehaviour
{

    [Header(" Elements")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI timerText;

    private const string Table = "Menu Labels";
    private const string Entry = "UI.Canvas.Game.WaveText";
    private int lastCurrent;
    private int lastTotal;

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void OnLocaleChanged(UnityEngine.Localization.Locale _)
    {
        UpdateWaveText(lastCurrent,lastTotal);
    }
    public void UpdateWaveText(int waveCurrent, int waveTotal)
    {
        var localeHandle = LocalizationSettings
            .StringDatabase
            .GetLocalizedStringAsync(Table, Entry, new object[] { waveCurrent, waveTotal });

        if (localeHandle.IsDone)
            waveText.text = localeHandle.Result;
        else
            localeHandle.Completed += op => waveText.text = op.Result;
    }

    // public void UpdateWaveText(string waveString) => waveText.text = waveString;
    public void UpdateTimerText(string timerString) => timerText.text = timerString;
}
