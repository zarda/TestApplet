using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

/**
 * Simple Example for the usage of SiSo C# SDK
 */
namespace Examples
{
    /**
     * Customized Subclass of fg_apc_data to use 
     */

    class MyApcData : fg_apc_data
    {
        public Fg_Struct fg;
        public uint port;
        public dma_mem mem;
        public uint displayid;

        public MyApcData(Fg_Struct fg, uint port, dma_mem mem, uint displayid)
            : base()
        {
            this.fg = fg;
            this.port = port;
            this.mem = mem;
            this.displayid = displayid;
        }
    }

    class AcqAPC
    {
        public static int getNrOfBoards()
        {
            int nrOfBoards = 0;
            byte[] buffer = new byte[256];
            uint buflen = 256;
            buffer[0] = 0;

            if (SiSoCsRt.Fg_getSystemInformation(null, Fg_Info_Selector.INFO_NR_OF_BOARDS, FgProperty.PROP_ID_VALUE, 0, buffer, ref buflen) == SiSoCsRt.FG_OK)
            {
                nrOfBoards = int.Parse(Encoding.ASCII.GetString(buffer));
            }
            return nrOfBoards;
        }

        public static int selectBoardDialog()
        {
            int boardType;
            int i = 0;

            int maxNrOfboards = 10;
            int nrOfBoardsFound = 0;
            int nrOfBoardsPresent = getNrOfBoards();
            int maxBoardIndex = -1;

            for (i = 0; i < maxNrOfboards; i++)
            {
                string boardName;
                bool skipIndex = false;
                boardType = SiSoCsRt.Fg_getBoardType(i);
                switch ((siso_board_type)(boardType))
                {
                    case siso_board_type.PN_MICROENABLE4AS1CL:
                        boardName = "microEnable IV AS1-CL";
                        break;
                    case siso_board_type.PN_MICROENABLE4AD1CL:
                        boardName = "microEnable IV AD1-CL";
                        break;
                    case siso_board_type.PN_MICROENABLE4VD1CL:
                        boardName = "microEnable IV VD1-CL";
                        break;
                    case siso_board_type.PN_MICROENABLE4AD4CL:
                        boardName = "microEnable IV AD4-CL";
                        break;
                    case siso_board_type.PN_MICROENABLE4VD4CL:
                        boardName = "microEnable IV VD4-CL";
                        break;
                    case siso_board_type.PN_MICROENABLE4AQ4GE:
                        boardName = "microEnable IV AQ4-GE";
                        break;
                    case siso_board_type.PN_MICROENABLE4VQ4GE:
                        boardName = "microEnable IV VQ4-GE";
                        break;

                    case siso_board_type.PN_MICROENABLE5AQ8CXP6B:
                        boardName = "microEnable 5 ironman AQ8-CXP";
                        break;
                    case siso_board_type.PN_MICROENABLE5VQ8CXP6B:
                        boardName = "microEnable 5 ironman VQ8-CXP";
                        break;
                    case siso_board_type.PN_MICROENABLE5A1CLHSF2:
                        boardName = "microEnable 5 ironman A1-CLHS-F2";
                        break;
                    case siso_board_type.PN_MICROENABLE5A1CXP4:
                        boardName = "microEnable 5 ironman A1-CXP";
                        break;
                    case siso_board_type.PN_MICROENABLE5VD8CL:
                        boardName = "microEnable 5 ironman VD8-CL";
                        break;
                    case siso_board_type.PN_MICROENABLE5AD8CL:
                        boardName = "microEnable 5 ironman AD8-CL";
                        break;
                    case siso_board_type.PN_MICROENABLE5AQ8CXP6D:
                        boardName = "microEnable 5 ironman AQ8-CXP6D";
                        break;
                    case siso_board_type.PN_MICROENABLE5VQ8CXP6D:
                        boardName = "microEnable 5 ironman VQ8-CXP6D";
                        break;
                    case siso_board_type.PN_MICROENABLE5AD8CLHSF2:
                        boardName = "microEnable 5 ironman AD8-CLHS-F2";
                        break;

                    case siso_board_type.PN_MICROENABLE5_LIGHTBRIDGE_VCL:
                        boardName = "LightBridge VCL";
                        break;
                    case siso_board_type.PN_MICROENABLE5_LIGHTBRIDGE_ACL:
                        boardName = "LightBridge ACL";
                        break;
                    case siso_board_type.PN_MICROENABLE5_MARATHON_ACL:
                        boardName = "microEnable 5 marathon ACL";
                        break;
                    case siso_board_type.PN_MICROENABLE5_MARATHON_VCL:
                        boardName = "microEnable 5 marathon VCL";
                        break;
                    case siso_board_type.PN_MICROENABLE5_MARATHON_ACX_SP:
                        boardName = "microEnable 5 marathon ACX SP";
                        break;
                    case siso_board_type.PN_MICROENABLE5_MARATHON_ACX_DP:
                        boardName = "microEnable 5 marathon ACX DP";
                        break;
                    case siso_board_type.PN_MICROENABLE5_MARATHON_VCX_QP:
                        boardName = "microEnable 5 marathon VCX QP";
                        break;
                    case siso_board_type.PN_MICROENABLE5_MARATHON_VF2_DP:
                        boardName = "microEnable 5 marathon VF2";
                        break;
                    default:
                        boardName = "Unknown / Unsupported Board";
                        skipIndex = true;
                        break;
                }
                if (!skipIndex)
                {
                    Console.WriteLine("Board ID {0}: {1} 0x{2:X}", i, boardName, boardType);
                    nrOfBoardsFound++;
                    maxBoardIndex = i;
                }
                if (nrOfBoardsFound >= nrOfBoardsPresent)
                {
                    break;// all boards are scanned
                }
            }


            if (nrOfBoardsFound <= 0)
            {
                Console.Write("No Boards found!");
                return -1;
            }

            Console.Write("\n=====================================\n\nPlease choose a board[0-{0}]: ", maxBoardIndex);
            //fflush(stdout);
            int userInput = -1;
            do
            {
                string inputBuffer = Console.ReadLine();
                userInput = int.Parse(inputBuffer);
                if (userInput > maxBoardIndex)
                {
                    Console.Write("Invalid selection, retry[0-{0}]: ", maxBoardIndex);
                }
            } while (userInput > maxBoardIndex);

            return userInput;
        }

