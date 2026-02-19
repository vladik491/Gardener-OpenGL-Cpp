using Gardener.gameClasses.Problems;

namespace Gardener.gameClasses.Mechanics.Trees
{
    // Интерфейс дерева
    public interface ITree
    {
        string Type { get; }
        ProblemFactory ProblemFactory { get; }
    }

    // Базовый класс дерева
    public class Tree : ITree
    {
        public string Type => "GenericTree";
        public ProblemFactory ProblemFactory { get; }

        // Инициализирует базовое дерево с фабрикой проблем
        public Tree(ProblemFactory problemFactory)
        {
            ProblemFactory = problemFactory;
        }
    }

    // Декоратор для яблони
    public class AppleTreeDecorator : ITree
    {
        private readonly ITree _tree;

        // Декорирует дерево как яблоню
        public AppleTreeDecorator(ITree tree)
        {
            _tree = tree;
        }

        public string Type => "AppleTree";
        public ProblemFactory ProblemFactory => new AppleTreeProblemFactory();
    }

    // Декоратор для груши
    public class PearTreeDecorator : ITree
    {
        private readonly ITree _tree;
        public PearTreeDecorator(ITree tree)
        {
            _tree = tree;
        }

        public string Type => "PearTree";
        public ProblemFactory ProblemFactory => new PearTreeProblemFactory();
    }
}