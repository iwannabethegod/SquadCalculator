namespace SquadCalculator.Weapons;

public class HellCannon : Weapon
{
    public override string Name { get; set; } = "Hell Cannon";
    public override double Velocity { get; set; } = 95.0;
    public override double Gravity { get; set; } = 9.8;
    public override AngleUnit AngleUnit { get; set; } = AngleUnit.Degrees;
    public override AngleType AngleType { get; set; } = AngleType.Both;
    public override int MaxRange { get; set; } = 924;
}