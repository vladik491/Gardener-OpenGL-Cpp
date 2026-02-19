namespace Gardener.gameClasses.Physics
{
    // Объект с коллизией
    internal class CollidableObject : ICollidable
    {

        private RectangleF _bounds; // Границы объекта
        public string ObjectType { get; } // Тип объекта
        public RectangleF CollisionBounds => _bounds;

        // Создаёт объект с заданными координатами, размером и типом
        public CollidableObject(float x, float y, float width, float height, string objectType = "Generic")
        {
            _bounds = new RectangleF(x, y, width, height);
            ObjectType = objectType;
        }
    }
}