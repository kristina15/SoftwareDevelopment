namespace IoC
{
    public class Export:Attribute
    {
        public Type Type { get; set; }

        public Export()
        {
            Type = null;
        }
        public Export(Type type)
        {
            Type = type;
        }
    }
}

