using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace avdfGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        private Player player;
        private GameData sessionData;
        private NetworkModel network;
        //private List<Capability> capabilityList;

        //private GameStartControl gameStart = null;
        private HostLobby hostLobby = null;
        //private HostGameEntryControl hostEntryControl = null;
        private HostGameControl hostGameControl = null;

        private PlayerLobby playerLobby = null;
        private PlayerGameControl playerGameControl = null;

        private Window1 sharedDisplay;

        private List<Narrative> playerNarratives;


        //private ObservableCollection<Game> games = new ObservableCollection<Game>();

        public MainWindow()
        {
            InitializeComponent();

            //SharpCloudInterfaceManager sc = new SharpCloudInterfaceManager();
            //sc.CreateClient();
            //sc.LoadStory();
            //sc.UpdateStory("Major Technology Area");


            //capabilityList = new List<Capability>();

            //gameStart = new GameStartControl();

            // Register the Bubble Event Handler 
            AddHandler(GameStartControl.HostJoinedEvent, new RoutedEventHandler(HostJoinedEventHandler));
            AddHandler(HostAssignVignettePanel.OpenNarrativeWritingEvent, new RoutedEventHandler(HostOpenNarrativeWritingEventHandler));
            AddHandler(GameStartControl.PlayerJoinedEvent, new RoutedEventHandler(PlayerJoinedEventHandler));
            AddHandler(HostGameEntryControl.StartSessionEvent, new RoutedEventHandler(StartSessionEventHandler));
            AddHandler(VignetteXpsControl.ChooseVignetteEvent, new RoutedEventHandler(ChooseVignetteEventHandler));
            AddHandler(HostCapabilityPanel.StartVoteEvent, new RoutedEventHandler(StartVoteEventHandler));
            AddHandler(HostCapabilityPanel.StopVoteEvent, new RoutedEventHandler(StopVoteEventHandler));
            AddHandler(HostCapabilityPanel.ShowVotesEvent, new RoutedEventHandler(ShowVotesEventHandler));
            AddHandler(HostCapabilityPanel.VoteReceivedEvent, new RoutedEventHandler(HostVoteReceivedEventHandler));
            AddHandler(PlayerCapabilityPanel.VoteReceivedEvent, new RoutedEventHandler(PlayerVoteReceivedEventHandler));
            AddHandler(PlayerCapabilityPanel.UnvoteReceivedEvent, new RoutedEventHandler(PlayerUnvoteReceivedEventHandler));

            AddHandler(PlayerGameControl.NarrativeSubmittedEvent, new RoutedEventHandler(NarrativeSubmittedEventHandler));
            AddHandler(PlayerGameControl.CapabilityChosenEvent, new RoutedEventHandler(PlayerCapabilityChosenEventHandler));

            AddHandler(HostGameControl.OpenAssessmentEvent, new RoutedEventHandler(OpenAssessmentEventHandler));
            AddHandler(HostGameControl.StartAssessmentEvent, new RoutedEventHandler(StartAssessmentEventHandler));
            AddHandler(HostGameControl.EndSessionEvent, new RoutedEventHandler(EndSessionEventHandler));
            AddHandler(HostPaletteControl.RequestAssessmentDataEvent, new RoutedEventHandler(RequestAssessmentDataEventHandler));
            AddHandler(HostPaletteControl.ShowAssessmentDataEvent, new RoutedEventHandler(ShowAssessmentDataEventHandler));
            AddHandler(HostPaletteControl.GetRatingsEvent, new RoutedEventHandler(GetRatingsEventHandler));

            AddHandler(PaletteControl.NewPaletteItemEvent, new RoutedEventHandler(NewPaletteItemEventHandler));
            AddHandler(PaletteControl.PaletteItemDragCompletedEvent, new RoutedEventHandler(PaletteItemDragCompletedEventHandler));
            AddHandler(PaletteControl.PaletteItemContentUpdateEvent, new RoutedEventHandler(PaletteItemContentUpdateEventHandler));


            //mainGrid.Children.Add(gameStart);
            //Grid.SetRow(gameStart, 1);
        }

        private void GetRatingsEventHandler(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ShowAssessmentDataEventHandler(object sender, RoutedEventArgs e)
        {
            string user = hostGameControl.hostPaletteControl.GetSelectedPlayerName();
            string capname = "";
            List<PaletteItemData> temp = new List<PaletteItemData>();

            // get name of requested player and pull palette items from gameDate
            foreach (PaletteItemData pid in sessionData.PaletteItemData)
            {
                if (pid.Author == user)
                {
                    temp.Add(pid);
                }
            }

            // send to control
            foreach (Capability cap in sessionData.CapabilityData)
            {
                if (cap.CapabilityId == temp[0].CapabilityIndex)
                {
                    capname = cap.CapabilityName;
                    break;
                }
            }

            sharedDisplay.sharedAssessmentPanel.chosenCapabilityTextBlock.Text = capname;
            sharedDisplay.sharedAssessmentPanel.playerPalette.BuildPaletteFromData(temp);

            sharedDisplay.ToggleView("assessment");
        }

        private void PlayerCapabilityChosenEventHandler(object sender, RoutedEventArgs e)
        {
            // store data
            sessionData.PlayerChosenCapability = playerGameControl.GetCapability();

            // send message to inform host
            network.SendMessage(String.Format("PC|{0}|{1}|", player.UserName, sessionData.PlayerChosenCapability.CapabilityId));

        }

        private void PaletteItemContentUpdateEventHandler(object sender, RoutedEventArgs e)
        {
            PaletteItemData pid = (e.OriginalSource as PaletteControl).NewestItem;

            // update data
            sessionData.PaletteItemData[sessionData.PaletteItemData.IndexOf(pid)].Content = pid.Content;

            string msg;

            if (player.IsHost)
            {
                // if host, send to selected player
                msg = String.Format("IU|1|{0}|{1}|{2}|", pid.Author, pid.ItemId, pid.Content);
                network.SendMessage(msg);
            }
            else
            {
                // if player, send to host - the 1 indicates this is a content update
                msg = String.Format("IU|1|{0}|{1}|{2}|", pid.Author, pid.ItemId, pid.Content);
                network.SendMessage(msg);
            }
        }

        private void PaletteItemDragCompletedEventHandler(object sender, RoutedEventArgs e)
        {
            PaletteItemData pid = (e.OriginalSource as PaletteControl).NewestItem;

            // update data
            sessionData.PaletteItemData[sessionData.PaletteItemData.IndexOf(pid)].Score = pid.Score;
            sessionData.PaletteItemData[sessionData.PaletteItemData.IndexOf(pid)].Position = pid.Position;

            string msg;

            if(player.IsHost)
            {
                // if host, send to selected player
                msg = String.Format("IU|0|{0}|{1}|{2}|{3}|{4}|", pid.Author, pid.ItemId, pid.Position.X, pid.Position.Y, pid.Score);
                network.SendMessage(msg);

            }
            else
            {
                // if player, send to host - the 0 indicates this is a position update
                msg = String.Format("IU|0|{0}|{1}|{2}|{3}|{4}|", pid.Author, pid.ItemId, pid.Position.X, pid.Position.Y, pid.Score);
                network.SendMessage(msg);
            }
        }

        private void NewPaletteItemEventHandler(object sender, RoutedEventArgs e)
        {
            PaletteItemData pid = ((PaletteControl)e.OriginalSource).NewestItem;

            string message = String.Format("PI|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|",                 
                pid.Author,                
                pid.CapabilityIndex,
                pid.Content, 
                pid.ItemId,
                pid.Position.X, 
                pid.Position.Y, 
                pid.Quadrant,
                pid.Score);

            sessionData.PaletteItemData.Add(pid);

            network.SendMessage(message);
        }

        private void SaveData(bool isHost)
        {
            string fname;
            try
            {
                if(isHost)
                    fname = System.Windows.Forms.Application.StartupPath + @"\HostData\host-" + player.UserName + "-session-" + sessionData.SessionName + "-data.json";
                else
                    fname = System.Windows.Forms.Application.StartupPath + @"\PlayerData\player-" + player.UserName + "-session-" + sessionData.SessionName + "-data.json";

                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;

                using (StreamWriter sw = new StreamWriter(fname))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, sessionData);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        //HOST
        private void EndSessionEventHandler(object sender, RoutedEventArgs e)
        {
            // save data
            SaveData(true);

            // tell all players the session is over
            network.SendMessage("SO|");
        }

        private void StartAssessmentEventHandler(object sender, RoutedEventArgs e)
        {
            // send message
            network.SendMessage("SA|");
        }

        private void RequestAssessmentDataEventHandler(object sender, RoutedEventArgs e)
        {
            // get user name
            string user = hostGameControl.hostPaletteControl.GetSelectedPlayerName();
            string capname = "";
            List<PaletteItemData> temp = new List<PaletteItemData>();
                
            // get name of requested player and pull palette items from gameDate
            foreach(PaletteItemData pid in sessionData.PaletteItemData)
            {
                if(pid.Author == user)
                {
                    temp.Add(pid);
                }
            }

            // send to control
            foreach(Capability cap in sessionData.CapabilityData)
            {
                if(cap.CapabilityId == temp[0].CapabilityIndex)
                {
                    capname = cap.CapabilityName;
                    break;
                }
            }
            hostGameControl.hostPaletteControl.SetAssessmentData(temp, capname);
        }

        private void OpenAssessmentEventHandler(object sender, RoutedEventArgs e)
        {
            // save data one more time


            // send message to players opening assessment
            network.SendMessage("OA|");

        }

        // FOR PLAYER
        private void NarrativeSubmittedEventHandler(object sender, RoutedEventArgs e)
        {
            string pname = ((PlayerGameControl)e.OriginalSource).GetPlayerName();

            Narrative narr = new Narrative();

            narr.AuthorName = pname;
            narr.SessionName = sessionData.SessionName;
            narr.Capability = ((PlayerGameControl)e.OriginalSource).GetCapability();
            narr.NarrativeData = ((PlayerGameControl)e.OriginalSource).NarrativeData;

            playerNarratives.Add(narr);

            // save data
            try
            {
                string fname = System.Windows.Forms.Application.StartupPath + @"\PlayerData\" + pname + "-narratives.json";
                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;

                using (StreamWriter sw = new StreamWriter(fname))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    //serializer.Serialize(writer, narr);
                    serializer.Serialize(writer, playerNarratives);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            // send data to host
            // 1 message per field
            // check length and divide as needed id, votes
            string s = String.Format("NR|{0}|{1}%{2}|", pname, narr.Capability.CapabilityId, narr.Capability.Votes);
            string result;
            foreach(NarrativeElement ne in narr.NarrativeData)
            {
                s += String.Format("|{0}%{1}|", ne.ElementName, ne.ElementContent);                
            }
            //s = String.Format("NR|{0}|END|1", pname);
            result = network.SendMessage(s);
        }

        // FOR HOST
        private void HandleNarrativeData(string[] data)
        {
            string pname = data[1];

            Capability c = new Capability();
            string[] tmp = data[2].Split('%');

            foreach(Capability cap in sessionData.CapabilityData)
            {
                if(cap.CapabilityId == int.Parse(tmp[0]))
                {
                    c = cap;
                    break;
                }
            }
            //c = capabilityList[ int.Parse(tmp[0]) ]; // need to loop through and match against id
            //c.Votes = int.Parse(tmp[1]);

            Narrative narr = new Narrative();            
            List<NarrativeElement> narrativeData = new List<NarrativeElement>();

            narr.AuthorName = pname;
            narr.SessionName = "TEST";
            narr.Capability = c;

            for(int i=3;i<data.Length;i++)
            {
                tmp = data[i].Split('%');
                NarrativeElement ne = new NarrativeElement();
                ne.ElementName = tmp[0];
                ne.ElementContent = tmp[1];
                narrativeData.Add(ne);
            }

            narr.NarrativeData = narrativeData;

            // store for later
            playerNarratives.Add(narr);

            try
            {
                string fname = System.Windows.Forms.Application.StartupPath + @"\HostData\player-narratives.json";
                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;

                using (StreamWriter sw = new StreamWriter(fname))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, playerNarratives);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            // ADD TO HOSTNARRATIVESPANEL
            this.Dispatcher.Invoke(() =>
            {
                //if (!hostGameControl.hostNarrativePanel.IsEnabled)
                //{
                //    hostGameControl.hostNarrativesTab.Visibility = Visibility.Visible;
                //    hostGameControl.gameTabs.SelectedItem = hostGameControl.hostNarrativesTab;
                //}
                //hostGameControl.hostNarrativePanel.AddNarrative(narr);
            });

            // add to shared display
            this.Dispatcher.Invoke(() =>
            {
                //sharedDisplay.sharedNarrativeViewPanel.AddNarrative(narr);
            });

        }




        public List<Capability> LoadCapabilities()
        {
            List<Capability> caps = new List<Capability>();
            //gameData.CapabilityData = new List<Capability>();

            // load vignette options database
            string path = System.Windows.Forms.Application.StartupPath + @"\Assets\capabilities.json";

            // This text is added only once to the file.
            string json = "";
            try
            {
                // Create a file to read from.
                json = File.ReadAllText(path);
                caps = JsonConvert.DeserializeObject<List<Capability>>(json);
                return caps;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }


        }

        #region HOST/PLAYER JOIN
        private void HostJoinedEventHandler(object sender, RoutedEventArgs e)
        {
            playerNarratives = new List<Narrative>();
            MakePlayer(true);
            ShowHostLobby();
        }

        private void PlayerJoinedEventHandler(object sender, RoutedEventArgs e)
        {
            playerNarratives = new List<Narrative>();
            MakePlayer(false);
            ShowPlayerLobby();
        }

        private void MakePlayer(bool h)
        {
            // Initialize player
            player = new Player();
            player.IsHost = h;
        }

        private void ShowHostLobby()
        {
            gameStart.Visibility = Visibility.Collapsed;
            hostLobby = new HostLobby();

            // Register the Bubble Event Handler 
            AddHandler(HostLobby.NewRoomEvent, new RoutedEventHandler(NewRoomEventHandler));

            mainGrid.Children.Add(hostLobby);
            Grid.SetRow(hostLobby, 1);
        }

        private void ShowPlayerLobby()
        {
            gameStart.Visibility = Visibility.Collapsed;
            playerLobby = new PlayerLobby();

            // Register the Bubble Event Handler 
            AddHandler(PlayerLobby.JoinRoomEvent, new RoutedEventHandler(JoinRoomEventHandler));

            mainGrid.Children.Add(playerLobby);
            Grid.SetRow(playerLobby, 1);

        }
        #endregion

        #region MAKE AND JOIN ROOM
        private void NewRoomEventHandler(object sender, RoutedEventArgs e)
        {
            // HOST
            player.UserName = hostLobby.userNameTextBox.Text;
            MakeRoom(hostLobby.roomNameTextBox.Text, player.UserName);
            MakeNetworkConnection(hostLobby.serverIpTextBox.Text, hostLobby.serverPortTextBox.Text);
        }

        private void MakeRoom(string n, string hn)
        {
            // HOST
            sessionData = new GameData(n);
            sessionData.SessionDateTime = System.DateTime.Now;
            sessionData.RoomName = n;
            sessionData.CapabilityData = LoadCapabilities();
            sessionData.IsHost = true;

            hostLobby.Visibility = Visibility.Collapsed;

            hostGameControl = new HostGameControl();
            mainGrid.Children.Add(hostGameControl);
            hostGameControl.hostPaletteControl.paletteControl.IsReadOnly = true;

            Grid.SetRow(hostGameControl, 1);

            // launch shared display
            sharedDisplay = new Window1();
            sharedDisplay.sharedAssessmentPanel.playerPalette.IsReadOnly = true;

            sharedDisplay.Show();
        }

        // PLAYER
        private void JoinRoomEventHandler(object sender, RoutedEventArgs e)
        {
            player.UserName = playerLobby.userNameTextBox.Text;

            JoinRoom(playerLobby.roomNameTextBox.Text);
            MakeNetworkConnection(playerLobby.serverIpTextBox.Text, playerLobby.serverPortTextBox.Text);
        }

        private void JoinRoom(string n)
        {
            sessionData = new GameData(n);
            sessionData.RoomName = n;
            sessionData.CapabilityData = LoadCapabilities();
            sessionData.SessionDateTime = System.DateTime.Now;
            sessionData.IsHost = false;

            playerLobby.Visibility = Visibility.Collapsed;

            playerGameControl = new PlayerGameControl();
            mainGrid.Children.Add(playerGameControl);
            Grid.SetRow(playerGameControl, 1);
        }

        private void MakeNetworkConnection(string ip, string port)
        {
            network = new NetworkModel();
            network.PropertyChanged += OnDataPropertyChanged;
            string result = network.StartNetwork(ip, port);
            player.WriteToLog(result);
            if (result == "Server Connected")
            {
                // NP | username | gameame | ishost
                result = network.SendMessage(String.Format("NP|{0}|{1}|{2}|", player.UserName, sessionData.RoomName, System.Convert.ToInt32(player.IsHost)));
                player.WriteToLog(result);
            }
        }
        #endregion

        #region HOST GAME PLAY
        private void StartSessionEventHandler(object sender, RoutedEventArgs e)
        {
            // GET SESSION NAME AND SEND IT TO PLAYERS
            sessionData.SessionName = hostGameControl.hostGameEntryControl.sessionNameTextBox.Text;
            network.SendMessage(String.Format("SN|{0}|", sessionData.SessionName));

            // prevent host from changing name once session has started
            hostGameControl.hostGameEntryControl.sessionNameTextBox.IsReadOnly = true; 

            hostGameControl.HostPlayerName = player.UserName;
            hostGameControl.choosevignetteTab.Visibility = Visibility.Visible;
            hostGameControl.gameTabs.SelectedIndex = 1;
        }


        private void HostOpenNarrativeWritingEventHandler(object sender, RoutedEventArgs e)
        {
            network.SendMessage(String.Format("ON|{0}|", player.UserId));
            //hostGameControl.hostNarrativesTab.Visibility = Visibility.Visible;
            //hostGameControl.gameTabs.SelectedItem = hostGameControl.hostNarrativesTab;

            this.Dispatcher.Invoke(() =>
            {
                sharedDisplay.ToggleView("narrative");
            });

        }

        private void ChooseVignetteEventHandler(object sender, RoutedEventArgs e)
        {
            // Initialize shared image view
            this.Dispatcher.Invoke(() =>
            {
                sharedDisplay.SetVignette(hostGameControl.hostVignettePanel.hostVignetteViewerControl.vignetteInkCanvas.Background);
                sharedDisplay.ToggleView("vignette");                
            });

            hostGameControl.capabilityVoteTab.Visibility = Visibility.Visible;
            hostGameControl.hostCapabilityPanel.SetCapabilities(sessionData.CapabilityData);

            // announce selected vignette to players - VG | hostID | vigID 
            network.SendMessage(String.Format("VG|{0}|{1}|", player.UserId, hostGameControl.hostVignettePanel.hostVignetteSelectionControl.VignetteIndex));
        }

        private void StartVoteEventHandler(object sender, RoutedEventArgs e)
        {
            // send message to players to start voting SV| hostID
            network.SendMessage(String.Format("SV|{0}|",player.UserId));
            // call to refresh
            hostGameControl.hostCapabilityPanel.SetCapabilities(sessionData.CapabilityData);
        }

        private void StopVoteEventHandler(object sender, RoutedEventArgs e)
        {
            // EV | hostID
            network.SendMessage(String.Format("EV|{0}|",player.UserId));

            // send player names to control
            //this.Dispatcher.Invoke(() =>
            //{
            //    hostGameControl.InitializeAssessmentControl(sessionData.PlayerNames);
            //});

            hostGameControl.hostAssessmentTab.Visibility = Visibility.Visible;

        }

        private void ShowVotesEventHandler(object sender, RoutedEventArgs e)
        {            
            //capabilityList.Sort();

            this.Dispatcher.Invoke(() =>
            {
                sharedDisplay.sharedBarChart.InitializeBarChart(sessionData.CapabilityData);
                sharedDisplay.ToggleView("capability");
            });

            //hostGameControl.assignCapabilityTab.Visibility = Visibility.Visible;
            //hostGameControl.gameTabs.SelectedItem = hostGameControl.assignCapabilityTab;
        }

        // HOST VERSION
        private void HostVoteReceivedEventHandler(object sender, RoutedEventArgs e)
        {
        }
        #endregion

        #region PLAYER PLAY
        private void PlayerVoteReceivedEventHandler(object sender, RoutedEventArgs e)
        {
            // need a variable on sender to hold the most recent voted capability
            int index = ((PlayerCapabilityPanel)e.OriginalSource).VotedCapabilityIndex;

            // store vote locally
            sessionData.CapabilityData[index].Votes++;

            // send message to host
            string msg = String.Format("PV|{0}|{1}|{2}|1|", player.UserId, sessionData.HostID, index);
            
            // VR will raise a broadcast event on server, sending index - PV | playerID | hostID | capId
            network.SendMessage(msg);

            player.WriteToLog(String.Format("Sending message: {0}", msg));
        }

        private void PlayerUnvoteReceivedEventHandler(object sender, RoutedEventArgs e)
        {
            // need a variable on sender to hold the most recent voted capability
            int index = ((PlayerCapabilityPanel)e.OriginalSource).VotedCapabilityIndex;

            // store vote locally
            sessionData.CapabilityData[index].Votes--;

            // send message to host
            string msg = String.Format("PV|{0}|{1}|{2}|-1|", player.UserId, sessionData.HostID, index);

            // VR will raise a broadcast event on server, sending index - PV | playerID | hostID | capId
            network.SendMessage(msg);

            player.WriteToLog(String.Format("Sending message: {0}", msg));
        }

        //private void OpenAssessment()
        //{
        //    // update UI
        //    player.WriteToLog("assesment started");

        //    // pass capabilities to refresh
        //    this.Dispatcher.Invoke(() =>
        //    {
        //        // enable assessment panel
        //        playerGameControl.OpenAssessment();
        //    });

        //}

        //private void OpenNarratives()
        //{
        //    player.WriteToLog("voting started");

        //    // pass capabilities to refresh
        //    this.Dispatcher.Invoke(() =>
        //    {
        //        // enable cpability panel
        //        //playerGameControl.playerNarrativesTab.IsEnabled = true;
        //        //playerGameControl.gameTabs.SelectedItem = playerGameControl.playerNarrativesTab;
        //        playerGameControl.SetPlayerName(player.UserName);
        //        playerGameControl.playerNarrativeChoicePanel.SetCapabilities(sessionData.CapabilityData);
        //    });
        //}

        // This makes the assessment (palette) interface active and populates the data it needs
        private void PlayerStartAssessment()
        {
            this.Dispatcher.Invoke(() =>
            {
                playerGameControl.SetPlayerName(player.UserName);
                playerGameControl.playerNarrativeChoicePanel.SetCapabilities(sessionData.CapabilityData);
                playerGameControl.StartAssessment();

            });
        }

        private void PlayerStopAssessment()
        {
            
        }


        #endregion


        #region COMMON METHODS
        /// <summary>
        /// NETWORK MESSAGE HANDLING
        /// </summary>
        private void OnDataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "MessageContent":
                    HandleNetworkMessage(network.MessageContent);
                    break;
            }
        }

        private void HandleNetworkMessage(string msg)
        {
            string[] data = msg.Split('|');

            player.WriteToLog(String.Format("Message receieved: {0}", msg));

            switch(data[0])
            {
                case "W":
                    player.UserId = int.Parse(data[1]);
                    break;
                case "PJ":
                    AddPlayer(int.Parse(data[1]),data[2]);
                    break;
                case "VG":
                    SetVignette(int.Parse(data[2]));
                    break;
                case "SV":
                    PlayerStartVoting();
                    break;
                case "EV":
                    PlayerStopVoting();
                    break;
                case "PV":
                    HandlePlayerVote(data[1], int.Parse(data[2]), int.Parse(data[3]));
                    break;
                case "ON":
                    //OpenNarratives();
                    break;
                case "NR":
                    HandleNarrativeData(data);
                    break;
                case "OA":
                    //OpenAssessment();
                    break;
                case "SN":
                    sessionData.SessionName = data[1];
                    break;
                case "SA":
                    PlayerStartAssessment();
                    break;
                case "EA":
                    PlayerStopAssessment();
                    break;
                case "SO":
                    PlayerEndSession();
                    break;
                case "PI":
                    AddPaletteItem(data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8]);
                    break;
                case "PC":
                    AddPlayerCapability(data[1], data[2]);
                    break;
                case "IU":
                    if(data[1]=="0")
                    {
                        // position update
                        UpdatePaletteItemPosition(player.IsHost, data[3], data[4], data[5], data[6]);
                    }
                    else if(data[1]=="1")
                    {
                        // content update
                        UpdatePaletteItemContent(player.IsHost, data[3], data[4]);
                    }
                    break;
                default:
                    System.Windows.MessageBox.Show(String.Format("Unrecognized message: {0} on client {1}", msg, player.UserName));
                    break;
            }
        }

        private void AddPlayerCapability(string name, string ci)
        {
            // get reference to player

            // 
        }

        public void UpdatePaletteItemPosition(bool isHost, string id, string px, string py, string score)
        {
            // get reference to item...probably a more efficient way to search for this, but it should work
            foreach(PaletteItemData pid in sessionData.PaletteItemData)
            {
                Console.WriteLine("");
                if(pid.ItemId == id)
                {
                    pid.Position = new Point(System.Convert.ToDouble(px), System.Convert.ToDouble(py));
                    pid.Score = System.Convert.ToDouble(score);

                    if (isHost)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            hostGameControl.hostPaletteControl.paletteControl.UpdatePaletteItem(pid);
                        });
                    }
                    else
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            playerGameControl.playerPaletteControl.playerPalette.UpdatePaletteItem(pid);
                        });                        
                    }
                    break;
                }
            }
        }

        public void UpdatePaletteItemContent(bool isHost, string id, string content)
        {
            // get reference to item...probably a more efficient way to search for this, but it should work
            foreach (PaletteItemData pid in sessionData.PaletteItemData)
            {
                if (pid.ItemId == id)
                {
                    pid.Content = content;

                    if (isHost)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            hostGameControl.hostPaletteControl.paletteControl.UpdatePaletteItem(pid);
                        });
                    }
                    else
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            playerGameControl.playerPaletteControl.playerPalette.UpdatePaletteItem(pid);
                        });
                    }
                    break;
                }
            }
        }

        private void AddPaletteItem(string author, string capid, string content, string id, string px, string py, string quadrant, string score)
        {
            PaletteItemData pid = new PaletteItemData();
            pid.Author = author;
            pid.CapabilityIndex = int.Parse(capid);
            pid.Content = content;
            pid.ItemId = id;
            pid.Position = new Point(System.Convert.ToDouble(px), System.Convert.ToDouble(py));
            pid.Quadrant = quadrant;
            pid.Score = System.Convert.ToDouble(score);

            sessionData.PaletteItemData.Add(pid);

            string name;

            if(player.IsHost)
            {
                name = hostGameControl.hostPaletteControl.GetSelectedPlayerName();
                //if(name == pid.Author)
                //{
                    this.Dispatcher.Invoke(() =>
                    {
                        //hostGameControl.hostPaletteControl.paletteControl.AddItemFromServer(pid);
                        hostGameControl.AddPlayerNameToAssessmentControl(pid.Author);
                    });
                //}
                if(name ==pid.Author)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        hostGameControl.hostPaletteControl.paletteControl.AddItemFromServer(pid);
                    });
                }
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    playerGameControl.playerPaletteControl.playerPalette.AddItemFromServer(pid);
                });
            }

        }

        private void PlayerEndSession()
        {
            this.Dispatcher.Invoke(() =>
            {
                // tell plpayerGameControl to end the session
                playerGameControl.EndSession();
            });


            // save data 
            SaveData(false);

        }


        #endregion



        private void AddPlayer(int id, string name)
        {
            sessionData.PlayerNames.Add(name);
            string result = hostGameControl.hostGameEntryControl.AddName(name);
            player.WriteToLog(result);
        }

        private void HandlePlayerVote(string pname, int cid, int dir)
        {
            // this is probably gonna fail if the player takes backk their vote as you don't track whether they were voting or unvoting in the messge
            // yep, it does...just need to wire up an added byte to the messag indicating direction of vote
            // OK TEST THIS...
            // not working...because cid no longer matches index afer sorting
            player.WriteToLog(String.Format("vote recieved for capability: {0} from player {1} in direction {2}", cid, pname, dir));
            foreach(Capability c in sessionData.CapabilityData)
            {
                if(c.CapabilityId==cid)
                {
                    c.Votes += dir;
                    player.WriteToLog(String.Format("match with capability: {0} (id: {1}), new vote count {2}", c.CapabilityName, c.CapabilityId, c.Votes));
                }
            }

            // refresh display
            this.Dispatcher.Invoke(() =>
            {
                //hostGameControl.hostCapabilityPanel.SetCapabilities(capabilityList);
                hostGameControl.hostCapabilityPanel.SetCapabilities(sessionData.CapabilityData);
            });
        }

        // PLAYER
        private void PlayerStartVoting()
        {
            player.WriteToLog("voting started");

            // pass capabilities to refresh
            this.Dispatcher.Invoke(() =>
            {
                //playerGameControl.playerCapabilityPanel.SetCapabilities(sessionData.CapabilityData);
                // enable cpability panel
                playerGameControl.StartVoting(sessionData.CapabilityData);
                //playerGameControl.capabilityVoteTab.Visibility = Visibility.Visible;
                //playerGameControl.gameTabs.SelectedItem = playerGameControl.capabilityVoteTab;
            });
        }

        private void PlayerStopVoting()
        {
            player.WriteToLog("voting ended");
            //MessageBox.Show("Voting has ended!");

            this.Dispatcher.Invoke(() =>
            {
                // enable cpability panel
                playerGameControl.playerCapabilityPanel.DisableVoting();
            });

        }

        private void SetVignette(int i)
        {
            // the check on IsHost probably isn't necessary, as hosts should never get this message, but doesn't hurt
            if(!player.IsHost)
            {
                this.Dispatcher.Invoke(() =>
                {
                    playerGameControl.SetVignette(i);
                });
                
            }
        }


        // METHODS


        
        private void Window_Closing(object s, System.ComponentModel.CancelEventArgs e)
        {
            if (sharedDisplay != null)
                sharedDisplay.Close();

            if(network != null)
            {
                network.SendMessage(String.Format("QT|{0}|", player.UserId));
                network.CloseNetworkConnection(player.IsHost, player.UserName);
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                if (this.WindowStyle == WindowStyle.SingleBorderWindow)
                {
                    this.ResizeMode = ResizeMode.NoResize;
                    this.WindowStyle = WindowStyle.None;
                    this.WindowState = WindowState.Maximized;
                    this.Topmost = true;
                }
                else
                {
                    this.ResizeMode = ResizeMode.CanResize;
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    this.WindowState = WindowState.Maximized;
                    this.Topmost = false;

                }
            }
        }

        private void closeAllButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                if (e.ClickCount == 2)
                {
                    AdjustWindowSize();
                }
                else
                {
                    this.DragMove();
                }
        }

        private void AdjustWindowSize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                this.ResizeMode = ResizeMode.CanResizeWithGrip;
            }
            else
            {
                this.ResizeMode = ResizeMode.NoResize;
                this.WindowState = WindowState.Maximized;
            }

        }
    }

}
