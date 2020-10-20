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
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Security.Policy;


namespace Osc_v5
{
    public partial class Form1 : Form
    {
        const int VSCALE = 10;
        const int TSCALE = 10;

        private class Number
        {
            private float fNumber; //base number is always stored in milli unit

            public float FNumber
            {
                get
                {
                    return fNumber;
                }
                set
                {
                    fNumber = value;
                    if (this.TransformParamsChange != null)
                        this.TransformParamsChange(this, new EventArgs());
                }
            }

            public EventHandler TransformParamsChange;
            public byte Id;


            public string gStr(string Dim)
            {
                string Unit = ""; float Div = 0f; string Format = "";
                if (Math.Abs(FNumber) >= 1000) { Unit = Dim; Div = 0.001f; }
                if (Math.Abs(FNumber) < 1) { Unit = "u" + Dim; Div = 1000.0f; }
                if (Math.Abs(FNumber) >= 1 && Math.Abs(FNumber) < 1000) { Unit = "m" + Dim; Div = 1; }

                if (Math.Abs((Math.Abs(FNumber * Div) * 1000 - Math.Round(Math.Abs(FNumber * Div)) * 1000)) > 1) Format = "0:0.000"; else Format = "0:0";

                return String.Format(@"" + "{" + Format + Unit + @"" + "}", FNumber * Div);
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
        private Number ZeroVoltPoint = new Number();
        private Number ZeroTimePoint = new Number();
        private Number VoltDiv = new Number();
        private Number TimeDiv = new Number();
        private Number ADC_VRef = new Number();
        private Number ADC_BitRes = new Number();
        private Number ADC_Clock = new Number();

        private Number msSleep = new Number();
        private float[] Stats = new float[10]; //FPS;dfx;dpx;V_Interval


        UInt16[] Sizeof_USB_Result = new UInt16[1];
        UInt16[] Sizeof_USB_hResult = new UInt16[1];
        UInt16[] USB_Device_Vendor = new ushort[1];
        UInt16[] USB_Device_Product = new ushort[1];
        UInt16[] USB_Device_bcd = new ushort[1];

        public byte USB2_Mode = 1;
        public byte USB2_TriggerSlope = 1;
        bool OptionFilterOutliers = true;

        public Form1()
        {
            InitializeComponent();
            U_InitializeComponent();

            Shown += Form1_Shown;
            Move += Form1_Move;
        }

        private void Form1_Move(Object sender, EventArgs e)
        {
            //NativeMethods.OGL_Window_SetPos(this.Location.X + 9 + this.panel2.Location.X, this.Location.Y + 33 + this.panel2.Location.Y);
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            //Init OpenGL window - needs to be here as when window is disposed it clears this Init as well.
            Task TskOpenGL = Task.Run(() =>
            {
                USB2_VertexBuffer[0] = new float[USB2_Win_W * 3 * 2]; //actual data
                USB2_VertexBuffer[1] = new float[USB2_Win_W * 3 * 2]; //Number of measurements
                USB2_VertexBuffer[2] = new float[USB2_Win_W * 3 * 2]; //Average of elements
                USB2_VertexBuffer[3] = new float[(UInt32)Math.Ceiling((double)(USB2_SecondsOfBuffer * 1000 / USB2_MillisecondsPerTransfer * USB2_NumberOfPackets * USB2_TransferSize / 2 * 2))]; //Average of elements

                Array.Clear(USB2_VertexBuffer[0], 0, USB2_VertexBuffer[0].Length);
                Array.Clear(USB2_VertexBuffer[1], 0, USB2_VertexBuffer[1].Length);
                Array.Clear(USB2_VertexBuffer[2], 0, USB2_VertexBuffer[2].Length);
                Array.Clear(USB2_VertexBuffer[3], 0, USB2_VertexBuffer[3].Length);

                Array.Clear(USB2_VertexBufferTrigger, 0, USB2_VertexBufferTrigger.Length);

                OGL_Screen_Status.i2_ConnectPtrs();
                OGL_Screen_Status.i2_SetAll(0,this);
                OGL_Screen_Status.i2_Set(1, OGL_Screen_Status.i2_USB2_OGL_Suspended, ChkListOGLSuspended, 0);
                OGL_Screen_Status.i2_Set(USB2_VertexBuffer[3].Length, OGL_Screen_Status.i2_USB2_OGL_LastDataPosition, null, 0);

                NativeMethods.OGL_Window_Init(USB2_Win_W, USB2_VertexBuffer[3], (UInt32)USB2_VertexBuffer[3].Length, USB2_VertexBufferTrigger, (UInt32) USB2_VertexBufferTrigger.Length);

            });

        }

         private void U_InitializeComponent()
        {

            //Init transform paramaters

            ZeroVoltPoint.TransformParamsChange += this.SetTransformParams;
            VoltDiv.TransformParamsChange += this.SetTransformParams;
            TimeDiv.TransformParamsChange += this.SetTransformParams;
            ADC_VRef.TransformParamsChange += this.SetTransformParams;
            ADC_BitRes.TransformParamsChange += this.SetTransformParams;
            ADC_Clock.TransformParamsChange += this.SetTransformParams;

            ZeroVoltPoint.Id = 1;
            VoltDiv.Id = 2;
            TimeDiv.Id = 3;
            ADC_VRef.Id = 4;
            ADC_BitRes.Id = 5;
            ADC_Clock.Id = 6;

            //Init VoltageDivider container
            VoltDiv.FNumber = 1000.0f;
            //Init VoltageAxis container
            for (int i = 0; i <= this.V_axis.GetUpperBound(0); i++) //initiatization of label text on load will not allow to overwrite
            {
                this.V_axis[i] = new Number();
                this.V_axis[i].FNumber = (i - this.V_axis.GetUpperBound(0) / 2) * VoltDiv.FNumber;
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
                this.Lbl_V_Axis[i].BringToFront();
                this.Lbl_V_Axis[i].Show();
            }
            //Init TimeDivider container
            TimeDiv.FNumber = 1000.0f;
            //Init TimeAxis container
            for (int i = 0; i < this.Lbl_T_Axis.GetUpperBound(0) + 1; i++)
            {
                if (this.T_axis[i] == null) this.T_axis[i] = new Number();
                this.T_axis[i].FNumber = 0 + (i - this.Lbl_T_Axis.GetUpperBound(0)) * TimeDiv.FNumber;
            }
            ZeroTimePoint.FNumber = this.T_axis[(UInt16)(this.Lbl_T_Axis.GetUpperBound(0) + 1) / 2].FNumber;
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
                this.Lbl_T_Axis[i].Text = this.T_axis[i].gStr("s");
                this.Lbl_T_Axis[i].BringToFront();
                this.Lbl_T_Axis[i].Show();
            }
            // Init VoltageDivider control
            this.CmbVoltDiv.Text = VoltDiv.gStr("V");
            //Init VoltageDivider control
            this.CmbTimeDiv.Text = TimeDiv.gStr("s");
            //Init PFS control
            this.CmbmsSleep.SelectedIndex = 0;
            //Init Bit resolution control
            ADC_BitRes.FNumber = ADC_BitRes.gFlt(TxtADC_Res.Text);
            //Init ADC clock control
            ADC_Clock.FNumber = ADC_Clock.gFlt(TxtADC_Clock.Text);
            //ADC_Vref control
            ADC_VRef.FNumber = ADC_VRef.gFlt(TxtADC_Vref.Text);
            //Zero Volt point number
            ZeroVoltPoint.FNumber = V_axis[(UInt16)(Lbl_V_Axis.GetUpperBound(0) + 1) / 2].FNumber;

            //USB_Mode default
            USB2_Mode = 1;
            //Trigger slope default
            RdButTrSlopeDown.Checked = true;

            TmrRefresh.Enabled = true;
            //Show Form
            this.Show();
        }
        private void ZeroVolt_Click(object sender, EventArgs e)
        {
            VoltDiv.FNumber = VoltDiv.gFlt(this.CmbVoltDiv.Text);
            for (int i = 0; i < this.Lbl_V_Axis.GetUpperBound(0) + 1; i++)
            {
                this.V_axis[i].FNumber = (i - this.Lbl_V_Axis.GetUpperBound(0) / 2) * VoltDiv.FNumber;
                this.Lbl_V_Axis[i].Text = this.V_axis[i].gStr("V");
            }
            ZeroVoltPoint.FNumber = this.V_axis[(UInt16)(this.Lbl_V_Axis.GetUpperBound(0) + 1) / 2].FNumber;

        }
        private void SetToZero_Click(object sender, EventArgs e)
        {
            TxtZeroVolt.Visible = true;
            TxtZeroVolt.Focus();
        }
        private void ButVUp_Click(object sender, EventArgs e)
        {
            VoltDiv.FNumber = VoltDiv.gFlt(this.CmbVoltDiv.Text);
            float Offset = VoltDiv.FNumber;//10f;
            for (int i = 0; i < this.Lbl_V_Axis.GetUpperBound(0) + 1; i++)
            {
                this.V_axis[i].FNumber = this.V_axis[i].FNumber + Offset;
                this.Lbl_V_Axis[i].Text = this.V_axis[i].gStr("V");
            }
            ZeroVoltPoint.FNumber = this.V_axis[(UInt16)(this.Lbl_V_Axis.GetUpperBound(0) + 1) / 2].FNumber;

        }
        private void ButVDown_Click(object sender, EventArgs e)
        {
            VoltDiv.FNumber = VoltDiv.gFlt(this.CmbVoltDiv.Text);
            float Offset = -VoltDiv.FNumber;//-10f;
            for (int i = 0; i < this.Lbl_V_Axis.GetUpperBound(0) + 1; i++)
            {
                this.V_axis[i].FNumber = this.V_axis[i].FNumber + Offset;
                this.Lbl_V_Axis[i].Text = this.V_axis[i].gStr("V");
            }
            ZeroVoltPoint.FNumber = this.V_axis[(UInt16)(this.Lbl_V_Axis.GetUpperBound(0) + 1) / 2].FNumber;

        }
        private void TxtZeroVolt_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)13: //Return
                    TxtZeroVolt.Visible = false;

