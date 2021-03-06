using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.Data.SqlServerCe;
using System.IO;
using Microsoft.Maps.MapControl.WPF;
using System.Globalization;

namespace NatureNetApplication
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    /// 

  

    public partial class SurfaceWindow1 : SurfaceWindow
    {
        
        int _loginnamecounter;
        int count = 0,check=0;
        string dir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            Location thecenter = new Location(38.771317, -76.711307);
            
            InitializeComponent();
            Default_menu.DataContext = this.Default_menu;
            AddWindowAvailabilityHandlers();
            mymap.UseInertia = true;
            mymap.ViewChangeOnFrame += new EventHandler<MapEventArgs>(mymap_ViewChangeOnFrame);

            mymap.AnimationLevel = AnimationLevel.Full;
            //MapLayer layer = new MapLayer();
            ////SurfaceDragDrop.AddDropHandler(this, new EventHandler<SurfaceDragDropEventArgs>(Drop_Event));
            //mymap.Center=thecenter;
            //mymap.ZoomLevel = 5;

            //layer.AddChild(new PushPinControl(), new Location(38.771317, -76.711307), PositionOrigin.BottomCenter);
            //mymap.Children.Add(layer);
            load_pushpins();
            int Usercount;
            DirectoryInfo info = new DirectoryInfo(dir);
            FileSystemInfo[] all = info.GetFileSystemInfos("*", SearchOption.AllDirectories);
            
            SqlCeConnection conn = null;
            
            string filesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NatureNetDataBase_Main.sdf");
            string connectionString = string.Format("Data Source="+filesPath);
            conn = new SqlCeConnection(connectionString);
            SqlCeCommand cmd = conn.CreateCommand();
           
            /*
            * GET Numer of current users in the database ( COUNT )
            */
            conn.Open();
            
            cmd.CommandText = "SELECT COUNT(*) AS NumberOfOrders FROM Users_login";
            object value = cmd.ExecuteScalar();

            conn.Close();
            //if (value == DBNull.Value)
            //{
            //    Errorbox direct = new Errorbox("ErrroooooR!!!!!", "bla", "bla2");
            //    Myscatterview.Items.Add(direct);
            //}
            //else
            //{
            //    _loginnamecounter= Convert.ToInt32(value);
            //    string teststring = Convert.ToString(value);
            //}


            /*
          * Load Numer of current users in the database ( COUNT )
          */
            if (value == DBNull.Value)
            {
                Usercount = 0;
            }
            else
            {
                _loginnamecounter = Convert.ToInt32(value);
                Usercount = Convert.ToInt32(value.ToString());
            }

            //conn.Open();
            //foreach (FileSystemInfo f in all)
            //{
                
            //    if (f.Attributes == FileAttributes.Directory)
            //    {
            //        //string _paren_name = new FileInfo(f.FullName).Directory.Name;
            //        cmd.CommandText = "SELECT Username FROM Users_login WHERE (Username = N'"+f.Name+"')";
            //        object _filename = _filename = cmd.ExecuteScalar(); ;
            //        conn.Close();
                    
            //        //if (f.Name == _filename.ToString())
            //        //{

            //        //}
            //        //else
            //        //{
            //            conn.Open();
            //            // Console.WriteLine(f.Name);
            //            // Console.WriteLine(f.Name);
            //            cmd.CommandText = "INSERT INTO User_login (Username, User_id) VALUES ('" + f.Name + "', '" + Usercount + "')";
            //            cmd.ExecuteNonQuery();
            //            conn.Close();
            //            // SqlCeCommand cmd3 = conn.CreateCommand();
            //       // }
            //        // SqlCeCommand cmd3 = conn.CreateCommand();
            //        conn.Open();
            //        cmd.CommandText = "SELECT Image_name FROM Image_Database WHERE (Image_name = N'" + f.Name + "')";
            //        object duplicatechecker = cmd.ExecuteScalar();
            //        conn.Close();
            //        string teststring = Convert.ToString(duplicatechecker);
            //        if (f.Name == teststring)
            //        {

            //        }
            //        else
            //        {
            //            conn.Open();
            //            cmd.CommandText = "INSERT INTO Image_Database (Image_name, Image_location) VALUES ('" + f.Name + "', '" + f.FullName + "')";
            //            object value3 = cmd.ExecuteScalar();
            //            conn.Close();
            //        }

            //    }

            //} 
            /*
            * Add Username tags from Database into tagbox
            */
            for (int i = 1; i <= _loginnamecounter; i++)
            {
                //SqlCeConnection conn2 = null;
                
                //string filesPath2 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NatureNetDataBase_Main.sdf");
                //string connectionString2 = string.Format("Data Source=" + filesPath);
                //conn2 = new SqlCeConnection(connectionString2);
                //conn2.Open();
                //SqlCeCommand cmd2 = conn2.CreateCommand();
                conn.Open();
                cmd.CommandText = "SELECT Username FROM Users_login WHERE (User_id ="+ i+")";
                object value2 = cmd.ExecuteScalar();

                conn.Close();
                if (value2 == DBNull.Value)
                {
                    Errorbox direct = new Errorbox("ErrroooooR!!!!!", "bla", "bla2");
                    Myscatterview.Items.Add(direct);
                }
                else
                {
                   // _loginnamecounter = Convert.ToInt32(value);
                    string teststring = Convert.ToString(value2);
                    Tagloadbox.Items.Add(teststring);
                }

            }
            /*
             * GET DIRECTORY INFO AND LOAD IMAGES LOCATION AND IMAGE NAME INTO IMAGE_DATABASE TABLE
             */
           
            //SqlCeCommand conn3 = null;
            //string filesPath3 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NatureNetDataBase_Main.sdf");
            //string connectionString3 = string.Format("Data Source=H:\\Users\\killer_domon\\Documents\\Visual Studio 2010\\Projects\\NatureNetApplication\\NatureNetApplication\\NatureNetDataBase_Main.sdf");
            //conn3 = new SqlCeCommand(connectionString3);
            conn.Open();
            object _filename_database = new object();
            object _Directory_parent = new object();
            foreach (FileSystemInfo f in all)
            {

               
                if (f.Attributes == FileAttributes.Archive)
                {
                    string _paren_name = new FileInfo(f.FullName).Directory.Name;
                    cmd.CommandText = "SELECT  Image_tag_name FROM Image_Map_to_Tags WHERE ( Image_tag_name = N'" + f.Name + "')";
                     _filename_database = cmd.ExecuteScalar();
                     if (_filename_database == null)
                     {
                         _filename_database = "ads"; 
                     }
                    cmd.CommandText = "SELECT  image_tag FROM Image_Map_to_Tags WHERE ( Image_tag_name = N'" + f.Name + "')";
                     _Directory_parent = cmd.ExecuteScalar();
                     if (_Directory_parent == null)
                     {
                         _Directory_parent = "ads";
                     }
                    if ((f.Name == _filename_database.ToString()) && (_paren_name == _Directory_parent.ToString()))
                    {
                        
                    }
                    else
                    {
                        // Console.WriteLine(f.Name);
                        // Console.WriteLine(f.Name);
                        cmd.CommandText = "INSERT INTO Image_Map_to_Tags (Image_tag_name, image_tag) VALUES ('" + f.Name + "', '" + _paren_name + "')";
                        cmd.ExecuteNonQuery();
                        
                        // SqlCeCommand cmd3 = conn.CreateCommand();
                    }
                   // SqlCeCommand cmd3 = conn.CreateCommand();
                    cmd.CommandText = "SELECT Image_name FROM Image_Database WHERE (Image_name = N'"+f.Name+"')";
                    object duplicatechecker =cmd.ExecuteScalar();
                    cmd.CommandText = "SELECT  Image_location FROM Image_Database WHERE (Image_name = N'" + f.Name + "')";
                    object duplicatelocationchecker = cmd.ExecuteScalar();
                    string teststring = Convert.ToString(duplicatechecker);
                    if (f.Name == teststring)
                    {
                        if (f.FullName == duplicatelocationchecker.ToString())
                        {
 
                        }
                        else
                        {
                            cmd.CommandText = "DELETE FROM Image_Database WHERE (Image_name= N'"+f.Name+"' AND Image_location= N'"+f.FullName+"')";
                            object oga=cmd.ExecuteNonQuery();
                            cmd.CommandText = "INSERT INTO Image_Database (Image_name, Image_location) VALUES ('" + f.Name + "', '" + f.FullName + "')";
                            object value3 = cmd.ExecuteScalar();
                        }

                    }
                    else
                    {

                        cmd.CommandText = "INSERT INTO Image_Database (Image_name, Image_location) VALUES ('" + f.Name + "', '" + f.FullName + "')";
                        object value3 = cmd.ExecuteScalar();
                    }
                    
                }

            } conn.Close();

            //Image_View_Window tester = new Image_View_Window();
            //ScatterViewItem tester2 = new ScatterViewItem();
            //tester2.Content= (tester);
            //tester2.Height = 550;
            //tester2.Width = 652;
            //tester2.CanScale = false;

            //Myscatterview.Items.Add(tester2);
            // Add handlers for window availability events
            
            
        }
      //  internal static NatureNetApplication.SurfaceWindow1 tW1= ;
        private void mymap_ViewChangeOnFrame(object sender, MapEventArgs e)
        {
            Map map = sender as Map;
            if (map != null)
            {
                Location mapCenter = map.Center;
                //txtLatitude.Text = string.Format(CultureInfo.InvariantCulture, "{0:F5}", mapCenter.Latitude);
                //txtLongitude.Text = string.Format(CultureInfo.InvariantCulture, "{0:F5}", mapCenter.Longitude);
            }
        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

        //private void Tagloadbox_Selected(object sender, SelectionChangedEventArgs args)
        //{
        //    object data;
        //    ListBoxItem item = ((sender as ListBox).SelectedItem as ListBoxItem);
        //    item.IsSelected = true;
        //    data = item.Content;

        //}

        private void Tagloadbox_Selected(object sender, RoutedEventArgs e)
        {
           
            string item = ((sender as ListBox).SelectedItem.ToString() );
           // item.IsSelected = true;
            //data = item.Content;
            Image_View_Window contentset = new Image_View_Window(item);
            ScatterViewItem tester2 = new ScatterViewItem();
            tester2.Content = (contentset);
            tester2.Height = 550;
            tester2.Width = 652;
            tester2.CanScale = false;
            tester2.DataContext = contentset;
            Myscatterview.Items.Add(tester2);
        }
        private void mymap_TouchDown(object sender, TouchEventArgs e)
        {
            
            if (check == 1)
            {
                MapLayer layer2 = new MapLayer();
                e.Handled = true;

                // Determin the location to place the pushpin at on the map.

                //Get the mouse click coordinates
                TouchPoint touchp = e.GetTouchPoint(this);
                //Convert the mouse coordinates to a locatoin on the map
                Location pinLocation = mymap.ViewportPointToLocation(touchp.Position);

                PushPinControl asdfgh = new PushPinControl(pinLocation);


                layer2.AddChild(asdfgh, pinLocation, PositionOrigin.Center);
                // The pushpin to add to the map.
                // Pushpin pin = new Pushpin();
                // pin.Location = pinLocation;
                //mymap.Children.
                // Adds the pushpin to the map.
                // mymap.Children.Add(pin);
               // asdfgh.pinno.Content = count;
                mymap.Children.Add(layer2);
                count++;
                check = 0;
                Drop_controler.Tag = "isdisabled";

            }
        }

        private void surfaceButton2_Click(object sender, RoutedEventArgs e)
        {
            if ((string)Drop_controler.Tag == "isdisabled")
            {
                check = 1;
                Drop_controler.Tag = "isenabled";
               // Color redColor = Color.FromArgb(255, 0, 0);
                Drop_controler.Background = Brushes.Red;
            }
            else
                if ((string)Drop_controler.Tag == "isenabled")
                {
                    check = 0;
                    Drop_controler.Tag = "isdisabled";
                    Drop_controler.Background = Brushes.Gray;
                }

        }
        public void load_pushpins()
        {

            SqlCeConnection conn = null;

            string filesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NatureNetDataBase_Main.sdf");
            string connectionString = string.Format("Data Source=" + filesPath);
            conn = new SqlCeConnection(connectionString);
            SqlCeCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) AS NumberOfOrders FROM PushPin_location";
            conn.Open();
            object value4 = cmd.ExecuteScalar();
            
            for (int i = 1; i <= Convert.ToInt32( value4); i++)
            {
                double x_position;
                double y_position;
                cmd.CommandText = "SELECT x_position FROM PushPin_location WHERE (pin_tag = N'"+i+"')";
                
                
                x_position = Convert.ToDouble( cmd.ExecuteScalar());
                cmd.CommandText = "SELECT y_position FROM PushPin_location WHERE (pin_tag = N'" + i + "')";
                y_position = Convert.ToDouble(cmd.ExecuteScalar());
                MapLayer layer2 = new MapLayer();
               
                
                Location pinLocation = new Location(x_position,y_position);

                PushPinControl asdfgh = new PushPinControl(pinLocation);


                layer2.AddChild(asdfgh, pinLocation, PositionOrigin.Center);
                // The pushpin to add to the map.
                // Pushpin pin = new Pushpin();
                // pin.Location = pinLocation;
                //mymap.Children.
                // Adds the pushpin to the map.
                // mymap.Children.Add(pin);
                // asdfgh.pinno.Content = count;
                mymap.Children.Add(layer2);

                
            }
            conn.Close();
        }
        private void surfaceButton1_Click(object sender, RoutedEventArgs e)
        {
           // Point box_center;
           // box_center = Default_menu.Center;
           //// Tag_Add_Box adder = new Tag_Add_Box();
           // ScatterViewItem tester2 = new ScatterViewItem();
           // tester2.Style = (Style)Resources["LibraryContainerInScatterViewItemStyle"];
           // tester2.Content = (adder);
           // tester2.Height = 420;
           // tester2.Width = 300;
           // tester2.CanScale = false;
           // tester2.Center= new Point(box_center.X+200, box_center.Y+200);
           // Myscatterview.Items.Add(tester2);
            Tagloadbox.Items.Add(New_tag_name.Text.ToString());
            string Template = New_tag_name.Text.ToString();
            New_tag_name.Text = "Tag created";
            SqlCeConnection conn = null;

            string filesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NatureNetDataBase_Main.sdf");
            string connectionString = string.Format("Data Source=" + filesPath);
            conn = new SqlCeConnection(connectionString);
            SqlCeCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandText = "INSERT INTO Users_login (Username, User_id) VALUES ('" + Template + "', '" + (++_loginnamecounter) + "')";

            cmd.ExecuteNonQuery();
            conn.Close();
            
        }

        private void togglemap_Click(object sender, RoutedEventArgs e)
        {
            if (mymap.Mode is AerialMode)
            {
                RoadMode r = new RoadMode();
                mymap.Mode = r;


            }
            else
            {
                AerialMode a = new AerialMode(true);
                mymap.Mode = a;
            }

        }

        private void surfaceButton2_Click_1(object sender, RoutedEventArgs e)
        {
            string[] tagInfo = test.Tag.ToString().Split(',');
           // var values = (object[])sender;
          //  if ((test.Tag).ToString() == "off")
            if (tagInfo[0] == "off")
            {
                test.Height = 70;
                test.Width = 70;
                test.Tag = "ok,testing";
                test.Center = new Point(35,35);
                minbutton.Margin=new Thickness(0,0,0,0);

            }
            else if (tagInfo[0] == "ok")
            {
            test.Height = 800;
            test.Width = 800;
            test.Tag = "off,testing";
            test.Center = new Point(390, 390);
                minbutton.Margin = new Thickness(706, 708, 0, 0);
            }
        }

        
    }
}