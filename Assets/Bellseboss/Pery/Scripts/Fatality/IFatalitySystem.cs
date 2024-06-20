using System.Collections.Generic;
using Bellseboss.Pery.Scripts.Input;

public interface IFatalitySystem
{
    // Define interface methods here
    void Configure(IFatality characterV2, ICharacterV2 cV2);
    void Fatality();
    bool IsStartFatality();
    void StartCinematic();
    bool FinishedCinematic();
    bool ReadInput(out INPUTS input);
    List<InputRead> GetInputs();
    void ContinueCinematic();
    void EndOfFatality();
    void HidePanelTitle();
    void ShowPanelInputs();
    void HidePanelInputs();
    void ShowPanelTitle();
    void ShowPanelTitle(string title);
    void CanReadInputs(bool b);
    void StartAudioFatality();
    void RestartAllElements();
    void FatalityPlayer();
    void FatalityEnemy();
}