using SibnordPayroll.BL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SibnordPayroll.CMD
{
    class Program
    {
        private const string EMPLOYEES_PATH = "Employees.csv";
        private const string TIME_TRACKINGS_PATH = "TimeTrackings.csv";

        private static List<Employee> employees;
        private static List<TimeTracking> timeTrackings;

        private static Employee currentEmployee;

        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в Sibnord Payroll.");
            InitializeEmployeesList();
            InitializeTimeTrackingList();

            if (employees.Count == 0)
            {
                Console.WriteLine("В программе нет ни одного пользователя.");
                Console.WriteLine("(З)авести первого пользователя? Или (В)ыйти из программы?");
                var key = Console.ReadKey().Key;
                if (key == ConsoleKey.P)
                {
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write("Введите ваше имя: ");
                    var firstEmployeeName = Console.ReadLine();
                    var firstEmployee = new Employee(firstEmployeeName);
                    SaveEmployee(firstEmployee);
                    SetCurrentEmployee(firstEmployeeName);
                }
                else if (key == ConsoleKey.D)
                {
                    Environment.Exit(0);
                }
            }
            else
            {
                Console.Write("Введите ваше имя: ");
                var name = Console.ReadLine();
                SetCurrentEmployee(name);
            }
            
            Console.WriteLine("Нажмите любую клавишу для продолжения..");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Добро пожаловать, {0}.", currentEmployee);

            Console.WriteLine("\nМЕНЮ");
            Console.WriteLine("1 - Добавить сотрудника");
            Console.WriteLine("2 - Добавить часы");
            Console.WriteLine("0 - Выйти из программы");

            while (true)
            {
                Console.SetCursorPosition(0, Console.CursorTop);

                var key = Console.ReadKey().Key;
                if (key == ConsoleKey.NumPad1)
                {
                    Console.WriteLine("Введите имя нового сотрудника");
                    var newEmployee = new Employee(Console.ReadLine());
                    SaveEmployee(newEmployee);
                }
                else if (key == ConsoleKey.NumPad2)
                {
                    Console.Write("Введите дату: ");
                    var date = DateTime.Parse(Console.ReadLine());

                    Console.Write("Введите имя сотрудника: ");
                    var employeeName = Console.ReadLine();

                    Console.Write("Введите количетво часов: ");
                    var hours = int.Parse(Console.ReadLine());

                    Console.Write("Введите описание: ");
                    var description = Console.ReadLine();

                    var newTimeTracking = new TimeTracking(date, employeeName, hours, description);
                    SaveTimeTracking(newTimeTracking);
                }
                else if (key == ConsoleKey.NumPad0)
                {
                    Environment.Exit(0);
                }
            }
        }

        private static void SetCurrentEmployee(string employeeName)
        {
            foreach (var employee in employees)
            {
                if (employeeName == employee.Name)
                {
                    currentEmployee = employee;
                    break;
                }
            }
            if (currentEmployee == null)
            {
                Console.WriteLine("Пользователь с таким именем не найден.");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        private static void InitializeEmployeesList()
        {
            employees = new List<Employee>();

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

        private static void InitializeTimeTrackingList()
        {
            timeTrackings = new List<TimeTracking>();

            if (File.Exists(TIME_TRACKINGS_PATH))
            {
                var fileStrings = File.ReadAllLines(TIME_TRACKINGS_PATH, Encoding.Default);
                foreach (var fileString in fileStrings)
                {
                    var fieldsString = fileString.Split(';');
                    var date = DateTime.Parse(fieldsString[0]);
                    var employeeName = fieldsString[1];
                    var hours = int.Parse(fieldsString[2]);
                    var description = fieldsString[3];
                    timeTrackings.Add(new TimeTracking(date, employeeName, hours, description));
                }
            }
            else
            {
                File.Create(TIME_TRACKINGS_PATH);
            }
        }

        private static void SaveEmployee(Employee employee)
        {
            employees.Add(employee);
            using (var sw = new StreamWriter(EMPLOYEES_PATH, true, Encoding.Default))
            {
                sw.WriteLine(employee.Name);
            }

            Console.WriteLine("Пользоватль {0} создан.", employee);
        }

        private static void SaveTimeTracking(TimeTracking timeTracking)
        {
            timeTrackings.Add(timeTracking);
            using (var sw = new StreamWriter(TIME_TRACKINGS_PATH, true, Encoding.Default))
            {
                sw.WriteLine("{0};{1};{2};{3}", timeTracking.Date, 
                                                timeTracking.EmployeeName, 
                                                timeTracking.Hours,
                                                timeTracking.Description);
            }

            Console.WriteLine("Документ за {0} для сотрудника {1} успешно создан.", timeTracking.Date, timeTracking.EmployeeName);
        }
    }
}
