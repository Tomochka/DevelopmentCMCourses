namespace Scene2d.Figures
{
    using System.Collections.Generic;

    public interface ICompositeFigure : IFigure
    {
        List<IFigure> ChildFigures { get; }
    }
}
