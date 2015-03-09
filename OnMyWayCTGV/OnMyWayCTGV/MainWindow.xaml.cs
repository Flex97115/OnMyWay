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
using System.Data.SqlClient;

namespace OnMyWayCTGV
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public onmywaydbCTGVEntities1 _onMyWayDb = new onmywaydbCTGVEntities1();
        public bool isAddMode = false;
        public bool isDelMode = false;
        public bool isMoveMode = false;
        public bool isDown = false;
        public bool canMove = false;
        public bool canSelectDishe = false;
        public bool canRun = false;
        public bool isInAdminPanel = false;
        public Button tableMoveButton = new Button();
        public int tableButtonId = 0;
        public int ClientId = 0;

        
        public MainWindow()
        {
            InitializeComponent();
                  
        }
        /*-------------------------------------- Function -------------------------------*/
        public void refreshclientComboBox(ComboBox box)
        {
            box.Items.Clear();
            List<Client> listClient = _onMyWayDb.Client.ToList();
            foreach (var objClient in listClient)
            {
                box.Items.Add(objClient.FirstName + " " + objClient.LastName);

            }
        }

        public void refreshDisheComboBox(ComboBox box)
        {
            box.Items.Clear();
            List<Dishe> listDishe = _onMyWayDb.Dishe.ToList();
            foreach (var objDishe in listDishe)
            {
                box.Items.Add(objDishe.disheName);

            }
        }

        public void setTable()
        {
            tableGrid.Children.Clear();
            List<Table> listTable = _onMyWayDb.Table.ToList();
            foreach (var objTable in listTable)
            {
                Button tableButton = new Button();
                tableButton.Visibility = Visibility.Visible;
                tableButton.Height = 100;
                tableButton.Width = 100;
                tableButton.BorderThickness = new Thickness(0);
                tableButton.BorderBrush = Brushes.Transparent;
                tableButton.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                tableButton.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                tableButton.Tag = objTable.Id;
                tableButton.Margin = new Thickness(objTable.x, objTable.y, 0, 0);
                if (objTable.status == 1)
                {
                    ImageBrush myBrush = new ImageBrush();
                    Image myImage = new Image();
                    myImage.Source = new BitmapImage(new Uri("pack://application:,,,/table_pleine.png"));
                    myBrush.ImageSource = myImage.Source;
                    tableButton.Background = myBrush;
                }
                else if (objTable.status == 0)
                {
                    ImageBrush myBrush = new ImageBrush();
                    Image myImage = new Image();
                    myImage.Source = new BitmapImage(new Uri("pack://application:,,,/table_100_100.png"));
                    myBrush.ImageSource = myImage.Source;
                    tableButton.Background = myBrush;
                }
                tableButton.Click += tableButton_Click;
                tableGrid.Children.Add(tableButton);
            }
        }

        public void setClientToChair()
        {
            ComboBoxDishe1.IsEnabled = false;
            ComboBoxDishe2.IsEnabled = false;
            ComboBoxDishe3.IsEnabled = false;
            ComboBoxDishe4.IsEnabled = false;
            Buttonclient1.Tag = "";
            Buttonclient2.Tag = "";
            Buttonclient3.Tag = "";
            Buttonclient4.Tag = "";
            Buttonclient1.Content = "";
            Buttonclient2.Content = "";
            Buttonclient3.Content = "";
            Buttonclient4.Content = "";
            Buttonclient1.IsEnabled = true;
            Buttonclient2.IsEnabled = true;
            Buttonclient3.IsEnabled = true;
            Buttonclient4.IsEnabled = true;

            clientListBox.Items.Clear();
            ComboBoxDishe1.Items.Clear();
            ComboBoxDishe2.Items.Clear();
            ComboBoxDishe3.Items.Clear();
            ComboBoxDishe4.Items.Clear();
            List<Client> listClient = _onMyWayDb.Client.Where(clients => clients.ChairId == null).ToList();
            foreach (var objClient in listClient)
            {
                clientListBox.Items.Add(objClient.FirstName + " " + objClient.LastName);
            }
            List<Dishe> listDishe = _onMyWayDb.Dishe.ToList();
            foreach (var objDishe in listDishe)
            {
                ComboBoxDishe1.Items.Add(objDishe.disheName);
                ComboBoxDishe2.Items.Add(objDishe.disheName);
                ComboBoxDishe3.Items.Add(objDishe.disheName);
                ComboBoxDishe4.Items.Add(objDishe.disheName);
            }

            List<Chair> listChair = _onMyWayDb.Chair.Where(chairs => chairs.tableId == tableButtonId).ToList();
            int i = 0;
            foreach (var objChair in listChair)
            {
                List<Client> sittingClient = _onMyWayDb.Client.Where(clients => clients.ChairId == objChair.Id).ToList();
                if (sittingClient.Count != 0)
                {
                    foreach (var objClient in sittingClient)
                    {
                        if (i == 0)
                        {
                            Buttonclient1.Content = objClient.FirstName + " " + objClient.LastName;
                            Buttonclient1.Tag = objClient.Id;
                            Buttonclient1.IsEnabled = false;
                            ComboBoxDishe1.Tag = objClient.Id;
                            ComboBoxDishe1.IsEnabled = true;
                            canSelectDishe = true;

                        }
                        else if (i == 1)
                        {
                            Buttonclient2.Content = objClient.FirstName + " " + objClient.LastName;
                            Buttonclient2.Tag = objClient.Id;
                            Buttonclient2.IsEnabled = false;
                            ComboBoxDishe2.Tag = objClient.Id;
                            ComboBoxDishe2.IsEnabled = true;
                            canSelectDishe = true;
                        }
                        else if (i == 2)
                        {
                            Buttonclient3.Content = objClient.FirstName + " " + objClient.LastName;
                            Buttonclient3.Tag = objClient.Id;
                            Buttonclient3.IsEnabled = false;
                            ComboBoxDishe3.Tag = objClient.Id;
                            ComboBoxDishe3.IsEnabled = true;
                            canSelectDishe = true;
                        }
                        else if (i == 3)
                        {
                            Buttonclient4.Content = objClient.FirstName + " " + objClient.LastName;
                            Buttonclient4.Tag = objClient.Id;
                            Buttonclient4.IsEnabled = false;
                            ComboBoxDishe4.Tag = objClient.Id;
                            ComboBoxDishe4.IsEnabled = true;
                            canSelectDishe = true;
                        }
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        Buttonclient1.Tag = objChair.Id;
                    }
                    else if (i == 1)
                    {
                        Buttonclient2.Tag = objChair.Id;
                    }
                    else if (i == 2)
                    {
                        Buttonclient3.Tag = objChair.Id;
                    }
                    else if (i == 3)
                    {
                        Buttonclient4.Tag = objChair.Id;
                    }
                }
                i++;
            }
            canRun = true;
        }



        /*-------------------------------------- Admin Panel -------------------------------*/
        private void adminPanel_Click(object sender, RoutedEventArgs e)
        {
            adminGrid.Visibility = Visibility.Visible;
            adminPanelButton.Visibility = Visibility.Hidden;
            MapButton.Visibility = Visibility.Visible;
            isInAdminPanel = true;
        }

        private void editRestaurant_Click(object sender, RoutedEventArgs e)
        {
            restaurantGrid.Visibility = Visibility.Visible;
        }

        private void manageclient_click(object sender, RoutedEventArgs e)
        {
            clientGrid.Visibility = Visibility.Visible;
        }

        private void Statistics_Button_Click(object sender, RoutedEventArgs e)
        {
            double price = 0;
            List<Client> listClient = _onMyWayDb.Client.Where(clients => clients.DisheId != null).ToList();
            if (listClient.Count != 0)
            {
                foreach (var objClient in listClient)
                {
                    int disheId = int.Parse(objClient.DisheId.ToString());
                    List<Dishe> listDish = _onMyWayDb.Dishe.Where(dishes => dishes.Id == disheId).ToList();
                    foreach (var objDishe in listDish)
                    {
                        price += objDishe.Pricer;
                    }
                }
                totalBillLabel.Content = price.ToString("F2")+" €";
            }
            List<Table> listTable = _onMyWayDb.Table.Where(tables => tables.status == 1).ToList();
            numEatingTable.Content = listTable.Count.ToString();
            statisticGrid.Visibility = Visibility.Visible;
        }
        private void BackStatistics_Click(object sender, RoutedEventArgs e)
        {
            statisticGrid.Visibility = Visibility.Hidden;
        }

        private void ManageDishe_Click(object sender, RoutedEventArgs e)
        {
            disheGrid.Visibility = Visibility.Visible;
        }

        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            adminGrid.Visibility = Visibility.Hidden;
            MapButton.Visibility = Visibility.Hidden;
            adminPanelButton.Visibility = Visibility.Visible;
            isInAdminPanel = false;
            setTable();
        }

        /*-------------------------------------- client -------------------------------*/
        private void Back_Manage_client_Click(object sender, RoutedEventArgs e)
        {
            clientGrid.Visibility = Visibility.Hidden;
        }

        /*-------------------------------------- Add client -------------------------------*/
        private void Add_client_Button_Click(object sender, RoutedEventArgs e)
        {
            addclientGrid.Visibility = Visibility.Visible;
        } 
        private void AddclientInDb_Button_Click(object sender, RoutedEventArgs e)
        {
            bool isAllLetterFN = false;
            bool isAllLetterLN = false;
            Client newClient = new Client();
            newClient.FirstName = FNAddclientTextBox.Text;
            newClient.LastName = LNAddclientTextBox.Text;
            isAllLetterFN = newClient.FirstName.All(char.IsLetter);
            isAllLetterLN = newClient.LastName.All(char.IsLetter);
            if (isAllLetterFN && isAllLetterLN)
            {
                _onMyWayDb.Client.Add(newClient);
                _onMyWayDb.SaveChanges();
                LNAddclientTextBox.Text = "";
                FNAddclientTextBox.Text = "";
            }
            
        }
        private void BackAddclient_Click(object sender, RoutedEventArgs e)
        {
            addclientGrid.Visibility = Visibility.Hidden;
        }

        /*-------------------------------------- Delete client -------------------------------*/
        private void Delete_client_Button_Click(object sender, RoutedEventArgs e)
        {
            refreshclientComboBox(DeleteclientComboBox);
            DeleteclientGrid.Visibility = Visibility.Visible;
        }
        private void DeleteclientInDb_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!DeleteclientComboBox.SelectedItem.Equals(null))
            {
                string[] clientArrayName = DeleteclientComboBox.SelectedItem.ToString().Split(' ');
                
                string clientFirstName = clientArrayName[0];
                string clientLastName = clientArrayName[1];
                List<Client> listClient = _onMyWayDb.Client.Where(clients => clients.LastName.Contains(clientLastName)).Where(clients => clients.FirstName.Contains(clientFirstName)).ToList();
                foreach (var objClient in listClient)
                {
                    _onMyWayDb.Client.Remove(objClient);
                }
                _onMyWayDb.SaveChanges();
                refreshclientComboBox(DeleteclientComboBox);
            }
        }

        private void BackDeleteclient_Click(object sender, RoutedEventArgs e)
        {
            DeleteclientGrid.Visibility = Visibility.Hidden;
        }


        /*-------------------------------------- Update client -------------------------------*/
        private void Update_client_Button_Click(object sender, RoutedEventArgs e)
        {
            refreshclientComboBox(UpdateclientComboBox);
            updateclientGrid.Visibility = Visibility.Visible;   
        }
        private void BackUpdateclient_Click(object sender, RoutedEventArgs e)
        {
            updateclientGrid.Visibility = Visibility.Hidden;
        }
        private void UpdateclientInDb_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!UpdateclientComboBox.SelectedItem.Equals(null))
            {
                string[] clientArrayName = UpdateclientComboBox.SelectedItem.ToString().Split(' ');
                string clientFirstName = clientArrayName[0];
                string clientLastName = clientArrayName[1];
                bool isAllLetterFN = false;
                bool isAllLetterLN = false;
                List<Client> listClient = _onMyWayDb.Client.Where(clients => clients.LastName.Contains(clientLastName)).Where(clients => clients.FirstName.Contains(clientFirstName)).ToList();
                foreach (var objClient in listClient)
                {
                    objClient.LastName = LNUpdateclientTextBox.Text;
                    objClient.FirstName = FNUpdateclientTextBox.Text;
                    isAllLetterFN = objClient.FirstName.All(char.IsLetter);
                    isAllLetterLN = objClient.LastName.All(char.IsLetter);
                    if (isAllLetterFN && isAllLetterLN)
                    {
                       _onMyWayDb.Entry(objClient).State = System.Data.Entity.EntityState.Modified;
                       _onMyWayDb.SaveChanges();
                       FNUpdateclientTextBox.Text = "";
                       LNUpdateclientTextBox.Text = "";
                    }
                }
                
                refreshclientComboBox(UpdateclientComboBox);
            }
        }
        private void UpdateclientComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UpdateclientComboBox.SelectedItem != null)
            {
                FNUpdateclientTextBox.Text = "";
                LNUpdateclientTextBox.Text = "";
                string[] clientArrayName = UpdateclientComboBox.SelectedItem.ToString().Split(' ');
                string firstName = clientArrayName[0];
                string lastName = clientArrayName[1];
                FNUpdateclientTextBox.Text = firstName;
                LNUpdateclientTextBox.Text = lastName;
            }
        }

        /*-------------------------------------- Dishe -------------------------------*/

        private void Back_Manage_Dishe_Click(object sender, RoutedEventArgs e)
        {
            disheGrid.Visibility = Visibility.Hidden;
        }
        /*-------------------------------------- Add Dishe -------------------------------*/
        private void Add_Dishe_Button_Click(object sender, RoutedEventArgs e)
        {
            addDisheGrid.Visibility = Visibility.Visible;
        }
        private void AddDisheInDb_Button_Click(object sender, RoutedEventArgs e)
        {
            Dishe newDishe = new Dishe();
            newDishe.disheName = AddDisheNameTextBox.Text;
            float dishePrice = 0;
            bool canParse = false;
            canParse = float.TryParse(AddDishePriceTextBox.Text, out dishePrice);
            if (canParse)
            {
                newDishe.Pricer = dishePrice;
                if (newDishe.disheName != "" && newDishe.Pricer != 0)
                {
                    _onMyWayDb.Dishe.Add(newDishe);
                    _onMyWayDb.SaveChanges();
                    AddDisheNameTextBox.Text = "";
                    AddDishePriceTextBox.Text = "";
                }
            }
        }

        private void BackAddDishe_Click(object sender, RoutedEventArgs e)
        {
            addDisheGrid.Visibility = Visibility.Hidden;
        }
        /*-------------------------------------- delete Dishe -------------------------------*/
        private void Delete_Dishe_Button_Click(object sender, RoutedEventArgs e)
        {
            refreshDisheComboBox(DeleteDisheComboBox);
            DeleteDisheGrid.Visibility = Visibility.Visible;
        }
        private void DeleteDisheInDb_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!DeleteDisheComboBox.SelectedItem.Equals(null))
            {
                string strDisheName = DeleteDisheComboBox.SelectedItem.ToString();
                List<Dishe> listDishe = _onMyWayDb.Dishe.Where(dishes => dishes.disheName.Contains(strDisheName)).ToList();
                foreach (var objDishe in listDishe)
                {
                    _onMyWayDb.Dishe.Remove(objDishe);
                }
                _onMyWayDb.SaveChanges();
                refreshDisheComboBox(DeleteDisheComboBox);
            }
        }

        private void BackDeleteDishe_Click(object sender, RoutedEventArgs e)
        {
            DeleteDisheGrid.Visibility = Visibility.Hidden;
        }
        /*-------------------------------------- Update Dishe -------------------------------*/
        private void Update_Dishe_Button_Click(object sender, RoutedEventArgs e)
        {
            refreshDisheComboBox(UpdateDisheComboBox);
            updateDisheGrid.Visibility = Visibility.Visible;
        }
        private void UpdateDisheInDb_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!UpdateDisheComboBox.SelectedItem.Equals(null))
            {
                string strDisheName = UpdateDisheComboBox.SelectedItem.ToString();
                bool canParse = false;
                float outValue = 0;
                List<Dishe> listDishe = _onMyWayDb.Dishe.Where(dishes => dishes.disheName.Contains(strDisheName)).ToList();
                foreach (var objDishe in listDishe)
                {
                    objDishe.disheName = UpdateDisheNameTextBox.Text;
                    canParse = float.TryParse(UpdateDishePriceTextBox.Text , out outValue);
                    if (canParse)
                    {
                        objDishe.Pricer = outValue;
                        if (objDishe.disheName != "" && objDishe.Pricer != 0)
                        {
                            _onMyWayDb.Entry(objDishe).State = System.Data.Entity.EntityState.Modified;
                            _onMyWayDb.SaveChanges();
                            UpdateDisheNameTextBox.Text = "";
                            UpdateDishePriceTextBox.Text = "";
                        }
                    }
                    refreshDisheComboBox(UpdateDisheComboBox);
                }
                
            }
        }

        private void BackUpdateDishe_Click(object sender, RoutedEventArgs e)
        {
            updateDisheGrid.Visibility = Visibility.Hidden;
        }

        private void UpdateDisheComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (UpdateDisheComboBox.SelectedItem != null)
            {
                UpdateDishePriceTextBox.Text = "";
                UpdateDisheNameTextBox.Text = "";
                string disheName = UpdateDisheComboBox.SelectedItem.ToString();
                List<Dishe> listDishe = _onMyWayDb.Dishe.Where(dishes => dishes.disheName.Contains(disheName)).ToList();
                foreach (var objDishe in listDishe)
                {
                    UpdateDishePriceTextBox.Text = objDishe.Pricer.ToString();
                    UpdateDisheNameTextBox.Text = objDishe.disheName;
                }


            }
        }
        /*-------------------------------------- Restaurant -------------------------------*/
        private void Add_Table_Button_Click(object sender, RoutedEventArgs e)
        {
            isInAdminPanel = false;
            isAddMode = true;
            adminGrid.Visibility = Visibility.Hidden;
            adminPanelButton.Visibility = Visibility.Hidden;
            FinishButton.Visibility = Visibility.Visible;
        }

        private void Delete_Table_Button_Click(object sender, RoutedEventArgs e)
        {
            isInAdminPanel = false;
            isDelMode = true;
            adminGrid.Visibility = Visibility.Hidden;
            adminPanelButton.Visibility = Visibility.Hidden;
            FinishButton.Visibility = Visibility.Visible;
        }

        private void Move_table_Button_Click(object sender, RoutedEventArgs e)
        {
            isInAdminPanel = false;
            isMoveMode = true;
            adminGrid.Visibility = Visibility.Hidden;
            adminPanelButton.Visibility = Visibility.Hidden;
            FinishButton.Visibility = Visibility.Visible;
        }

        private void Back_Manage_Table_Click(object sender, RoutedEventArgs e)
        {

            restaurantGrid.Visibility = Visibility.Hidden;
        }

        /*-------------------------------------- table Grid -------------------------------*/

        private void tableButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isInAdminPanel)
            {
                Button buttonSender = (Button)sender;
                tableButtonId = int.Parse(buttonSender.Tag.ToString());
                List<Table> notEatingTable = _onMyWayDb.Table.Where(tables => tables.status == 0).Where(tables => tables.Id == tableButtonId).ToList();
                if (isMoveMode)
                {
                    canMove = true;
                    buttonSender.Visibility = Visibility.Hidden;
                }
                else if (isDelMode)
                {
                    List<Table> listTable = _onMyWayDb.Table.Where(tables => tables.Id == tableButtonId).ToList();
                    List<Chair> listChair = _onMyWayDb.Chair.Where(chairs => chairs.tableId == tableButtonId).ToList();
                    foreach (var objTable in listTable)
                    {
                        _onMyWayDb.Table.Remove(objTable);
                        foreach (var objChair in listChair)
                        {
                            _onMyWayDb.Chair.Remove(objChair);
                            List<Client> listClient = _onMyWayDb.Client.Where(clients => clients.ChairId == objChair.Id).ToList();
                            foreach (var objClient in listClient)
                            {
                                objClient.ChairId = null;
                                objClient.DisheId = null;
                            }
                        }
                        _onMyWayDb.SaveChanges();
                        setTable();
                    }
                }
                else if (notEatingTable.Count != 0)
                {
                    setClientToChair();
                    tableStatutGrid.Visibility = Visibility.Visible;
                }
                else
                {
                    orderListBox.Items.Clear();
                    double price = 0;

                    List<Client> listClient = _onMyWayDb.Client.Where(clients => clients.Chair.tableId == tableButtonId).Where(clients => clients.DisheId != null).ToList();
                    if (listClient.Count != 0)
                    {
                        foreach (var objClient in listClient)
                        {
                            int disheId = int.Parse(objClient.DisheId.ToString());
                            List<Dishe> listDish = _onMyWayDb.Dishe.Where(dishes => dishes.Id == disheId).ToList();
                            foreach (var objDishe in listDish)
                            {
                                orderListBox.Items.Add(objDishe.disheName + " ............... " + objDishe.Pricer + " €");
                                price += objDishe.Pricer;
                            }
                        }
                        LabelPrice.Content = price.ToString("F2");
                        tableNumber.Content = tableButtonId.ToString();
                    }
                    eatingTableGrid.Visibility = Visibility.Visible;

                }
            }

        }
        private void tableGridLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDown = true;
        }

        private void tableGridLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDown )
            {
                if (isAddMode)
                {
                    Table newTable = new Table();
                    newTable.x = int.Parse(Mouse.GetPosition(tableGrid).X.ToString());
                    newTable.y = int.Parse(Mouse.GetPosition(tableGrid).Y.ToString());
                    newTable.status = 0;
                    _onMyWayDb.Table.Add(newTable);
                    for (int i = 0; i <= 3; i++)
                    {
                        Chair newChair = new Chair();
                        newChair.tableId = newTable.Id;
                        _onMyWayDb.Chair.Add(newChair);
                    }
                    _onMyWayDb.SaveChanges();
                    setTable();
                    isDown = false;
                }
                if (isMoveMode && canMove)
                {
                    List<Table> listTable = _onMyWayDb.Table.Where(tables => tables.Id == tableButtonId).ToList();
                    foreach (var objTable in listTable)
                    {
                        objTable.x = int.Parse(Mouse.GetPosition(tableGrid).X.ToString());
                        objTable.y = int.Parse(Mouse.GetPosition(tableGrid).Y.ToString());
                        _onMyWayDb.Entry(objTable).State = System.Data.Entity.EntityState.Modified;
                        _onMyWayDb.SaveChanges();
                        tableGrid.Children.Remove(tableMoveButton);
 
                        canMove = false;
                        isDown = false;
                        setTable();
                    }
                    
                }
            }
        }
        private void MouseMoveTableGrid(object sender, MouseEventArgs e)
        {
            if (isMoveMode && canMove)
            {
                tableGrid.Children.Remove(tableMoveButton);
                List<Table> listTable = _onMyWayDb.Table.Where(tables => tables.Id == tableButtonId).ToList();
                foreach (var objTable in listTable)
                {
                        Point position = e.GetPosition(this);   
                        tableMoveButton.Height = 100;
                        tableMoveButton.Width = 100;
                        ImageBrush myBrush = new ImageBrush();
                        Image myImage = new Image();
                        myImage.Source = new BitmapImage(new Uri("pack://application:,,,/table_100_100.png"));
                        myBrush.ImageSource = myImage.Source;
                        tableMoveButton.Background = myBrush;
                        tableMoveButton.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        tableMoveButton.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        tableMoveButton.Margin = new Thickness(position.X-100, position.Y-100, 0, 0);
                        tableGrid.Children.Add(tableMoveButton);              
                }
            }
        }
        private void tableGridInit(object sender, EventArgs e)
        {
            setTable();
        }

        private void finish_Click(object sender, RoutedEventArgs e)
        {
            isInAdminPanel = true;
            isDelMode = false;
            isMoveMode = false;
            isAddMode = false;
            adminGrid.Visibility = Visibility.Visible;
            adminPanelButton.Visibility = Visibility.Visible;
            FinishButton.Visibility = Visibility.Hidden;
            setTable();
               
        }
        /*-------------------------------------- table statut Grid -------------------------------*/
        private void selectclientInclientListBox(object sender, SelectionChangedEventArgs e)
        {
            ListBox listbox = (ListBox)sender;
            if (canRun)
            {
                if (listbox.SelectedItem != null)
                {
                    string[] selectedArrayName = listbox.SelectedItem.ToString().Split(' ');
                    string strFirstName = selectedArrayName[0];
                    string strLastName = selectedArrayName[1];
                    List<Client> listClient = _onMyWayDb.Client.Where(clients => clients.FirstName.Contains(strFirstName)).Where(clients => clients.LastName.Contains(strLastName)).ToList();
                    foreach (var objClient in listClient)
                    {
                        ClientId = objClient.Id;
                    }
                } 
            }
                      
        }

        private void clientButton_Clik(object sender, RoutedEventArgs e)
        {
            Button chairChoose = (Button)sender;
            int valueChairId = int.Parse(chairChoose.Tag.ToString());
            if (ClientId != 0)
            {
                List<Client> listClient = _onMyWayDb.Client.Where(clients => clients.Id == ClientId).ToList();
                foreach (var objClient in listClient)
                {
                    objClient.ChairId = valueChairId;
                    _onMyWayDb.Entry(objClient).State = System.Data.Entity.EntityState.Modified;
                    _onMyWayDb.SaveChanges();
                    setClientToChair();
                    ClientId = 0;
                }
            }
            
        }

        private void SelectDisheInComboBox(object sender, SelectionChangedEventArgs e)
        {
            ComboBox selectedComboBox = (ComboBox)sender;
            int valueClientId = int.Parse(selectedComboBox.Tag.ToString());
            if (selectedComboBox.SelectedItem != null)
            {
                string strDisheName = selectedComboBox.SelectedItem.ToString();
                List<Dishe> listDishe = _onMyWayDb.Dishe.Where(dishes => dishes.disheName.Contains(strDisheName)).ToList();
                foreach (var objDishe in listDishe)
                {
                    int disheId = objDishe.Id;
                    List<Client> listClient = _onMyWayDb.Client.Where(clients => clients.Id == valueClientId).ToList();
                    foreach (var objClient in listClient)
                    {
                        objClient.DisheId = disheId;
                        _onMyWayDb.Entry(objClient).State = System.Data.Entity.EntityState.Modified;
                        _onMyWayDb.SaveChanges();
                    }
                }
                
            }
        }

        private void BackStatutTable_Click(object sender, RoutedEventArgs e)
        {
            List<Table> listTable = _onMyWayDb.Table.Where(tables => tables.Id == tableButtonId).ToList();
            if (listTable.Count != 0)
            {
                List<Client> listClient = _onMyWayDb.Client.Where(clients => clients.Chair.tableId == tableButtonId).Where(clients => clients.DisheId != null).ToList();
                if ( listClient.Count != 0)
                {
                    foreach (var objTable in listTable)
                    {
                        objTable.status = 1;
                        _onMyWayDb.Entry(objTable).State = System.Data.Entity.EntityState.Modified;
                        _onMyWayDb.SaveChanges();
                    }
                }
                
            }
            setTable();
            tableStatutGrid.Visibility = Visibility.Hidden;
        }

        private void BackEatingTable_Click(object sender, RoutedEventArgs e)
        {
            setTable();
            eatingTableGrid.Visibility = Visibility.Hidden;
        }
        
             
    }
}
