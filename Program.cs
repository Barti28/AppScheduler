using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using Microsoft.Win32.TaskScheduler;//For creating task scheduler, we are using TaskScheduler.dll for installing in Package Manager:Install-Package TaskScheduler -Version 2.8.15
namespace AppScheduler
{
    class Program
    { 
        static void Main(string[] args)
        {
           XmlReader reader = XmlReader.Create("C:\\Users\\User\\source\\repos\\AppScheduler\\Tasks.xml");
            string repeat= reader.GetAttribute("repeat");
            string time = reader.GetAttribute("time");
            string day = reader.GetAttribute("day");
            string task = reader.GetAttribute("task");

            while (reader.Read())//while loop for true and false
            {
                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "Task"))
                {
                    if (reader.HasAttributes)
                    { switch (repeat)
                        {
                            case "SH"://Task Scheduler that run only one time

                                using (TaskService ts = new TaskService())

                                {
                                    
                                    TaskDefinition td = ts.NewTask();

                                    td.RegistrationInfo.Description = "My first task scheduler";

                                    td.Triggers.Add(new TimeTrigger() { StartBoundary = Convert.ToDateTime(time) });
                                    td.Actions.Add(new ExecAction(@"C:/sample.exe", null, null));

                                    ts.RootFolder.RegisterTaskDefinition(task, td);

                                }
                                break;

                            case "H"://Task Scheduler that run repeate in every hour we have set in tasks
                                int hours;
                                int.TryParse(time, out hours);
                                using (TaskService ts = new TaskService())

                                {

                                    TaskDefinition td = ts.NewTask();

                                    td.RegistrationInfo.Description = "My first task scheduler";



                                    TimeTrigger trigger = new TimeTrigger();

                                    trigger.StartBoundary = DateTime.Now;

                                    trigger.Repetition.Interval = TimeSpan.FromMinutes(hours);

                                    td.Triggers.Add(trigger);



                                    td.Actions.Add(new ExecAction(@"C:/sample.exe", null, null));

                                    ts.RootFolder.RegisterTaskDefinition(task, td);

                                }

                                break;

                            case "D":// Task Scheduler that run daily


                                using (TaskService ts = new TaskService())

                                    {

                                        TaskDefinition td = ts.NewTask();

                                        td.RegistrationInfo.Description = "My first task scheduler";



                                        DailyTrigger daily = new DailyTrigger();

                                        daily.StartBoundary = Convert.ToDateTime(DateTime.Today.ToShortDateString() + time);

                                        daily.DaysInterval = 1;

                                        td.Triggers.Add(daily);



                                        td.Actions.Add(new ExecAction(@"C:/sample.exe", null, null));

                                        ts.RootFolder.RegisterTaskDefinition(task, td);

                                    }

                                

                                break;

                            case "W"://Task Scheduler that run weekly on a specified day Monday and Tuesday
                                DayOfWeek day1 = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), day);
                                using (TaskService ts = new TaskService())

                                {

                                    TaskDefinition td = ts.NewTask();

                                    td.RegistrationInfo.Description = "My first task scheduler";



                                    WeeklyTrigger week = new WeeklyTrigger();

                                    week.StartBoundary = Convert.ToDateTime(DateTime.Today.ToShortDateString() + time);

                                    week.WeeksInterval = 1;

                                    week.DaysOfWeek = Microsoft.Win32.TaskScheduler.DaysOfTheWeek.Monday |
                               Microsoft.Win32.TaskScheduler.DaysOfTheWeek.Tuesday;

                                    td.Triggers.Add(week);



                                    td.Actions.Add(new ExecAction(@"C:/sample.exe", null, null));

                                    ts.RootFolder.RegisterTaskDefinition(task, td);

                                }
                                break;
                        }
                      
                    }
                }
            }
        }


    }
}
