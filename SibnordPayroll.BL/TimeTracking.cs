using System;

namespace SibnordPayroll.BL
{
    public class TimeTracking
    {
        public DateTime Date { get; }
        public string EmployeeName { get; }
        public double Hours { get; }
        public string Description { get; }
        
        public TimeTracking(DateTime date, string employeeName, double hours, string description)
        {
            Date = date;
            EmployeeName = employeeName;
            Hours = hours;
            Description = description;
        }
    }
}
