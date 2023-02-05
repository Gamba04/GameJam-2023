public interface IInteractable
{
    public bool Enabled { get; }

    public void Interact(Player player);
}