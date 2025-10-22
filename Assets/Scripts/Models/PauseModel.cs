using Base;

namespace Models
{
    public class PauseModel : Model
    {
        public string TargetGameScene { get; private set; } = "MainMenu";
    }
}