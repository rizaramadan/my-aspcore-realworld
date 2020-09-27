using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace my_aspcore_realworld.Entities
{
    public abstract class Enumerable : IComparable
    {
        public string Name { get; }

        public int Id { get; }

        protected Enumerable(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumerable
        {
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public |
                                             BindingFlags.Static |
                                             BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Enumerable otherValue)) return false;

            bool typeMatches = GetType().Equals(obj.GetType());
            bool valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return string.GetHashCode($"{Id}{Name}", StringComparison.OrdinalIgnoreCase);
        }

        public int CompareTo(object other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other), "cannot compare null");
            return Id.CompareTo(((Enumerable)other).Id);
        }

        public static bool operator ==(Enumerable left, Enumerable right)
        {
            if (left is null)
            {
                return right is null;
            }
            return left.Equals(right);
        }
        public static bool operator !=(Enumerable left, Enumerable right)
        {
            return !(left == right);
        }
        public static bool operator <(Enumerable left, Enumerable right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left), "cannot less-than null");
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Enumerable left, Enumerable right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left), "cannot less-than-equal null");
            return left.CompareTo(right) <= 0;
        }
        public static bool operator >(Enumerable left, Enumerable right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left), "cannot greater-than null");
            return left.CompareTo(right) > 0;
        }
        public static bool operator >=(Enumerable left, Enumerable right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left), "cannot greater-than-equal null");
            return left.CompareTo(right) >= 0;
        }
    }
}
