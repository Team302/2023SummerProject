using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkTables;
using NetworkTables.Tables;
using System.Windows.Forms;
using System.Diagnostics;

namespace NTUtils
{
    public class NTViewer
    {
        NetworkTable table;
        TreeView tree;
        bool hasAttachedListener = false;
        bool hasConnected = false;

        private static Action<ITable, string, Value, NotifyFlags> onSubTableCreation;
        private static Action<ITable, string, Value, NotifyFlags> onTableChange;

        public NTViewer(TreeView ntTree)
        {
            tree = ntTree;

            onSubTableCreation = (table, key, value, flags) =>
            {
                tree.BeginInvoke(new Action(() =>
                {
                    if(flags == NotifyFlags.NotifyNew)
                    {
                        string tableName = ((NetworkTable)table).ToString();
                        Debug.WriteLine("Table Name: " + tableName);
                    }
                }));
            };

            onTableChange = (table, key, value, flags) =>
            {
                Debug.WriteLine("Table with change: " + table);
                Debug.WriteLine("Key with change: " + key);
                Debug.WriteLine("New value: " + value);
                Debug.WriteLine("Flags: " + flags);
            };
        }
        
        public void ConnectToNetworkTables()
        {
            NetworkTable.SetClientMode();
            NetworkTable.SetIPAddress("localhost"); //may have to change this to team number later?
            NetworkTable.SetPort(57231);
            //NetworkTable.SetTeam(2023);
            NetworkTable.Initialize();

            Action<IRemote, ConnectionInfo, bool> onConnect = (iRemote, connectInfo, b) =>
            {
                if (!hasConnected)
                {
                    table = NetworkTable.GetTable("");

                    AddListeners(table);

                    hasConnected = true;
                }
            };

            if(!hasAttachedListener)
            {
                NetworkTable.AddGlobalConnectionListener(onConnect, true);
                hasAttachedListener = true;
            }
        }

        public void AddListeners(NetworkTable table)
        {
            table.AddTableListenerEx(onTableChange, NotifyFlags.NotifyNew | NotifyFlags.NotifyUpdate | NotifyFlags.NotifyDelete);
            table.AddSubTableListener(onSubTableCreation);
            foreach (string subtable in table.GetSubTables())
            {
                NetworkTable subNT = (NetworkTable)table.GetSubTable(subtable);
                AddListeners(subNT);

                TreeNode newNode = new TreeNode(subtable);
                foreach(string key in subNT.GetKeys())
                {
                    newNode.Nodes.Add(key);
                }

                tree.BeginInvoke(new Action(() =>
                {
                    tree.Nodes.Add(newNode);

                }));
            }
        }
    }
}
