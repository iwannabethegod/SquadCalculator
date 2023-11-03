using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using SquadCalculator.ViewModels;
using SquadCalculator.Weapons;
using Brushes = System.Windows.Media.Brushes;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace SquadCalculator.Models;

public enum ZoomLevel
{
    ZoomLevel1 = 1,
    ZoomLevel2 = 2,
    ZoomLevel3 = 3,
    ZoomLevel4 = 4,
    ZoomLevel5 = 5,
    ZoomLevel6 = 6,
    ZoomLevel7 = 7
}
public static class MapControl
{
    private static Point _mouseClickPosition;
    
    private static bool _isLeftDown;
    private static bool _isMouseMove;
    private static bool _allowMove = true;

    private static Canvas? CanvasDrawingArea { get; set; } = new Canvas();
    private static Canvas? OuterCanvas { get; set; } = new Canvas();
    private static TranslateTransform? CanvasTransform { get; set; } = new TranslateTransform();
    private static ScaleTransform? CanvasScale { get; set; } = new ScaleTransform();
     
    private static double _leftX = 0;
    private static double _rightX = 0;
    private static double _topY = 0;
    private static double _bottomY = 0;

    private static Point _origin;
    private static Point _start;
    
    private static double[] _scales = {0.2, 0.3, 0.45, 0.675, 1.0115, 1.52051, 2.271};
    private static ZoomLevel _currentZoom = ZoomLevel.ZoomLevel1;
    private static FireCoordinates _fireCoordinates = new FireCoordinates();
    private static List<TargetPoint> _targets = new List<TargetPoint>();
    
    private static Bitmap? _bitmapHeightmap;

    private static Image _mortarImage = new Image()
    {
        Source = new BitmapImage(new Uri("mortar.png", UriKind.Relative))
    };

    private static Image _targetImage = new Image()
    {
        Source = new BitmapImage(new Uri("target.png", UriKind.Relative))
    };
    private static Rectangle HeightMapImage { get; set; }
    public static Map SelectedMap
    {
        get
        {
            return MainViewModel.selectedItem;
        }
    }

    public static Weapon SelectedWeapon
    {
        get
        {
            return MainViewModel.selectedWeapon;
        }
    }
    
    public static Calculation Calc { get; set; } = new Calculation();

    public static void MouseMove(object sender, MouseEventArgs e)
    {
        try
        {
            Point pt = e.GetPosition((Canvas)sender);
           
            HitTestResult hitTestResult = VisualTreeHelper.HitTest(CanvasDrawingArea, pt);
          
            if (hitTestResult.VisualHit is PointBase == false)
            {
                if (_isLeftDown)
                {
                    if (_allowMove)
                    {
                        _isMouseMove = true;
                       
                        var outerCanvasPos = e.GetPosition(OuterCanvas);
                       
                        Vector v = _start - outerCanvasPos;
                        CanvasTransform.X = _origin.X - v.X;
                        CanvasTransform.Y = _origin.Y - v.Y;
                       
                        CheckBordersCoord();
                    }
                }
            }
            else
            {
                _isMouseMove = false;
                _isLeftDown = false;
            }
        }
        catch (Exception ex)
        {
            //
        }
    }
    private static void CheckBordersCoord()
    {
        _leftX = Math.Round(0 - CanvasDrawingArea.ActualWidth * CanvasScale.ScaleX + 100);
        _rightX = Math.Round(0 + OuterCanvas.ActualWidth - 100);
       
        _topY = Math.Round(0 - CanvasDrawingArea.ActualHeight * CanvasScale.ScaleY + 100);
        _bottomY = Math.Round(0 + OuterCanvas.ActualHeight - 100);
        
        if (CanvasTransform.X <= _leftX) CanvasTransform.X = _leftX;
        if (CanvasTransform.X >= _rightX) CanvasTransform.X = _rightX;
     
        if (CanvasTransform.Y <= _topY) CanvasTransform.Y = _topY;
        if (CanvasTransform.Y >= _bottomY) CanvasTransform.Y = _bottomY;
    }
    public static void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _mouseClickPosition = e.GetPosition(CanvasDrawingArea);
        
        if (e.ClickCount == 2)
        {
            if (SelectedMap != null)
            {
                CreateTarget();
            }
        }
            
