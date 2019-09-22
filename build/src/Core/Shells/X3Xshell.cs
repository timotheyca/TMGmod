using DuckGame;

namespace TMGmod.Core.Shells
{
    public class X3XShell : EjectedShell
    {
        public X3XShell(float xpos, float ypos)
          : base(xpos, ypos, Mod.GetPath<TMGmod>("X3XShell"))
        {
        }
    }
}