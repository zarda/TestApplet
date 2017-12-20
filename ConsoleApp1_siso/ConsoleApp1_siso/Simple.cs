using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp1_siso
{
    public partial class Simple : Form
    {
        public Simple()
        {
            InitializeComponent();
            InitializeFG();
            CheckCardState();
        }

        private void CheckCardState()
        {
            
            foreach (var board in boardType)
            {
                richTextBox.Text += "Board Type: " +
                        ((siso_board_type)board).ToString() + "\n";
                richTextBox.Text += "Board Name: " +
                    SiSoCsRt.Fg_getBoardNameByType(board, 0) + "\n";
                richTextBox.Text += "Software Version: " +
                    SiSoCsRt.Fg_getSWVersion() + "\n";
            }

            foreach (var fg in FG)
            {
                richTextBox.Text += "Series number: " +
                            SiSoCsRt.Fg_getSerialNumber(fg) + "\n";

                for (int i = 0; i < 82; i++)
                {
                    var paraName = SiSoCsRt.Fg_getParameterName(fg, i);
                    string value = string.Empty;
                    var getPara = SiSoCsRt.Fg_getParameterWithString(fg, SiSoCsRt.Fg_getParameterId(fg, i), out value, camPort);
                    richTextBox.Text += $"Parameter {i + 1} {SiSoCsRt.Fg_getErrorDescription(getPara)} :  " +
                                     paraName + " = " + value +
                                    "\n";
                }  
            }
        
            
        }

        private List<Fg_Struct> FG = null;
        private int boardNumber = 0;
        private List<int> boardId = new List<int>();
        private List<int> boardType = new List<int>();
        private uint camPort = (uint)(SiSoCsRt.PORT_A);
        private void InitializeFG()
        {
            try
            {
                boardNumber = getNrOfBoards();
                boardId.Clear();
                boardType.Clear();
                for (int i = 0; i < boardNumber; i++)
                {
                    boardId.Add(i);
                }
                foreach (var id in boardId)
                {
                    boardType.Add(SiSoCsRt.Fg_getBoardType(id));                    
                }
                FG.Add(SiSoCsRt.Fg_Init("QuadAreaGray16", 0));
                FG.Add(SiSoCsRt.Fg_Init("FrameGrabberTest", 0));

                richTextBox.Text += "Siso ready." + "\n\r";
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        private void Simple_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Clean up
            foreach (var fg in FG)
            {
                if (fg != null)
                {
                    //SiSoCsRt.Fg_stopAcquire(fg, camPort);
                    //SiSoCsRt.Fg_FreeMemEx(fg, memHandle);
                    SiSoCsRt.Fg_FreeGrabber(fg);
                } 
            }
        }
        public static int getNrOfBoards()
        {
            int nrOfBoards = 0;
            byte[] buffer = new byte[256];
            uint buflen = 256;
            buffer[0] = 0;

            if (SiSoCsRt.FG_OK == SiSoCsRt.Fg_getSystemInformation(
                    null, 
                    Fg_Info_Selector.INFO_NR_OF_BOARDS, 
                    FgProperty.PROP_ID_VALUE, 
                    0, 
                    buffer, 
                    ref buflen))
            {
                nrOfBoards = int.Parse(Encoding.ASCII.GetString(buffer));
            }
            return nrOfBoards;
        }
    }
}
