using System;

namespace SibnordPayroll.BL
{
    public class TimeTracking
    {
        public DateTime Date { get; }
        public Employee Employee { get; }
        public double Hours { get; }
        public string Description { get; }
        
        public TimeTracking(DateTime date, Employee employee, double hours, string description)
        {
            Date = date;
            Employee = employee;
            Hours = hours;
            Description = description;
        }
    }
}
