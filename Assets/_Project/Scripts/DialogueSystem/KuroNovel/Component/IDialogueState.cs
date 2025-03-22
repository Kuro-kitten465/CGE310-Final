namespace KuroNovel.Component
{
    public enum DialogueStateType { Idle, Typing, WaitingForInput, Choice, End }

    public interface IDialogueState
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}