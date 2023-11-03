using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SquadCalculator;

public class MortarPoint : PointBase
{
    private readonly double _scale;
    private readonly double _circleRadiusInPixels;
    private readonly int _canvasWidth = 4096;
    private readonly int _maxDistance = 1230;
    public Ellipse RadiusEllipse { get; set; }
    public MortarPoint(double width, double height, PointType locType, Image iconImage, int mapSize, Point creationPoint) : base(width, height, locType, iconImage)
    {
        _scale = (double)_canvasWidth / mapSize;
        _circleRadiusInPixels = _maxDistance * _scale;
        RadiusEllipse = CreateMortarRadius();
        Canvas.SetLeft(RadiusEllipse, creationPoint.X - _circleRadiusInPixels);
        Canvas.SetTop(RadiusEllipse, creationPoint.Y - _circleRadiusInPixels);
    }

    protected override void CanvasPoint_MouseMove(object sender, MouseEventArgs e)
    {
        base.CanvasPoint_MouseMove(sender, e);
        Canvas.SetLeft(RadiusEllipse, Position.X - _circleRadiusInPixels);
        Canvas.SetTop(RadiusEllipse, Position.Y - _circleRadiusInPixels);
    }

    private Ellipse CreateMortarRadius()
    {
        double centerX = Position.X;
        double centerY = Position.Y;
        
        Ellipse mortarRadius = new Ellipse()
        {
            Width = _circleRadiusInPixels * 2,
            Height = _circleRadiusInPixels * 2,
            StrokeThickness = 4,
            Stroke = Brushes.LawnGreen,
        };
        Panel.SetZIndex(mortarRadius, 0);
        
        Canvas.SetLeft(mortarRadius, centerX - _circleRadiusInPixels);
        Canvas.SetTop(mortarRadius, centerY - _circleRadiusInPixels );
        return mortarRadius;
    }
}