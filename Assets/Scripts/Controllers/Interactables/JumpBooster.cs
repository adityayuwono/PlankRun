namespace Assets.Scripts.Controllers.Interactables
{
    public class JumpBooster : EnemiesTargetThis, IInteractable
    {
        public override void Interact(Character character)
        {
            character.Control.BoostedJump();
        }
    }
}