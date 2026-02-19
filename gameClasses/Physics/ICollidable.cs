namespace Gardener.gameClasses.Physics
{
    /// <summary>
    /// Интерфейс для объектов с коллизией
    /// </summary>
    public interface ICollidable
    {
        // Границы для проверки столкновений
        RectangleF CollisionBounds { get; }
    }
}