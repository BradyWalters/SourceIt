using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using SourceIt._classes;
using System.Timers;

namespace SourceIt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private static System.Timers.Timer timer;

        public MainWindow()
        {
            InitializeComponent();
            string[] args = Environment.GetCommandLineArgs();

            //if (args.Length > 0)
            // {
            //     File.Text = args[0];
            // }
        }

        private void Add_Source(string fileName)
        {
            string sql_Add = "INSERT INTO Files ([date_added],[name]) VALUES(SYSDATETIME(),'" + fileName + "')";

            localDB.Execute_SQL(sql_Add);

            DataTable search = searchDB(fileName);

            if (search.Rows.Count == 0)
            {
                Console.Write("File was not added");
                return;
            }


            int id = Convert.ToInt32(search.Rows[0]["Id"].ToString());
            string source = Source.Text;
            string sql_source = "INSERT INTO Sources ([file_id],[source]) VALUES(" + id + ",'" + source + "')";
            localDB.Execute_SQL(sql_source);

            Output.Text = "Source was successfully added!";
        }

        private DataTable searchDB(string fileName)
        {
            string sql_Search = "SELECT TOP 1 * FROM Files WHERE [name] like '" + fileName + "'";

            return localDB.GetDataTable(sql_Search);
        }

        private string find_source(int id)
        {
            string sql_Search = "SELECT TOP 1 * FROM Sources WHERE [file_id] like " + id;
            DataTable results = localDB.GetDataTable(sql_Search);

            if (results.Rows.Count > 0) return results.Rows[0]["source"].ToString();
            else return "";
        }

        /*
         * The Source It button, which either adds the source or finds the source of the file
         */
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Output.Text = "";
            string fileName = File.Text;

            DataTable search = searchDB(fileName);

            if (search.Rows.Count == 0 && Source.Text != "")
            {
                Add_Source(fileName);
            }
            else if(search.Rows.Count > 0)
            {
                Source.Text = find_source(Convert.ToInt32(search.Rows[0]["Id"].ToString()));
                if (Source.Text == "") Output.Text = "No Source Found :(";
                else Output.Text = "Source Found!";
            }
            else
            {
                Output.Text = "No Source Found :(";
            }
        }

        /*
         * The Delete Current Source Button  
         */
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Output.Text = "";
            DataTable results = searchDB(File.Text);
            int id = Convert.ToInt32(results.Rows[0]["Id"].ToString());
            string sql_delete = "DELETE FROM Files WHERE [name] like '" + File.Text + "'";
            localDB.Execute_SQL(sql_delete);
            sql_delete = "DELETE FROM Sources WHERE [file_id] like " + id;
            localDB.Execute_SQL(sql_delete);
            Output.Text = "Source successfully deleted.";
            Source.Text = "";
        }

        /*
         * The Browse Button
         */ 
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Output.Text = "";
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string file = dlg.FileName;
                File.Text = file;
                Source.Text = "";
            }
        }
    }
}