                    ZeroVoltPoint.FNumber = ZeroVoltPoint.gFlt(TxtZeroVolt.Text);
                    VoltDiv.FNumber = VoltDiv.gFlt(this.CmbVoltDiv.Text);
                    for (int i = 0; i < this.Lbl_V_Axis.GetUpperBound(0) + 1; i++)
                    {
                        this.V_axis[i].FNumber = (i - this.Lbl_V_Axis.GetUpperBound(0) / 2) * VoltDiv.FNumber + ZeroVoltPoint.FNumber;
                        this.Lbl_V_Axis[i].Text = this.V_axis[i].gStr("V");
                    }

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
                    string secondValidChar = "0123456789umV,.";
                    string bodyValidChar = "0123456789,.";
                    string secondlastValidChar = "0123456789,.um";
                    string lastValidChar = "umV";
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
            VoltDiv.FNumber = VoltDiv.gFlt(CmbVoltDiv.SelectedItem.ToString());
            for (int i = 0; i < this.Lbl_V_Axis.GetUpperBound(0) + 1; i++)
            {
                if (this.V_axis[i] == null) this.V_axis[i] = new Number();
                this.V_axis[i].FNumber = ZeroVoltPoint.FNumber + (i - this.Lbl_V_Axis.GetUpperBound(0) / 2) * VoltDiv.FNumber;
                this.Lbl_V_Axis[i].Text = this.V_axis[i].gStr("V");
            }
            ZeroVoltPoint.FNumber = this.V_axis[(UInt16)(this.Lbl_V_Axis.GetUpperBound(0) + 1) / 2].FNumber;
        }
        private void CmbTimeDiv_SelectedValueChanged(object sender, EventArgs e)
        {
            //TimeDiv.FNumber = TimeDiv.gFlt(CmbTimeDiv.SelectedItem.ToString());
            TimeDiv.FNumber = TimeDiv.gFlt(CmbTimeDiv.SelectedItem.ToString());
            for (int i = 0; i < this.Lbl_T_Axis.GetUpperBound(0) + 1; i++)
            {
                if (this.T_axis[i] == null) this.T_axis[i] = new Number();

                this.T_axis[i].FNumber = i * TimeDiv.FNumber + ZeroTimePoint.FNumber - (TimeDiv.FNumber * this.Lbl_T_Axis.GetUpperBound(0) / 2.0f);

                this.Lbl_T_Axis[i].Text = this.T_axis[i].gStr("s");
            }
            ZeroTimePoint.FNumber = this.T_axis[(UInt16)(this.Lbl_T_Axis.GetUpperBound(0) + 1) / 2].FNumber;
        }
        private void TxtADC_Clock_Leave(object sender, EventArgs e)
        {
            ADC_Clock.FNumber = ADC_Clock.gFlt(TxtADC_Clock.Text);

        }
        private void TxtADC_Vref_Leave(object sender, EventArgs e)
        {
            ADC_VRef.FNumber = ADC_VRef.gFlt(TxtADC_Vref.Text);

        }
        private void TxtADC_Res_Leave(object sender, EventArgs e)
        {
            ADC_BitRes.FNumber = ADC_BitRes.gFlt(TxtADC_Res.Text);

        }

         private void CmbmsSleep_SelectedIndexChanged(object sender, EventArgs e)
        {
            msSleep.FNumber = msSleep.gFlt(CmbmsSleep.SelectedItem.ToString()); //Entry is in FPS to be converted to ms!
            msSleep.FNumber = (msSleep.FNumber == -999) ? -999 : 1000.0f / msSleep.FNumber;

        }

        private void TmrRefresh_Tick(object sender, EventArgs e)
        {
            float i2_USB2_OGL_Window_Framesfl;
            unsafe
            {
                i2_USB2_OGL_Window_Framesfl = *((float*)OGL_Screen_Status.i2_USB2_OGL_Window_Frames.ToPointer());
            }

            lblFPS.Text = i2_USB2_OGL_Window_Framesfl.ToString("#0.#ms");
            
            LblADC_Res_V.Text = Stats[3].ToString("#0.##mV");
            LblTmrSuspended.Text = Stats[5].ToString("#0.####ms");
            LblKSamplePerSec.Text = Stats[9].ToString("#0.###kSps");

            USB2_Device_Status.Action.i2_UpdateChkList(ChkListUSBInit);
            USB2_Device_Status.i2_UpdateChkList(ChkListUSBState);


        }

