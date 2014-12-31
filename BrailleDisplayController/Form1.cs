using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrailleDisplayController
{
    public partial class Form1 : Form
    {
        SerialPort port;
        byte[] packet;
        String[] brailleList;
        int numAlphabet = 26;
        int packetLength = 24;

        public Form1()
        {
            port = new SerialPort("COM4", 115200);
            port.Open();
            port.WriteTimeout = 2000;

            packet = new byte[packetLength];
            brailleList = new String[numAlphabet];
            initBrailleList();

            InitializeComponent();
        }

        private void initBrailleList()
        {
            brailleList[0] = "100000";
            brailleList[1] = "101000";
            brailleList[2] = "110000";
            brailleList[3] = "110100";
            brailleList[4] = "100100";
            brailleList[5] = "111000";
            brailleList[6] = "111100";
            brailleList[7] = "101100";
            brailleList[8] = "011000";
            brailleList[9] = "011100";
            brailleList[10] = "100010";
            brailleList[11] = "101010";
            brailleList[12] = "110010";
            brailleList[13] = "110110";
            brailleList[14] = "100110";
            brailleList[15] = "111010";
            brailleList[16] = "111110";
            brailleList[17] = "101110";
            brailleList[18] = "011010";
            brailleList[19] = "011110";
            brailleList[20] = "100011";
            brailleList[21] = "101011";
            brailleList[22] = "011101";
            brailleList[23] = "110011";
            brailleList[24] = "110111";
            brailleList[25] = "100111";
        }

        
        private byte[] textToPacket(String writtenText)
        {
            byte[] packet = new byte[packetLength];
            char[] writtenCharArray = writtenText.ToLower().ToCharArray();
            char writtenChar;
            String charBraille;
            int length = (writtenText.Length > 20)? 20:writtenText.Length;

            for (int charIndex = 0; charIndex < length; charIndex++)
            {
                writtenChar = writtenCharArray[charIndex];
                if (writtenChar >= 'a' && writtenChar <= 'z')
                    charBraille = brailleList[writtenChar - 'a'];
                else
                    charBraille = "000000";

                for(int j=0; j<3; j++){
                    if (!charBraille.Equals("000000"))
                        packet[ charIndex / 4 + 6 * j ] += (byte)(Math.Pow(2, 2*(3 - (charIndex % 4))) * Convert.ToByte(charBraille.Substring(j * 2, 2), 2));
                }
                /*
                // 0th char
                packet[0] += (byte)(Math.Pow(2,3) * Convert.ToByte(charBraille.Substring(0,2), 2));
                packet[5] += (byte)(Math.Pow(2,3) * Convert.ToByte(charBraille.Substring(2,2), 2));
                packet[10] += (byte)(Math.Pow(2,3) * Convert.ToByte(charBraille.Substring(4,2), 2));
                packet[15] += (byte)(Math.Pow(2,3) * Convert.ToByte(charBraille.Substring(6,2), 2));

                // 1st char
                packet[0] += (byte)(Math.Pow(2,2) * Convert.ToByte(charBraille.Substring(0,2), 2));
                packet[5] += (byte)(Math.Pow(2,2) * Convert.ToByte(charBraille.Substring(2,2), 2));
                packet[10] += (byte)(Math.Pow(2,2) * Convert.ToByte(charBraille.Substring(4,2), 2));
                packet[15] += (byte)(Math.Pow(2,2) * Convert.ToByte(charBraille.Substring(6,2), 2));

                // 4th char
                packet[1] += (byte)(Math.Pow(2,3) * Convert.ToByte(charBraille.Substring(0,2), 2));
                packet[6] += (byte)(Math.Pow(2,3) * Convert.ToByte(charBraille.Substring(2,2), 2));
                packet[11] += (byte)(Math.Pow(2,3) * Convert.ToByte(charBraille.Substring(4,2), 2));
                packet[16] += (byte)(Math.Pow(2,3) * Convert.ToByte(charBraille.Substring(6,2), 2));
                */
            }

            return packet;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            String writtenText = textBox1.Text;
            packet = textToPacket(writtenText);
            port.Write(packet, 0, packetLength);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            port.Close();
        }
    }
}

