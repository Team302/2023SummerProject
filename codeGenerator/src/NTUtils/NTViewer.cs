using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkTables;
using NetworkTables.Tables;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace NTUtils
{
    public class NTViewer
    {
        NetworkTable table;
        TreeView tree;
        Collection<TreeNode> treeNodes = new Collection<TreeNode>();
        bool hasAttachedListener = false;
        bool hasConnected = false;

        private static Action<ITable, string, Value, NotifyFlags> onSubTableCreation;
        private static Action<ITable, string, Value, NotifyFlags> onTableChange;

        public NTViewer(TreeView ntTree)
        {
            tree = ntTree;

            onSubTableCreation = (table, key, value, flag) =>
            {
                tree.BeginInvoke(new Action(() =>
                {
                    if(flag == NotifyFlags.NotifyNew)
                    {
                        string tableName = ((NetworkTable)table).ToString();
                        Debug.WriteLine("Table Name: " + tableName);
                    }
                }));
            };

            onTableChange = (table, key, value, flag) =>
            {
                switch(flag)
                {
                    case NotifyFlags.NotifyNew:
                        AddToTree(table, key, value);
                        break;
                    case NotifyFlags.NotifyUpdate:
                        UpdateNodeInTree(table, key, value);
                        break;
                    default:
                        //No-op for delete at the moment
                        break;
                }
            };
        }

        private string GetTreePath(string tableName, string key)
        {
            return "";
        }

        private void UpdateNodeInTree(ITable table, string key, Value value)
        {
            
        }

        private void AddToTree(ITable table, string key, Value value)
        {
            throw new NotImplementedException();
        }

        public void ConnectToNetworkTables()
        {
            NetworkTable.SetClientMode();
            NetworkTable.SetIPAddress("localhost"); //may have to change this to team number later?
            NetworkTable.SetPort(57231);
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
                    newNode.Nodes.Add(key + "(" + GetNTValueAsString(subNT, key) + ")");
                }

                treeNodes.Add(newNode);

                tree.BeginInvoke(new Action(() =>
                {
                    tree.Nodes.Add(newNode);
                }));
            }
        }

        private string GetNTValueAsString(NetworkTable subNT, string key)
        {
            foreach(string testKey in subNT.GetKeys()) 
            {
                if(testKey == key)
                {
                    switch(subNT.GetValue(key, Value.MakeString("NO VALUE")).Type)
                    {
                        case NtType.Boolean:
                            return subNT.GetBoolean(key, false).ToString();
                            break;
                        case NtType.String:
                            return subNT.GetString(key, "NO VALUE");
                            break;
                        case NtType.Double:
                            return subNT.GetNumber(key, -1.302).ToString();
                            break;
                        default:
                            return "UNKNOWN TYPE";
                            break;
                    }
                }
            }
            return "UNKNOWN";
        }

        public void FilterTree(string text)
        {
            foreach(TreeNode node in treeNodes)
            {
                if (node != null)
                {
                    if (!node.Text.ToLower().Contains(text.ToLower()))
                    {
                        tree.Nodes.Remove(node);
                    }
                    else 
                    {
                        if (!tree.Nodes.Contains(node))
                            tree.Nodes.Add(node);
                    }
                }
            }
        }
    }
}
