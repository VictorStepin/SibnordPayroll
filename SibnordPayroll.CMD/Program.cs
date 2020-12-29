using SibnordPayroll.BL;
using System;
using System.Collections.Generic;

namespace SibnordPayroll.CMD
{
    class Program
    {
        private static Employee currentEmployee;

        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в Sibnord Payroll.");

            if (DataStorage.employees.Count == 0)
            {
                Console.WriteLine("В программе нет ни одного пользователя.");
                Console.WriteLine("(З)авести первого пользователя? Или (В)ыйти из программы?");
                var key = Console.ReadKey().Key;
                Console.SetCursorPosition(0, Console.CursorTop);
                if (key == ConsoleKey.P)
                {
                    Console.Write("Введите ваше имя: ");
                    
                    var firstEmployeeName = Console.ReadLine();
                    var firstEmployee = new Employee(firstEmployeeName);
                    DataStorage.SaveEmployee(firstEmployee);
                    
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
            Console.WriteLine("3 - Сформировать отчет по сотрудникам за пероид");
            Console.WriteLine("0 - Выйти из программы");

            while (true)
            {
                Console.SetCursorPosition(0, Console.CursorTop);

                var key = Console.ReadKey().Key;
                Console.SetCursorPosition(0, Console.CursorTop);
                if (key == ConsoleKey.NumPad1)
                {
                    Console.Write("Введите имя нового сотрудника: ");

                    DataStorage.SaveEmployee(new Employee(Console.ReadLine()));
                }
                else if (key == ConsoleKey.NumPad2)
                {
                    Console.Write("Введите дату: ");
                    var date = DateTime.Parse(Console.ReadLine());

                    Console.Write("Введите имя сотрудника: ");
                    var employeeToAddHours = DataStorage.EmployeeByName(Console.ReadLine());
                    if (employeeToAddHours != null)
                    {
                        Console.Write("Введите количетво часов: ");
                        var hours = int.Parse(Console.ReadLine());

                        Console.Write("Введите описание: ");
                        var description = Console.ReadLine();

                        var newTimeTracking = new TimeTracking(date, employeeToAddHours, hours, description);
                        DataStorage.SaveTimeTracking(newTimeTracking);
                    }
                    else
                    {
                        Console.WriteLine("Сотрудника с таким именем нет в программе.");
                    }
                }
                else if (key == ConsoleKey.NumPad3)
                {
                    Console.Write("Введите начало периода: ");
                    var startDate = DateTime.Parse(Console.ReadLine());

                    Console.Write("Введите конец периода: ");
                    var endDate = DateTime.Parse(Console.ReadLine());

                    CreateAndShowGeneralReport(startDate, endDate);
                }
                else if (key == ConsoleKey.NumPad0)
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Введите корректный номер пункта меню.");
                }
            }
        }

        private static void SetCurrentEmployee(string employeeName)
        {
            foreach (var employee in DataStorage.employees)
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

        private static void CreateAndShowGeneralReport(DateTime startDate, DateTime endDate)
        {
            var timeTrackingsInPeriod = new List<TimeTracking>();
            foreach (var timeTracking in DataStorage.timeTrackings)
            {
                if (timeTracking.Date >= startDate && timeTracking.Date <= endDate)
                {
                    timeTrackingsInPeriod.Add(timeTracking);
                }
            }

            if (timeTrackingsInPeriod.Count == 0)
            {
                Console.WriteLine("Не найдено документов учета времени за указанный период");
            }
            else
            {
                Employee employeeToCalculate = null;
                var hours = 0d;
                int i = 0;
                while (timeTrackingsInPeriod.Count > 0)
                {
                    if (employeeToCalculate == null)
                    {
                        employeeToCalculate = timeTrackingsInPeriod[i].Employee;
                    }
                    else
                    {
                        if (employeeToCalculate == timeTrackingsInPeriod[i].Employee)
                        {
                            hours += timeTrackingsInPeriod[i].Hours;
                            timeTrackingsInPeriod.RemoveAt(i);
                            if (timeTrackingsInPeriod.Count == 1)
                            {
                                hours += timeTrackingsInPeriod[i].Hours;
                                var estimate = CalculateTheEstimate(hours, employeeToCalculate.MonthRate);
                                Console.WriteLine("{0} отработал {1} часов и заработал за период {2} руб.",
                                          employeeToCalculate.Name,
                                          hours,
                                          estimate);
                                break;
                            }
                        }
                        else
                        {
                            i++;
                        }
                        
                        if (i + 1 == timeTrackingsInPeriod.Count)
                        {
                            var estimate = CalculateTheEstimate(hours, employeeToCalculate.MonthRate);
                            Console.WriteLine("{0} отработал {1} часов и заработал за период {2} руб.",
                                      employeeToCalculate.Name,
                                      hours,
                                      estimate);

                            employeeToCalculate = null;
                            hours = 0d;
                            i = 0;
                        }
                    }
                }
            }
        }

        private static decimal CalculateTheEstimate(double hours, decimal monthRate)
        {
            return (decimal)hours / 160m * monthRate;
        }
    }
}
