using Bellseboss.Pery.Scripts.Input;

public interface IFatalitySystem
{
    // Define interface methods here
    void Configure(IFatality characterV2, ICharacterV2 cV2);
    void Fatality();
    bool IsStartFatality();
}