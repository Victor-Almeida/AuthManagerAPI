using System.Reflection;

namespace AuthManager.Domain.Primitives
{
    public abstract class Enumeration<TEnumeration> where TEnumeration : IEnumeration
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public static bool Equals(TEnumeration obj1, TEnumeration obj2) => obj1.Id == obj2.Id && nameof(obj1) == nameof(obj2);

        public static IEnumerable<TEnumeration> GetAll() =>
            typeof(TEnumeration).GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
                     .Select(f => f.GetValue(null))
                     .Cast<TEnumeration>();


        public static TEnumeration? GetById(int id) =>
            typeof(TEnumeration).GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
                     .Select(f => f.GetValue(null))
                     .Cast<TEnumeration>()
                     .FirstOrDefault(x => x.Id == id);
    }
}
