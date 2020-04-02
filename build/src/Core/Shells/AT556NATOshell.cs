using DuckGame;

namespace TMGmod.Core.Shells
{
    public class AT556NATOShell : EjectedShell
    {
        public AT556NATOShell(float xpos, float ypos)
          : base(xpos, ypos, Mod.GetPath<TMGmod>("AT556NATOShell"))
        {
            scale *= 0.707f;
        }
    }
}