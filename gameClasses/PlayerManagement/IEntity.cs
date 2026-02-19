namespace Gardener.gameClasses.PlayerManagement
{
    // Интерфейс для игровых сущностей (персонажей, объектов)
    public interface IEntity
    {
        // Координаты сущности
        float PositionX { get; }
        float PositionY { get; }

        // Обновляет состояние сущности (движение, действия)
        void Update(float deltaTime, bool moveUp, bool moveDown, bool moveLeft, bool moveRight);

        // Рендерит сущность на экране
        void Render();
    }
}