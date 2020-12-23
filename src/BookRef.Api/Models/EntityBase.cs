using System;

namespace BookRef.Api.Models
{
    public abstract class EntityBase
    {
        protected EntityBase()
        {
        }

        protected EntityBase(
            long id)
            : this() =>
            Id = id;

        public long Id { get; set; }

        public override bool Equals(
            object? obj)
        {
            if (!(obj is EntityBase other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (GetRealType() != other.GetRealType())
            {
                return false;
            }

            if (Id == 0 || other.Id == 0)
            {
                return false;
            }

            return Id == other.Id;
        }

        public static bool operator ==(
            EntityBase a,
            EntityBase b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(
            EntityBase a,
            EntityBase b) =>
            !(a == b);

        public override int GetHashCode() =>
            (GetRealType().ToString() + Id)
            .GetHashCode(StringComparison.InvariantCulture);

        private Type GetRealType()
        {
            Type type = GetType();

            if (type.ToString().Contains("Castle.Proxies.", StringComparison.InvariantCulture) && type.BaseType != null)
            {
                return type.BaseType;
            }

            return type;
        }
    }
}
