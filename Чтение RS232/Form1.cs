using System.ComponentModel;
using System.IO.Ports;
using System.Text;

namespace ЧтениеRS232
{
    public partial class Form1 : Form
    {
        public SerialPort SerialPort = new();
        public string[] serialPorts;
        private int num_ports = 0;
        public Byte[] data = new byte[8];

        public Form1()
        {
            InitializeComponent();
        }

        /* Обновление списка портов */

        private void button1_Click_1(object sender, EventArgs e)
        {
            UpdatePorts();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SerialPort.Close();
            try
            {
                for (int i = 0; i < num_ports; i++)
                {
                    //тут если подключаться к подключенному, то возникала ошибка

                    if (comboBox1.SelectedIndex == i && !SerialPort.IsOpen)
                    {
                        SerialPort.PortName = comboBox1.Text;
                        break;
                    }
                }
                SerialPort.Open();
                backgroundWorker1.RunWorkerAsync();

                button1.Visible = false;
                button2.Visible = false;
                comboBox1.Enabled = false;
            }
            catch
            {
                button1.Text = "COM порт не найден!";
                return;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            UpdatePorts();
        }

        private void UpdatePorts()
        {
            num_ports = 0;
            try
            {
                serialPorts = SerialPort.GetPortNames();
                foreach (string serialPort in serialPorts)
                {
                    comboBox1.Items.Add(serialPort);
                    num_ports++;
                }
            }
            catch
            {
                button1.Text = "COM порт не найден!";
                return;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (; ; )
            {
                if (SerialPort.IsOpen)
                {
                    int bytesRead = SerialPort.Read(data, 0, 8);
                    var result = Encoding.ASCII.GetString(data, 0, bytesRead);
                    backgroundWorker1.ReportProgress(0, result);

                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label2.Text = label2.Text + e.UserState;
        }
    }
}