using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Osc_v5;

namespace Osc_v5
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>


        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
              
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            //TskOpenGL.Dispose();
            NativeMethods.OGL_Window_Dispose();
            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ButStartStopADC = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CmbTimeDiv = new System.Windows.Forms.ComboBox();
            this.TxtZeroVolt = new System.Windows.Forms.TextBox();
            this.CmbVoltDiv = new System.Windows.Forms.ComboBox();
            this.ButSetToZero = new System.Windows.Forms.Button();
            this.ButVUp = new System.Windows.Forms.Button();
            this.ButVDown = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtADC_Vref = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtADC_Res = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblFPS = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.CmbRB_Size = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.LblADC_Res_V = new System.Windows.Forms.Label();
            this.ChkoptionExtrapolated = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.LblTmrBuildScreen = new System.Windows.Forms.Label();
            this.LblTmrDrawScreen = new System.Windows.Forms.Label();
            this.LblTmrTransfdata = new System.Windows.Forms.Label();
            this.LblTmrWaitfordata = new System.Windows.Forms.Label();
            this.CmbmsSleep = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.LblTmrSuspended = new System.Windows.Forms.Label();
            this.ChkOptionFilterOutliers = new System.Windows.Forms.CheckBox();
            this.TxtUSB_State = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.TxtADC_Clock = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LblKSamplePerSec = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label23 = new System.Windows.Forms.Label();
            this.CmbStatRefresh = new System.Windows.Forms.ComboBox();
            this.Lbldpx = new System.Windows.Forms.Label();
            this.Lbldfx = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.LblUSBConnected = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label22 = new System.Windows.Forms.Label();
            this.LblUSBTransfers = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.TxtADCCheckPatternLength = new System.Windows.Forms.MaskedTextBox();
            this.TxtADCMeasureLength = new System.Windows.Forms.MaskedTextBox();
            this.TxtADCTickLength = new System.Windows.Forms.MaskedTextBox();
            this.LblADCCheckPatternLength = new System.Windows.Forms.Label();
            this.TxtADCCheckPattern = new System.Windows.Forms.MaskedTextBox();
            this.LblADCCheckPattern = new System.Windows.Forms.Label();
            this.LblADCMeasureLength = new System.Windows.Forms.Label();
            this.LblADCTickLength = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.TmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButStartStopADC
            // 
            this.ButStartStopADC.Location = new System.Drawing.Point(867, 138);
            this.ButStartStopADC.Name = "ButStartStopADC";
            this.ButStartStopADC.Size = new System.Drawing.Size(71, 75);
            this.ButStartStopADC.TabIndex = 1;
            this.ButStartStopADC.Text = "Connect to device";
            this.ButStartStopADC.UseVisualStyleBackColor = true;
            this.ButStartStopADC.Click += new System.EventHandler(this.ButStartStopADC_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Controls.Add(this.CmbTimeDiv);
            this.panel1.Controls.Add(this.TxtZeroVolt);
            this.panel1.Controls.Add(this.CmbVoltDiv);
            this.panel1.Controls.Add(this.ButSetToZero);
            this.panel1.Controls.Add(this.ButVUp);
            this.panel1.Controls.Add(this.ButVDown);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(0, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(837, 816);
            this.panel1.TabIndex = 3;
            // 
            // CmbTimeDiv
            // 
            this.CmbTimeDiv.BackColor = System.Drawing.SystemColors.WindowText;
            this.CmbTimeDiv.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.CmbTimeDiv.FormatString = "0.0mV";
            this.CmbTimeDiv.FormattingEnabled = true;
            this.CmbTimeDiv.Items.AddRange(new object[] {
            "10s",
            "1s",
            "100ms",
            "10ms",
            "1ms",
            "100us",
            "10us",
            "1us"});
            this.CmbTimeDiv.Location = new System.Drawing.Point(770, 791);
            this.CmbTimeDiv.Name = "CmbTimeDiv";
            this.CmbTimeDiv.Size = new System.Drawing.Size(67, 21);
            this.CmbTimeDiv.TabIndex = 15;
            this.CmbTimeDiv.SelectedValueChanged += new System.EventHandler(this.CmbTimeDiv_SelectedValueChanged);
            // 
            // TxtZeroVolt
            // 
            this.TxtZeroVolt.Location = new System.Drawing.Point(764, 422);
            this.TxtZeroVolt.Name = "TxtZeroVolt";
            this.TxtZeroVolt.Size = new System.Drawing.Size(72, 20);
            this.TxtZeroVolt.TabIndex = 14;
            this.TxtZeroVolt.Visible = false;
            this.TxtZeroVolt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtZeroVolt_KeyPress);
            // 
            // CmbVoltDiv
            // 
            this.CmbVoltDiv.BackColor = System.Drawing.SystemColors.WindowText;
            this.CmbVoltDiv.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.CmbVoltDiv.FormatString = "0.0mV";
            this.CmbVoltDiv.FormattingEnabled = true;
            this.CmbVoltDiv.Items.AddRange(new object[] {
            "1mV",
            "5mV",
            "10mV",
            "100mV",
            "1000mV"});
            this.CmbVoltDiv.Location = new System.Drawing.Point(770, 764);
            this.CmbVoltDiv.Name = "CmbVoltDiv";
            this.CmbVoltDiv.Size = new System.Drawing.Size(67, 21);
            this.CmbVoltDiv.TabIndex = 14;
            this.CmbVoltDiv.SelectedValueChanged += new System.EventHandler(this.CmbVoltDiv_SelectedValueChanged);
            // 
            // ButSetToZero
            // 
            this.ButSetToZero.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ButSetToZero.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ButSetToZero.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButSetToZero.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.ButSetToZero.Location = new System.Drawing.Point(817, 400);
            this.ButSetToZero.Margin = new System.Windows.Forms.Padding(0);
            this.ButSetToZero.Name = "ButSetToZero";
            this.ButSetToZero.Size = new System.Drawing.Size(20, 20);
            this.ButSetToZero.TabIndex = 13;
            this.ButSetToZero.Text = "0";
            this.ButSetToZero.UseVisualStyleBackColor = false;
            this.ButSetToZero.Click += new System.EventHandler(this.SetToZero_Click);
            // 
            // ButVUp
            // 
            this.ButVUp.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ButVUp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ButVUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButVUp.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.ButVUp.Location = new System.Drawing.Point(817, 381);
            this.ButVUp.Margin = new System.Windows.Forms.Padding(0);
            this.ButVUp.Name = "ButVUp";
            this.ButVUp.Size = new System.Drawing.Size(20, 20);
            this.ButVUp.TabIndex = 11;
            this.ButVUp.Text = "+";
            this.ButVUp.UseVisualStyleBackColor = false;
            this.ButVUp.Click += new System.EventHandler(this.ButVUp_Click);
            // 
            // ButVDown
            // 
            this.ButVDown.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ButVDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ButVDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButVDown.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.ButVDown.Location = new System.Drawing.Point(817, 419);
            this.ButVDown.Margin = new System.Windows.Forms.Padding(0);
            this.ButVDown.Name = "ButVDown";
            this.ButVDown.Size = new System.Drawing.Size(20, 20);
            this.ButVDown.TabIndex = 9;
            this.ButVDown.Text = "-";
            this.ButVDown.UseVisualStyleBackColor = false;
            this.ButVDown.Click += new System.EventHandler(this.ButVDown_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Info;
            this.panel2.Location = new System.Drawing.Point(68, 68);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(682, 682);
            this.panel2.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "ADC Voltage ref:";
            // 
            // TxtADC_Vref
            // 
            this.TxtADC_Vref.Location = new System.Drawing.Point(49, 29);
            this.TxtADC_Vref.Mask = "0.0V";
            this.TxtADC_Vref.Name = "TxtADC_Vref";
            this.TxtADC_Vref.Size = new System.Drawing.Size(35, 20);
            this.TxtADC_Vref.TabIndex = 8;
            this.TxtADC_Vref.Text = "33";
            this.TxtADC_Vref.ValidatingType = typeof(int);
            this.TxtADC_Vref.Leave += new System.EventHandler(this.TxtADC_Vref_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "ADC resolution:";
            // 
            // TxtADC_Res
            // 
            this.TxtADC_Res.Location = new System.Drawing.Point(50, 28);
            this.TxtADC_Res.Mask = "00bit";
            this.TxtADC_Res.Name = "TxtADC_Res";
            this.TxtADC_Res.Size = new System.Drawing.Size(35, 20);
            this.TxtADC_Res.TabIndex = 10;
            this.TxtADC_Res.Text = "12";
            this.TxtADC_Res.ValidatingType = typeof(int);
            this.TxtADC_Res.Leave += new System.EventHandler(this.TxtADC_Res_Leave);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(7, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "OpenGL - ms / frame:";
            // 
            // lblFPS
            // 
            this.lblFPS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblFPS.Location = new System.Drawing.Point(119, 12);
            this.lblFPS.Name = "lblFPS";
            this.lblFPS.Size = new System.Drawing.Size(61, 15);
            this.lblFPS.TabIndex = 11;
            this.lblFPS.Text = "Init...";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(132, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Datapoints read per frame:";
            // 
            // CmbRB_Size
            // 
            this.CmbRB_Size.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbRB_Size.Items.AddRange(new object[] {
            "65536‬",
            "32768‬",
            "16384‬",
            "8192‬",
            "4096‬",
            "2048‬",
            "1024"});
            this.CmbRB_Size.Location = new System.Drawing.Point(30, 50);
            this.CmbRB_Size.Name = "CmbRB_Size";
            this.CmbRB_Size.Size = new System.Drawing.Size(72, 21);
            this.CmbRB_Size.TabIndex = 11;
            this.CmbRB_Size.SelectionChangeCommitted += new System.EventHandler(this.CmbRB_Size_SelectionChangeCommitted);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(132, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(139, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Voltage interval (ADC resol):";
            // 
            // LblADC_Res_V
            // 
            this.LblADC_Res_V.AutoSize = true;
            this.LblADC_Res_V.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LblADC_Res_V.Location = new System.Drawing.Point(186, 30);
            this.LblADC_Res_V.Name = "LblADC_Res_V";
            this.LblADC_Res_V.Size = new System.Drawing.Size(32, 15);
            this.LblADC_Res_V.TabIndex = 20;
            this.LblADC_Res_V.Text = "Init...";
            // 
            // ChkoptionExtrapolated
            // 
            this.ChkoptionExtrapolated.AutoSize = true;
            this.ChkoptionExtrapolated.Checked = true;
            this.ChkoptionExtrapolated.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkoptionExtrapolated.Location = new System.Drawing.Point(869, 219);
            this.ChkoptionExtrapolated.Name = "ChkoptionExtrapolated";
            this.ChkoptionExtrapolated.Size = new System.Drawing.Size(110, 17);
            this.ChkoptionExtrapolated.TabIndex = 2;
            this.ChkoptionExtrapolated.Text = "Extrapolate points";
            this.ChkoptionExtrapolated.UseVisualStyleBackColor = true;
            this.ChkoptionExtrapolated.CheckedChanged += new System.EventHandler(this.ChkoptionExtrapolated_CheckedChanged);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(28, 90);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(114, 15);
            this.label10.TabIndex = 24;
            this.label10.Text = "Time to build screen:";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(28, 109);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(114, 15);
            this.label11.TabIndex = 25;
            this.label11.Text = "Time to draw screen:";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(28, 131);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(114, 15);
            this.label12.TabIndex = 26;
            this.label12.Text = "Time to transform data:";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(28, 153);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(114, 15);
            this.label13.TabIndex = 27;
            this.label13.Text = "Time to collect data:";
            // 
            // LblTmrBuildScreen
            // 
            this.LblTmrBuildScreen.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LblTmrBuildScreen.Location = new System.Drawing.Point(161, 90);
            this.LblTmrBuildScreen.Name = "LblTmrBuildScreen";
            this.LblTmrBuildScreen.Size = new System.Drawing.Size(82, 15);
            this.LblTmrBuildScreen.TabIndex = 28;
            this.LblTmrBuildScreen.Text = "Init...";
            // 
            // LblTmrDrawScreen
            // 
            this.LblTmrDrawScreen.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LblTmrDrawScreen.Location = new System.Drawing.Point(161, 109);
            this.LblTmrDrawScreen.Name = "LblTmrDrawScreen";
            this.LblTmrDrawScreen.Size = new System.Drawing.Size(82, 15);
            this.LblTmrDrawScreen.TabIndex = 29;
            this.LblTmrDrawScreen.Text = "Init...";
            // 
            // LblTmrTransfdata
            // 
            this.LblTmrTransfdata.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LblTmrTransfdata.Location = new System.Drawing.Point(161, 131);
            this.LblTmrTransfdata.Name = "LblTmrTransfdata";
            this.LblTmrTransfdata.Size = new System.Drawing.Size(82, 15);
            this.LblTmrTransfdata.TabIndex = 30;
            this.LblTmrTransfdata.Text = "Init...";
            // 
            // LblTmrWaitfordata
            // 
            this.LblTmrWaitfordata.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LblTmrWaitfordata.Location = new System.Drawing.Point(161, 153);
            this.LblTmrWaitfordata.Name = "LblTmrWaitfordata";
            this.LblTmrWaitfordata.Size = new System.Drawing.Size(82, 15);
            this.LblTmrWaitfordata.TabIndex = 31;
            this.LblTmrWaitfordata.Text = "Init...";
            // 
            // CmbmsSleep
            // 
            this.CmbmsSleep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbmsSleep.FormattingEnabled = true;
            this.CmbmsSleep.Items.AddRange(new object[] {
            "As fast as possible: inf",
            "10",
            "20",
            "30",
            "40",
            "50",
            "60"});
            this.CmbmsSleep.Location = new System.Drawing.Point(161, 223);
            this.CmbmsSleep.Name = "CmbmsSleep";
            this.CmbmsSleep.Size = new System.Drawing.Size(68, 21);
            this.CmbmsSleep.TabIndex = 12;
            this.CmbmsSleep.SelectedIndexChanged += new System.EventHandler(this.CmbmsSleep_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(28, 226);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(30, 13);
            this.label14.TabIndex = 33;
            this.label14.Text = "FPS:";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(28, 175);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(114, 15);
            this.label15.TabIndex = 34;
            this.label15.Text = "Time suspended:";
            // 
            // LblTmrSuspended
            // 
            this.LblTmrSuspended.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LblTmrSuspended.Location = new System.Drawing.Point(161, 175);
            this.LblTmrSuspended.Name = "LblTmrSuspended";
            this.LblTmrSuspended.Size = new System.Drawing.Size(82, 15);
            this.LblTmrSuspended.TabIndex = 35;
            this.LblTmrSuspended.Text = "Init...";
            // 
            // ChkOptionFilterOutliers
            // 
            this.ChkOptionFilterOutliers.AutoSize = true;
            this.ChkOptionFilterOutliers.Checked = true;
            this.ChkOptionFilterOutliers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkOptionFilterOutliers.Location = new System.Drawing.Point(869, 242);
            this.ChkOptionFilterOutliers.Name = "ChkOptionFilterOutliers";
            this.ChkOptionFilterOutliers.Size = new System.Drawing.Size(84, 17);
            this.ChkOptionFilterOutliers.TabIndex = 3;
            this.ChkOptionFilterOutliers.Text = "Filter outliers";
            this.ChkOptionFilterOutliers.UseVisualStyleBackColor = true;
            this.ChkOptionFilterOutliers.CheckedChanged += new System.EventHandler(this.ChkOptionFilterOutliers_CheckedChanged);
            // 
            // TxtUSB_State
            // 
            this.TxtUSB_State.Location = new System.Drawing.Point(944, 181);
            this.TxtUSB_State.Multiline = true;
            this.TxtUSB_State.Name = "TxtUSB_State";
            this.TxtUSB_State.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtUSB_State.Size = new System.Drawing.Size(219, 32);
            this.TxtUSB_State.TabIndex = 37;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(159, 11);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(106, 13);
            this.label16.TabIndex = 38;
            this.label16.Text = "ADC Tick frequency:";
            // 
            // TxtADC_Clock
            // 
            this.TxtADC_Clock.Location = new System.Drawing.Point(168, 29);
            this.TxtADC_Clock.Mask = "0000000Hz";
            this.TxtADC_Clock.Name = "TxtADC_Clock";
            this.TxtADC_Clock.Size = new System.Drawing.Size(67, 20);
            this.TxtADC_Clock.TabIndex = 9;
            this.TxtADC_Clock.Text = "8000000";
            this.TxtADC_Clock.ValidatingType = typeof(int);
            this.TxtADC_Clock.Leave += new System.EventHandler(this.TxtADC_Clock_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 201);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "ADC - KSps:";
            // 
            // LblKSamplePerSec
            // 
            this.LblKSamplePerSec.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LblKSamplePerSec.Location = new System.Drawing.Point(161, 200);
            this.LblKSamplePerSec.Name = "LblKSamplePerSec";
            this.LblKSamplePerSec.Size = new System.Drawing.Size(82, 15);
            this.LblKSamplePerSec.TabIndex = 41;
            this.LblKSamplePerSec.Text = "Init...";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1039, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(106, 31);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 42;
            this.pictureBox1.TabStop = false;
            // 
            // label17
            // 
            this.label17.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label17.Font = new System.Drawing.Font("Candara", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(1039, 45);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(104, 15);
            this.label17.TabIndex = 43;
            this.label17.Text = "Isochronous USB";
            // 
            // label18
            // 
            this.label18.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label18.Font = new System.Drawing.Font("Candara", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(1039, 61);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(104, 21);
            this.label18.TabIndex = 44;
            this.label18.Text = "Multithreading";
            // 
            // label19
            // 
            this.label19.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label19.Font = new System.Drawing.Font("Candara", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(1039, 76);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(104, 21);
            this.label19.TabIndex = 45;
            this.label19.Text = "C WinUSB DLL";
            // 
            // label20
            // 
            this.label20.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label20.Font = new System.Drawing.Font("Candara", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(1039, 91);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(104, 21);
            this.label20.TabIndex = 46;
            this.label20.Text = "C OpenGL DLL";
            // 
            // label21
            // 
            this.label21.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label21.Font = new System.Drawing.Font("Candara", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(1039, 106);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(104, 15);
            this.label21.TabIndex = 47;
            this.label21.Text = "C# interface";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LblADC_Res_V);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.TxtADC_Res);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(878, 412);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 55);
            this.groupBox1.TabIndex = 48;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.TxtADC_Clock);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.TxtADC_Vref);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(878, 355);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(279, 56);
            this.groupBox2.TabIndex = 49;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label23);
            this.groupBox3.Controls.Add(this.CmbStatRefresh);
            this.groupBox3.Controls.Add(this.Lbldpx);
            this.groupBox3.Controls.Add(this.LblKSamplePerSec);
            this.groupBox3.Controls.Add(this.Lbldfx);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.LblTmrSuspended);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.CmbmsSleep);
            this.groupBox3.Controls.Add(this.LblTmrWaitfordata);
            this.groupBox3.Controls.Add(this.LblTmrTransfdata);
            this.groupBox3.Controls.Add(this.LblTmrDrawScreen);
            this.groupBox3.Controls.Add(this.LblTmrBuildScreen);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.lblFPS);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(878, 548);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(279, 282);
            this.groupBox3.TabIndex = 50;
            this.groupBox3.TabStop = false;
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(132, 258);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(82, 18);
            this.label23.TabIndex = 61;
            this.label23.Text = "Refresh every:";
            // 
            // CmbStatRefresh
            // 
            this.CmbStatRefresh.FormattingEnabled = true;
            this.CmbStatRefresh.Items.AddRange(new object[] {
            "500",
            "1000",
            "2000",
            "5000",
            "Never"});
            this.CmbStatRefresh.Location = new System.Drawing.Point(216, 255);
            this.CmbStatRefresh.Name = "CmbStatRefresh";
            this.CmbStatRefresh.Size = new System.Drawing.Size(57, 21);
            this.CmbStatRefresh.TabIndex = 65;
            this.CmbStatRefresh.SelectedIndexChanged += new System.EventHandler(this.CmbStatRefresh_SelectedIndexChanged);
            // 
            // Lbldpx
            // 
            this.Lbldpx.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Lbldpx.Location = new System.Drawing.Point(161, 41);
            this.Lbldpx.Name = "Lbldpx";
            this.Lbldpx.Size = new System.Drawing.Size(82, 15);
            this.Lbldpx.TabIndex = 64;
            this.Lbldpx.Text = "Init...";
            // 
            // Lbldfx
            // 
            this.Lbldfx.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Lbldfx.Location = new System.Drawing.Point(161, 66);
            this.Lbldfx.Name = "Lbldfx";
            this.Lbldfx.Size = new System.Drawing.Size(82, 15);
            this.Lbldfx.TabIndex = 63;
            this.Lbldfx.Text = "Init...";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(124, 13);
            this.label7.TabIndex = 62;
            this.label7.Text = "% screen shifted / frame:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 13);
            this.label6.TabIndex = 61;
            this.label6.Text = "Pixels shifted per frame:";
            // 
            // LblUSBConnected
            // 
            this.LblUSBConnected.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LblUSBConnected.Location = new System.Drawing.Point(944, 141);
            this.LblUSBConnected.Name = "LblUSBConnected";
            this.LblUSBConnected.Size = new System.Drawing.Size(218, 37);
            this.LblUSBConnected.TabIndex = 51;
            this.LblUSBConnected.Text = "Not connected to device";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label22);
            this.groupBox4.Controls.Add(this.LblUSBTransfers);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.CmbRB_Size);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Location = new System.Drawing.Point(877, 467);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(283, 82);
            this.groupBox4.TabIndex = 52;
            this.groupBox4.TabStop = false;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(154, 29);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(98, 13);
            this.label22.TabIndex = 17;
            this.label22.Text = "( x1024byte / 1 ms)";
            // 
            // LblUSBTransfers
            // 
            this.LblUSBTransfers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LblUSBTransfers.Location = new System.Drawing.Point(183, 50);
            this.LblUSBTransfers.Name = "LblUSBTransfers";
            this.LblUSBTransfers.Size = new System.Drawing.Size(35, 15);
            this.LblUSBTransfers.TabIndex = 16;
            this.LblUSBTransfers.Text = "Init...";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(138, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(127, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Number of USB transfers:";
            // 
            // TxtADCCheckPatternLength
            // 
            this.TxtADCCheckPatternLength.Location = new System.Drawing.Point(135, 10);
            this.TxtADCCheckPatternLength.Mask = "00bit";
            this.TxtADCCheckPatternLength.Name = "TxtADCCheckPatternLength";
            this.TxtADCCheckPatternLength.Size = new System.Drawing.Size(35, 20);
            this.TxtADCCheckPatternLength.TabIndex = 4;
            this.TxtADCCheckPatternLength.Text = "7";
            this.TxtADCCheckPatternLength.ValidatingType = typeof(int);
            this.TxtADCCheckPatternLength.Leave += new System.EventHandler(this.TxtADCCheckPatternLength_Leave);
            // 
            // TxtADCMeasureLength
            // 
            this.TxtADCMeasureLength.Location = new System.Drawing.Point(135, 35);
            this.TxtADCMeasureLength.Mask = "00bit";
            this.TxtADCMeasureLength.Name = "TxtADCMeasureLength";
            this.TxtADCMeasureLength.Size = new System.Drawing.Size(35, 20);
            this.TxtADCMeasureLength.TabIndex = 6;
            this.TxtADCMeasureLength.Text = "12";
            this.TxtADCMeasureLength.ValidatingType = typeof(int);
            this.TxtADCMeasureLength.Leave += new System.EventHandler(this.TxtADCMeasureLength_Leave);
            // 
            // TxtADCTickLength
            // 
            this.TxtADCTickLength.Location = new System.Drawing.Point(135, 60);
            this.TxtADCTickLength.Mask = "00bit";
            this.TxtADCTickLength.Name = "TxtADCTickLength";
            this.TxtADCTickLength.Size = new System.Drawing.Size(35, 20);
            this.TxtADCTickLength.TabIndex = 7;
            this.TxtADCTickLength.Text = "12";
            this.TxtADCTickLength.ValidatingType = typeof(int);
            this.TxtADCTickLength.Leave += new System.EventHandler(this.TxtADCTickLength_Leave);
            // 
            // LblADCCheckPatternLength
            // 
            this.LblADCCheckPatternLength.AutoSize = true;
            this.LblADCCheckPatternLength.Location = new System.Drawing.Point(9, 13);
            this.LblADCCheckPatternLength.Name = "LblADCCheckPatternLength";
            this.LblADCCheckPatternLength.Size = new System.Drawing.Size(114, 13);
            this.LblADCCheckPatternLength.TabIndex = 40;
            this.LblADCCheckPatternLength.Text = "Check Pattern Length:";
            // 
            // TxtADCCheckPattern
            // 
            this.TxtADCCheckPattern.AsciiOnly = true;
            this.TxtADCCheckPattern.Location = new System.Drawing.Point(226, 10);
            this.TxtADCCheckPattern.Mask = "0xAA";
            this.TxtADCCheckPattern.Name = "TxtADCCheckPattern";
            this.TxtADCCheckPattern.Size = new System.Drawing.Size(35, 20);
            this.TxtADCCheckPattern.TabIndex = 5;
            this.TxtADCCheckPattern.Text = "055";
            this.TxtADCCheckPattern.ValidatingType = typeof(int);
            this.TxtADCCheckPattern.Leave += new System.EventHandler(this.TxtADCCheckPattern_Leave);
            // 
            // LblADCCheckPattern
            // 
            this.LblADCCheckPattern.AutoSize = true;
            this.LblADCCheckPattern.Location = new System.Drawing.Point(179, 13);
            this.LblADCCheckPattern.Name = "LblADCCheckPattern";
            this.LblADCCheckPattern.Size = new System.Drawing.Size(44, 13);
            this.LblADCCheckPattern.TabIndex = 57;
            this.LblADCCheckPattern.Text = "Pattern:";
            // 
            // LblADCMeasureLength
            // 
            this.LblADCMeasureLength.AutoSize = true;
            this.LblADCMeasureLength.Location = new System.Drawing.Point(9, 38);
            this.LblADCMeasureLength.Name = "LblADCMeasureLength";
            this.LblADCMeasureLength.Size = new System.Drawing.Size(112, 13);
            this.LblADCMeasureLength.TabIndex = 58;
            this.LblADCMeasureLength.Text = "ADC Measure Length:";
            // 
            // LblADCTickLength
            // 
            this.LblADCTickLength.AutoSize = true;
            this.LblADCTickLength.Location = new System.Drawing.Point(9, 63);
            this.LblADCTickLength.Name = "LblADCTickLength";
            this.LblADCTickLength.Size = new System.Drawing.Size(92, 13);
            this.LblADCTickLength.TabIndex = 59;
            this.LblADCTickLength.Text = "ADC Tick Length:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.LblADCTickLength);
            this.groupBox5.Controls.Add(this.LblADCMeasureLength);
            this.groupBox5.Controls.Add(this.LblADCCheckPattern);
            this.groupBox5.Controls.Add(this.TxtADCCheckPattern);
            this.groupBox5.Controls.Add(this.LblADCCheckPatternLength);
            this.groupBox5.Controls.Add(this.TxtADCTickLength);
            this.groupBox5.Controls.Add(this.TxtADCMeasureLength);
            this.groupBox5.Controls.Add(this.TxtADCCheckPatternLength);
            this.groupBox5.Location = new System.Drawing.Point(877, 263);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(279, 91);
            this.groupBox5.TabIndex = 60;
            this.groupBox5.TabStop = false;
            // 
            // TmrRefresh
            // 
            this.TmrRefresh.Interval = 1000;
            this.TmrRefresh.Tick += new System.EventHandler(this.TmrRefresh_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1171, 858);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.LblUSBConnected);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.TxtUSB_State);
            this.Controls.Add(this.ChkOptionFilterOutliers);
            this.Controls.Add(this.ChkoptionExtrapolated);
            this.Controls.Add(this.ButStartStopADC);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "YetAnother USB Oscilloscope";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button ButStartStopADC;
        private Panel panel1;
        private Panel panel2;
        private Button ButSetToZero;
        private Button ButVUp;
        private Button ButVDown;
        private ComboBox CmbVoltDiv;
        private TextBox TxtZeroVolt;
        private ComboBox CmbTimeDiv;
        private Label label2;
        private MaskedTextBox TxtADC_Vref;
        private Label label3;
        private MaskedTextBox TxtADC_Res;
        private Label label4;
        private Label lblFPS;
        private Label label5;
        private ComboBox CmbRB_Size;
        private Label label8;
        private Label LblADC_Res_V;
        private CheckBox ChkoptionExtrapolated;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label LblTmrBuildScreen;
        private Label LblTmrDrawScreen;
        private Label LblTmrTransfdata;
        private Label LblTmrWaitfordata;
        private ComboBox CmbmsSleep;
        private Label label14;
        private Label label15;
        private Label LblTmrSuspended;
        private CheckBox ChkOptionFilterOutliers;
        private TextBox TxtUSB_State;
        private Label label16;
        private MaskedTextBox TxtADC_Clock;
        private Label label1;
        private Label LblKSamplePerSec;
        private PictureBox pictureBox1;
        private Label label17;
        private Label label18;
        private Label label19;
        private Label label20;
        private Label label21;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label LblUSBConnected;
        private GroupBox groupBox4;
        private Label LblUSBTransfers;
        private Label label9;
        private MaskedTextBox TxtADCCheckPatternLength;
        private MaskedTextBox TxtADCMeasureLength;
        private MaskedTextBox TxtADCTickLength;
        private Label LblADCCheckPatternLength;
        private MaskedTextBox TxtADCCheckPattern;
        private Label LblADCCheckPattern;
        private Label LblADCMeasureLength;
        private Label LblADCTickLength;
        private GroupBox groupBox5;
        private Label Lbldpx;
        private Label Lbldfx;
        private Label label7;
        private Label label6;
        private Label label22;
        private Timer TmrRefresh;
        private Label label23;
        private ComboBox CmbStatRefresh;
    }
}



