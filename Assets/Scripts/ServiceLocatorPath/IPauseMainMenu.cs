namespace ServiceLocatorPath
{
    public interface IPauseMainMenu
    {
        void Pause();
        
        PauseMenu.OnPause onPause { get; set; }
    }
}