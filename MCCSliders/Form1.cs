using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace MCCSliders
{
    public partial class Form1 : Form
    {
        public static Process process;
        public string presetsFile = "MCCSliders_settings.txt";
        public string currentMap;
        public long taginstanceOffset = 0;
        public int firstPersonWeaponOffset = 0x4CC;
        public int multiplier = 1000;
        public long metaMagic = 0;
        public Dictionary<int, item> items;
        public Dictionary<int, int> itemsConversion = new Dictionary<int, int>(); 
        // keep a conversion dictionary between combobox and weapons list
        // key is combobox index
        // value is weapons list item ID
        // what was the special comment block thingy again

        public class item
        {
            public string mapname;
            public string tagname;
            public long offset;
            public float X;
            public float Y;
            public float Z;
        }
        public Form1()
        {
            InitializeComponent();
            Button_reset_Click(null, null);
            checkBox_auto_apply.Checked = true;
        }
        private void Button_reset_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            if (!UpdateProcess()) // check if process is available
                return;
            
            if (!GetInfo()) // read some info to get tags and map name
                return;

            if (!GetPresets()) // read presets from the settings file
            {
                Log($"ERROR: could not parse settings {presetsFile}");
                return;
            }

            if (!UpdateComboBox()) // update weapons list
            {
                // Log($"ERROR: could not parse settings {presetsFile}");
                return;
            }

            UpdateAllWeapons();

        }
        public bool UpdateProcess()
        {
            var processes = Process.GetProcessesByName("MCC-Win64-Shipping");

            if (processes.Length == 0)
            {
                Log("ERROR: game is not running or EAC is enabled.");
                return false;
            }

            process = processes[0];

            var hProc = OpenProcess(ProcessAccessFlags.VMRead, false, (int)process.Id);

            return true;
        }
        public bool GetInfo()
        {
            if (process == null)
            {
                Log("WARNING: game is not running. Press the reload button once the game is running with EAC disabled.");
                return false;
            }

            var BaseModuleAddress = ProcessModuleMemoryStream(process);
            if (BaseModuleAddress == -1)
            {
                Log($"FATAL ERROR: You cannot use mods in MCC matchmaking with EAC enabled. \n" +
                    $"To use this tool in custom games, run MCC trough steam and select MCC Anti-Cheat Disabled." +
                    $"This tool can only be used while playing Halo Reach, not in MCC's menu.");
                return false;
            }

            var magic = 0x0000000180000000;
            var pointer = 0x1825B6D80; // mcc reach v1.1305.0.0, from xboxchaos/assembly
            var MemMagicOffset = 0xA0A0;
            var _moduleaddress = BaseModuleAddress; // -magic was already done
            var point1 = _moduleaddress + pointer;

            try
            {
                var test = BitConverter.ToInt64(ReadMem(process, point1), 0); // read from memory
            }
            catch (Exception e)
            {
                Log("ERROR: game is not running or EAC is enabled. Relaunch the game trough Steam and disable EAC to play with mods on custom games.");
                return false;
            }

            var point2 = BitConverter.ToInt64(ReadMem(process, point1), 0); // read from memory
            if (point2 == 0)
            {
                Log("ERROR: game is not running or EAC is enabled or player is in the menu. " +
                    "Relaunch the game trough Steam and disable EAC to play with mods on custom games.");
                return false;
            }
            var point3 = point2 + MemMagicOffset;
            metaMagic = BitConverter.ToInt64(ReadMem(process, point3), 0); // read from memory

            // get map name
            var mapnameB = ReadMem(process, point2 + 0x1CC, 64);
            var mapnameFull = "";
            foreach (var a in mapnameB)
            {
                if (a == 0)
                    break;
                mapnameFull = $"{mapnameFull}{(char)a}";
            }

            var mapname = mapnameFull.Split("\\".ToCharArray()).Last();
            currentMap = mapname;

            Log($"Detected map {mapnameFull}; {mapname}");
            return true;
        }
        public long ProcessModuleMemoryStream(Process process) // from XboxChaos Assembly on Github
        {
            var _process = process;
            ProcessModule _processModule = null;

            try
            {
                foreach (ProcessModule m in process.Modules)
                    ;
            }
            catch
            {
                return -1;
            }

            foreach (ProcessModule m in process.Modules)
                if (Path.GetFileNameWithoutExtension(m.FileName) == "haloreach")
                    _processModule = m;

            var _moduleaddress = (long)_processModule.BaseAddress - 0x180000000;

            return _moduleaddress;
        }
        public bool GetPresets()
        {
            items = new Dictionary<int, item>();

            if (!File.Exists(presetsFile))
            {
                Log($"ERROR: settings file does not exist: {presetsFile}.\n" +
                    $"Extract the provided presets file and place it in the same folder as this executable/app.");

                return false;
            }

            var csv = ReadCsv($"{presetsFile}");

            int i = -1;
            foreach (var a in csv)
            {
                i++;
                var b = a.Split(",".ToCharArray());

                if (b.Length < 3)
                {
                    Log($"ERROR: failed to parse settings at line {i+1} for {presetsFile}.\n" +
                        $"Expected format: mapname, tagname, tag offset, X, Y, Z (the last 3 values can be ignored)");
                    return false;
                }

                float x = 0;
                float y = 0;
                float z = 0;

                if (b.Length > 5)
                {
                    x = StringToFloat(b[3]);
                    y = StringToFloat(b[4]);
                    z = StringToFloat(b[5]);
                }

                items.Add(i, new item {
                    mapname = b[0],
                    tagname = b[1],
                    offset = HexToLong(b[2]) * 4 + 0x50000000,
                    X = x,
                    Y = y,
                    Z = z
                });
            }

            return true;
        }
        public bool UpdateComboBox()
        {
            comboBox1.Items.Clear();
            itemsConversion.Clear();

            var i = -1;
            var j = -1;
            foreach (var a in items)
            {
                i++;
                if (a.Value.mapname == currentMap)
                {
                    j++;
                    comboBox1.Items.Add(a.Value.tagname);
                    itemsConversion.Add(j, i);
                }
            }

            if (comboBox1.Items.Count == 0)
            {
                Log($"ERROR: settings file is likely corrupted or is missing the weapons list for this map: {currentMap}");
                return false;
            }

            comboBox1.SelectedIndex = 0;

            return true;
        }
        public void UpdateAllWeapons()
        {
            foreach (var a in items)
            {
                if (itemsConversion.ContainsValue(a.Key))
                {
                    var tagname = a.Value.tagname;
                    taginstanceOffset = a.Value.offset;
                    WriteMem(process, metaMagic + taginstanceOffset + firstPersonWeaponOffset + 0x0, BitConverter.GetBytes(a.Value.X));
                    WriteMem(process, metaMagic + taginstanceOffset + firstPersonWeaponOffset + 0x4, BitConverter.GetBytes(a.Value.Y));
                    WriteMem(process, metaMagic + taginstanceOffset + firstPersonWeaponOffset + 0x8, BitConverter.GetBytes(a.Value.Z));
                }
            }
        }
        public void doThingy(TextBox textBox, TrackBar trackBar, int wat)
        {
            textBox.Text = "" + trackBar.Value;

            var a = comboBox1.SelectedIndex;
            if ((uint)a == 0xFFFFFFFF) // this should never happen but it di
                return;

            var weap = items[itemsConversion[a]];

            taginstanceOffset = weap.offset;
            switch(wat) // this is really unnecessarily bad
            {
                case 0:
                    weap.X = (float)trackBar.Value / multiplier;
                    break;
                case 1:
                    weap.Y = (float)trackBar.Value / multiplier;
                    break;
                case 2:
                    weap.Z = (float)trackBar.Value / multiplier;
                    break;
            }

            WriteMem(process, metaMagic + taginstanceOffset + firstPersonWeaponOffset + wat * 4, BitConverter.GetBytes((float)trackBar.Value / multiplier));

        }
        private void trackBar_X_Scroll(object sender, System.EventArgs e)
        {
            doThingy(textBox_X, trackBar_X, 0);
        }
        private void trackBar_Y_Scroll(object sender, EventArgs e)
        {
            doThingy(textBox_Y, trackBar_Y, 1);
        }
        private void trackBar_Z_Scroll(object sender, EventArgs e)
        {
            doThingy(textBox_Z, trackBar_Z, 2);
        }
        private void Button_writeToFile_Click(object sender, EventArgs e)
        {
            var newItems = new List<string>();

            foreach (var a in items)
            { 
                var b = a.Value;
                var d = $"{b.mapname},{b.tagname},{((a.Value.offset - 0x50000000) / 4):X8},{b.X},{b.Y},{b.Z}";
                newItems.Add(d);
            }

            WriteToFile(newItems, presetsFile);
        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var weap = items[itemsConversion[comboBox1.SelectedIndex]];

            taginstanceOffset = weap.offset;

            Int32.TryParse($"{weap.X * multiplier}".Split(".".ToCharArray())[0], NumberStyles.Integer, null, out int X);
            Int32.TryParse($"{weap.Y * multiplier}".Split(".".ToCharArray())[0], NumberStyles.Integer, null, out int Y);
            Int32.TryParse($"{weap.Z * multiplier}".Split(".".ToCharArray())[0], NumberStyles.Integer, null, out int Z);

            trackBar_X.Value = X;
            trackBar_Y.Value = Y;
            trackBar_Z.Value = Z;

            trackBar_X_Scroll(null, null);
            trackBar_Y_Scroll(null, null);
            trackBar_Z_Scroll(null, null);

            Button_writeToFile_Click(null, null);
        }
        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length; 
            richTextBox1.ScrollToCaret();
        }
        private void Button_zeroAll_Click(object sender, EventArgs e)
        {
            foreach (var weap in items)
            {
                weap.Value.X = 0;
                weap.Value.Y = 0;
                weap.Value.Z = 0;
                if (itemsConversion.ContainsValue(weap.Key))
                {
                    taginstanceOffset = weap.Value.offset;
                    WriteMem(process, metaMagic + taginstanceOffset + firstPersonWeaponOffset + 0x0, new byte[4]);
                    WriteMem(process, metaMagic + taginstanceOffset + firstPersonWeaponOffset + 0x4, new byte[4]);
                    WriteMem(process, metaMagic + taginstanceOffset + firstPersonWeaponOffset + 0x8, new byte[4]);
                }
            }
        }
        private void Button_applyToAll_Click(object sender, EventArgs e)
        {
            var selectedWeap = items[itemsConversion[comboBox1.SelectedIndex]];

            foreach (var weap in items)
            {
                weap.Value.X = selectedWeap.X;
                weap.Value.Y = selectedWeap.Y;
                weap.Value.Z = selectedWeap.Z;

                if (itemsConversion.ContainsValue(weap.Key))
                {
                    taginstanceOffset = weap.Value.offset;
                    WriteMem(process, metaMagic + taginstanceOffset + firstPersonWeaponOffset + 0x0, BitConverter.GetBytes(selectedWeap.X));
                    WriteMem(process, metaMagic + taginstanceOffset + firstPersonWeaponOffset + 0x4, BitConverter.GetBytes(selectedWeap.Y));
                    WriteMem(process, metaMagic + taginstanceOffset + firstPersonWeaponOffset + 0x8, BitConverter.GetBytes(selectedWeap.Z));
                }
            }
        }

        #region Memory editing utilities
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_OPERATION = 0x0008;

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, Int64 lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(IntPtr process, Int64 address, byte[] buffer, int size, out int bytesRead);

        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(IntPtr hProcess);

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }

        public static void WriteMem(Process p, long address, byte[] val)
        {
            var hProc = OpenProcess(ProcessAccessFlags.All, false, (int)p.Id);

            int outs = 0;
            WriteProcessMemory(hProc, address, val, (UInt32)val.Length, out outs);

            CloseHandle(hProc);
        }
        public static byte[] ReadMem(Process p, long address, int length = 8)
        {
            var hProc = OpenProcess(ProcessAccessFlags.All, false, (int)p.Id);

            int outs = 0;
            byte[] buffer = new byte[length];
            ReadProcessMemory(hProc, address, buffer, buffer.Length, out outs);

            CloseHandle(hProc);

            return buffer;
        }
        #endregion

        #region Utilities
        public static void WriteToFile(List<string> in_, string file)
        {
            var fileOut = new FileInfo(file);
            if (File.Exists(file))
                File.Delete(file);

            int i = -1;
            try
            {
                using (var csvStream = fileOut.OpenWrite())
                using (var csvWriter = new StreamWriter(csvStream))
                {
                    foreach (var a in in_)
                    {
                        csvStream.Position = csvStream.Length;
                        csvWriter.WriteLine(a);
                        i++;
                    }
                }
            }
            catch
            { }

        }
        public void Log(string in_)
        {
            richTextBox1.Text = richTextBox1.Text + $"\n{in_}";
        }
        private static List<string> ReadCsv(string filename)
        {
            var output = new List<string>();

            using (var reader = new StreamReader(filename))
            {
                var line = "";
                while (line != null)
                {
                    line = reader.ReadLine();
                    output.Add(line);
                }

                if (output.Last() == null)
                    output.RemoveAt(output.Count - 1);
            }

            return output;
        }
        public float StringToFloat(string val)
        {
            if (!float.TryParse(val, NumberStyles.Float, null, out float ex))
                return 0;

            return ex;
        }
        public long HexToLong(string val)
        {
            if (!long.TryParse(val, NumberStyles.HexNumber, null, out long ex))
                return 0;

            return ex;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if(checkBox_auto_apply.Checked)
                Button_reset_Click(null, null);
        }
        #endregion

        /*
         * TODO
         * 
         * Verify every 5 seconds to apply changes in memory, so the user doesn't need to press the reload button every time.
         * Save changes to .map.
         * Read tags list from memory instead to work with modded .map files and future tag updates.
         * Lock weapons to prevent global override
         * 
         */

    }
}
