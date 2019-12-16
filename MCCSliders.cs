using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCCSliders
{
    public partial class Form1 : Form
    {
        public int address = 0;

        #region Certified  Memory Correction Tool
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_OPERATION = 0x0008;

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, Int64 lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

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
        #endregion

        public static Process process;

        public Form1()
        {
            var processes = Process.GetProcessesByName("eldorado");

            if (processes.Length == 0)
                return;

            process = processes[0];

            var hProc = OpenProcess(ProcessAccessFlags.VMRead, false, (int)process.Id);

            InitializeComponent();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            var newAddress = textBox6.Text.Split("x".ToCharArray()).Last();

            if (int.TryParse(newAddress, NumberStyles.HexNumber, null, out int address_))
                address = address_;
        }

        private void trackBar1_Scroll(object sender, System.EventArgs e)
        {
            textBox1.Text = "" + trackBar1.Value;

            byte[] c = BitConverter.GetBytes(trackBar1.Value);

            c = new byte[] { c[0] };

            Program.WriteMem(process, address, c);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, NumberStyles.Integer, null, out int val))
                trackBar1.Value = val;

            byte[] c = BitConverter.GetBytes(trackBar1.Value);

            c = new byte[] { c[0] };

            Program.WriteMem(process, address, c);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            textBox4.Text = "" + trackBar4.Value;

            byte[] c = BitConverter.GetBytes(trackBar4.Value);

            Program.WriteMem(process, address, c);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox4.Text, NumberStyles.Integer, null, out int val))
                trackBar4.Value = val;

            byte[] c = BitConverter.GetBytes(trackBar4.Value);

            Program.WriteMem(process, address, c);
        }
    }
}
