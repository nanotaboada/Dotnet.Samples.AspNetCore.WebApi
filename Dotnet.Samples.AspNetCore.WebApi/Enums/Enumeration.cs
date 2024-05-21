using System.Reflection;

namespace Dotnet.Samples.AspNetCore.WebApi.Enums;

/// <summary>
/// Enumeration class
/// https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/enumeration-classes-over-enum-types
/// </summary>
public abstract class Enumeration
{
    public int Id { get; private set; }

    public string Text { get; private set; }

    protected Enumeration(int id, string name) => (Id, Text) = (id, name);

    public override string ToString() => Text;

    public static IEnumerable<T> GetAll<T>()
        where T : Enumeration =>
        typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration enumeration)
        {
            return false;
        }

        var areSameType = GetType().Equals(obj.GetType());
        var areSameValue = Id.Equals(enumeration.Id);

        return areSameType && areSameValue;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);
}
