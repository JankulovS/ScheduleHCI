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
        
    }
}
