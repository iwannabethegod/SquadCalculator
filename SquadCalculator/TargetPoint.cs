using System.Windows.Controls;
using System.Windows.Input;

namespace SquadCalculator;

public class TargetPoint : PointBase
{
    public FireResult? TargetFireResult { get; private set; } = new FireResult("0", "0","0");
    
    public delegate FireResult TargetHandler(FireCoordinates f);
    public event TargetHandler? ReadyTarget;
    
    public Grid Grid { get; set; } = new Grid();
    FireCoordinates _fireCoordinates = new FireCoordinates();
    public TargetPoint(double width, double height, PointType pointType, Image iconImage) : base(width, height, pointType, iconImage)
    {
        _fireCoordinates.TargetPositionOnMap = (Position.X, Position.Y);
        _fireCoordinates.TargetPositionOnHeightMap = (PositionOnHeightMap.X, PositionOnHeightMap.Y);
    }
    
    private void OnReadyTarget()
    {
        ReadyTarget?.Invoke(_fireCoordinates);
    }
    public void TargetReady()
    {
        OnReadyTarget();
    }

    public void SetFireResult(FireResult fireResult)
    {
        TargetFireResult.Distance = fireResult.Distance;
        TargetFireResult.Azimuth = fireResult.Azimuth;
        TargetFireResult.Elevation = fireResult.Elevation;
    }
    
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        Canvas.SetLeft(Grid, Canvas.GetLeft(this) + 35 * this.ScaleTransform.ScaleX);
        Canvas.SetTop(Grid, Canvas.GetTop(this)-50 * this.ScaleTransform.ScaleY);
    }
}