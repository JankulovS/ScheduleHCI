using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Schedule.Commands
{
    public static class RoutedCommands
    {
        public static readonly RoutedUICommand SearchCommand = new RoutedUICommand(
            "Search Command",
            "SearchCommand",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Enter)
            }
            );

        public static readonly RoutedUICommand EndSearch = new RoutedUICommand(
            "End Search",
            "EndSearch",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Escape)
            }
            );

        public static readonly RoutedUICommand Search = new RoutedUICommand(
            "Search",
            "Search",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.S, ModifierKeys.Alt)
            }
            );

        public static readonly RoutedUICommand Filter = new RoutedUICommand(
            "Filter",
            "Filter",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F, ModifierKeys.Alt)
            }
            );

        // save schedule
        public static readonly RoutedUICommand Save = new RoutedUICommand(
            "Save",
            "Save",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.S, ModifierKeys.Control)
            }
            );

        // load schedule
        public static readonly RoutedUICommand Load = new RoutedUICommand(
            "Load",
            "Load",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.L, ModifierKeys.Control)
            }
            );  
        
        // new schedule
        public static readonly RoutedUICommand New = new RoutedUICommand(
            "New",
            "New",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.N, ModifierKeys.Control)
            }
            );

        public static readonly RoutedUICommand AddClassroom = new RoutedUICommand(
            "Add Classroom",
            "AddClassroom",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.R, ModifierKeys.Alt)
            }
            );

        public static readonly RoutedUICommand AddSubject = new RoutedUICommand(
            "Add Subject",
            "AddSubject",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.B, ModifierKeys.Alt)
            }
            );

        public static readonly RoutedUICommand AddCourse = new RoutedUICommand(
            "Add Course",
            "AddCourse",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.U, ModifierKeys.Alt)
            }
            );

        public static readonly RoutedUICommand AddSoftware = new RoutedUICommand(
            "Add Software",
            "AddSoftware",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.W, ModifierKeys.Alt)
            }
            );

        public static readonly RoutedUICommand Help = new RoutedUICommand(
            "Help",
            "Help",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F1)
            }
            );

        public static readonly RoutedUICommand Delete = new RoutedUICommand(
            "Delete",
            "Delete",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Delete)
            }
            );
    }
}
