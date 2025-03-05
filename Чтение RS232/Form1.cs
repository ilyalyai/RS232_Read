using System.ComponentModel;
using System.IO.Ports;
using System.Text;

namespace ЧтениеRS232
{
    public partial class Form1 : Form
    {
        public SerialPort SerialPort { get; set;}
        public Byte[] data = new byte[8];

        public Form1()
        {
            SerialPort = new();
            InitializeComponent();
        }

        /* Обновление списка портов */

        private void button1_Click_1(object sender, EventArgs e)
        {
            UpdatePorts();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (SerialPort.IsOpen)
                    SerialPort.Close();
                SerialPort.PortName = comboBox1.Text;
                SerialPort.Open();
                if (!backgroundWorker1.IsBusy)
                    backgroundWorker1.RunWorkerAsync();

                //button1.Visible = false;
                //button2.Visible = false;
                //comboBox1.Enabled = false;
                label1.Text = $"Подключено к {comboBox1.Text}";
                label1.Font = new Font(label1.Font, FontStyle.Bold);
                button2.Text = "Переключиться";
            }
            catch
            {
                label1.Font = new Font(label1.Font, FontStyle.Bold);
                label1.Text = "COM порт не найден!";
                return;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            UpdatePorts();
        }

        private void UpdatePorts()
        {
            try
            {
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(SerialPort.GetPortNames());
            }
            catch
            {
                label1.Font = new Font(label1.Font, FontStyle.Bold);
                label1.Text = "COM порт не найден!";
                return;
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            for (; ; )
            {
                if (SerialPort.IsOpen)
                {
                    int bytesRead = SerialPort.Read(data, 0, 8);
                    var result = Encoding.ASCII.GetString(data, 0, bytesRead);
                    backgroundWorker1.ReportProgress(0, result);
                    if (result.Length == 8)
                    {
                        /*var io = SearchForObj(result);
                        if (io != null)
                        {
                            this.Invoke(new MethodInvoker(delegate
                            {
                                SerialPort.Close();
                                Service.UI.OpenInNewTabPage(io, false);
                                this.Dispose();
                            }));
                        }*/
                    }
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label2.Text += "\n" + e.UserState;
        }
    }
}