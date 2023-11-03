using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;

namespace SquadCalculator;

public enum PointType
{
    Mortar,
    Target
}

public abstract class PointBase : FrameworkElement
{
    //private Point _startPoint;

    public Point Position  = new Point();
    public Point PositionOnHeightMap = new Point();
    public PointType PointType { get; set; }
    private Image _icon;
    public ScaleTransform ScaleTransform { get; set; } = new ScaleTransform();

    private bool _isDragging = false;

    public delegate void MoveHandler(object sender);
    public event MoveHandler? Moved;
    protected PointBase(double width, double height, PointType pointType, Image iconImage)
    {
        PointType = pointType;
        _icon = iconImage;
        Width = width;
        Height = height;
        RenderTransform = ScaleTransform;
        
        MouseLeftButtonDown += CanvasPoint_MouseLeftButtonDown;
        MouseMove += CanvasPoint_MouseMove;
        MouseLeftButtonUp += CanvasPoint_MouseLeftButtonUp;
        
    }
    
    private void CanvasPoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _isDragging = true;
        CaptureMouse();
    }

    protected virtual void CanvasPoint_MouseMove(object sender, MouseEventArgs e)
    {
        if (_isDragging)
        {
            Moved?.Invoke(this);
            Point newPoint = e.GetPosition(Parent as UIElement);
           
            /*Console.WriteLine($"---------------------------");
            Console.WriteLine($"{newPoint} NEW POINT");
            Console.WriteLine($"---------------------------");*/
            if (newPoint.X > 0 && newPoint.Y > 0 && newPoint.X < 4096 && newPoint.Y < 4096)
            {
                Canvas.SetLeft(this, newPoint.X);
                Canvas.SetTop(this, newPoint.Y);
                Position.X = Canvas.GetLeft(this);
                Position.Y = Canvas.GetTop(this);
            }
        }
    }

    private void CanvasPoint_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        _isDragging = false;
        ReleaseMouseCapture();
    }
    
    protected override void OnRender(DrawingContext drawingContext)
    {
        if (_icon != null && _icon.Source != null)
        {
            drawingContext.DrawImage(_icon.Source, new Rect(-Width/2, -Height/2, Width, Height));
        }
    }
}