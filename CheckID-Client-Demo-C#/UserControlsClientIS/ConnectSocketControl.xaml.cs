﻿using ClientInspectionSystem.LoadData;
using log4net;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClientInspectionSystem.UserControlsClientIS {
    /// <summary>
    /// Interaction logic for ConnectSocketControl.xaml
    /// </summary>
    public partial class ConnectSocketControl : UserControl {
        private IniFile iniFile = new IniFile("Data\\clientIS.ini");
        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region CONSTRUCTOR
        public ConnectSocketControl() {
            InitializeComponent();

            this.Dispatcher.Invoke(() => {
                //txtIP.Text = ClientExtentions.GetLocalIPAddress();
                txtIP.Text = "127.0.0.1";
                if (txtIP.Text.Equals(string.Empty) || txtPort.Text.Equals(string.Empty)) {
                    if (btnOkConnect != null) {
                        btnOkConnect.IsEnabled = false;
                    }
                }
            });
        }
        #endregion

        #region HANDLE BUTTON CONNECT SOCKET
        private void btnOkConnect_Click(object sender, RoutedEventArgs e) {
            Window parentWindow;
            MainWindow mainWindow = null;
            try {
                parentWindow = Window.GetWindow(this);
                mainWindow = (MainWindow)parentWindow.FindName("mainWindow");
                if (mainWindow.isWSS) {
                    needForConnectSocket(mainWindow);
                }
                else {
                    needForConnectSocket(mainWindow);
                }
                this.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex) {
                logger.Error(ex);
                this.Visibility = Visibility.Collapsed;
            }
        }

        private void needForConnectSocket(MainWindow mainWindow) {
            mainWindow.connectionSocket = new SocketClient.Connection(txtIP.Text, int.Parse(txtPort.Text),
                                                                      mainWindow.isWSS, mainWindow.pluginClientDelegate);

            mainWindow.connectionSocket.connect();

            //Find Connect
            //mainWindow.connectionSocket.connectSocketServer(mainWindow);



            mainWindow.Dispatcher.Invoke(async () => {
                try {
                    //Task.Delay(InspectionSystemContanst.DIALOG_TIME_OUT_1k);
                    //Update 2022.02.28 TIME OUT INI FILE
                    await Task.Factory.StartNew(() => {
                        try {
                            PluginICAOClientSDK.Response.DeviceDetails.DeviceDetailsResp deviceDetailsResp = mainWindow.connectionSocket.getDeviceDetails(true, true,
                                                                                                                                                          mainWindow.timeoutInterval);

                            mainWindow.Dispatcher.InvokeAsync(() => {
                                if (null != deviceDetailsResp) {
                                    LoadDataForDataGrid.loadDataDetailsDeviceNotConnect(mainWindow.dataGridDetails, deviceDetailsResp.data.deviceSN,
                                                    deviceDetailsResp.data.deviceName, deviceDetailsResp.data.lastScanTime,
                                                    deviceDetailsResp.data.totalPreceeded.ToString());
                                }
                            });
                            logger.Warn("[GET DEVICE DETAILS]");
                        }
                        catch (Exception ex) {
                            mainWindow.Dispatcher.Invoke(() => {
                                LoadDataForDataGrid.loadDataDetailsDeviceNotConnect(mainWindow.dataGridDetails, string.Empty,
                                                                                    string.Empty, string.Empty, string.Empty);
                            });
                            return;
                        }
                    });
                }
                catch (Exception eDeviceDetails) {
                    logger.Error(eDeviceDetails);
                    LoadDataForDataGrid.loadDataDetailsDeviceNotConnect(mainWindow.dataGridDetails, string.Empty,
                                                                         string.Empty, string.Empty, string.Empty);
                }
            });

            //if (!mainWindow.isConnectDenied) {

            //}
            //else {
            //    logger.Debug("=> XXXX");
            //}
        }
        #endregion

        #region HANDLE TEXT BOX IP
        private void txtIP_TextChanged(object sender, TextChangedEventArgs e) {
            if (txtIP.Text.Equals(string.Empty)) {
                if (btnOkConnect != null) {
                    btnOkConnect.IsEnabled = false;
                }
            }
            else {
                if (btnOkConnect != null) {
                    btnOkConnect.IsEnabled = true;
                }
            }
        }

        private void txtIP_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            //Regex regex = new Regex("[^0-9]+");
            //e.Handled = regex.IsMatch(e.Text);
        }
        #endregion

        #region HANDLE TEXT BOX PORT
        private void txtPort_TextChanged(object sender, TextChangedEventArgs e) {
            if (txtPort.Text.Equals(string.Empty)) {
                if (btnOkConnect != null) {
                    btnOkConnect.IsEnabled = false;
                }
            }
            else {
                if (btnOkConnect != null) {
                    btnOkConnect.IsEnabled = true;
                }
            }
        }

        private void txtPort_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion

        #region HADNLE BUTTON CANCEL CONNECT SOCKET
        //CANCEL CONNECT
        private void btnCancelConnect_Click(object sender, RoutedEventArgs e) {
            this.Visibility = Visibility.Collapsed;
            Window parentWindow = Window.GetWindow(this);
            MainWindow mainWindow = (MainWindow)parentWindow.FindName("mainWindow");
            mainWindow.btnConnect.IsEnabled = true;
        }
        #endregion

        #region HADNLE ENTER KEY
        private void txtIP_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                btnOkConnect_Click(sender, e);
            }
        }

        private void txtPort_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                btnOkConnect_Click(sender, e);
            }
        }
        #endregion
    }
}
