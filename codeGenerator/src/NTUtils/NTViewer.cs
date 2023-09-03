using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkTables;
using NetworkTables.Tables;
using System.Diagnostics;

namespace NTUtils
{
    public class NTViewer
    {
        static NetworkTable table;
        static HashSet<string> subtables = new HashSet<string>();
        static bool hasAttachedListener = false;
        public static void ConnectToNetworkTables()
        {
            NetworkTable.SetClientMode();
            NetworkTable.SetIPAddress("localhost"); //may have to change this to team number later?
            NetworkTable.SetPort(57231);
            //NetworkTable.SetTeam(2023);
            NetworkTable.Initialize();

            Action<IRemote, ConnectionInfo, bool> onConnect = (iRemote, connectInfo, b) =>
            {
                table = NetworkTable.GetTable("");
                subtables = table.GetSubTables();
                Debug.WriteLine("Connected!");
                //addValueListeners(smartDashboard);
                //smartDashboard.AddSubTableListener(this);
            };

            if(!hasAttachedListener)
            {
                NetworkTable.AddGlobalConnectionListener(onConnect, true);
                hasAttachedListener = true;
            }
        }
    }
}
