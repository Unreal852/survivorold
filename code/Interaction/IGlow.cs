namespace Survivor.Interaction;

public interface IGlow
{
	public virtual Color GlowColor => Color.White;
	public virtual float GlowWidth => 0.2f;
	public         void  SetGlow( bool enableGlow );
}
