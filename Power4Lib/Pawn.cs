using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Power4Lib
{
    [Serializable]
    public class Pawn : ICloneable, IEquatable<Pawn>
    {
        public char Char { get; set; }
        public bool Winning { get; set; }
        public ConsoleColor Color { get; set; }

        public Pawn(char @char, bool winning = false)
        {
            Char = @char;
            Winning = winning;
        }

        public override string ToString()
        {
            return Char.ToString();
        }

        #region ICloneable support
        public object Clone()
        {
            return new Pawn(Char, Winning) { Color = Color };
        }
        #endregion

        #region Equals support
        public override bool Equals(object obj)
        {
            return Equals(obj as Pawn);
        }

        public bool Equals(Pawn other)
        {
            return other != null &&
                   Char == other.Char &&
                   Color == other.Color;
        }

        public override int GetHashCode()
        {
            var hashCode = 1217968573;
            hashCode = hashCode * -1521134295 + Char.GetHashCode();
            hashCode = hashCode * -1521134295 + Color.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Pawn left, Pawn right)
        {
            return EqualityComparer<Pawn>.Default.Equals(left, right);
        }

        public static bool operator !=(Pawn left, Pawn right)
        {
            return !(left == right);
        }
        #endregion
    }
}
