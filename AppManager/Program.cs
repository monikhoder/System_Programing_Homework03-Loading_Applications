using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;
namespace AppManager
{
    internal static class Program
    {
        static AppDomain visualizerDomain, drawerDomain;
        static Assembly visualizerAsm, drawerAsm;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            visualizerDomain = AppDomain.CurrentDomain;
            //AppDomain.CreateDomain("Visualizer Domain");
            Debug.WriteLine(visualizerDomain.BaseDirectory);
            visualizerAsm = AssemblyLoadContext.Default.LoadFromAssemblyPath(
            @"C:\Users\Rinmo\source\repos\System programing\Homework 03\Homework03\TextVisualizer\bin\Debug\net8.0-windows\TextVisualizer.dll");

            drawerDomain = AppDomain.CurrentDomain;
            drawerAsm = AssemblyLoadContext.Default.LoadFromAssemblyPath(
                @"C:\Users\Rinmo\source\repos\System programing\Homework 03\Homework03\TextDrawer\bin\Debug\net8.0-windows\TextDrawer.dll");

            var drawerForm = drawerAsm.GetType("TextDrawer.Form1");
            var drawerFormObj = Activator.CreateInstance(drawerForm);
            new Thread(() =>
            {
                var show = drawerForm.GetMethod("ShowDialog", []);
                show.Invoke(drawerFormObj, null);
            }).Start();

            var visualForm = visualizerAsm.GetType("TextVisualizer.Form1");
            var visualFormObj = Activator.CreateInstance(visualForm,
                drawerAsm.GetModule("TextDrawer.dll"), drawerFormObj);
            var showVisual = visualForm.GetMethod("ShowDialog", []);
            showVisual.Invoke(visualFormObj, null);
        }


    }
}