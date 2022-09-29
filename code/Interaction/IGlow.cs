namespace Survivor.Interaction;

public interface IGlow
{
	public virtual Color GlowColor => Color.White;
	public virtual int   GlowWidth => 1;
	public         void  SetGlow( bool enableGlow );
}
