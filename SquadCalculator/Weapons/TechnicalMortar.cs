namespace SquadCalculator.Weapons;

public class TechnicalMortar : Weapon
{
    public override string Name { get; set; } = "Technical Mortar";
    public override double Velocity { get; set; } = 109.890938;
    public override double Gravity { get; set; } = 9.8;
    public override AngleUnit AngleUnit { get; set; } = AngleUnit.Degrees;
    public override AngleType AngleType { get; set; } = AngleType.High;
    public override int MaxRange { get; set; } = 1230;
}