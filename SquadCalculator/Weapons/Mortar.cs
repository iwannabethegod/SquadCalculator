namespace SquadCalculator.Weapons;

public class Mortar : Weapon
{
    public override string Name { get; set; } = "Mortar";
    public override double Velocity { get; set; } = 109.890938;
    public override double Gravity { get; set; } = 9.8;
    public override AngleUnit AngleUnit { get; set; } = AngleUnit.Milliradians;
}