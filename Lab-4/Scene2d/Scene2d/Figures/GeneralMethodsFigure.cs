namespace Scene2d.Figures
{
    using System;
    using Scene2d;

    public class GeneralMethodsFigure
    {
        public static void ReflectFigure(ReflectOrientation orientation, ScenePoint centerFigure, ref ScenePoint[] points)
        {
            if (orientation == ReflectOrientation.Vertical)
            {
                for (var i = 0; i < points.Length; i++)
                {
                    points[i].X += (centerFigure.X - points[i].X) * 2;
                }
            }

            if (orientation == ReflectOrientation.Horizontal)
            {
                for (var i = 0; i < points.Length; i++)
                {
                    points[i].Y += (centerFigure.Y - points[i].Y) * 2;
                }
            }
        }

        public static void RotateFigure(double angle, ScenePoint centerFigure, ref ScenePoint[] points)
        {
            var rad = (Math.PI / 180) * angle;
            
            for (var i = 0; i < points.Length; i++)
            {
                var X = points[i].X;
                var Y = points[i].Y;

                points[i].X = centerFigure.X + (X - centerFigure.X) * Math.Cos(rad) - (Y - centerFigure.Y) * Math.Sin(rad);
                points[i].Y = centerFigure.Y + (X - centerFigure.X) * Math.Sin(rad) + (Y - centerFigure.Y) * Math.Cos(rad);
            }
        }

        public static void Move(ScenePoint vector, ref ScenePoint[] points)
        {
            for (var i = 0; i < points.Length; i++)
            {
                points[i].X += vector.X;
                points[i].Y += vector.Y;
            }
        }
    }
}