        var pos = e.GetPosition(OuterCanvas);
        _start = pos;
        
        _origin = new Point(CanvasTransform.X, CanvasTransform.Y); 
        
        _isLeftDown = true;
        if (_isMouseMove)
        {
            _isMouseMove = false;
        }
        
    }

    public static void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        _isLeftDown = false;
    }

    public static void MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        Point pt = e.GetPosition((Canvas)sender);

        HitTestResult hitTestResult = VisualTreeHelper.HitTest(CanvasDrawingArea, pt);
        if (hitTestResult != null)
        {
            if (hitTestResult.VisualHit is TargetPoint hit)
            {
                CanvasDrawingArea.Children.Remove(hit);
                CanvasDrawingArea.Children.Remove(hit.Grid);
                _targets.Remove(hit);
            }
        }
    }

    public static void MouseWheel(object sender, MouseWheelEventArgs e)
    {
        Point mousePos = e.GetPosition(CanvasDrawingArea);
       
        if (e.Delta > 0)
        {
            if (_currentZoom >= ZoomLevel.ZoomLevel7) return;
            _currentZoom += 1;
                            
            ChangeSize(mousePos, e);
            ChangeIconSize();
            CheckBordersCoord();
        }
        else if (e.Delta < 0)
        {
            if (_currentZoom <= ZoomLevel.ZoomLevel1) return;
            _currentZoom -= 1;
                            
            ChangeSize(mousePos, e);
            ChangeIconSize();
            CheckBordersCoord();
        }
    }

    public static void OnMouseLeave(object sender, MouseButtonEventArgs e)
    {
        _isLeftDown = false;
    }
    
    private static FireResult Shoot(FireCoordinates coords)
    {
        FireResult result = Calc.Shoot(_bitmapHeightmap, SelectedWeapon, coords);
        return result;
    }
      
    private static double SelectZoom(ZoomLevel z, bool reverse = false)
    {
        if (reverse)
        {
            double[] reversedArray = _scales.Reverse().ToArray();
            return reversedArray[(int) z - 1];
        }
        return _scales[(int)z - 1];
    }
      
    private static void ChangeSize(Point mousePos, MouseWheelEventArgs e)
        {
            if (_currentZoom > ZoomLevel.ZoomLevel1 || _currentZoom < ZoomLevel.ZoomLevel7)
            {
                double absoluteX = mousePos.X * CanvasScale.ScaleX + CanvasTransform.X;
                double absoluteY = mousePos.Y * CanvasScale.ScaleY + CanvasTransform.Y;
                double zoom = SelectZoom(_currentZoom);
                
                CanvasScale.ScaleX = zoom;
                CanvasScale.ScaleY = zoom;
                
                CanvasTransform.X = absoluteX - mousePos.X * CanvasScale.ScaleX;
                CanvasTransform.Y = absoluteY - mousePos.Y * CanvasScale.ScaleY;
            }
        }

    private static void ChangeIconSize()
    {
        foreach (var child in CanvasDrawingArea.Children)
        {
            if (child is PointBase icon) 
            {
                    icon.ScaleTransform.ScaleX = SelectZoom(_currentZoom, true);
                    icon.ScaleTransform.ScaleY = SelectZoom(_currentZoom, true);
                    if (child is TargetPoint target)
                    {
                        ChangeInfoPos(target);
                    }
            }
        }
    }

    private static void ChangeInfoPos(TargetPoint target)
    {
        Canvas.SetLeft(target.Grid, Canvas.GetLeft(target) + 35 * target.ScaleTransform.ScaleX);
        Canvas.SetTop(target.Grid, Canvas.GetTop(target)-50 * target.ScaleTransform.ScaleY); 
    }
    
    public static void SetCanvasStartPosition()
    {
        List<UIElement> elementsToRemove = new List<UIElement>();
        foreach (var t in CanvasDrawingArea.Children)
        {
            if (t is TargetPoint target)
            {
                elementsToRemove.Add(target);
                elementsToRemove.Add(target.Grid);
            }

            if (t is MortarPoint mortar)
            {
                elementsToRemove.Add(mortar);
                elementsToRemove.Add(mortar.RadiusEllipse);
            }
        }
        foreach (var element in elementsToRemove)
        {
            CanvasDrawingArea.Children.Remove(element);
        }
        
        CanvasScale.ScaleY = SelectZoom(_currentZoom);
        CanvasScale.ScaleX = SelectZoom(_currentZoom);
        
        CanvasTransform.X = OuterCanvas.ActualWidth/2 - (CanvasDrawingArea.ActualWidth * CanvasScale.ScaleX)/2;
        CanvasTransform.Y = 0;
        
        _bitmapHeightmap = new Bitmap(SelectedMap.HeightMapPath);
        CreateMortar();
    }

    public static void SetProperties(
        Canvas drawingArea, 
        Canvas outerCanvas, 
        TranslateTransform canvasTransform, 
        ScaleTransform canvasScale,
        Rectangle heightMapImage)
    {
        CanvasDrawingArea = drawingArea;
        OuterCanvas = outerCanvas;
        CanvasTransform = canvasTransform;
        CanvasScale = canvasScale;
        HeightMapImage = heightMapImage;
    }


    
    private static void CreateTarget()
    {
        Console.WriteLine("target");
        TargetPoint target = new TargetPoint(50, 50, PointType.Target, _targetImage);
        target.ScaleTransform.ScaleX = SelectZoom(_currentZoom, true);
        target.ScaleTransform.ScaleY = SelectZoom(_currentZoom, true);
        CanvasDrawingArea.Children.Add(target);
        
        Canvas.SetLeft(target, Math.Round(_mouseClickPosition.X));
        Canvas.SetTop(target, Math.Round(_mouseClickPosition.Y));

        target.Position.X = Canvas.GetLeft(target);
        target.Position.Y = Canvas.GetTop(target);
        
        Point heightMapPoint = CanvasDrawingArea.TranslatePoint(target.Position,HeightMapImage);
        target.PositionOnHeightMap = heightMapPoint;
        
        target.ReadyTarget += Shoot;
        target.Moved += UpdateTargetWhileMoving;
        target.TargetReady();
        
        TranslateCoordinates(target);
            
        target.SetFireResult(Shoot(_fireCoordinates));
        _targets.Add(target);
         
        Grid targetGrid = SetGrid(target);
        targetGrid.RenderTransform = target.ScaleTransform;
        target.Grid = targetGrid;
        CanvasDrawingArea.Children.Add(targetGrid);
        Canvas.SetLeft(targetGrid, target.Position.X * CanvasScale.ScaleX);
        Canvas.SetTop(targetGrid, target.Position.Y);
    }

    private static void CreateMortar()
    {
        var foundElement = CanvasDrawingArea.Children.OfType<MortarPoint>().FirstOrDefault();
        
        if (foundElement == null)
        {
            Point creationPoint = new Point()
            {
                X = CanvasDrawingArea.Width / 2,
                Y = CanvasDrawingArea.Height / 2

            };
            
            MortarPoint mortar = new MortarPoint(50, 50, PointType.Mortar, _mortarImage, SelectedMap.Size, creationPoint);
            mortar.ScaleTransform.ScaleX = SelectZoom(_currentZoom, true);
            mortar.ScaleTransform.ScaleY = SelectZoom(_currentZoom, true);
            CanvasDrawingArea.Children.Add(mortar);
            
            Canvas.SetLeft(mortar, creationPoint.X);
            Canvas.SetTop(mortar, creationPoint.Y);
            
            mortar.Position.X = Canvas.GetLeft(mortar);
            mortar.Position.Y = Canvas.GetTop(mortar);
            
            Point mapPoint = CanvasDrawingArea.TranslatePoint(mortar.Position, HeightMapImage);
            mortar.PositionOnHeightMap = mapPoint;
            mortar.Moved += UpdateMortarWhileMoving;
            
            TranslateCoordinates(mortar);
            
            CanvasDrawingArea.Children.Add(mortar.RadiusEllipse);
        }
    }

    private static void TranslateCoordinates<T>(T point) where T : PointBase?
    {
        if(point is MortarPoint mortar)
        {
            Point mortarCoordinates = CanvasDrawingArea.TranslatePoint(mortar.Position, HeightMapImage);
            _fireCoordinates.MortarPositionOnHeightMap = (mortarCoordinates.X, mortarCoordinates.Y);
            (double, double) pos = (Canvas.GetLeft(mortar), Canvas.GetTop(mortar));
            _fireCoordinates.MortarPositionOnMap = pos;
        }

        if (point is TargetPoint target)
        {
            Point targetCoordinates = CanvasDrawingArea.TranslatePoint(target.Position, HeightMapImage);
            _fireCoordinates.TargetPositionOnHeightMap = (targetCoordinates.X, targetCoordinates.Y);
            (double, double) targetPos = (Canvas.GetLeft(target), Canvas.GetTop(target));
            _fireCoordinates.TargetPositionOnMap = targetPos;
        }
    }
    
    private static void UpdateMortarWhileMoving(object sender)
    {
        TranslateCoordinates(sender as MortarPoint);
            
        foreach (var target in _targets)
        {
            UpdateTargetWhileMoving(target);
        }
    }
    private static void UpdateTargetWhileMoving(object sender)
    {
        if (sender is TargetPoint target)
        {
            TranslateCoordinates(target);
            target.SetFireResult(Shoot(_fireCoordinates));
        }
    }
    
    private static Grid SetGrid(TargetPoint target)
    { 
        Grid grid = new Grid();
        ScaleTransform gridTransform = new ScaleTransform();
        grid.RenderTransform = gridTransform;
        gridTransform.ScaleX = SelectZoom(_currentZoom, true);
        gridTransform.ScaleY = SelectZoom(_currentZoom, true);
        RowDefinition row0 = new RowDefinition();
        RowDefinition row1 = new RowDefinition();
        RowDefinition row2 = new RowDefinition();
        ColumnDefinition column0 = new ColumnDefinition();
        ColumnDefinition column1 = new ColumnDefinition();
        
        column0.Width = new GridLength(150, GridUnitType.Pixel);
        column1.Width = new GridLength(1, GridUnitType.Auto);
        row0.Height = new GridLength(1, GridUnitType.Auto);
        row1.Height = new GridLength(1, GridUnitType.Auto);
        row2.Height = new GridLength(1, GridUnitType.Auto);
        
        grid.RowDefinitions.Add(row0);
        grid.RowDefinitions.Add(row1);
        grid.RowDefinitions.Add(row2);
        grid.ColumnDefinitions.Add(column0);
        grid.ColumnDefinitions.Add(column1);
   
        TextBlock distance = new TextBlock();
        TextBlock elevation = new TextBlock();
        TextBlock azimuth = new TextBlock();
        
          
        Binding distanceBinding = new Binding("Distance");
        distanceBinding.Source = target.TargetFireResult;
        distance.SetBinding(TextBlock.TextProperty, distanceBinding);

        Binding elevationBinding = new Binding("Elevation");
        elevationBinding.Source = target.TargetFireResult;
        elevation.SetBinding(TextBlock.TextProperty, elevationBinding);

        Binding azimuthBinding = new Binding("Azimuth");
        azimuthBinding.Source = target.TargetFireResult;
        azimuth.SetBinding(TextBlock.TextProperty, azimuthBinding);
        
        distance.TextAlignment = TextAlignment.Left;
        elevation.TextAlignment = TextAlignment.Left;
        azimuth.TextAlignment = TextAlignment.Left;
        distance.FontSize = 30;
        elevation.FontSize = 40;
        azimuth.FontSize = 30;
        
        distance.Foreground = Brushes.Red;
        elevation.Foreground = Brushes.Red;
        azimuth.Foreground = Brushes.Red;
        
        distance.FontWeight = FontWeights.UltraBold;
        elevation.FontWeight = FontWeights.UltraBold;
        azimuth.FontWeight = FontWeights.UltraBold;
        
        
        DropShadowEffect dropShadowEffect = new DropShadowEffect();
        dropShadowEffect.Color = Colors.Black;
        dropShadowEffect.BlurRadius = 10;
        dropShadowEffect.ShadowDepth = 0;

        elevation.Effect = dropShadowEffect;
        distance.Effect = dropShadowEffect;
        azimuth.Effect = dropShadowEffect;
        
        Grid.SetRow(elevation,0);
        Grid.SetColumnSpan(elevation,2);
        
        Grid.SetRow(azimuth, 1);
        Grid.SetColumn(azimuth,0);
        
        Grid.SetColumn(distance,0);
        Grid.SetRow(distance, 2);
        
        grid.Children.Add(distance);
        grid.Children.Add(elevation);
        grid.Children.Add(azimuth);
        return grid;
    }
}