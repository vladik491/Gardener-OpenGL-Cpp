using Gardener.gameClasses.Problems;
using Gardener.gameClasses.Mechanics.Trees;

namespace Gardener.gameClasses.Logic
{
    // Управляет состоянием игрока и дерева
    public class PlayerState
    {
        // Блок: Объекты и состояния
        private readonly ITree _tree; 
        private Problem _problem; 
        private float _nextProblemTime; 
        private float _currentTime;
        private readonly Random _random; 
        private int _fruitStage;
        private bool _fruitOnGround; 
        private bool _lastFruitAppeared; 

        // Блок: Константы
        private const float MinProblemInterval = 8f;
        private const float MaxProblemInterval = 12f;
        private const int MaxStage = 3;

        // Публичные свойства - состояние фрукта и проблем
        public int FruitStage => _fruitStage;
        public Problem Problem => _problem;
        public bool FruitOnGround => _fruitOnGround;
        public bool LastFruitAppeared => _lastFruitAppeared;

        // Инициализирует состояние игрока с заданным деревом
        public PlayerState(ITree tree)
        {
            _tree = tree;
            _random = new Random();
            _nextProblemTime = _random.Next((int)MinProblemInterval, (int)MaxProblemInterval + 1);
            _currentTime = 0f;
            _fruitStage = 0;
            _fruitOnGround = false;
            _lastFruitAppeared = false;
        }

        // Обновляет состояние дерева и проблем
        public void Update(float deltaTime, int boxState, int maxBoxState, Action<string> playSound)
        {
            _currentTime += deltaTime;

            if (!_lastFruitAppeared)
            {
                if (boxState < maxBoxState && !_fruitOnGround)
                {
                    if (_problem != null)
                    {
                        _problem.TimeLeft -= deltaTime;
                        if (_problem.TimeLeft <= 0)
                        {
                            _problem = null;
                            _fruitStage = 0;
                            playSound("soundFailure");
                            if (!_fruitOnGround)
                                _nextProblemTime = _currentTime + _random.Next((int)MinProblemInterval, (int)MaxProblemInterval + 1);
                        }
                    }
                    else if (_currentTime >= _nextProblemTime)
                    {
                        if (_tree.ProblemFactory != null)
                        {
                            _problem = _tree.ProblemFactory.CreateProblem(_currentTime);
                            _nextProblemTime = float.MaxValue;
                            playSound("soundError");
                        }
                    }
                }
                else if (boxState >= maxBoxState)
                {
                    _problem = null;
                    _nextProblemTime = float.MaxValue;
                }
            }
            else
            {
                _problem = null;
                _nextProblemTime = float.MaxValue;
            }

            if (_fruitStage == MaxStage && !_fruitOnGround)
            {
                _fruitOnGround = true;
                if (boxState + 1 >= maxBoxState)
                {
                    _nextProblemTime = float.MaxValue;
                    _lastFruitAppeared = true;
                }
            }
        }

        // Увеличивает стадию роста фрукта
        public void AddFruitStage()
        {
            if (_fruitStage < MaxStage) _fruitStage++;
        }

        // Сбрасывает стадию роста фрукта
        public void ResetFruitStage()
        {
            _fruitStage = 0;
        }

        // Устанавливает состояние фрукта на земле
        public void SetFruitOnGround(bool value)
        {
            _fruitOnGround = value;
            if (!_fruitOnGround && !_lastFruitAppeared)
                _nextProblemTime = _currentTime + _random.Next((int)MinProblemInterval, (int)MaxProblemInterval + 1);
        }

        // Сбрасывает текущую проблему
        public void ResetProblem()
        {
            _problem = null;
            if (!_fruitOnGround && !_lastFruitAppeared)
                _nextProblemTime = _currentTime + _random.Next((int)MinProblemInterval, (int)MaxProblemInterval + 1);
        }
    }
}