        /**
         * APC Callback function.
         * Must have same signature as SiSoCallback.Fg_ApcFuncDelegate
         */
        static int apcCallback(uint imgNr, fg_apc_data userData)
        {
            MyApcData data = (MyApcData)userData;

            SiSoCsRt.DrawBuffer((int)(data.displayid), SiSoCsRt.Fg_getImagePtrEx(data.fg, (int)(imgNr), data.port, data.mem), (int)(imgNr), "");

            return 0;
        }

        static void Main(string[] args)
        {
            Fg_Struct fg = null;
            int boardId = selectBoardDialog();
            uint camPort = (uint)(SiSoCsRt.PORT_A);
            int nrOfPicturesToGrab = 1000;
            int nbBuffers = 4;
            uint width = 512;
            uint height = 512;
            int samplePerPixel = 1;
            uint bytePerSample = 1;
            bool isSlave = false;
            bool useCameraSimulator = true;

            if (boardId < 0)
            {
                return;
            }

            string applet;
            switch ((siso_board_type)(SiSoCsRt.Fg_getBoardType(boardId)))
            {
                case siso_board_type.PN_MICROENABLE4AS1CL:
                    applet = "SingleAreaGray16";
                    break;
                case siso_board_type.PN_MICROENABLE4AD1CL:
                case siso_board_type.PN_MICROENABLE4AD4CL:
                case siso_board_type.PN_MICROENABLE4VD1CL:
                case siso_board_type.PN_MICROENABLE4VD4CL:
                    applet = "DualAreaGray16";
                    break;
                case siso_board_type.PN_MICROENABLE4AQ4GE:
                case siso_board_type.PN_MICROENABLE4VQ4GE:
                    applet = "QuadAreaGray16";
                    break;
                default:
                    if (SiSoCsRt.Fg_findApplet((uint)(boardId), out applet) != 0)
                    {
                        Console.WriteLine("No applet is found\n");
                        return;
                    }
                    break;
            }

            fg = SiSoCsRt.Fg_InitEx(applet, (uint)(boardId), isSlave ? 1 : 0);

            if (fg == null)
            {
                Console.WriteLine("error in Fg_Init: {0}\n", SiSoCsRt.Fg_getLastErrorDescription(null));
                return;
            }
            else
            {
                Console.WriteLine("Fg_InitEx Success \n");
            }

            uint imageSize = (uint)(bytePerSample * samplePerPixel * width * height);

            // Create Display
            int dispId0 = SiSoCsRt.CreateDisplay((uint)(8 * bytePerSample * samplePerPixel), width, height);
            SiSoCsRt.SetBufferWidth(dispId0, width, height);

            // Allocate DMA
            uint totalBufferSize = (uint)(width * height * samplePerPixel * bytePerSample * nbBuffers);
            dma_mem memHandle = SiSoCsRt.Fg_AllocMemEx(fg, totalBufferSize, nbBuffers);
            if (memHandle == null)
            {
                Console.WriteLine("error in Fg_AllocMemEx: {0}\n", SiSoCsRt.Fg_getLastErrorDescription(fg));
                SiSoCsRt.CloseDisplay(dispId0);
                SiSoCsRt.Fg_FreeGrabber(fg);
                return;
            }

            // Set Applet Parameters
            /* Image width of the acquisition window. */
            if (SiSoCsRt.Fg_setParameterWithUInt(fg, SiSoCsRt.FG_WIDTH, width, camPort) < 0)
            {
                Console.WriteLine("Fg_setParameter(FG_WIDTH) failed: {0}\n", SiSoCsRt.Fg_getLastErrorDescription(fg));
                SiSoCsRt.Fg_FreeMemEx(fg, memHandle);
                SiSoCsRt.CloseDisplay(dispId0);
                SiSoCsRt.Fg_FreeGrabber(fg);
                return;
            }

            /* Image height of the acquisition window. */
            if (SiSoCsRt.Fg_setParameterWithUInt(fg, SiSoCsRt.FG_HEIGHT, height, camPort) < 0)
            {
                Console.WriteLine("Fg_setParameter(FG_HEIGHT) failed: {0}\n", SiSoCsRt.Fg_getLastErrorDescription(fg));
                SiSoCsRt.Fg_FreeMemEx(fg, memHandle);
                SiSoCsRt.CloseDisplay(dispId0);
                SiSoCsRt.Fg_FreeGrabber(fg);
                return;
            }

            int bitAlignment = SiSoCsRt.FG_LEFT_ALIGNED;
            if (SiSoCsRt.Fg_setParameterWithInt(fg, SiSoCsRt.FG_BITALIGNMENT, bitAlignment, camPort) < 0)
            {
                Console.WriteLine("Fg_setParameter(FG_BITALIGNMENT) failed: {0}\n", SiSoCsRt.Fg_getLastErrorDescription(fg));
                //SiSoCsRt.Fg_FreeMemEx(fg, memHandle);
                //SiSoCsRt.CloseDisplay(dispId0);
                //SiSoCsRt.Fg_FreeGrabber(fg);
                //return;
            }

            if (useCameraSimulator)
            {
                if (SiSoCsRt.Fg_setParameterWithInt(fg, SiSoCsRt.FG_GEN_ENABLE, (int)(FgImageSourceTypes.FG_GENERATOR), camPort) < 0)
                {
                    Console.WriteLine("Fg_setParameter(FG_GEN_ENABLE) failed: {0}\n", SiSoCsRt.Fg_getLastErrorDescription(fg));
                    SiSoCsRt.Fg_FreeMemEx(fg, memHandle);
                    SiSoCsRt.CloseDisplay(dispId0);
                    SiSoCsRt.Fg_FreeGrabber(fg);
                    return;
                }
            }

            /* Reading back parameters */
            {
                int oWidth = 0;
                int oHeight = 0;
                int oBitAlignment = 0;
                int oGenEnabled = 0;
                string oString = "";
                if (SiSoCsRt.Fg_getParameterWithInt(fg, SiSoCsRt.FG_WIDTH, out oWidth, camPort) == 0)
                {
                    Console.WriteLine("Width = {0}", oWidth);
                }
                if (SiSoCsRt.Fg_getParameterWithInt(fg, SiSoCsRt.FG_HEIGHT, out oHeight, camPort) == 0)
                {
                    Console.WriteLine("Height = {0}", oHeight);
                }
                if (SiSoCsRt.Fg_getParameterWithString(fg, SiSoCsRt.FG_HAP_FILE, out oString, camPort) == 0)
                {
                    Console.WriteLine("Hap File = {0}", oString);
                }
                if (SiSoCsRt.Fg_getParameterWithInt(fg, SiSoCsRt.FG_BITALIGNMENT, out oBitAlignment, camPort) == 0)
                {
                    string align;
                    if (oBitAlignment == SiSoCsRt.FG_LEFT_ALIGNED)
                    {
                        align = "Left Aligned";
                    }
                    else if (oBitAlignment == SiSoCsRt.FG_RIGHT_ALIGNED)
                    {
                        align = "Right Aligned";
                    }
                    else
                    {
                        align = "Unknown";
                    }
                    Console.WriteLine("Bit Alignment = {0}", align);
                }
                if (SiSoCsRt.Fg_getParameterWithInt(fg, SiSoCsRt.FG_GEN_ENABLE, out oGenEnabled, camPort) == 0)
                {
                    string gen;
                    if (oGenEnabled == (int)(FgImageSourceTypes.FG_GENERATOR))
                    {
                        gen = "enabled";
                    }
                    else
                    {
                        gen = "disabled";
                    }
                    Console.WriteLine("Generator: {0}", gen);
                }
            }


            /* Register acquisition call back */
            // Define FgApcControl instance to handle the callback
            FgApcControl apcCtrl = new FgApcControl(5, (uint)(Fg_Apc_Flag.FG_APC_DEFAULTS));
            fg_apc_data gApcData = new MyApcData(fg, (uint)(camPort), memHandle, (uint)(dispId0));
            apcCtrl.setApcCallbackFunction(apcCallback, gApcData);

            // Register the FgApcControl instance to the Fg_Struct instance
            int result = SiSoCsRt.Fg_registerApcHandler(fg, camPort, apcCtrl, FgApcControlFlags.FG_APC_CONTROL_BASIC);
            if (result != SiSoCsRt.FG_OK)
            {
                Console.WriteLine("registering APC handler failed: {0}\n", SiSoCsRt.Fg_getErrorDescription(result));
                SiSoCsRt.Fg_FreeMemEx(fg, memHandle);
                SiSoCsRt.CloseDisplay(dispId0);
                SiSoCsRt.Fg_FreeGrabber(fg);
                return;
            }

            // Start acquisition
            if ((SiSoCsRt.Fg_AcquireEx(fg, camPort, nrOfPicturesToGrab, SiSoCsRt.ACQ_STANDARD, memHandle)) < 0)
            {
                Console.WriteLine("Fg_AcquireEx() failed: {0}\n", SiSoCsRt.Fg_getLastErrorDescription(fg));
                SiSoCsRt.Fg_FreeMemEx(fg, memHandle);
                SiSoCsRt.CloseDisplay(dispId0);
                SiSoCsRt.Fg_FreeGrabber(fg);
                return;
            }

            Thread.Sleep(50000);
            SiSoCsRt.CloseDisplay(dispId0);

            // Clean up
            if (fg != null)
            {
                SiSoCsRt.Fg_registerApcHandler(fg, camPort, null, FgApcControlFlags.FG_APC_CONTROL_BASIC);
                SiSoCsRt.Fg_stopAcquire(fg, camPort);
                SiSoCsRt.Fg_FreeMemEx(fg, memHandle);
                SiSoCsRt.Fg_FreeGrabber(fg);
            }

            Console.WriteLine("Exited.\n");
        }
    }
}
