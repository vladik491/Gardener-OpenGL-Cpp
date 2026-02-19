namespace Gardener.gameClasses.Physics
{
    // Управляет коллизиями в игре
    public class CollisionManager
    {
        private readonly List<ICollidable> _staticCollidablesPlayer1 = new List<ICollidable>(); // Статические объекты для игрока 1
        private readonly List<ICollidable> _staticCollidablesPlayer2 = new List<ICollidable>(); // Статические объекты для игрока 2

        // Добавляет статический объект для конкретного игрока
        public void AddStaticObject(ICollidable collidable, int playerId)
        {
            if (collidable == null) return;

            if (playerId == 1)
                _staticCollidablesPlayer1.Add(collidable);
            else if (playerId == 2)
                _staticCollidablesPlayer2.Add(collidable);
        }

        // Добавляет общий статический объект для обоих игроков
        public void AddCommonStaticObject(ICollidable collidable)
        {
            if (collidable == null) return;

            _staticCollidablesPlayer1.Add(collidable);
            _staticCollidablesPlayer2.Add(collidable);
        }

        // Проверяет столкновение для игрока
        public bool CheckCollision(RectangleF boundsToCheck, int playerId)
        {
            var collidables = playerId == 1 ? _staticCollidablesPlayer1 : _staticCollidablesPlayer2;
            foreach (var staticObject in collidables)
            {
                if (boundsToCheck.IntersectsWith(staticObject.CollisionBounds))
                {
                    string objectType = (staticObject as CollidableObject)?.ObjectType ?? "Unknown";
                    return true;
                }
            }
            return false;
        }

        // Проверяет столкновение, игнорируя указанный объект
        public bool CheckCollision(RectangleF boundsToCheck, ICollidable ignoreObject, int playerId)
        {
            var collidables = playerId == 1 ? _staticCollidablesPlayer1 : _staticCollidablesPlayer2;
            foreach (var staticObject in collidables)
            {
                if (staticObject == ignoreObject) continue;
                if (boundsToCheck.IntersectsWith(staticObject.CollisionBounds))
                {
                    string objectType = (staticObject as CollidableObject)?.ObjectType ?? "Unknown";
                    return true;
                }
            }
            return false;
        }

        // Возвращает все статические объекты для игрока
        public IEnumerable<ICollidable> GetAllStaticCollidables(int playerId)
        {
            return playerId == 1 ? _staticCollidablesPlayer1 : _staticCollidablesPlayer2;
        }

        // Очищает списки статических объектов
        public void Clear()
        {
            _staticCollidablesPlayer1.Clear();
            _staticCollidablesPlayer2.Clear();
        }
    }
}