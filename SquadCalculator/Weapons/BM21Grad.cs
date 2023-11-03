namespace SquadCalculator.Weapons;

public class BM21Grad : Weapon
{
    public override string Name { get; set; } = "BM-21 Grad";
    public override double Velocity { get; set; } = 200.0;
    public override double Gravity { get; set; } = 9.8 * 2;
    public override AngleUnit AngleUnit { get; set; } = AngleUnit.Degrees;
    
}