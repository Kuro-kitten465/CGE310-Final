namespace Kuro.Components
{
    public interface IInteractable
    {
        public bool TriggerOneTimeOnly { get; }
        public bool HasTriggered { get; }
        void Interact(PlayerManager manager);
    }
}