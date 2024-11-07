using RPG.Control;

public interface IRayCastable
{
    public bool HandleRayCast(PlayerController playerController);
    public ECursorType eCursorType { get; }
}