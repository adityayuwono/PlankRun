namespace Assets.Scripts.Controllers.Interactables
{
    public class Plank : EnemiesTargetThis, IInteractable
    {
        private bool _isDisabled = false;

        public override void Interact(Character character)
        {
            if (!_isDisabled)
            {
                _isDisabled = true;
                character.AddPlank();
                Destroy(gameObject);
            }
        }
    }
}