using System.Reflection;

namespace AuthManager.Domain.Primitives;

public abstract class Enumeration
{
    public int Id { get; private set; }
    public string Name { get; private set; }

    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static bool Equals<T>(T obj1, T obj2) where T : Enumeration => obj1.Id == obj2.Id && nameof(obj1) == nameof(obj2);

    public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
        typeof(T).GetFields(BindingFlags.Public |
                        BindingFlags.Static |
                        BindingFlags.DeclaredOnly)
                    .Select(f => f.GetValue(null))
                    .Cast<T>();

    public static T? GetById<T>(int id) where T : Enumeration =>
        typeof(T).GetFields(BindingFlags.Public |
                        BindingFlags.Static |
                        BindingFlags.DeclaredOnly)
                    .Select(f => f.GetValue(null))
                    .Cast<T>()
                    .FirstOrDefault(x => x.Id == id);
}
