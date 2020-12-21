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

        private static List<Employee> employees;
        private static Employee currentEmployee;

        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в Sibnord Payroll.");
            InitializeEmployeesList();

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

        private static void SaveEmployee(Employee employee)
        {
            employees.Add(employee);
            using (var sw = new StreamWriter(EMPLOYEES_PATH, true, Encoding.Default))
            {
                sw.Write("\n{0}", employee.Name);
            }

            Console.WriteLine("Пользоватль {0} создан.", employee);
        }
    }
}
