using System.Threading.Tasks;

public interface IGameState
{
    Task<StateResult> DoAction(object data);
    void InitialConfigurations();
    void FinishConfiguration();
}