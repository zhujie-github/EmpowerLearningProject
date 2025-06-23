namespace Company.Core.Ioc
{
    public enum Lifetime
    {
        Transient,
        Singleton
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple =false)]
    public class ExposedServiceAttribute(Lifetime lifetime = Lifetime.Transient, bool autoInitialize = false, params Type[] types) : Attribute
    {
        public Lifetime Lifetime
        {
            get; private set;
        } = lifetime;

        public bool AutoInitialize
        {
            get; private set;
        } = autoInitialize;

        public Type[] Types { get; private set; } = types;
    }
}
