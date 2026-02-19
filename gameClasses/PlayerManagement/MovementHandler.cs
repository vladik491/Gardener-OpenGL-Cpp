using Gardener.gameClasses.Physics;
using Gardener.gameClasses.PlayerManagement.DirectionStrategies;
using Gardener.gameClasses.Rendering;

namespace Gardener.gameClasses.PlayerManagement
{
    // Управляет движением и анимацией игрока
    public class MovementHandler
    {
        // Константы: параметры движения
        private const float MoveSpeed = 150f;
        private const float AnimationSpeed = 12f; 

        // Блок: Данные карты и рендеринга
        private readonly Map _map; // Карта игры
        private readonly (int Width, int Height, int SheetWidth)[] _spriteData; 
        private readonly DirectionStrategy _directionStrategy; 

        // Блок: Данные коллизии
        private readonly CollisionManager _collisionManager; 
        private readonly float _scale; 
        private readonly int _playerId; 
        private readonly float _collisionOffsetX; 
        private readonly float _collisionOffsetY; 
        private readonly float _collisionWidth;
        private readonly float _collisionHeight; 

        // Блок: Текущее состояние
        private float _positionX; 
        private float _positionY;
        private float _animationFrame; 
        private int _currentDirection;
        private bool _isMoving; 

        // Свойства: доступ к состоянию
        public float PositionX => _positionX;
        public float PositionY => _positionY;
        public int CurrentDirection => _currentDirection;
        public float AnimationFrame => _animationFrame;
        public bool IsMoving => _isMoving;

        // Конструктор: инициализирует обработчик движения
        public MovementHandler(
            Map map,
            (int Width, int Height, int SheetWidth)[] spriteData,
            float initialX,
            float initialY,
            float scale,
            int playerId,
            CollisionManager collisionManager,
            float collisionOffsetX,
            float collisionOffsetY,
            float collisionWidth,
            float collisionHeight)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
            _spriteData = spriteData ?? throw new ArgumentNullException(nameof(spriteData));
            _positionX = initialX;
            _positionY = initialY;
            _scale = scale;
            _playerId = playerId;
            _collisionManager = collisionManager ?? throw new ArgumentNullException(nameof(collisionManager));
            _collisionOffsetX = collisionOffsetX;
            _collisionOffsetY = collisionOffsetY;
            _collisionWidth = collisionWidth;
            _collisionHeight = collisionHeight;
            _animationFrame = 0;
            _currentDirection = 0;
            _isMoving = false;
            _directionStrategy = new DirectionStrategy();
        }

        /// <summary>
        /// Обновляет позицию и анимацию игрока на основе ввода.
        /// </summary>
        /// <param name="deltaTime">Время с последнего обновления</param>
        /// <param name="moveUp">Движение вверх</param>
        /// <param name="moveDown">Движение вниз</param>
        /// <param name="moveLeft">Движение влево</param>
        /// <param name="moveRight">Движение вправо</param>
        public void Update(float deltaTime, bool moveUp, bool moveDown, bool moveLeft, bool moveRight)
        {
            // Вычисляет направление и движение
            (float dx, float dy, bool requestedMove) = CalculateMovementDirection(moveUp, moveDown, moveLeft, moveRight);

            _isMoving = false;

            if (requestedMove)
            {
                // Нормализует вектор движения
                (dx, dy) = NormalizeMovement(dx, dy);
                UpdateDirection(dx, dy);

                // Применяет скорость
                dx *= MoveSpeed * deltaTime;
                dy *= MoveSpeed * deltaTime;

                float targetX = _positionX + dx;
                float targetY = _positionY + dy;

                // Проверяет коллизии
                RectangleF targetBounds = new RectangleF(
                    targetX + _collisionOffsetX,
                    targetY + _collisionOffsetY,
                    _collisionWidth, _collisionHeight);

                (float actualDx, float actualDy) = ResolveCollisions(targetX, targetY, dx, dy);

                if (actualDx != 0 || actualDy != 0)
                {
                    _positionX += actualDx;
                    _positionY += actualDy;
                    _isMoving = true;
                    UpdateAnimation(deltaTime);
                }
            }

            // Сбрасывает анимацию, если игрок не движется
            if (!_isMoving)
            {
                _animationFrame = 0;
            }

            ClampPosition();
        }

        // Вычисляет направление движения на основе ввода
        private (float dx, float dy, bool requestedMove) CalculateMovementDirection(bool moveUp, bool moveDown, bool moveLeft, bool moveRight)
        {
            float dx = 0f, dy = 0f;
            bool requestedMove = false;

            if (moveUp) { dy -= 1; requestedMove = true; }
            if (moveDown) { dy += 1; requestedMove = true; }
            if (moveLeft) { dx -= 1; requestedMove = true; }
            if (moveRight) { dx += 1; requestedMove = true; }

            return (dx, dy, requestedMove);
        }

        // Нормализует вектор движения для равномерной скорости
        private (float dx, float dy) NormalizeMovement(float dx, float dy)
        {
            float length = (float)Math.Sqrt(dx * dx + dy * dy);
            if (length > 0)
            {
                dx /= length;
                dy /= length;
            }
            return (dx, dy);
        }

        // Обновляет направление игрока
        private void UpdateDirection(float dx, float dy)
        {
            if (dx != 0 || dy != 0)
            {
                var strategy = _directionStrategy.GetStrategy(dx, dy);
                _currentDirection = strategy.DirectionIndex;
            }
        }

        // Проверяет и разрешает коллизии
        private (float actualDx, float actualDy) ResolveCollisions(float targetX, float targetY, float dx, float dy)
        {
            RectangleF targetBounds = new RectangleF(
                targetX + _collisionOffsetX,
                targetY + _collisionOffsetY,
                _collisionWidth, _collisionHeight);

            if (!_collisionManager.CheckCollision(targetBounds, _playerId))
            {
                return (dx, dy);
            }

            float actualDx = 0;
            float actualDy = 0;

            // Проверяет движение только по X
            RectangleF boundsX = new RectangleF(
                targetX + _collisionOffsetX,
                _positionY + _collisionOffsetY,
                _collisionWidth, _collisionHeight);
            if (!_collisionManager.CheckCollision(boundsX, _playerId))
            {
                actualDx = dx;
            }

            // Проверяет движение только по Y
            RectangleF boundsY = new RectangleF(
                _positionX + _collisionOffsetX,
                targetY + _collisionOffsetY,
                _collisionWidth, _collisionHeight);
            if (!_collisionManager.CheckCollision(boundsY, _playerId))
            {
                actualDy = dy;
            }

            return (actualDx, actualDy);
        }

        // Обновляет кадр анимации при движении
        private void UpdateAnimation(float deltaTime)
        {
            _animationFrame += AnimationSpeed * deltaTime;
            if (_animationFrame >= 6)
            {
                _animationFrame -= 6;
            }
        }

        // Ограничивает позицию игрока в пределах карты
        private void ClampPosition()
        {
            var (frameWidth, frameHeight, _) = _spriteData[_currentDirection];
            float scaledWidth = frameWidth * _scale;
            float scaledHeight = frameHeight * _scale;
            _positionX = Math.Clamp(_positionX, 0, _map.Width - scaledWidth);
            _positionY = Math.Clamp(_positionY, 0, _map.Height - scaledHeight);
        }
    }
}