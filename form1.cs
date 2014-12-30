using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;


 
//254,108, 1 Turn On Relay 1 in Bank 1 
//254,108, 2 Turn On Relay 1 in Bank 2 
//254,109, 2 Turn On Relay 2 in Bank 2 
//254,110, 2 Turn On Relay 3 in Bank 2 
//254,108, 3 Turn On Relay 1 in Bank 3 
//254,111, 3 Turn On Relay 4 in Bank 3 
//254,112, 3 Turn On Relay 5 in Bank 3 
  
//254,112, 0 Turn On Relay 5 in All Relay Banks (Bank 0
//flip off and on  254,50,70,0,0,1,1	#last number is the relay number 
// 254,246 (get version, good way to see if you have a connection)
//com 7 is new gate
//com 5 is original gate



namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        SerialPort LeftGatePort = new SerialPort("COM5",9600);
        SerialPort RightGatePort = new SerialPort("COM7", 9600);
        SerialPort Buttons = new SerialPort("COM9", 9600);

        public Form1()
        {
            InitializeComponent();

           // LeftGatePort.Open();

            Buttons.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            Buttons.Open();
           


        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {// Event for receiving data
            // Read the buffer to text box.
          //  statusBar1.Text = "Getting Data";
        //    statusBar1.Refresh();
        //    txtData.Text = txtData.Text + port.ReadExisting();
            var s = Buttons.ReadExisting().Trim();

            Console.WriteLine("GateOpen called, button {0}", s);

            if (cbDebug.Checked)
            {
                Console.WriteLine("Button test (debug) mode, so not calling gate.");
                return;
            }
           // Console.WriteLine(s);

            switch(s)
            {
                case "1":
                    Console.WriteLine("B1");
                    GateOpen(RightGatePort);
                    break;

                case "2":
                    Console.WriteLine("Last Test...B2");
                    GateOpen(LeftGatePort);
                    break;

                default:
                    Console.WriteLine("Unrecognized from device. ");
                    break;





            }

        }


        private void GateOpen(SerialPort s)
        {
           

            byte[] b = new byte[] { 254, 50, 70, 0, 0, 1, 0 };
            try
            {
                if (!s.IsOpen)
                {
                    s.Open();
                    s.Write(b, 0, 7);
                    s.Close();
                    return;
                }
                else
                {
                    try 
                    { 
                        s.Write(b, 0, 7);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                 
                 
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());

            }



        }


        private void button1_Click(object sender, EventArgs e)
        {
            GateOpen(LeftGatePort);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GateOpen(RightGatePort);

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) //left open connection
        {
            if(checkBox1.Checked)
            {

                Console.WriteLine("Left Gate Connection Open");
                if(!LeftGatePort.IsOpen)
                    LeftGatePort.Open();

            }
            else
            {
                Console.WriteLine("Closing if open");

                if (LeftGatePort.IsOpen)
                    LeftGatePort.Close();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) //right gate
        {
            if (checkBox2.Checked)
            {

                Console.WriteLine("Right Gate Connection Open");
                if (!RightGatePort.IsOpen)
                    RightGatePort.Open();

            }
            else
            {
                Console.WriteLine("Closing if open");

                if (RightGatePort.IsOpen)
                    RightGatePort.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void cbDebug_CheckedChanged(object sender, EventArgs e)
        {
           
        }
    }
}
