namespace Gardener.mainMenuClasses.panelClasses
{
    // Панель с двойной буферизацией для уменьшения мерцания
    public class DoubleBufferedPanel : Panel
    {
        // Конструктор: Включает двойную буферизацию
        public DoubleBufferedPanel()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
        }
    }
}