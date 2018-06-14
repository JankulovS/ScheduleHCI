public class VisualHelper

{

    //EnableRowsMoveProperty is used to enable rows moving by mouse drag and move in data grid

    //the only requirement is to ItemsSource collection of datagrid be a ObservableCollection or at least IList collection

    public static readonly DependencyProperty EnableRowsMoveProperty =

        DependencyProperty.RegisterAttached("EnableRowsMove", typeof(bool), typeof(VisualHelper), new PropertyMetadata(false, EnableRowsMoveChanged));



    public static bool GetEnableRowsMove(DataGrid obj)

    {

        return (bool)obj.GetValue(EnableRowsMoveProperty);

    }



    public static void SetEnableRowsMove(DataGrid obj, bool value)

    {

        obj.SetValue(EnableRowsMoveProperty, value);

    }

}