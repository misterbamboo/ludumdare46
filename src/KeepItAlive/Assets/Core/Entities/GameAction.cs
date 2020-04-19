using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Core.Entities
{
    public class GameAction
    {
        public string Text { get; set; }
        public int X { get; set; }
        public int Z { get; set; }
        public GameActionTypes ActionType { get; set; }
    }
}
