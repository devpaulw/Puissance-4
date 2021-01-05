using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Power4Lib
{
    [Serializable]
    public class Player : IEquatable<Player>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Pawn Pawn { get; set; }

        public Player(int id, string name, Pawn pawn)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Pawn = pawn;
        }

        public override string ToString()
        {
            return $"Player {Name} (Id {Id} ; Pawn: '{Pawn}')";
        }

        #region IEquatable support
        public override bool Equals(object obj)
        {
            return Equals(obj as Player);
        }

        public bool Equals(Player other)
        {
            return other != null &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }

        public static bool operator ==(Player left, Player right)
        {
            return EqualityComparer<Player>.Default.Equals(left, right);
        }

        public static bool operator !=(Player left, Player right)
        {
            return !(left == right);
        }
        #endregion
    }
}
