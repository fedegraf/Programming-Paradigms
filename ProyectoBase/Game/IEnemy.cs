using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public interface IEnemy
    {
        Vector2 Speed { get; }
        int MaxHealth { get; }
        bool IsFacingRight { get; }
    }
}