        private void CmbStatRefresh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbStatRefresh.Text == "Never")
            {
                TmrRefresh.Enabled = false;
            }
            else
            {
                TmrRefresh.Interval = Convert.ToInt32(CmbStatRefresh.Text);
            }
        }

        public const UInt32 USB2_BufferSize8 = USB2_NumberOfPackets * USB2_TransferSize; //limit Buffersize to 511 x (2byte ADC reading + 2byte Tick data) x 3 packets
        public const UInt16 USB2_TransferSize = 500;
        public const byte USB2_NumberOfPackets = 50;
        public const UInt32 USB2_Win_W = 682;
        public const byte USB2_Win_Div = 10;
        public const byte USB2_MillisecondsPerTransfer = 55;
        public const byte USB2_SecondsOfBuffer = 10;
        public static Int16[] USB2_Buffer = new Int16[(UInt32)Math.Ceiling((double)(USB2_SecondsOfBuffer * 1000 / USB2_MillisecondsPerTransfer * USB2_NumberOfPackets * USB2_TransferSize / 2 * 2))];//double buffer needed to ensure full screen can be drawn @10secs.
        public Int32 USB2_BufferPosition16 = (Int32)Math.Ceiling((double)(USB2_SecondsOfBuffer * 1000 / USB2_MillisecondsPerTransfer * USB2_NumberOfPackets * USB2_TransferSize / 2));
        public Int32 USB2_BufferIterations = 0; //For performance reasons, ie. to avoid during first iteration searches take through the whole buffer
        public Int32 USB2_NumberOfReads = 0;
        

        public static float[][] USB2_VertexBuffer = new float[4][];        
        public static float[] USB2_VertexBufferTrigger = new float[2 * 2 * 2]; //Two lines, of two dimension tupple

        Stopwatch USB2_Watch = new Stopwatch();
        double[] USB2_WatchData = new double[10000];
        UInt32 USB2_WatchDataCounter = 0;

        

        public class _OGL_Screen_Status
        {
            ~_OGL_Screen_Status()
            {
                Marshal.FreeHGlobal(i2_USB2_OGL_Suspended);
                Marshal.FreeHGlobal(i2_USB2_OGL_ScreenDrawn);
                Marshal.FreeHGlobal(i2_USB2_OGL_Window_Frames);
                Marshal.FreeHGlobal(i2_USB2_OGL_Extrapolate);
                Marshal.FreeHGlobal(i2_USB2_OGL_LastDataPosition);
            }


            public IntPtr i2_USB2_OGL_Suspended = Marshal.AllocHGlobal(1);
            public IntPtr i2_USB2_OGL_ScreenDrawn = Marshal.AllocHGlobal(1);
            public IntPtr i2_USB2_OGL_Window_Frames = Marshal.AllocHGlobal(1);
            public IntPtr i2_USB2_OGL_Extrapolate = Marshal.AllocHGlobal(1);
            public IntPtr i2_USB2_OGL_LastDataPosition = Marshal.AllocHGlobal(1);

            public void i2_ConnectPtrs()
            {
                NativeMethods.OGL_Suspended(i2_USB2_OGL_Suspended);
                NativeMethods.OGL_pScreenDrawn(i2_USB2_OGL_ScreenDrawn);
                NativeMethods.OGL_Window_Frames(i2_USB2_OGL_Window_Frames);
                NativeMethods.OGL_Extrapolate(i2_USB2_OGL_Extrapolate);
                NativeMethods.OGL_LastDataPosition(i2_USB2_OGL_LastDataPosition);
            }
            //public delegate void _Delegate_SetItemChecked(CheckedListBox chkListBox);
            //public static void Method_SetItemChecked(CheckedListBox chkListBox)
            //{
            //  chkListBox.SetItemChecked(0, true);
            //}
            public void i2_Set(byte value, IntPtr item, CheckedListBox chkListOGL, byte index)
            {
                Marshal.WriteByte(item, value);
                //_Delegate_SetItemChecked delegate_SetItemChecked = Method_SetItemChecked;
                if (chkListOGL != null)
                    //Pass newly created method SetItemChecked into the running thread of chkListOGL to be executed
                    chkListOGL.Invoke(new Action<byte, IntPtr,CheckedListBox,byte>((ivalue,iitem,ichkLstBox,iindex)=> 
                    {ichkLstBox.SetItemChecked(iindex, (Marshal.ReadByte(iitem) == 1) ? true : false); }),value,item,chkListOGL,index);
                
            }
            public void i2_Set(Int32 value, IntPtr item, CheckedListBox chkListOGL, byte index)
            {
                Marshal.WriteInt32(item, value);
                //_Delegate_SetItemChecked delegate_SetItemChecked = Method_SetItemChecked;
                if (chkListOGL != null)
                    //Pass newly created method SetItemChecked into the running thread of chkListOGL to be executed
                    chkListOGL.Invoke(new Action<Int32, IntPtr, CheckedListBox, byte>((ivalue, iitem, ichkLstBox, iindex) =>
                    { ichkLstBox.SetItemChecked(iindex, (Marshal.ReadByte(iitem) == 1) ? true : false); }), value, item, chkListOGL, index);

            }
            public void i2_SetAll(Int32 value, Form This)
            {
                Control chkListOGLSuspended = This.Controls.Find("ChkListOGLSuspended", true)[0];
                Control chkoptionExtrapolated = This.Controls.Find("ChkoptionExtrapolated", true)[0];

                Marshal.WriteByte(i2_USB2_OGL_Suspended, (byte) value);
                Marshal.WriteByte(i2_USB2_OGL_ScreenDrawn, (byte)value);
                Marshal.WriteByte(i2_USB2_OGL_Window_Frames, (byte)value);
                Marshal.WriteByte(i2_USB2_OGL_Extrapolate, (byte)value);
                Marshal.WriteInt32(i2_USB2_OGL_LastDataPosition, value);
                if (chkListOGLSuspended != null)
                    //Pass newly created method SetItemChecked into the running thread of chkListOGL to be executed
                    chkListOGLSuspended.Invoke(new Action<byte, CheckedListBox>((ivalue, ichkLstBox) =>
                    { 
                        ichkLstBox.SetItemChecked(0, (Marshal.ReadByte(i2_USB2_OGL_Suspended) == 1) ? true : false); 
                    }), (byte) value, chkListOGLSuspended);
                if (chkoptionExtrapolated != null)
                    //Pass newly created method SetItemChecked into the running thread of chkListOGL to be executed
                    chkoptionExtrapolated.Invoke(new Action<byte, CheckBox>((ivalue, ichkBox) =>
                    {
                        ichkBox.Checked = (Marshal.ReadByte(i2_USB2_OGL_Extrapolate) == 1) ? true : false;
                    }), (byte) value, chkoptionExtrapolated);
            }
        }
        public _OGL_Screen_Status OGL_Screen_Status = new _OGL_Screen_Status();

        public class _USB2_Device_Status
        {
            ~_USB2_Device_Status()
            {
                Marshal.FreeHGlobal(i2_Started);
                Marshal.FreeHGlobal(i2_Stopped);
                Marshal.FreeHGlobal(i2_Initialized);
                Marshal.FreeHGlobal(i2_Errored);
                Marshal.FreeHGlobal(i2_RequestToStop);
                Marshal.FreeHGlobal(i2_USB2_BackBufferFilled);
                Marshal.FreeHGlobal(i2_USB2_BackBufferLocked);
            }

            public IntPtr i2_Started = Marshal.AllocHGlobal(1);
            public IntPtr i2_Stopped = Marshal.AllocHGlobal(1);
            public IntPtr i2_Initialized = Marshal.AllocHGlobal(1);
            public IntPtr i2_Errored = Marshal.AllocHGlobal(1);
            public IntPtr i2_RequestToStop = Marshal.AllocHGlobal(1);
            public IntPtr i2_USB2_BackBufferFilled = Marshal.AllocHGlobal(1);
            public IntPtr i2_USB2_BackBufferLocked = Marshal.AllocHGlobal(1);

            public void i2_Set(byte value,IntPtr item, CheckedListBox chkListUSBState, byte index)
            {
                Marshal.WriteByte(item, value);
                if (chkListUSBState != null)
                    //Pass newly created method SetItemChecked into the running thread of chkListUSBState to be executed
                    chkListUSBState.Invoke(new Action<byte, IntPtr, CheckedListBox, byte>((ivalue, iitem, ichkLstBox, iindex) =>
                    { chkListUSBState.SetItemChecked(iindex, (Marshal.ReadByte(iitem) == 1) ? true : false); }), value, item, chkListUSBState, index);
                    //chkListUSBState.SetItemChecked(index, (Marshal.ReadByte(item) == 1) ? true : false);
            }
            public void i2_SetAll(byte value, CheckedListBox chkListUSBState)
            {
                Marshal.WriteByte(i2_Started, value);
                Marshal.WriteByte(i2_Stopped, value);
                Marshal.WriteByte(i2_Initialized, value);
                Marshal.WriteByte(i2_Errored, value);
                Marshal.WriteByte(i2_RequestToStop, value);
                if (chkListUSBState != null)
                {
                    chkListUSBState.Invoke(new Action<byte, CheckedListBox>((ivalue, ichkLstBox) => {
                        ichkLstBox.SetItemChecked(0, (Marshal.ReadByte(i2_Initialized) == 1) ? true : false);
                        ichkLstBox.SetItemChecked(1, (Marshal.ReadByte(i2_Started) == 1) ? true : false);
                        ichkLstBox.SetItemChecked(2, (Marshal.ReadByte(i2_RequestToStop) == 1) ? true : false);
                        ichkLstBox.SetItemChecked(3, (Marshal.ReadByte(i2_Stopped) == 1) ? true : false);
                        ichkLstBox.SetItemChecked(4, (Marshal.ReadByte(i2_Errored) == 1) ? true : false);
                    }), value, chkListUSBState);
                }
            }
            public void i2_UpdateChkList(CheckedListBox chkListUSBState)
            {
                if (chkListUSBState != null)
                {
                    chkListUSBState.Invoke(new Action<CheckedListBox>((ichkLstBox) => {
                        ichkLstBox.SetItemChecked(0, (Marshal.ReadByte(i2_Initialized) == 1) ? true : false);
                        ichkLstBox.SetItemChecked(1, (Marshal.ReadByte(i2_Started) == 1) ? true : false);
                        ichkLstBox.SetItemChecked(2, (Marshal.ReadByte(i2_RequestToStop) == 1) ? true : false);
                        ichkLstBox.SetItemChecked(3, (Marshal.ReadByte(i2_Stopped) == 1) ? true : false);
                        ichkLstBox.SetItemChecked(4, (Marshal.ReadByte(i2_Errored) == 1) ? true : false);
                    }), chkListUSBState);
                }
            }
            public void i2_ConnectPtrs()
            {
                NativeMethods.USB2_Device_Status_Started(i2_Started);
                NativeMethods.USB2_Device_Status_Stopped(i2_Stopped);
                NativeMethods.USB2_Device_Status_RequestToStop(i2_RequestToStop);
                NativeMethods.USB2_Device_Status_Initialized(i2_Initialized);
                NativeMethods.USB2_Device_Status_Errored(i2_Errored);
                NativeMethods.USB2_Device_BackBufferFilled(i2_USB2_BackBufferFilled);
                NativeMethods.USB2_Device_BackBufferLocked(i2_USB2_BackBufferLocked);
            }
            public class _Action
            {
                public IntPtr i2_Initialized = Marshal.AllocHGlobal(1);
                public IntPtr i2_DeviceFound = Marshal.AllocHGlobal(1);
                public IntPtr i2_GetDescriptor = Marshal.AllocHGlobal(1);
                public IntPtr i2_GetInterface = Marshal.AllocHGlobal(1);
                public IntPtr i2_GetIsochPipe = Marshal.AllocHGlobal(1);
                public IntPtr i2_GetInterval = Marshal.AllocHGlobal(1);
                public IntPtr i2_SetTransferChars = Marshal.AllocHGlobal(1);
                public IntPtr i2_SetOverlappedStructure = Marshal.AllocHGlobal(1);
                public IntPtr i2_SetOverlappedEvents = Marshal.AllocHGlobal(1);
                public IntPtr i2_SetIsochPackets = Marshal.AllocHGlobal(1);
                public IntPtr i2_RegisterIsochBuffer = Marshal.AllocHGlobal(1);
                public IntPtr i2_ResetPipe = Marshal.AllocHGlobal(1);
                public IntPtr i2_EndAtFrame = Marshal.AllocHGlobal(1);
                public IntPtr i2_ReadIsochPipe = Marshal.AllocHGlobal(1);

                public void i2_Set(byte value, IntPtr item, CheckedListBox chkListUSBInit, byte index)
                {
                    Marshal.WriteByte(item, value);
                    if (chkListUSBInit != null)
                        //Pass newly created method SetItemChecked into the running thread of chkListOGL to be executed
                        chkListUSBInit.Invoke(new Action<byte, IntPtr, CheckedListBox, byte>((ivalue, iitem, ichkLstBox, iindex) =>
                        { chkListUSBInit.SetItemChecked(iindex, (Marshal.ReadByte(iitem) == 1) ? true : false); }), value, item, chkListUSBInit, index);
                        //chkListUSBInit.SetItemChecked(index, (Marshal.ReadByte(item) == 1) ? true : false);
                }

                public void i2_SetAllAction(byte value, CheckedListBox chkListUSBInit)
                {
                    Marshal.WriteByte(i2_DeviceFound, value);
                    Marshal.WriteByte(i2_GetDescriptor, value);
                    Marshal.WriteByte(i2_GetInterface, value);
                    Marshal.WriteByte(i2_GetIsochPipe, value);
                    Marshal.WriteByte(i2_GetInterval, value);
                    Marshal.WriteByte(i2_SetTransferChars, value);
                    Marshal.WriteByte(i2_SetOverlappedStructure, value);
                    Marshal.WriteByte(i2_SetOverlappedEvents, value);
                    Marshal.WriteByte(i2_SetIsochPackets, value);
                    Marshal.WriteByte(i2_SetIsochPackets, value);
                    Marshal.WriteByte(i2_ResetPipe, value);
                    Marshal.WriteByte(i2_EndAtFrame, value);
                    if (chkListUSBInit != null) {
                        chkListUSBInit.Invoke(new Action<byte, CheckedListBox>((ivalue, ichkLstBox) => {
                            chkListUSBInit.SetItemChecked(0, (Marshal.ReadByte(i2_DeviceFound) == 1) ? true : false);
                            chkListUSBInit.SetItemChecked(1, (Marshal.ReadByte(i2_GetDescriptor) == 1) ? true : false);
                            chkListUSBInit.SetItemChecked(2, (Marshal.ReadByte(i2_GetInterface) == 1) ? true : false);
                            chkListUSBInit.SetItemChecked(3, (Marshal.ReadByte(i2_GetIsochPipe) == 1) ? true : false);
                            chkListUSBInit.SetItemChecked(4, (Marshal.ReadByte(i2_GetInterval) == 1) ? true : false);
                            chkListUSBInit.SetItemChecked(5, (Marshal.ReadByte(i2_SetTransferChars) == 1) ? true : false);
                            chkListUSBInit.SetItemChecked(6, (Marshal.ReadByte(i2_SetOverlappedStructure) == 1) ? true : false);
                            chkListUSBInit.SetItemChecked(7, (Marshal.ReadByte(i2_SetOverlappedEvents) == 1) ? true : false);
                            chkListUSBInit.SetItemChecked(8, (Marshal.ReadByte(i2_SetIsochPackets) == 1) ? true : false);
                            chkListUSBInit.SetItemChecked(9, (Marshal.ReadByte(i2_SetIsochPackets) == 1) ? true : false);
                            chkListUSBInit.SetItemChecked(10, (Marshal.ReadByte(i2_ResetPipe) == 1) ? true : false);
                            chkListUSBInit.SetItemChecked(11, (Marshal.ReadByte(i2_EndAtFrame) == 1) ? true : false);
                        }), value, chkListUSBInit);
                    }


                }
                public void i2_UpdateChkList(CheckedListBox chkListUSBInit)
                {
                    if (chkListUSBInit != null)
                    {
                        chkListUSBInit.Invoke(new Action<CheckedListBox>((ichkLstBox) => {
                            ichkLstBox.SetItemChecked(0, (Marshal.ReadByte(i2_DeviceFound) == 1) ? true : false);
                            ichkLstBox.SetItemChecked(1, (Marshal.ReadByte(i2_GetDescriptor) == 1) ? true : false);
                            ichkLstBox.SetItemChecked(2, (Marshal.ReadByte(i2_GetInterface) == 1) ? true : false);
                            ichkLstBox.SetItemChecked(3, (Marshal.ReadByte(i2_GetIsochPipe) == 1) ? true : false);
                            ichkLstBox.SetItemChecked(4, (Marshal.ReadByte(i2_GetInterval) == 1) ? true : false);
                            ichkLstBox.SetItemChecked(5, (Marshal.ReadByte(i2_SetTransferChars) == 1) ? true : false);
                            ichkLstBox.SetItemChecked(6, (Marshal.ReadByte(i2_SetOverlappedStructure) == 1) ? true : false);
                            ichkLstBox.SetItemChecked(7, (Marshal.ReadByte(i2_SetOverlappedEvents) == 1) ? true : false);
                            ichkLstBox.SetItemChecked(8, (Marshal.ReadByte(i2_SetIsochPackets) == 1) ? true : false);
                            ichkLstBox.SetItemChecked(9, (Marshal.ReadByte(i2_SetIsochPackets) == 1) ? true : false);
                            ichkLstBox.SetItemChecked(10, (Marshal.ReadByte(i2_ResetPipe) == 1) ? true : false);
                            ichkLstBox.SetItemChecked(11, (Marshal.ReadByte(i2_EndAtFrame) == 1) ? true : false);
                        }), chkListUSBInit);
                    }
                }
                public void i2_ConnectPtrs()
                {
                    NativeMethods.USB2_Device_Status_Action_DeviceFound(i2_DeviceFound);
                    NativeMethods.USB2_Device_Status_Action_GetDescriptor(i2_GetDescriptor);
                    NativeMethods.USB2_Device_Status_Action_GetInterface(i2_GetInterface);
                    NativeMethods.USB2_Device_Status_Action_GetIsochPipe(i2_GetIsochPipe);
                    NativeMethods.USB2_Device_Status_Action_GetInterval(i2_GetInterval);
                    NativeMethods.USB2_Device_Status_Action_SetTransferChars(i2_SetTransferChars);
                    NativeMethods.USB2_Device_Status_Action_SetOverlappedStructure(i2_SetOverlappedStructure);
                    NativeMethods.USB2_Device_Status_Action_SetOverlappedEvents(i2_SetOverlappedEvents);
                    NativeMethods.USB2_Device_Status_Action_SetIsochPackets(i2_SetIsochPackets);
                    NativeMethods.USB2_Device_Status_Action_RegisterIsochBuffer(i2_RegisterIsochBuffer);
                    NativeMethods.USB2_Device_Status_Action_ResetPipe(i2_ResetPipe);
                    NativeMethods.USB2_Device_Status_Action_EndAtFrame(i2_EndAtFrame);
                    NativeMethods.USB2_Device_Status_Action_ReadIsochPipe(i2_ReadIsochPipe);
                }
                ~_Action()
                {
                    Marshal.FreeHGlobal(i2_Initialized);
                    Marshal.FreeHGlobal(i2_DeviceFound);
                    Marshal.FreeHGlobal(i2_GetDescriptor);
                    Marshal.FreeHGlobal(i2_GetInterface);
                    Marshal.FreeHGlobal(i2_GetIsochPipe);
                    Marshal.FreeHGlobal(i2_GetInterval);
                    Marshal.FreeHGlobal(i2_SetTransferChars);
                    Marshal.FreeHGlobal(i2_SetOverlappedStructure);
                    Marshal.FreeHGlobal(i2_SetOverlappedEvents);
                    Marshal.FreeHGlobal(i2_SetIsochPackets);
                    Marshal.FreeHGlobal(i2_RegisterIsochBuffer);
                    Marshal.FreeHGlobal(i2_ResetPipe);
                    Marshal.FreeHGlobal(i2_EndAtFrame);
                    Marshal.FreeHGlobal(i2_ReadIsochPipe);
                }
            }
            public _Action Action = new _Action();
        }
        public _USB2_Device_Status USB2_Device_Status = new _USB2_Device_Status();

        /* Event system:
         * |-------------------------->          v       <-------------------------->         v          <----------------->     ^
         *   USB(cpp - start) READ IN    Backbufferfilled     TRANSFORMVERTEX         BufferTransformed      MOVEBUFFER        Release backbuffer filled & Buffertransformed
         *   <----------------------------------------------------------------------->
         *                               TRANSFORMVERTEX
         *    => system will not read while Backbuffer is still filled and not moved
         *    => system will not move unless Backbuffer is filled and transformed
         *    => system will transform continuously unless while moving
         *    => TO DO system will only transfor one time per screen draw and wait for the transform as long as possible
         */



        public void butUSB2_Click(object sender, EventArgs e)
        {
            try
            {
                if (butUSB2.Text == "Connect to USB device")
                {
                    butUSB2.Enabled = false;
                    ButMemoryMode.Enabled = false;

                    
                    TxtUSB2.Clear();

                    Array.Clear(USB2_Buffer, 0, USB2_Buffer.Length);
                    USB2_BufferIterations = 0; //Keep track of number of times the buffer was filled
                    USB2_BufferPosition16 = USB2_Buffer.Length; //Pointer where the buffer is writing

                    USB2_Device_Status.i2_ConnectPtrs();
                    USB2_Device_Status.Action.i2_ConnectPtrs();
                    
                    USB2_Device_Status.Action.i2_SetAllAction(0, ChkListUSBInit);
                    USB2_Device_Status.i2_SetAll(0, ChkListUSBState);

                    bool success = NativeMethods.USB2_Device_Initialize(USB2_Buffer, (UInt32)USB2_Buffer.Length * 2, USB2_BufferSize8, USB2_NumberOfPackets, 1);
                    USB2_Device_Status.i2_UpdateChkList(ChkListUSBState);
                    USB2_Device_Status.Action.i2_UpdateChkList(ChkListUSBInit);
                    if (Marshal.ReadByte(USB2_Device_Status.i2_Initialized) == 0) throw new ArgumentException("USB not initialized");
                    ChkListUSBState.SetItemChecked(0, (Marshal.ReadByte(USB2_Device_Status.i2_Initialized) == 1) ? true : false);

                    
                    if (Marshal.ReadByte(USB2_Device_Status.Action.i2_DeviceFound) == 0) throw new ArgumentException("1");
                    if (Marshal.ReadByte(USB2_Device_Status.Action.i2_GetDescriptor) == 0) throw new ArgumentException("2");
                    if (Marshal.ReadByte(USB2_Device_Status.Action.i2_GetInterface) == 0) throw new ArgumentException("3");
                    if (Marshal.ReadByte(USB2_Device_Status.Action.i2_GetIsochPipe) == 0) throw new ArgumentException("4");
                    if (Marshal.ReadByte(USB2_Device_Status.Action.i2_GetInterval) == 0) throw new ArgumentException("5");
                    if (Marshal.ReadByte(USB2_Device_Status.Action.i2_SetTransferChars) == 0) throw new ArgumentException("6");
                    if (Marshal.ReadByte(USB2_Device_Status.Action.i2_SetOverlappedStructure) == 0) throw new ArgumentException("7");
                    if (Marshal.ReadByte(USB2_Device_Status.Action.i2_SetOverlappedEvents) == 0) throw new ArgumentException("8");
                    if (Marshal.ReadByte(USB2_Device_Status.Action.i2_SetIsochPackets) == 0) throw new ArgumentException("9");
                    if (Marshal.ReadByte(USB2_Device_Status.Action.i2_SetIsochPackets) == 0) throw new ArgumentException("10");
                    if (Marshal.ReadByte(USB2_Device_Status.Action.i2_ResetPipe) == 0) throw new ArgumentException("11");
                    if (Marshal.ReadByte(USB2_Device_Status.Action.i2_EndAtFrame) == 0) throw new ArgumentException("12");

                    USB2_Device_Status.Action.i2_UpdateChkList(ChkListUSBInit);

                    TxtUSB2.Clear();
                    TxtUSB2.AppendText("USB device succesfully initialized");

                    Array.Clear(USB2_VertexBuffer[0], 0, USB2_VertexBuffer[0].Length);
                    Array.Clear(USB2_VertexBuffer[1], 0, USB2_VertexBuffer[1].Length);
                    Array.Clear(USB2_VertexBuffer[2], 0, USB2_VertexBuffer[2].Length);
                    Array.Clear(USB2_VertexBuffer[3], 0, USB2_VertexBuffer[3].Length);

                    USB2_Device_Status.i2_Set(0, USB2_Device_Status.i2_USB2_BackBufferFilled, null, 0);
                    USB2_Device_Status.i2_Set(0, USB2_Device_Status.i2_USB2_BackBufferLocked, null, 0);
                    OGL_Screen_Status.i2_Set(1, OGL_Screen_Status.i2_USB2_OGL_Suspended, ChkListOGLSuspended, 0);
                    OGL_Screen_Status.i2_Set(1, OGL_Screen_Status.i2_USB2_OGL_ScreenDrawn, null, 0);
                    
                    
                    butUSB2.Text = "Disconnect from USB device";
                    butUSB2.Enabled = true;

                    Task USB2_Start = Task.Run(() =>
                    {
                        NativeMethods.USB2_Device_Start(100, 0);
                    });
                    Task USB2_CheckStarted = Task.Run(() =>
                    {
                        while (Marshal.ReadByte(USB2_Device_Status.i2_Started) == 0) ;
                    });
                    if (USB2_CheckStarted.Wait(500) == false)
                    { throw new ArgumentException("USB could not be started"); }
                    USB2_Device_Status.i2_UpdateChkList(ChkListUSBState);
                    USB2_Device_Status.Action.i2_UpdateChkList(ChkListUSBInit);




                    /* Task must transform the read data until and no later than, allowing to finish transformation, the signal is given that the backbuffer is filled.
                     * At the even the backbuffer is filled, the moving task is to kick in, holding all other activities.
                     * From the moment moving is done, reading from USB and transforming of data is to be allowed asynchronously
                     */

                    OGL_Screen_Status.i2_Set(0, OGL_Screen_Status.i2_USB2_OGL_Suspended, ChkListOGLSuspended, 0);

                    Task USB2_ProcessBuffer = Task.Run(() =>
                    {
                        //while (Marshal.ReadByte(USB2_Device_Status.i2_Started) == 0);
                        while (Marshal.ReadByte(USB2_Device_Status.i2_Started) == 1)
                        {
                            if (Marshal.ReadByte(USB2_Device_Status.i2_USB2_BackBufferFilled) == 1)
                            {// Do not allow to continue as long as the backbuffer is not filled or the front buffer is filled

                                //USB2_Device_Status.i2_Set(1, USB2_Device_Status.i2_USB2_BackBufferLocked, null, 0);//performance direct written
                                Marshal.WriteByte(USB2_Device_Status.i2_USB2_BackBufferLocked, 0);//Lock the backbuffer for threading purposes, ensuring no concurrent writes

                                for (UInt32 i = 0; i < USB2_Buffer.Length; i++)
                                {
                                    if (i < USB2_Buffer.Length - USB2_BufferSize8 / 2)
                                    {
                                        if (USB2_Buffer[i + USB2_BufferSize8 / 2] == 32237)
                                            i = i; //will be hit when the USB cannt transfer all the data, lower size per packet is adviced
                                        USB2_Buffer[i] = USB2_Buffer[i + USB2_BufferSize8 / 2];
                                    }
                                    else
                                        USB2_Buffer[i] = 32237;
                                }

                                //Array.Copy(USB2_Buffer, USB2_BufferSize8 / 2, USB2_Buffer, 0, USB2_Buffer.Length-USB2_BufferSize8/2);
                                //for (Int32 i = USB2_Buffer.Length - (Int32)USB2_BufferSize8 / 2; i < USB2_Buffer.Length; i++) USB2_Buffer[i] = 65535;
                                USB2_BufferPosition16 -= (Int32)(USB2_BufferSize8 / 2);
                                if (USB2_BufferPosition16 <= (Int32)(USB2_BufferSize8 / 2))
                                { //USB2_BufferPosition16 is at the head of the datablock
                                    USB2_BufferPosition16 = USB2_Buffer.Length; USB2_BufferIterations++;
                                    if (USB2_Mode == 2)
                                    {

                                        USB2_Device_Status.i2_Set(1, USB2_Device_Status.i2_RequestToStop, ChkListUSBState, 2);
                                        USB2_Device_Status.i2_Set(0, USB2_Device_Status.i2_USB2_BackBufferFilled, null, 0); //Give signal to USB_Device to continue
                                        
                                        while (Marshal.ReadByte(USB2_Device_Status.i2_Started) == 1) ; //Hold as long as USB is in started state

                                        OGL_Screen_Status.i2_Set(1, OGL_Screen_Status.i2_USB2_OGL_Suspended, ChkListOGLSuspended, 0);

                                        USB2_Device_Status.i2_UpdateChkList(ChkListUSBInit);
                                    }
                                }
                                Marshal.WriteByte(USB2_Device_Status.i2_USB2_BackBufferFilled, 0);//Gives trigger to USB_Device_Swap() to continue and disallows slippage
                                Marshal.WriteByte(USB2_Device_Status.i2_USB2_BackBufferLocked, 0);//Unlock Backbuffer for writing
                                USB2_Device_Status.i2_Set(0, USB2_Device_Status.i2_USB2_BackBufferFilled, null, 0); //Performance reasons directly written
                                USB2_Device_Status.i2_Set(0, USB2_Device_Status.i2_USB2_BackBufferLocked, null, 0); //Performance reasons directly written
                            } 
                            else //Transform and draw
                            {
                                switch (USB2_Mode)
                                {
                                    case 1:
                                        Standard_Mode2();
                                        break;
                                    
                                    case 4:
                                        Triggered_Mode();
                                        break;
                                }
                            }
                        }
                    });
                }
                else
                {
                    butUSB2.Enabled = false;
                    ButMemoryMode.Enabled = false;

                    USB2_Device_Status.i2_Set(0, USB2_Device_Status.i2_Started, ChkListUSBState, 1);
                    USB2_Device_Status.i2_Set(1, USB2_Device_Status.i2_RequestToStop, ChkListUSBState, 2);

                    while (Marshal.ReadByte(USB2_Device_Status.i2_Started) == 1) ; //Hold as long as USB is in started state
                    USB2_Device_Status.i2_UpdateChkList(ChkListUSBState);
                    USB2_Device_Status.Action.i2_UpdateChkList(ChkListUSBInit);

                    //Reset buffer
                    for (UInt32 i = 0; i < USB2_VertexBuffer[0].Length; i += 2 * 3) //Starting with 0 as Tick data is first, skipping per 2
                    {
                        USB2_VertexBuffer[0][i] = (float)(i / 2 / 3) / (float)USB2_Win_W * 2f - 1f;
                        USB2_VertexBuffer[0][i + 2] = (float)(i / 2 / 3) / (float)USB2_Win_W * 2f - 1f;
                        USB2_VertexBuffer[0][i + 4] = (float)(i / 2 / 3) / (float)USB2_Win_W * 2f - 1f;
                        USB2_VertexBuffer[1][i] = 1;
                        USB2_VertexBuffer[1][i + 2] = 1;
                        USB2_VertexBuffer[1][i + 4] = 1;
                    }

                    //Reset axis
                    CmbTimeDiv.Invoke(new Action(() =>
                    {
                        TimeDiv.FNumber = TimeDiv.gFlt(CmbTimeDiv.SelectedItem.ToString());
                        for (int i = 0; i < this.Lbl_T_Axis.GetUpperBound(0) + 1; i++)
                        {
                            if (this.T_axis[i] == null) this.T_axis[i] = new Number();
                            this.T_axis[i].FNumber = 0 + (i - this.Lbl_T_Axis.GetUpperBound(0)) * TimeDiv.FNumber;
                            this.Lbl_T_Axis[i].Text = this.T_axis[i].gStr("s");
                        }
                        ZeroTimePoint.FNumber = this.T_axis[(UInt16)(this.Lbl_T_Axis.GetUpperBound(0) + 1) / 2].FNumber;
                    }));

                    OGL_Screen_Status.i2_Set(1, OGL_Screen_Status.i2_USB2_OGL_Suspended, ChkListOGLSuspended, 0);
                    USB2_Device_Status.Action.i2_Set(0, USB2_Device_Status.Action.i2_Initialized, ChkListUSBInit, 0);
                    USB2_Device_Status.Action.i2_SetAllAction(0, ChkListUSBInit);

                    TxtUSB2.Clear();
                    TxtUSB2.AppendText("USB stopped");

                    butUSB2.Text = "Connect to USB device";
                    butUSB2.Enabled = true;
                    ButMemoryMode.Enabled = true;
                }
            }
            catch (ArgumentException exception)
            {
                USB2_Device_Status.i2_Set(0, USB2_Device_Status.i2_Started, ChkListUSBState, 1);
                USB2_Device_Status.i2_Set(0, USB2_Device_Status.i2_Stopped, ChkListUSBState, 3);
                USB2_Device_Status.i2_Set(1, USB2_Device_Status.i2_Errored, ChkListUSBState, 4);
                switch (exception.Message)
                {
                    case "USB not initialized":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("USB could not be initialized within 500 ms");
                        break;
                    case "1":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Could not open the device.");
                        break;
                    case "2":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Could not get descriptor.");
                        break;
                    case "3":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Could not get interface.");
                        break;
                    case "4":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Failed to get USB pipe. ");
                        break;
                    case "5":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Interval information or Maximum bytes interval invalid. ");
                        break;
                    case "6":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Transfer characteristics couldn't be set. ");
                        break;
                    case "7":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Overlapped structure couldn't be set. ");
                        break;
                    case "8":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Overlapped events couldn't be set. ");
                        break;
                    case "9":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Isochronous Packets couldn't be created. ");
                        break;
                    case "10":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Isochronous buffer couldn't be registred. Probably wrongly sized databuffer.");
                        break;
                    case "11":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Pipe couldn't be reset. ");
                        break;
                    case "12":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Buffer not set as a multiple of framesize. ");
                        break;
                    default:
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText(exception.Message);
                        break;
                }
            }


            USB2_Device_Status.i2_UpdateChkList(ChkListUSBState);
            USB2_Device_Status.Action.i2_UpdateChkList(ChkListUSBInit);
            butUSB2.Enabled = true;
        }

        public void Standard_Mode2()
        {
            //Collect read data
            USB2_Watch.Start();

            //Initialize and check conditions
            float timeDiv = (TimeDiv.FNumber == 0) ? throw new ArgumentException("Params error") : TimeDiv.FNumber;
            float aDC_Clock = (ADC_Clock.FNumber == 0) ? throw new ArgumentException("Params error") : ADC_Clock.FNumber;
            //float aDC_Clock = (ADC_Clock.FNumber == 0) ? throw new ArgumentException("Params error") : ADC_Clock.FNumber;
            float tickRange = timeDiv * USB2_Win_Div / 1000 * aDC_Clock;
            float aDC_bitres = (ADC_BitRes.FNumber == 0) ? throw new ArgumentException("Params error") : ADC_BitRes.FNumber;
            float aDC_Vref = (ADC_VRef.FNumber == 0) ? throw new ArgumentException("Params error") : ADC_VRef.FNumber;
            float zeroVoltPoint = ZeroVoltPoint.FNumber;
            float voltDiv = (VoltDiv.FNumber == 0) ? throw new ArgumentException("Params error") : VoltDiv.FNumber;
            float aDCScaleFactor = aDC_Vref / (float)(Math.Pow(2, aDC_bitres) - 1); //mV/ADC_reading
            float aDCMoveFactor = zeroVoltPoint / (voltDiv * USB2_Win_Div / 2); //mV/{0...100%}                        

            Array.Clear(USB2_VertexBuffer[3], 0, USB2_VertexBuffer[3].Length);

            Int32 RingBufferIndex = USB2_Buffer.Length - 1;
            Int32 LastDataPosition = 0;
            /*Search for first valable datapoint, ie. <>65535. Dispersed non-valable datapoints are assumed to not exist
                *We let RingBufferIndex=1 pass, as it would mean there is no valable data at all and it will not execute anything
                */
            while (USB2_Buffer[RingBufferIndex] == 32237 && RingBufferIndex >= 3) RingBufferIndex -= 2;

            UInt64 cumTickRange = 0;// USB2_Buffer[RingBufferIndex - 1];
            float vertexIndex = 1.0f - 2.0f * ((float)cumTickRange / tickRange);
            float aDCReading = -aDCMoveFactor + (USB2_Buffer[RingBufferIndex] * aDCScaleFactor) / (voltDiv * USB2_Win_Div / 2);

            /* 1) As time data is ordered, run through the Ringbuffer until cumulative Tick data is bigger than time scope requested by user. 
                * 2) RingBufferIndex needs to remain above or equal to 3, to ensure staying above or at 0 array limit, {3;2} --> {1;0}
                * 3) RingBufferIterations is there for performance reasons to makes sure the scan does not occur through the whole buffer for first time.
                */
            while (cumTickRange <= (UInt64)tickRange && RingBufferIndex >= 3 && !(USB2_BufferIterations == 0 && RingBufferIndex < USB2_BufferPosition16))
            {


                /* Vertexbuffer layout:
                    * 0                          Datapoint x 682 (=Screenresolution const)
                    * <------------------------------------------------------------------------->
                    *   <------------------------------------------------>
                    *                   1 DATAPOINT
                    *   <-------------> + <-----------> + <-------------->
                    *      LOWER             MID               UPPER
                    *   {Tick;Reading}  +  {Tick;Reading}  + {Tick;Reading}
                    *      0      1           2      3          4       5
                    *   Each vertexbuffer item = {Datapoint, # of occurences, Average}
                    *   
                    *   
                    */

                USB2_VertexBuffer[3][LastDataPosition] = vertexIndex; LastDataPosition++;
                USB2_VertexBuffer[3][LastDataPosition] = aDCReading; LastDataPosition++;

                if (USB2_Buffer[RingBufferIndex] != 32237) //To catch unwritten data area, which occur when USB cannt follow the selected data Packet size
                {
                    RingBufferIndex -= 2; //jump per two

                    cumTickRange += (UInt16)USB2_Buffer[RingBufferIndex - 1]; //last element is an ADC_read
                    vertexIndex = 1.0f - 2.0f * ((float)cumTickRange / tickRange);
                    if (vertexIndex < -1.0f) break; //performance: time is ordered so as soon out of scope, skip
                    aDCReading = -aDCMoveFactor + (USB2_Buffer[RingBufferIndex] * aDCScaleFactor) / (voltDiv * USB2_Win_Div / 2);

                }
                else RingBufferIndex -= 2;
            };

            //Signal Screendraw
            OGL_Screen_Status.i2_Set((Int32)LastDataPosition, OGL_Screen_Status.i2_USB2_OGL_LastDataPosition, null, 0); //Provide datapoint size to be drawn
            OGL_Screen_Status.i2_Set(0, OGL_Screen_Status.i2_USB2_OGL_ScreenDrawn, null, 0);
            while (Marshal.ReadByte(OGL_Screen_Status.i2_USB2_OGL_ScreenDrawn) == 0) ; //Wait for the screen to be drawn

            //Performance counting
            USB2_Watch.Stop(); USB2_WatchData[USB2_WatchDataCounter] = (double)USB2_Watch.ElapsedTicks / Stopwatch.Frequency; if (USB2_WatchDataCounter >= USB2_WatchData.Length - 1) USB2_WatchDataCounter = 0; else USB2_WatchDataCounter++;
            USB2_Watch.Reset(); USB2_Watch.Start();
        }

       
        public void Triggered_Mode()
        {
            //Collect read data
            USB2_Watch.Start();

            //Initialize and check conditions
            float timeDiv = (TimeDiv.FNumber == 0) ? throw new ArgumentException("Params error") : TimeDiv.FNumber;
            float aDC_Clock = (ADC_Clock.FNumber == 0) ? throw new ArgumentException("Params error") : ADC_Clock.FNumber;
            //float aDC_Clock = (ADC_Clock.FNumber == 0) ? throw new ArgumentException("Params error") : ADC_Clock.FNumber;
            float tickRange = timeDiv * USB2_Win_Div / 1000 * aDC_Clock;
            float aDC_bitres = (ADC_BitRes.FNumber == 0) ? throw new ArgumentException("Params error") : ADC_BitRes.FNumber;
            float aDC_Vref = (ADC_VRef.FNumber == 0) ? throw new ArgumentException("Params error") : ADC_VRef.FNumber;
            float zeroVoltPoint = ZeroVoltPoint.FNumber;
            float voltDiv = (VoltDiv.FNumber == 0) ? throw new ArgumentException("Params error") : VoltDiv.FNumber;
            float aDCScaleFactor = aDC_Vref / (float)(Math.Pow(2, aDC_bitres) - 1); //mV/ADC_reading
            float aDCMoveFactor = zeroVoltPoint / (voltDiv * USB2_Win_Div / 2); //mV/{0...100%}                        

            Int32 vertexIndexOrigin = (Int32)((USB2_VertexBufferTrigger[0] + 1.0f) / 2 * (USB2_Win_W - 1));

            float triggerTPoint = USB2_VertexBufferTrigger[0];
            float triggerVPointf = USB2_VertexBufferTrigger[5];

            Array.Clear(USB2_VertexBuffer[3], 0, USB2_VertexBuffer[3].Length);


            //Collect read data
            USB2_Watch.Start();


            Int32 RingBufferIndex = USB2_Buffer.Length - 1;
            Int32 lastDataPosition = 0;
            /*Search for first valable datapoint, ie. <>65535. Dispersed non-valable datapoints are assumed to not exist
                *We let RingBufferIndex=1 pass, as it would mean there is no valable data at all and it will not execute anything
                */
            while (USB2_Buffer[RingBufferIndex] == 32237 && RingBufferIndex >= 3) RingBufferIndex -= 2;

            UInt64 cumTickRange = 0;// USB2_Buffer[RingBufferIndex - 1];
            float vertexIndex = 1.0f - 2.0f * ((float)cumTickRange / tickRange);
            float aDCReading = -aDCMoveFactor + (USB2_Buffer[RingBufferIndex] * aDCScaleFactor) / (voltDiv * USB2_Win_Div / 2);

            /* 1) As time data is ordered, run through the Ringbuffer until cumulative Tick data is bigger than time scope requested by user. 
                * 2) RingBufferIndex needs to remain above or equal to 3, to ensure staying above or at 0 array limit, {3;2} --> {1;0}
                * 3) RingBufferIterations is there for performance reasons to makes sure the scan does not occur through the whole buffer for first time.
                */
            while (cumTickRange <= (UInt64)tickRange && RingBufferIndex >= 3 && !(USB2_BufferIterations == 0 && RingBufferIndex < USB2_BufferPosition16))
            {


                /* Vertexbuffer layout:
                    * 0                          Datapoint x 682 (=Screenresolution const)
                    * <------------------------------------------------------------------------->
                    *   <------------------------------------------------>
                    *                   1 DATAPOINT
                    *   <-------------> + <-----------> + <-------------->
                    *      LOWER             MID               UPPER
                    *   {Tick;Reading}  +  {Tick;Reading}  + {Tick;Reading}
                    *      0      1           2      3          4       5
                    *   Each vertexbuffer item = {Datapoint, # of occurences, Average}
                    *   
                    *   
                    */
                USB2_VertexBuffer[3][lastDataPosition] = vertexIndex; lastDataPosition++;
                USB2_VertexBuffer[3][lastDataPosition] = aDCReading; lastDataPosition++;


                if (USB2_Buffer[RingBufferIndex] != 32237) //To catch unwritten data area, which occur when USB cannt follow the selected data Packet size
                {
                    RingBufferIndex -= 2; //jump per two

                    cumTickRange += (UInt16) USB2_Buffer[RingBufferIndex - 1];//last element is an ADC_read
                    vertexIndex = 1.0f - 2.0f * ((float)cumTickRange / tickRange);
                    if (vertexIndex < -1.0f) break; //performance: time is ordered so as soon out of scope, skip
                    aDCReading = -aDCMoveFactor + (USB2_Buffer[RingBufferIndex] * aDCScaleFactor) / (voltDiv * USB2_Win_Div / 2);
                }
                else RingBufferIndex -= 2;
            };

            //Calculate trigger point
            Int32 triggerPoint = 0;
            
            for (Int32 i = 1; i < lastDataPosition-1; i+=2)
            {
                bool result1 = false;
                bool result2 = false;
                if (USB2_TriggerSlope == 1)
                { 
                    result2 = USB2_VertexBuffer[3][i] < triggerVPointf;
                    result1 = USB2_VertexBuffer[3][i+2] > triggerVPointf;
                }
                else
                {
                    result2 = USB2_VertexBuffer[3][i] > triggerVPointf;
                    result1 = USB2_VertexBuffer[3][i + 2] < triggerVPointf;
                }
                if (result1 && result2)
                {
                    triggerPoint = i-1;
                    /*TxtUSB2.Invoke(new Action(() =>
                    {
                        TxtUSB2.AppendText(triggerPoint.ToString());
                        TxtUSB2.AppendText("\r\n");
                    }));*/
                    break;
                }
            }

            //reposition screen
            float beta = triggerTPoint - USB2_VertexBuffer[3][triggerPoint];
            for (Int32 i = 0; i < USB2_VertexBuffer[3].Length-1 ; i+=2)
            {
                USB2_VertexBuffer[3][i] = USB2_VertexBuffer[3][i] + beta; 
            }

            //Signal Screendraw
            OGL_Screen_Status.i2_Set((Int32)lastDataPosition, OGL_Screen_Status.i2_USB2_OGL_LastDataPosition, null, 0); //Provide datapoint size to be drawn
            OGL_Screen_Status.i2_Set(0, OGL_Screen_Status.i2_USB2_OGL_ScreenDrawn, null, 0);
            while (Marshal.ReadByte(OGL_Screen_Status.i2_USB2_OGL_ScreenDrawn) == 0) ; //Wait for the screen to be drawn

            //Performance counting
            USB2_Watch.Stop(); USB2_WatchData[USB2_WatchDataCounter] = (double)USB2_Watch.ElapsedTicks / Stopwatch.Frequency; if (USB2_WatchDataCounter >= USB2_WatchData.Length - 1) USB2_WatchDataCounter = 0; else USB2_WatchDataCounter++;
            USB2_Watch.Reset(); USB2_Watch.Start();

        }

        public void Memory_Mode()
        {
            //Collect read data
            USB2_Watch.Start();

            //Initialize and check conditions
            float aDC_Clock = (ADC_Clock.FNumber == 0) ? throw new ArgumentException("Params error") : ADC_Clock.FNumber;
            float aDC_Vref = (ADC_VRef.FNumber == 0) ? throw new ArgumentException("Params error") : ADC_VRef.FNumber;
            float aDC_bitres = (ADC_BitRes.FNumber == 0) ? throw new ArgumentException("Params error") : ADC_BitRes.FNumber;
            float timeDiv = (TimeDiv.FNumber == 0) ? throw new ArgumentException("Params error") : TimeDiv.FNumber;
            float voltDiv = (VoltDiv.FNumber == 0) ? throw new ArgumentException("Params error") : VoltDiv.FNumber;

            float zeroTimePoint = ZeroTimePoint.FNumber;
            float zeroVoltPoint = ZeroVoltPoint.FNumber;

            float timeScaleFactor = (float)1000 / aDC_Clock;
            float aDCScaleFactor = aDC_Vref / (float)(Math.Pow(2, aDC_bitres) - 1); //mV/ADC_reading


            float timeMoveFactor = zeroTimePoint / (timeDiv * USB2_Win_Div / 2);
            float aDCMoveFactor = zeroVoltPoint / (voltDiv * USB2_Win_Div / 2); //mV/{0...100%}                        

            Array.Clear(USB2_VertexBuffer[3], 0, USB2_VertexBuffer[3].Length);
            

            Int32 RingBufferIndex = USB2_Buffer.Length - 1;
            Int32 LastDataPosition = 0;
            /*Search for first valable datapoint, ie. <>65535. Dispersed non-valable datapoints are assumed to not exist
                *We let RingBufferIndex=1 pass, as it would mean there is no valable data at all and it will not execute anything
                */

            UInt64 cumTickRange = 0;// USB2_Buffer[RingBufferIndex - 1];
            //float vertexIndex = 1.0f - 2.0f * ((float)cumTickRange / tickRange);
            float tickRange = timeDiv * USB2_Win_Div / 1000 * aDC_Clock;
            float alpha = -2.0f/tickRange;
            float beta = alpha * (zeroTimePoint * aDC_Clock/1000);
            float vertexIndex = alpha * (float)cumTickRange + beta;
            //float vertexIndex = 1.0f - 2.0f * ((float)cumTickRange / tickRange);
            //vertexIndex = vertexIndex + 1.0f / 2.0f - zeroTimePoint / (timeDiv * USB2_Win_Div);
            float aDCReading = -aDCMoveFactor + (USB2_Buffer[RingBufferIndex] * aDCScaleFactor) / (voltDiv * USB2_Win_Div / 2);

            //here we dont pursue performance yet (stop searching after out of bound time element)
            while (RingBufferIndex >= 3 ) //&& !(USB2_BufferIterations == 0 && RingBufferIndex < USB2_BufferPosition16)
            {


                /* Vertexbuffer layout:
                    * 0                          Datapoint x 682 (=Screenresolution const)
                    * <------------------------------------------------------------------------->
                    *   <------------------------------------------------>
                    *                   1 DATAPOINT
                    *   <-------------> + <-----------> + <-------------->
                    *      LOWER             MID               UPPER
                    *   {Tick;Reading}  +  {Tick;Reading}  + {Tick;Reading}
                    *      0      1           2      3          4       5
                    *   Each vertexbuffer item = {Datapoint, # of occurences, Average}
                    *   
                    *   
                    */

                USB2_VertexBuffer[3][LastDataPosition] = vertexIndex; LastDataPosition++;
                USB2_VertexBuffer[3][LastDataPosition] = aDCReading; LastDataPosition++;

                //Check if valid measurement
                if (USB2_Buffer[RingBufferIndex] != 32237 && USB2_Buffer[RingBufferIndex - 1] != 32237)
                {
                    RingBufferIndex -= 2; //jump per two
                    //if valid measurement calculate index and ADC_reading
                    cumTickRange += (UInt16)USB2_Buffer[RingBufferIndex - 1];//last element is an ADC_read
                    alpha = -2.0f / tickRange;
                    beta = alpha * (zeroTimePoint * aDC_Clock/1000);
                    vertexIndex = alpha * (float)cumTickRange + beta;
                    //vertexIndex = 1.0f - 2.0f * ((float)cumTickRange / tickRange);
                    //vertexIndex = vertexIndex + 1.0f / 2.0f - zeroTimePoint / (timeDiv * USB2_Win_Div);
                    aDCReading = -aDCMoveFactor + (USB2_Buffer[RingBufferIndex] * aDCScaleFactor) / (voltDiv * USB2_Win_Div / 2);

                    //Check if ADC reading within time bounds

                }
                else RingBufferIndex -= 2;
            };

            //Filter array
            UInt32 CopyPosition = 0;
            if (Marshal.ReadByte(OGL_Screen_Status.i2_USB2_OGL_Extrapolate) == 1)
            {
                float[] USB2_FilterBuffer = new float[USB2_VertexBuffer[3].Length];
                Array.Clear(USB2_FilterBuffer, 0, USB2_FilterBuffer.Length);

                for (UInt32 i = 0; i < USB2_VertexBuffer[3].Length - 2; i += 2)
                {
                    if (USB2_VertexBuffer[3][i] != 0 && USB2_VertexBuffer[3][i] >= -1 && USB2_VertexBuffer[3][i] <= 1)
                        if (USB2_VertexBuffer[3][i + 1] != 0 && USB2_VertexBuffer[3][i + 1] >= -1 && USB2_VertexBuffer[3][i + 1] <= 1)
                        {
                            USB2_FilterBuffer[CopyPosition] = USB2_VertexBuffer[3][i]; CopyPosition++;
                            USB2_FilterBuffer[CopyPosition] = USB2_VertexBuffer[3][i + 1]; CopyPosition++;
                        }
                }
                Array.Copy(USB2_FilterBuffer, USB2_VertexBuffer[3], USB2_VertexBuffer[3].Length);
            }

            //Signal Screendraw
            OGL_Screen_Status.i2_Set((Int32)LastDataPosition, OGL_Screen_Status.i2_USB2_OGL_LastDataPosition, null, 0); //Provide datapoint size to be drawn
            OGL_Screen_Status.i2_Set(0, OGL_Screen_Status.i2_USB2_OGL_ScreenDrawn, null, 0);
            while (Marshal.ReadByte(OGL_Screen_Status.i2_USB2_OGL_ScreenDrawn) == 0) ; //Wait for the screen to be drawn

            //Performance counting
            USB2_Watch.Stop(); USB2_WatchData[USB2_WatchDataCounter] = (double)USB2_Watch.ElapsedTicks / Stopwatch.Frequency; if (USB2_WatchDataCounter >= USB2_WatchData.Length - 1) USB2_WatchDataCounter = 0; else USB2_WatchDataCounter++;
            USB2_Watch.Reset(); USB2_Watch.Start();
        }

        public void SetTransformParams(object sender, EventArgs e)
        {
        //ZeroVoltPoint.Id = 1;
        //VoltDiv.Id = 2;
        //TimeDiv.Id = 3;
        //ADC_VRef.Id = 4;
        //ADC_BitRes.Id = 5;
        //ADC_Clock.Id = 6;
        try
        {
            Number nmbr = (Number)sender;
            switch (nmbr.Id)
            {
                case 1:

                    break;
                case 2:

                    break;
                case 3:
                case 6:

                    break;
                case 4:

                    break;
                case 5:

                    break;
                case 7:

                    break;

                default:

                    break;
            }
        }
        catch (ArgumentException exception)
        {
            switch (exception.Message)
            {
                default:
                    TxtUSB2.AppendText(exception.Message);
                    break;
                }
            }

        }

        private void ButMemoryMode_Click(object sender, EventArgs e)
        {
            try
            {
                if (ButMemoryMode.Text == "Start Memory Mode")
                {
                    ButMemoryMode.Enabled = false;
                    butUSB2.Enabled = false;
                    ButTUp.Enabled = true;
                    ButTDown.Enabled = true;
                    USB2_Mode = 5; //Set mode to memory mode
                    RdButUSBMode_Memory.Checked = true;
                    TxtUSB2.Clear();
                    ZeroTimePoint.FNumber = this.T_axis[(UInt16)(this.Lbl_T_Axis.GetUpperBound(0) + 1) / 2].FNumber;

                    ButMemoryMode.Text = "Stop Memory Mode";
                    ButMemoryMode.Enabled = true;

                    OGL_Screen_Status.i2_Set(0, OGL_Screen_Status.i2_USB2_OGL_Suspended, ChkListOGLSuspended, 0);

                    Task USB2_ProcessBuffer = Task.Run(() =>
                    {
                        //while (Marshal.ReadByte(USB2_Device_Status.i2_Started) == 0);
                        while (USB2_Mode == 5)
                        {
                            Memory_Mode();
                        }
                    });
                }
                else
                {
                    ButMemoryMode.Enabled = false;
                    butUSB2.Enabled = false;
                    ButTUp.Enabled = false;
                    ButTDown.Enabled = false;

                    //Reset TimeAxis container

                    for (int i = 0; i < this.Lbl_T_Axis.GetUpperBound(0) + 1; i++)
                    {
                        this.T_axis[i].FNumber = 0 + (i - this.Lbl_T_Axis.GetUpperBound(0)) * TimeDiv.FNumber;
                        this.Lbl_T_Axis[i].Text = this.T_axis[i].gStr("s");
                    } //Every time axis is redrawn zero point needs reset
                    ZeroTimePoint.FNumber = this.T_axis[(UInt16)(this.Lbl_T_Axis.GetUpperBound(0) + 1) / 2].FNumber;

                    USB2_Mode = 1;
                    RdButUSBMode_Standard.Checked = true;

                    OGL_Screen_Status.i2_Set(1, OGL_Screen_Status.i2_USB2_OGL_Suspended, ChkListOGLSuspended, 0);

                    TxtUSB2.Clear();
                    TxtUSB2.AppendText("Memory mode stopped");

                    ButMemoryMode.Text = "Start Memory Mode";
                    ButMemoryMode.Enabled = true; 
                    butUSB2.Enabled = true;
                }
            }
            catch (ArgumentException exception)
            {
                USB2_Device_Status.i2_Set(0, USB2_Device_Status.i2_Started, ChkListUSBState, 1);
                USB2_Device_Status.i2_Set(0, USB2_Device_Status.i2_Stopped, ChkListUSBState, 3);
                USB2_Device_Status.i2_Set(1, USB2_Device_Status.i2_Errored, ChkListUSBState, 4);
                switch (exception.Message)
                {
                    case "USB not initialized":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("USB could not be initialized within 500 ms");
                        break;
                    case "1":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Could not open the device.");
                        break;
                    case "2":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Could not get descriptor.");
                        break;
                    case "3":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Could not get interface.");
                        break;
                    case "4":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Failed to get USB pipe. ");
                        break;
                    case "5":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Interval information or Maximum bytes interval invalid. ");
                        break;
                    case "6":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Transfer characteristics couldn't be set. ");
                        break;
                    case "7":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Overlapped structure couldn't be set. ");
                        break;
                    case "8":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Overlapped events couldn't be set. ");
                        break;
                    case "9":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Isochronous Packets couldn't be created. ");
                        break;
                    case "10":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Isochronous buffer couldn't be registred. Probably wrongly sized databuffer.");
                        break;
                    case "11":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Pipe couldn't be reset. ");
                        break;
                    case "12":
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText("\r\n Buffer not set as a multiple of framesize. ");
                        break;
                    default:
                        TxtUSB2.Clear();
                        TxtUSB2.AppendText(exception.Message);
                        break;
                }
            }


            USB2_Device_Status.i2_UpdateChkList(ChkListUSBState);
            USB2_Device_Status.Action.i2_UpdateChkList(ChkListUSBInit);
           
        }

        private void CmbVoltDiv_Validating(object sender, CancelEventArgs e)
        {
            ComboBox ComboList = (ComboBox)sender;
            int count = 0;
            int index = 0;
            foreach (object item in ComboList.Items) { if (ComboList.Text == item.ToString()) count++; }
            if (count == 0)
            {
                index = ComboList.Items.Add(ComboList.Text);
                ComboList.SelectedIndex = index;
                CmbVoltDiv_SelectedValueChanged(sender, EventArgs.Empty);
            }
        }

        private void CmbVoltDiv_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == (Keys) 13) CmbTimeDiv.Select();
        }

        private void CmbTimeDiv_Validating(object sender, CancelEventArgs e)
        {
            ComboBox ComboList = (ComboBox)sender;
            int count = 0;
            int index = 0;
            foreach (object item in ComboList.Items) { if (ComboList.Text == item.ToString()) count++; }
            if (count == 0)
            {
                index = ComboList.Items.Add(ComboList.Text);
                ComboList.SelectedIndex = index;
                CmbTimeDiv_SelectedValueChanged(sender, EventArgs.Empty);
            }
        }

        private void CmbTimeDiv_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == (Keys)13) CmbVoltDiv.Select();
        }

        private void ButTUp_Click(object sender, EventArgs e)
        {
            TimeDiv.FNumber = TimeDiv.gFlt(this.CmbTimeDiv.Text);
            float Offset = TimeDiv.FNumber;//10f;
            for (int i = 0; i < this.Lbl_T_Axis.GetUpperBound(0) + 1; i++)
            {
                this.T_axis[i].FNumber = this.T_axis[i].FNumber + Offset;
                this.Lbl_T_Axis[i].Text = this.T_axis[i].gStr("s");
            }
            ZeroTimePoint.FNumber = this.T_axis[(UInt16)(this.Lbl_T_Axis.GetUpperBound(0) + 1) / 2].FNumber;
        }

        private void ButTDown_Click(object sender, EventArgs e)
        {
            TimeDiv.FNumber = TimeDiv.gFlt(this.CmbTimeDiv.Text);
            float Offset = -TimeDiv.FNumber;//10f;
            for (int i = 0; i < this.Lbl_T_Axis.GetUpperBound(0) + 1; i++)
            {
                this.T_axis[i].FNumber = this.T_axis[i].FNumber + Offset;
                this.Lbl_T_Axis[i].Text = this.T_axis[i].gStr("s");
            }
            ZeroTimePoint.FNumber = this.T_axis[(UInt16)(this.Lbl_T_Axis.GetUpperBound(0) + 1) / 2].FNumber;

        }

        private void TrckTTrigger_MouseUp(object sender, MouseEventArgs e)
        {
            USB2_VertexBufferTrigger[0] = (float) TrckTTrigger.Value / 1000 * 2f -1f;
            USB2_VertexBufferTrigger[1] = (float) -1.0f;
            USB2_VertexBufferTrigger[2] = (float) TrckTTrigger.Value / 1000 * 2f - 1f;
            USB2_VertexBufferTrigger[3] = (float) 1.0f;
        }
        private void TrckVTrigger_MouseUp(object sender, MouseEventArgs e)
        {
            USB2_VertexBufferTrigger[4] = (float)-1.0f;
            USB2_VertexBufferTrigger[5] = (float)TrckVTrigger.Value / 1000 * 2f - 1f;
            USB2_VertexBufferTrigger[6] = (float)1.0f;
            USB2_VertexBufferTrigger[7] = (float)TrckVTrigger.Value / 1000 * 2f - 1f;
        }

        private void RdButUSBMode_Triggered_CheckedChanged(object sender, EventArgs e)
        {
            USB2_Mode = 4;
        }

        private void RdButUSBMode_Roll_CheckedChanged(object sender, EventArgs e)
        {
            USB2_Mode = 3;
        }

        private void RdButUSBMode_Standard_CheckedChanged(object sender, EventArgs e)
        {
            USB2_Mode = 1;
        }

        private void RdButUSBMode_Memory_CheckedChanged(object sender, EventArgs e)
        {
            USB2_Mode = 5;
        }

        private void ChkoptionExtrapolated_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkoptionExtrapolated.Checked == true)
                OGL_Screen_Status.i2_Set(1, OGL_Screen_Status.i2_USB2_OGL_Extrapolate, null, 0);
            else
                OGL_Screen_Status.i2_Set(0, OGL_Screen_Status.i2_USB2_OGL_Extrapolate, null, 0);
        }

        private void RdButTrSlopeUp_CheckedChanged(object sender, EventArgs e)
        {
            USB2_TriggerSlope = 2;
        }

        private void RdButTrSlopeDown_CheckedChanged(object sender, EventArgs e)
        {
            USB2_TriggerSlope = 1;
        }
    }
}

