using System.Reflection;

namespace BrickManager.BrickInventorySystem.Core.SeedWork;

public abstract class ValueObject<T> : IEquatable<T> where T : ValueObject<T>
{
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        var other = obj as T;

        return Equals(other);
    }

    public virtual bool Equals(T? other)
    {
        if (other is null)
            return false;

        var type = GetType();
        var otherType = other.GetType();

        if (type != otherType)
            return false;

        var fields = GetFields(this);

        foreach (var field in fields)
        {
            var value1 = field.GetValue(other);
            var value2 = field.GetValue(this);

            if (value1 is null)
            {
                if (value2 is not null)
                    return false;
            }
            else if (!value1.Equals(value2))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        var fields = GetFields(this);

        const int startValue = 17;
        const int multiplier = 59;

        return fields
            .Select(field => field.GetValue(this))
            .Where(value => value != null)
            .Aggregate(startValue, (current, value) => current * multiplier + value.GetHashCode());
    }

    public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
        => !(a == b);

    public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
        => ReferenceEquals(a, b) || a?.Equals(b) == true;

    private static IEnumerable<FieldInfo> GetFields(object obj)
    {
        var type = obj.GetType();
        var fields = new List<FieldInfo>();

        while (type != typeof(object))
        {
            if (type is null)
                continue;

            var fieldInfos = type.GetFields(BindingFlags.Instance
                                            | BindingFlags.NonPublic | BindingFlags.Public);

            fields.AddRange(fieldInfos);

            type = type.BaseType;
        }

        return fields;
    }
}