using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace Osc_v5
{
    public partial class Form1 : Form
    {
        const int VSCALE = 10;
        const int TSCALE = 10;

        private class Number
        {
            public float fNumber; //base number is always stored in milli unit
            public string gStr(string Dim)
            {
                string Unit = ""; float Div = 0f; string Format = "";
                if (Math.Abs(fNumber) >= 1000) { Unit = Dim; Div = 0.001f; }
                if (Math.Abs(fNumber) < 1) { Unit = "u" + Dim; Div = 1000.0f; }
                if (Math.Abs(fNumber) >= 1 && Math.Abs(fNumber) < 1000) { Unit = "m" + Dim; Div = 1; }

                if (Math.Abs((Math.Abs(fNumber * Div) * 1000 - Math.Round(Math.Abs(fNumber * Div)) * 1000)) > 1) Format = "0:0.000"; else Format = "0:0";

                return String.Format(@"" + "{" + Format + Unit + @"" + "}", fNumber * Div);
            }
            public Number()
            {
                fNumber = 0;
            }
            public float gFlt(string Str)
            {
                if (Str.Length > 0)
                {
                    if (!Char.IsDigit(Str[Str.Length - 1]))
                    {
                        if (Str.Length > 2)
                        {
                            switch (Str.Substring(Str.Length - 3, 3))
                            {
                                case "bit":
                                    return 1.0f * float.Parse(Str.Substring(0, Str.Length - 3));
                                case "inf":
                                    return -999;
                            }
                        }
                        if (Str.Length > 1)
                        {
                            switch (Str.Substring(Str.Length - 2, 2))
                            {
                                case "mV":
                                    return float.Parse(Str.Substring(0, Str.Length - 2));
                                case "uV":
                                    return 0.001f * float.Parse(Str.Substring(0, Str.Length - 2));
                                case "us":
                                    return 0.001f * float.Parse(Str.Substring(0, Str.Length - 2));
                                case "ms":
                                    return float.Parse(Str.Substring(0, Str.Length - 2));
                                case "Hz":
                                    return 1.0f * float.Parse(Str.Substring(0, Str.Length - 2));
                                default:
                                    return 1000.0f * float.Parse(Str.Substring(0, Str.Length - 1));
                            }
                        }
                    }
                    else
                    {
                        return 1.0f * float.Parse(Str);
                    }

                }
                return 0.000f;
            }
        }

        private Label[] Lbl_V_Axis = new System.Windows.Forms.Label[VSCALE + 1];
        private Label[] Lbl_T_Axis = new System.Windows.Forms.Label[TSCALE + 1];

        private Number[] V_axis = new Number[VSCALE + 1];
        private Number[] T_axis = new Number[TSCALE + 1];
        private Number zeroVoltPoint = new Number();
        private Number VoltDiv = new Number();
        private Number TimeDiv = new Number();
        private Number ADC_VRef = new Number();
        private Number ADC_BitRes = new Number();
        private Number ADC_Clock = new Number();
        private Number ADC_Check_Pattern_Length = new Number();
        private Number ADC_Measure_Length = new Number();
        private Number ADC_Tick_Length = new Number();
        private Number ADC_Check_Pattern = new Number();

        private Number msSleep = new Number();
        private float[] Stats = new float[10]; //FPS;dfx;dpx;V_Interval

        const Int32 RINGBUFFER = 262144;
        UInt32 NumberOfSamples = 1024;
        UInt32 IsochTransferCount = 0;

        UInt32[] ADC_Reading = new UInt32[RINGBUFFER+1];
        UInt32[] ADC_Tick = new UInt32[RINGBUFFER+1];
        Int32 RB_U = 0;
        Int32 RB_L = 1 - (0 + 1); //RB_U=0
        Int32 RB_Size = RINGBUFFER;
        //int ADC_Res = 256;

        byte[] USB_readbuffer = new byte[RINGBUFFER*sizeof(Int32)/sizeof(byte)]; //Buffer will only be used up to sizeof which is dynamic
                    //Buffer size needs to be multiplied towards the size of the ADC_Reading buffer
        Int32 Sizeof_USB_readbuffer = 4096;
        private int[] USB_Results;// //memory allocated afte acquiring sizeof from DLL
        private long[] USB_hResults; //memory allocated afte acquiring sizeof from DLL
        UInt16[] Sizeof_USB_Result= new UInt16[1];
        UInt16[] Sizeof_USB_hResult = new UInt16[1];
        UInt16[] USB_Device_Vendor = new ushort[1];
        UInt16[] USB_Device_Product = new ushort[1];
        UInt16[] USB_Device_bcd = new ushort[1];


        bool OptionFilterOutliers = true;
        bool ContinueToCollectAndSendData = true;
        double TmrCollectAndSend=0;

        Task TskOpenGL;// = new Task(() => NativeMethods.OGL_Window_Init());
        Task TskCollectAndSendData;// = new Task(() => CollectAndSendData());
        Task TskUSB;
        //lblFPS.Text = FPS.ToString("00.0");
        public Form1()
        {
            InitializeComponent();
            U_InitializeComponent();
            TskOpenGL = new Task(() => NativeMethods.OGL_Window_Init());
            TskCollectAndSendData = new Task(() => CollectAndSendData());
            TskUSB = new Task(() => NativeMethods.USB_Init());
            Shown += Form1_Shown;
            Move += Form1_Move;
        }

        private void Form1_Move(Object sender, EventArgs e)
        {
            NativeMethods.OGL_Window_SetPos(this.Location.X + 9 + this.panel2.Location.X, this.Location.Y + 33 + this.panel2.Location.Y);
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            //Init OpenGL window - needs to be here as when window is disposed it clears this Init as well.
            TskOpenGL.Start();
        }

        private void SendTransformParams()
        {
            bool result = false;

            UInt16 _ADC_bitres; UInt16 _ADC_Vref; UInt16 _zeroVolt; UInt16 _VoltDiv; float _TimeDiv; UInt32 _ADC_Clock;
            bool optionExtrapolated; float _msSleep; bool optionFilterOutliers;

            if (ADC_BitRes.fNumber==0) { ADC_BitRes.fNumber = ADC_BitRes.gFlt(TxtADC_Res.Text); }
            if (ADC_VRef.fNumber == 0) { ADC_VRef.fNumber = ADC_VRef.gFlt(TxtADC_Vref.Text); }
            if (zeroVoltPoint.fNumber == 0) { zeroVoltPoint.fNumber= V_axis[(UInt16)(Lbl_V_Axis.GetUpperBound(0) + 1) / 2].fNumber; }
            if (VoltDiv.fNumber == 0) { VoltDiv.fNumber = VoltDiv.gFlt(this.CmbVoltDiv.Text); }
            if (TimeDiv.fNumber == 0) { TimeDiv.fNumber = TimeDiv.gFlt(CmbTimeDiv.SelectedItem.ToString()); }
            if (ADC_Clock.fNumber == 0) { ADC_Clock.fNumber = ADC_Clock.gFlt(TxtADC_Clock.Text); }
            if (msSleep.fNumber == 0) { msSleep.fNumber = msSleep.gFlt("inf"); }

            _ADC_bitres = (UInt16)ADC_BitRes.fNumber;
            _ADC_Vref = (UInt16) ADC_VRef.fNumber;
            _zeroVolt = (UInt16) zeroVoltPoint.fNumber;
            _VoltDiv = (UInt16) VoltDiv.fNumber;
            _TimeDiv = TimeDiv.fNumber;
            _ADC_Clock = (UInt32)ADC_Clock.fNumber;
            _msSleep = (float) msSleep.fNumber;

            optionExtrapolated = ChkoptionExtrapolated.Checked;
            optionFilterOutliers = ChkOptionFilterOutliers.Checked;

            result = NativeMethods.SendTransformParams(_ADC_bitres, _ADC_Vref, _zeroVolt, _VoltDiv, _TimeDiv, _ADC_Clock , optionExtrapolated, _msSleep, optionFilterOutliers) ;
            if (result == false)
            {
                var msgresult = MessageBox.Show("Could not update graph", "error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        private void CollectAndSendData()
        {
            Stopwatch Watch = new Stopwatch();
            if (NativeMethods.USB_ReadyToRead() == true)
            { //Keep loop going in waiting mode // Is a switch
                NativeMethods.USB_SetBeginReading(true); //Get USB in active mode

                while (ContinueToCollectAndSendData == true) //is a switch
                {
                    //COLLECT
                    if (NativeMethods.USB_GetEndReading() == true)
                    { // Wait for signal data is read from ADC, keep loop moving

                        RB_L = (RB_U + 1 > RB_Size) ? 1 : RB_U + 1; //r(RB_U + 1, RB_Size, 0);
                        RB_U = (RB_U + (Int32)NumberOfSamples > RB_Size) ? (Int32)NumberOfSamples : RB_U + (Int32)NumberOfSamples; //r(RB_L + NumberOfSamples - 1, RB_Size, 0);

                        NativeMethods.USB_ReadbufferTransfered(USB_readbuffer, ADC_Reading, ADC_Tick, RB_L,RB_U); //Start transferring data
                        NativeMethods.USB_SetReadbufferTransfered();
                        while (!NativeMethods.DataProcessed()) ;  // is a push //Wait for the data to have been displayed --> ASSUMES WE WANT TO ALWAYS DISPLAY ALL!!
                        NativeMethods.SetDataProcessed(false);
                        Watch.Start();

                        NativeMethods.SetSuspended(false);
                        while (!NativeMethods.SendData(ADC_Reading, ADC_Tick, RB_L, RB_U)) ; //Communication requires send and retrieval of succesfull retrieval --> proven necessary certainly at start up
                        NativeMethods.SetReadingLock(false);


                        Watch.Stop(); TmrCollectAndSend = (double)Watch.ElapsedTicks / Stopwatch.Frequency; //Measure idlde state
                    }
                }
            }
            else { NativeMethods.SetSuspended(true); }
        }
        private bool USB_ReportState()
        {
            while (!NativeMethods.USB_ReadyToReportState()) ;
            NativeMethods.USB_Sizeof_USB_Result(Sizeof_USB_Result, Sizeof_USB_hResult);
            USB_Results = new int[Sizeof_USB_Result[0]];
            USB_hResults = new long[Sizeof_USB_hResult[0]];

            NativeMethods.USB_GetState(USB_Results, USB_hResults, USB_Device_Vendor, USB_Device_Product, USB_Device_bcd);

            TxtUSB_State.Clear();
            if (Convert.ToString(USB_hResults[0], 2).Substring(0, 1) == "1")
            {  //Open Device
                if (USB_Results[18] == 1)
                {  //no Device 
                    TxtUSB_State.AppendText("Device not connected or driver not installed.");
                    goto Error;
                }
                else
                {
                    TxtUSB_State.AppendText("\r\n Failed looking for device, HRESULT 0x");
                    TxtUSB_State.AppendText(Convert.ToString(USB_hResults[0], 16));
                    goto Error;
                }
            } else
            {
                LblUSBConnected.Text = "Connected to device: VID_" + USB_Device_Vendor[0].ToString() + " PID_" + USB_Device_Product[0].ToString() + "; bcdUSB " + USB_Device_bcd[0].ToString();
                TxtUSB_State.AppendText("Device connected"); //TO DO add device name
            }
            if (USB_Results[1] == 0) { TxtUSB_State.AppendText("\r\n Could not get descriptor."); goto Error; }
            else { TxtUSB_State.AppendText("\r\n Device descriptor received");}
            if (USB_Results[2] == 0) { TxtUSB_State.AppendText("\r\n Could not get interface."); goto Error; }
            else { TxtUSB_State.AppendText("\r\n Device interface received"); }
            if (USB_Results[3] == 0) { TxtUSB_State.AppendText("\r\n Failed to get USB pipe. "); goto Error; }
            else { TxtUSB_State.AppendText("\r\n USB pipe identified"); }
            if (USB_Results[4] == 0) { TxtUSB_State.AppendText("\r\n Interval information or Maximum bytes interval invalid. "); goto Error; }
            else { TxtUSB_State.AppendText("\r\n Interval information received"); }
            //if (USB_Results[5] == 0) { TxtUSB_State.AppendText("\r\n Read Transfer failed. "); goto Error; }
            //else { TxtUSB_State.AppendText("\r\n Initiating read transfer"); } Depends on the starting of USB while USB starting is waiting for state (circular dep.)
            if (USB_Results[6] == 0) { TxtUSB_State.AppendText("\r\n Readbuffer not set at end of a frame. "); goto Error; }
            else { TxtUSB_State.AppendText("\r\n Readbuffer accepted"); }
            if (USB_Results[7] == 0) { TxtUSB_State.AppendText("\r\n Could not allocate memory. "); goto Error; }
            else { TxtUSB_State.AppendText("\r\n Memory allocated"); }
            if (USB_Results[8] == 0) { TxtUSB_State.AppendText("\r\n Could not register isoch buffer. "); goto Error; }
            else {   TxtUSB_State.AppendText("\r\n Isoch buffer registered"); }
            if (USB_Results[9] == 0) { TxtUSB_State.AppendText("\r\n Could not reset pipe. "); goto Error; }
            else { TxtUSB_State.AppendText("\r\n Pipe resetted"); }
            if (USB_Results[10] == 0) { TxtUSB_State.AppendText("\r\n Could not set overlapped result. "); goto Error; }
            else { TxtUSB_State.AppendText("\r\n Overlapped result set"); }
            if (USB_Results[11] == 0) { TxtUSB_State.AppendText("\r\n Could not get framenumber. "); goto Error; }
            else { TxtUSB_State.AppendText("\r\n Framenumber received"); }
            //if (USB_Results[12] == 0) { TxtUSB_State.AppendText("\r\n Could not read isoch pipe. "); goto Error; }
            //else { TxtUSB_State.AppendText("\r\n Isoch pipe read"); } Circular reference
            //if (USB_Results[13] == 0) { TxtUSB_State.AppendText("\r\n Could not get overlapped result. "); goto Error; }
            //else { TxtUSB_State.AppendText("\r\n Overlapped result received"); } Circular reference
            //if (USB_Results[14] == 0) { TxtUSB_State.AppendText("\r\n Could not complete transfer. "); goto Error; }
            //else { TxtUSB_State.AppendText("\r\n Transfer completed"); } Circular reference
            //if (USB_Results[15] == 0) { TxtUSB_State.AppendText("\r\n Could not unregister isoch buffer. "); goto Error; }
            //else { TxtUSB_State.AppendText("\r\n Isoch buffer unregistered"); } Circular reference
            if (USB_Results[16] == 0) { TxtUSB_State.AppendText("\r\n Could not find pipe. "); goto Error; }
            else {  TxtUSB_State.AppendText("\r\n USB Pipe found"); }
            //if (USB_Results[17] == 0) { TxtUSB_State.AppendText("\r\n Failed to start read. "); goto Error; }
            //else { TxtUSB_State.AppendText("\r\n Start reading"); } Circular reference
            return true;

        Error:
            return false;
        }

        private void U_InitializeComponent()
        {
            //Init VoltageDivider container
            VoltDiv.fNumber = 1000.0f;
            //Init VoltageAxis container
            for (int i = 0; i <= this.V_axis.GetUpperBound(0); i++) //initiatization of label text on load will not allow to overwrite
            {
                this.V_axis[i] = new Number();
                this.V_axis[i].fNumber = (i - this.V_axis.GetUpperBound(0) / 2) * VoltDiv.fNumber;
            }
            //Init VoltageAxis control
            for (int i = 0; i < this.Lbl_V_Axis.GetUpperBound(0) + 1; i++)
            {
                this.Lbl_V_Axis[i] = new Label();
                this.Lbl_V_Axis[i].AutoSize = true;
                this.Lbl_V_Axis[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.Lbl_V_Axis[i].ForeColor = System.Drawing.SystemColors.ActiveCaption;
                this.Lbl_V_Axis[i].Location = new System.Drawing.Point(753, -(i * 68) + 744);
                this.Lbl_V_Axis[i].Margin = new System.Windows.Forms.Padding(0);
                this.Lbl_V_Axis[i].Name = "Lbl_V_Axis[" + i + "]";
                this.Lbl_V_Axis[i].Size = new System.Drawing.Size(56, 13);
                this.Lbl_V_Axis[i].TabIndex = 23;
                this.Lbl_V_Axis[i].TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                if (i == this.Lbl_V_Axis.GetUpperBound(0) / 2)
                {
                    this.Lbl_V_Axis[i].Click += new System.EventHandler(this.ZeroVolt_Click);
                }
                this.panel1.Controls.Add(this.Lbl_V_Axis[i]);
                this.Lbl_V_Axis[i].Text = this.V_axis[i].gStr("V");
                this.Lbl_V_Axis[i].Show();
            }
            //Init TimeDivider container
            TimeDiv.fNumber = 1000.0f;
            //Init TimeAxis container
            for (int i = 0; i <= this.T_axis.GetUpperBound(0); i++) //initiatization of label text on load will not allow to overwrite
            {
                this.T_axis[i] = new Number();
                this.T_axis[i].fNumber = (i - this.T_axis.GetUpperBound(0) / 2) * TimeDiv.fNumber;
            }
            //Init TimeAxis control
            for (int i = 0; i < this.Lbl_T_Axis.GetUpperBound(0) + 1; i++)
            {
                this.Lbl_T_Axis[i] = new Label();
                this.Lbl_T_Axis[i].AutoSize = true;
                this.Lbl_T_Axis[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.Lbl_T_Axis[i].ForeColor = System.Drawing.SystemColors.ActiveCaption;
                this.Lbl_T_Axis[i].Location = new System.Drawing.Point(50 + (i * 68), 755);
                this.Lbl_T_Axis[i].Margin = new System.Windows.Forms.Padding(0);
                this.Lbl_T_Axis[i].Name = "Lbl_T_Axis[" + i + "]";
                this.Lbl_T_Axis[i].Size = new System.Drawing.Size(56, 13);
                this.Lbl_T_Axis[i].TabIndex = 23;
                this.Lbl_T_Axis[i].TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                this.panel1.Controls.Add(this.Lbl_T_Axis[i]);
                this.Lbl_T_Axis[i].Text = this.V_axis[i].gStr("s");
                this.Lbl_T_Axis[i].Show();
            }
            // Init VoltageDivider control
            this.CmbVoltDiv.Text = VoltDiv.gStr("V");
            //Init VoltageDivider control
            this.CmbTimeDiv.Text = TimeDiv.gStr("s");
            //Init PFS control
            this.CmbmsSleep.SelectedIndex = 0;
            //Init Ringbuffer control
            this.CmbRB_Size.Text = "1024";
            NumberOfSamples = (UInt32)Convert.ToInt32(CmbRB_Size.Text.Trim((char)8236), 10);
            RB_U = (Int32)NumberOfSamples - (Int32)NumberOfSamples; //Necessary to have at first run new data input on 0
            RB_L = 1 - (RB_U + 1);
            //Init ADC communication pattern
            TxtADCCheckPatternLength.Text = "7bit";
            ADC_Check_Pattern_Length.fNumber = ADC_Check_Pattern.gFlt(TxtADCCheckPatternLength.Text);
            TxtADCMeasureLength.Text = "12bit";
            ADC_Measure_Length.fNumber = ADC_Measure_Length.gFlt(TxtADCMeasureLength.Text);
            TxtADCTickLength.Text = "12bit";
            ADC_Tick_Length.fNumber = ADC_Tick_Length.gFlt(TxtADCTickLength.Text);
            TxtADCCheckPattern.Text = "055";
            ADC_Check_Pattern.fNumber = Convert.ToUInt32(TxtADCCheckPattern.Text, 16);
            TmrRefresh.Enabled = true;
            //Show Form
            this.Show();
        }
        private void ZeroVolt_Click(object sender, EventArgs e)
        {
            VoltDiv.fNumber = VoltDiv.gFlt(this.CmbVoltDiv.Text);
            for (int i = 0; i < this.Lbl_V_Axis.GetUpperBound(0) + 1; i++)
            {
                this.V_axis[i].fNumber = (i - this.Lbl_V_Axis.GetUpperBound(0) / 2) * VoltDiv.fNumber;
                this.Lbl_V_Axis[i].Text = this.V_axis[i].gStr("V");
            }
            zeroVoltPoint.fNumber = this.V_axis[(UInt16)(this.Lbl_V_Axis.GetUpperBound(0) + 1) / 2].fNumber;
            SendTransformParams();
        }
        private void SetToZero_Click(object sender, EventArgs e)
        {
            TxtZeroVolt.Visible = true;
            TxtZeroVolt.Focus();
        }
        private void ButVUp_Click(object sender, EventArgs e)
        {
            VoltDiv.fNumber = VoltDiv.gFlt(this.CmbVoltDiv.Text);
            float Offset = 10f;
            for (int i = 0; i < this.Lbl_V_Axis.GetUpperBound(0) + 1; i++)
            {
                this.V_axis[i].fNumber = this.V_axis[i].fNumber + Offset;
                this.Lbl_V_Axis[i].Text = this.V_axis[i].gStr("V");
            }
            zeroVoltPoint.fNumber = this.V_axis[(UInt16) (this.Lbl_V_Axis.GetUpperBound(0) + 1)/2].fNumber;
            SendTransformParams();
        }
        private void ButVDown_Click(object sender, EventArgs e)
        {
            VoltDiv.fNumber = VoltDiv.gFlt(this.CmbVoltDiv.Text);
            float Offset = -10f;
                for (int i = 0; i < this.Lbl_V_Axis.GetUpperBound(0) + 1; i++)
                {
                    this.V_axis[i].fNumber = this.V_axis[i].fNumber + Offset;
                    this.Lbl_V_Axis[i].Text = this.V_axis[i].gStr("V");
                }
            zeroVoltPoint.fNumber = this.V_axis[(UInt16)(this.Lbl_V_Axis.GetUpperBound(0) + 1) / 2].fNumber;
            SendTransformParams();
        }
        private void TxtZeroVolt_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)13: //Return
                    TxtZeroVolt.Visible = false;
                    
                    zeroVoltPoint.fNumber = zeroVoltPoint.gFlt(TxtZeroVolt.Text);
                    VoltDiv.fNumber = VoltDiv.gFlt(this.CmbVoltDiv.Text);
                    for (int i = 0; i < this.Lbl_V_Axis.GetUpperBound(0) + 1; i++)
                    {
                        this.V_axis[i].fNumber = (i - this.Lbl_V_Axis.GetUpperBound(0) / 2) * VoltDiv.fNumber+zeroVoltPoint.fNumber;
                        this.Lbl_V_Axis[i].Text = this.V_axis[i].gStr("V");
                    }
                    SendTransformParams();
                    break;
                case (char)8: //Backspace
                    break;
                default:
                    string Input = "";
                    for (int i = 0; i <= TxtZeroVolt.SelectionStart - 1; i++) Input += TxtZeroVolt.Text[i];
                    Input += e.KeyChar;
                    if (TxtZeroVolt.TextLength > 0) { for (int i = TxtZeroVolt.SelectionStart + TxtZeroVolt.SelectionLength; i <= TxtZeroVolt.TextLength - 1; i++) Input += TxtZeroVolt.Text[i]; }
                    if (Input[Input.Length - 1] != 'V') { Input += 'V'; }
                    //string allValidChar = "-+0123456789mV,.";// + (char)8 + (char)127;
                    string firstValidChar = "-+0123456789";// + (char)8 + (char)127;
                    string secondValidChar = "0123456789mV,.";
                    string bodyValidChar = "0123456789,.";
                    string secondlastValidChar = "0123456789,.m";
                    string lastValidChar = "mV";
                    char[] InputArr = Input.ToCharArray();
                    int Up = InputArr.GetUpperBound(0);
                    if (Up > -1 && firstValidChar.IndexOf(InputArr[0]) == -1) { InputArr[0] = '_'; }
                    if (Up > 0 && secondValidChar.IndexOf(InputArr[1]) == -1) { InputArr[1] = '_'; }
                    for (int i = 2; i <= Up - 2; i++) { if (bodyValidChar.IndexOf(InputArr[i]) == -1) { InputArr[i] = '_'; } }
                    if (Up > 0 && secondlastValidChar.IndexOf(InputArr[Up - 1]) == -1) { InputArr[Up - 1] = '_'; }
                    if (Up > 0 && lastValidChar.IndexOf(InputArr[Up]) == -1) { InputArr[Up] = '_'; }

                    int len = 0; for (int i = 0; i <= InputArr.GetUpperBound(0); i++) { if (InputArr[i] != '_') { len++; } }
                    char[] OutputArr = new char[len]; int y = 0;
                    for (int i = 0; i <= InputArr.GetUpperBound(0); i++)
                    {
                        if (InputArr[i] != '_') { OutputArr[y] = InputArr[i]; y++; }
                    }
                    //OutputArr[OutputArr.GetUpperBound(0)] = "\n";
                    this.TxtZeroVolt.Text = new string(OutputArr);
                    if (TxtZeroVolt.Text.Length > 0) { TxtZeroVolt.Select(TxtZeroVolt.Text.Length - 1, 0); }
                    e.Handled = true;
                    break;
            }

        }
        private void CmbVoltDiv_SelectedValueChanged(object sender, EventArgs e)
        {
            VoltDiv.fNumber = VoltDiv.gFlt(CmbVoltDiv.SelectedItem.ToString());
            for (int i = 0; i < this.Lbl_V_Axis.GetUpperBound(0) + 1; i++)
            {
                if (this.V_axis[i] == null) this.V_axis[i] = new Number();
                this.V_axis[i].fNumber = zeroVoltPoint.fNumber+(i - this.Lbl_V_Axis.GetUpperBound(0) / 2) * VoltDiv.fNumber;
                this.Lbl_V_Axis[i].Text = this.V_axis[i].gStr("V");
            }
            SendTransformParams();
        }
        private void CmbTimeDiv_SelectedValueChanged(object sender, EventArgs e)
        {
            TimeDiv.fNumber = TimeDiv.gFlt(CmbTimeDiv.SelectedItem.ToString());
            for (int i = 0; i < this.Lbl_T_Axis.GetUpperBound(0) + 1; i++)
            {
                if (this.T_axis[i] == null) this.T_axis[i] = new Number();
                this.T_axis[i].fNumber = 0 + (i - this.Lbl_T_Axis.GetUpperBound(0)) * TimeDiv.fNumber;
                this.Lbl_T_Axis[i].Text = this.T_axis[i].gStr("s");
            }
            SendTransformParams();
        }
        private void ButStartStopADC_Click(object sender, EventArgs e)
        {
            if (ButStartStopADC.Text == "Connect to device") {
                TskUSB.Start();
                if (USB_ReportState()==true)
                {
                    CmbRB_Size.Enabled = false;
                    TxtADCCheckPatternLength.Enabled = false;
                    TxtADCCheckPattern.Enabled = false;
                    TxtADCMeasureLength.Enabled = false;
                    TxtADCTickLength.Enabled = false;
                    ContinueToCollectAndSendData = true;
                    //Init USB params in front of CollectandSendData (cannt be integrated as multithreading exception would occur)
                    NativeMethods.USB_Set_Isoch_Transfer((UInt32)NumberOfSamples, (UInt32)ADC_Check_Pattern.fNumber, (UInt32)ADC_Check_Pattern_Length.fNumber, (UInt32)ADC_Measure_Length.fNumber, (UInt32)ADC_Tick_Length.fNumber);
                    IsochTransferCount = NativeMethods.USB_GetIsochTransferCount();
                    LblUSBTransfers.Text = IsochTransferCount.ToString(); //elevate the issue of multi threaded call because of compiler optimization
                    TskCollectAndSendData.Start();
                    
                    ButStartStopADC.Text = "Disconnect from device";
                } else
                {
                    NativeMethods.USB_SetCloseRequest();
                    TskUSB.Wait();
                    TskUSB.Dispose();
                    TskUSB = new Task(() => NativeMethods.USB_Init());
                }

            } else
            {
                
                NativeMethods.SetSuspended(true);
                ContinueToCollectAndSendData = false;
                NativeMethods.SetReadingLock(false);
                NativeMethods.GetStats(Stats);

                
                lblFPS.Text = Stats[0].ToString("#0.#ms");
                Lbldfx.Text = Stats[2].ToString("#0.####%");
                Lbldpx.Text = Stats[1].ToString("#0.##px");
                LblADC_Res_V.Text = Stats[3].ToString("#0.##mV");
                LblTmrBuildScreen.Text = Stats[4].ToString("#0.####ms");
                LblTmrSuspended.Text = Stats[5].ToString("#0.####ms");
                LblTmrDrawScreen.Text = Stats[6].ToString("#0.####ms");
                LblTmrTransfdata.Text = Stats[7].ToString("#0.####ms");
                LblTmrWaitfordata.Text = Stats[8].ToString("#0.####ms");
                LblKSamplePerSec.Text = Stats[9].ToString("#0.###kSps");


                TskCollectAndSendData.Wait();
                TskCollectAndSendData.Dispose();
                TskCollectAndSendData = new Task(() => CollectAndSendData());
                NativeMethods.USB_SetCloseRequest(); //first set closure request than give signal of Readbuffertransfered to allow closure
                NativeMethods.USB_SetReadbufferTransfered(); //to allow USB closure to continue in case not last transfer occured

                TskUSB.Wait();
                TskUSB.Dispose();
                TskUSB = new Task(() => NativeMethods.USB_Init());
                CmbRB_Size.Enabled = true;
                TxtADCCheckPatternLength.Enabled = true;
                TxtADCCheckPattern.Enabled = true;
                TxtADCMeasureLength.Enabled = true;
                TxtADCTickLength.Enabled = true;

                LblUSBConnected.Text = "Disconnected from device";
                ButStartStopADC.Text = "Connect to device";
            }
        }

        private void TxtADC_Clock_Leave(object sender, EventArgs e)
        {
            ADC_Clock.fNumber = ADC_Clock.gFlt(TxtADC_Clock.Text);

            SendTransformParams();
        }
        private void TxtADC_Vref_Leave(object sender, EventArgs e)
        {
            ADC_VRef.fNumber = ADC_VRef.gFlt(TxtADC_Vref.Text);
            SendTransformParams();
        }
        private void TxtADC_Res_Leave(object sender, EventArgs e)
        {
            ADC_BitRes.fNumber = ADC_BitRes.gFlt(TxtADC_Res.Text);
            SendTransformParams();
        }
        private void CmbRB_Size_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string TxtRB_size;
            TxtRB_size = CmbRB_Size.Text.Trim((char)8236);
            NumberOfSamples = (UInt32) Convert.ToInt32(TxtRB_size, 10);
            RB_U = (Int32) NumberOfSamples - (Int32) NumberOfSamples; //Necessary to have at first run new data input on 0
            RB_L = 1 - (RB_U + 1);
        }
        private void TxtADCCheckPatternLength_Leave(object sender, EventArgs e)
        {
            ADC_Check_Pattern_Length.fNumber = ADC_Check_Pattern.gFlt(TxtADCCheckPatternLength.Text);
        }
        private void TxtADCMeasureLength_Leave(object sender, EventArgs e)
        {
            ADC_Measure_Length.fNumber = ADC_Measure_Length.gFlt(TxtADCMeasureLength.Text);
            if (ADC_Measure_Length.fNumber < ADC_BitRes.fNumber)
            {
                MessageBox.Show("ADC resolution higher than bit string returned from device", "Communication error setting", MessageBoxButtons.OK,MessageBoxIcon.Error);
                TxtADCMeasureLength.Text = "12bit";
                ADC_Measure_Length.fNumber = ADC_Measure_Length.gFlt(TxtADCMeasureLength.Text);
            }
            ADC_Measure_Length.fNumber = ADC_Measure_Length.gFlt(TxtADCMeasureLength.Text);
        }
        private void TxtADCTickLength_Leave(object sender, EventArgs e)
        {
            ADC_Tick_Length.fNumber = ADC_Tick_Length.gFlt(TxtADCTickLength.Text);
        }
        private void TxtADCCheckPattern_Leave(object sender, EventArgs e)
        {
            try
            {
                ADC_Check_Pattern.fNumber = Convert.ToUInt32(TxtADCCheckPattern.Text, 16);
            }
            catch (System.FormatException)
            {
                TxtADCCheckPattern.Text = "055"; 
            }
        }
        private void ChkoptionExtrapolated_CheckedChanged(object sender, EventArgs e)
        {
            SendTransformParams();
        }

        private void CmbmsSleep_SelectedIndexChanged(object sender, EventArgs e)
        {
            msSleep.fNumber = msSleep.gFlt(CmbmsSleep.SelectedItem.ToString()); //Entry is in FPS to be converted to ms!
            msSleep.fNumber = (msSleep.fNumber==-999)? -999: 1000.0f / msSleep.fNumber;
            SendTransformParams();
        }

        private void ChkOptionFilterOutliers_CheckedChanged(object sender, EventArgs e)
        {
            OptionFilterOutliers = ChkOptionFilterOutliers.Checked;
            SendTransformParams();
        }

        private void TmrRefresh_Tick(object sender, EventArgs e)
        {
            NativeMethods.GetStats(Stats);

            lblFPS.Text = Stats[0].ToString("#0.#ms");
            Lbldfx.Text = Stats[2].ToString("#0.####%");
            Lbldpx.Text = Stats[1].ToString("#0.##px");
            LblADC_Res_V.Text = Stats[3].ToString("#0.##mV");
            LblTmrBuildScreen.Text = Stats[4].ToString("#0.####ms");
            LblTmrSuspended.Text = Stats[5].ToString("#0.####ms");
            LblTmrDrawScreen.Text = Stats[6].ToString("#0.####ms");
            LblTmrTransfdata.Text = Stats[7].ToString("#0.####ms");
            LblTmrWaitfordata.Text = Stats[8].ToString("#0.####ms");
            LblKSamplePerSec.Text = Stats[9].ToString("#0.###kSps");

        }

        private void CmbStatRefresh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbStatRefresh.Text == "Never")
            {
                TmrRefresh.Enabled = false;
            } else
            {
                TmrRefresh.Interval = Convert.ToInt32(CmbStatRefresh.Text);
            }
        }
    }
}