internal static class NativeMethods
{
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void OGL_Window_Init();
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void OGL_Window_Dispose();
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void OGL_Window_SetPos(int x, int y);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetWin32Window();

    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool SendData(UInt32[] Readings, UInt32[] Ticks, Int32 RB_L, Int32 RB_U);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool DataProcessed();

    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool SendTransformParams(UInt16 ADC_bitres, UInt16 ADC_Vref, UInt16 zeroVolt, UInt16 VoltDiv,
        float TimeDiv, UInt32 ADC_Clock, bool optionExtrapolated, float msSleep, bool optionFilterOutliers);

    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool GetStats(float[] InStats);

//    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
//    public static extern bool SetNumberOfSamples(UInt32 NumberOfSamples);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void DrawData();
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool SetSuspended(bool Suspended);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Dispose();
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool SetReadingLock(bool ReadLock);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool SetDataProcessed(bool DataProcessed);

    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_1.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB_Init();

    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_1.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool USB_GetState(int[] InResults,long[] InhResults, UInt16[] USBVendor, UInt16[] USBProduct, UInt16[] USBbcd); //need to use Native types, bool for example does not work
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_1.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool USB_Sizeof_USB_Result(UInt16[] Sizeof_InResults, UInt16[] Sizeof_InhResults);

    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_1.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool USB_SetBeginReading(bool BeginReading);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_1.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool USB_ReadbufferTransfered(byte[] USB_readbuffer, UInt32[] ADC_Reading, UInt32[] ADC_Tick, Int32 RB_L, Int32 RB_U);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_1.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool USB_ReadyToRead();
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_1.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool USB_GetEndReading();
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_1.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool USB_SetReadbufferTransfered();
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_1.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB_SetCloseRequest();
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_1.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool USB_ReadyToReportState();
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_1.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool USB_Set_Isoch_Transfer(UInt32 NumberOfSamples, UInt32 ADC_Check_Pattern, UInt32 ADC_Check_Pattern_Length, UInt32 ADC_Measure_Length, UInt32 ADC_Tick_Length);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_1.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern UInt32 USB_GetIsochTransferCount();




}

