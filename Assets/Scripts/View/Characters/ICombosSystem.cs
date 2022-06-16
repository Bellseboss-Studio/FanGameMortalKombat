namespace View.Characters
{
    public interface ICombosSystem
    {
        void ExecuteKick(PlayerCharacter playerCharacter);
        void ExecutePunch(PlayerCharacter playerCharacter);
        void ResetCombo();
    }
}