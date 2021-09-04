namespace Assets.Scripts.Interfaces
{
    public interface IControlCharacter
    {
        void Move();

        void Rotate(float signedDirection);

        void Jump(float multiplier = 1f);

        void BoostedJump();
    }
}
