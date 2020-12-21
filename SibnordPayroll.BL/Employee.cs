namespace SibnordPayroll.BL
{
    public class Employee
    {
        public string Name { get; }

        public Employee(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
