namespace SquadCalculator;

public class FireCoordinates
{
    public (double X, double Y) MortarPositionOnMap { get; set; } = default;
    public (double X, double Y) TargetPositionOnMap { get; set; } = default;
    public (double X, double Y) MortarPositionOnHeightMap { get; set; } = default;
    public (double X, double Y) TargetPositionOnHeightMap { get; set; } = default;

    public override string ToString()
    {
        return $"FireCoordinates: \n {MortarPositionOnMap} {TargetPositionOnMap} \n" +
               $" {MortarPositionOnHeightMap} {TargetPositionOnHeightMap}";
    }
}