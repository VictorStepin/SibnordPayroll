using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SibnordPayroll.BL
{
    public static class DataStorage
    {
        private const string EMPLOYEES_PATH = "Employees.csv";
        private const string TIME_TRACKINGS_PATH = "TimeTrackings.csv";

        public static readonly List<Employee> employees;
        public static readonly List<TimeTracking> timeTrackings;

        static DataStorage()
        {
            employees = new List<Employee>();
            timeTrackings = new List<TimeTracking>();

            LoadEmployees();
            LoadTimeTrackings();
        }

        private static void LoadEmployees()
        {
            if (File.Exists(EMPLOYEES_PATH))
            {
                var employeesStrings = File.ReadAllLines(EMPLOYEES_PATH, Encoding.Default);
                foreach (var employeeString in employeesStrings)
                {
                    employees.Add(new Employee(employeeString));
                }
            }
            else
            {
                File.Create(EMPLOYEES_PATH);
            }
        }

        private static void LoadTimeTrackings()
        {
            if (File.Exists(TIME_TRACKINGS_PATH))
            {
                var fileStrings = File.ReadAllLines(TIME_TRACKINGS_PATH, Encoding.Default);
                foreach (var fileString in fileStrings)
                {
                    var fieldsString = fileString.Split(';');
                    var date = DateTime.Parse(fieldsString[0]);
                    var employee = EmployeeByName(fieldsString[1]);
                    var hours = int.Parse(fieldsString[2]);
                    var description = fieldsString[3];
                    timeTrackings.Add(new TimeTracking(date, employee, hours, description));
                }
            }
            else
            {
                File.Create(TIME_TRACKINGS_PATH);
            }
        }

        public static void SaveEmployee(Employee employee)
        {
            employees.Add(employee);
            using (var sw = new StreamWriter(EMPLOYEES_PATH, true, Encoding.Default))
            {
                sw.WriteLine(employee.Name);
            }

            Console.WriteLine("Пользоватль {0} создан.", employee); // Убрать UI-методы из бизнес-логики!
        }

        public static void SaveTimeTracking(TimeTracking timeTracking)
        {
            timeTrackings.Add(timeTracking);
            using (var sw = new StreamWriter(TIME_TRACKINGS_PATH, true, Encoding.Default))
            {
                sw.WriteLine("{0};{1};{2};{3}", timeTracking.Date.ToShortDateString(),
                                                timeTracking.Employee.Name,
                                                timeTracking.Hours,
                                                timeTracking.Description);
            }

            Console.WriteLine("Документ за {0} для сотрудника {1} успешно создан.", timeTracking.Date.ToShortDateString(), timeTracking.Employee.Name); // Убрать UI-методы из бизнес-логики!
        }

        public static Employee EmployeeByName(string employeeName)
        {
            foreach (var employee in employees)
            {
                if (employee.Name == employeeName)
                {
                    return employee;
                }
            }

            return null; // НЕЛЬЗЯ ВОЗВРАЩАТЬ NULL!
        }
    }
}
