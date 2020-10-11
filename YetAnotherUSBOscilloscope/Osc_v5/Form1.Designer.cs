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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.TrckVTrigger = new System.Windows.Forms.TrackBar();
            this.ButTDown = new System.Windows.Forms.Button();
            this.ButTUp = new System.Windows.Forms.Button();
            this.TrckTTrigger = new System.Windows.Forms.TrackBar();
            this.CmbTimeDiv = new System.Windows.Forms.ComboBox();
            this.TxtZeroVolt = new System.Windows.Forms.TextBox();
            this.CmbVoltDiv = new System.Windows.Forms.ComboBox();
            this.ButSetToZero = new System.Windows.Forms.Button();
            this.ButVUp = new System.Windows.Forms.Button();
            this.ButVDown = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtADC_Vref = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtADC_Res = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblFPS = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.LblADC_Res_V = new System.Windows.Forms.Label();
            this.ChkoptionExtrapolated = new System.Windows.Forms.CheckBox();
            this.CmbmsSleep = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.LblTmrSuspended = new System.Windows.Forms.Label();
            this.ChkOptionFilterOutliers = new System.Windows.Forms.CheckBox();
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
            this.TmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.TxtUSB2 = new System.Windows.Forms.TextBox();
            this.butUSB2 = new System.Windows.Forms.Button();
            this.ChkListUSBInit = new System.Windows.Forms.CheckedListBox();
            this.ChkListUSBState = new System.Windows.Forms.CheckedListBox();
            this.ChkListOGLSuspended = new System.Windows.Forms.CheckedListBox();
            this.ButMemoryMode = new System.Windows.Forms.Button();
            this.GrpBoxUSBMode = new System.Windows.Forms.GroupBox();
            this.RdButUSBMode_Memory = new System.Windows.Forms.RadioButton();
            this.RdButUSBMode_Triggered = new System.Windows.Forms.RadioButton();
            this.RdButUSBMode_Roll = new System.Windows.Forms.RadioButton();
            this.RdButUSBMode_Standard = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrckVTrigger)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrckTTrigger)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.GrpBoxUSBMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.TrckVTrigger);
            this.panel1.Controls.Add(this.ButTDown);
            this.panel1.Controls.Add(this.ButTUp);
            this.panel1.Controls.Add(this.TrckTTrigger);
            this.panel1.Controls.Add(this.CmbTimeDiv);
            this.panel1.Controls.Add(this.TxtZeroVolt);
            this.panel1.Controls.Add(this.CmbVoltDiv);
            this.panel1.Controls.Add(this.ButSetToZero);
            this.panel1.Controls.Add(this.ButVUp);
            this.panel1.Controls.Add(this.ButVDown);
            this.panel1.Location = new System.Drawing.Point(0, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(837, 816);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Info;
            this.panel2.Location = new System.Drawing.Point(68, 68);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(682, 682);
            this.panel2.TabIndex = 0;
            // 
            // TrckVTrigger
            // 
            this.TrckVTrigger.Location = new System.Drawing.Point(33, 53);
            this.TrckVTrigger.Maximum = 1000;
            this.TrckVTrigger.Name = "TrckVTrigger";
            this.TrckVTrigger.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.TrckVTrigger.Size = new System.Drawing.Size(45, 710);
            this.TrckVTrigger.TabIndex = 67;
            this.TrckVTrigger.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TrckVTrigger_MouseUp);
            // 
            // ButTDown
            // 
            this.ButTDown.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ButTDown.Enabled = false;
            this.ButTDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ButTDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButTDown.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.ButTDown.Location = new System.Drawing.Point(374, 793);
            this.ButTDown.Margin = new System.Windows.Forms.Padding(0);
            this.ButTDown.Name = "ButTDown";
            this.ButTDown.Size = new System.Drawing.Size(20, 20);
            this.ButTDown.TabIndex = 68;
            this.ButTDown.Text = "<";
            this.ButTDown.UseMnemonic = false;
            this.ButTDown.UseVisualStyleBackColor = false;
            this.ButTDown.Click += new System.EventHandler(this.ButTDown_Click);
            // 
            // ButTUp
            // 
            this.ButTUp.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ButTUp.Enabled = false;
            this.ButTUp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ButTUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButTUp.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.ButTUp.Location = new System.Drawing.Point(411, 793);
            this.ButTUp.Margin = new System.Windows.Forms.Padding(0);
            this.ButTUp.Name = "ButTUp";
            this.ButTUp.Size = new System.Drawing.Size(20, 20);
            this.ButTUp.TabIndex = 67;
            this.ButTUp.Text = ">";
            this.ButTUp.UseVisualStyleBackColor = false;
            this.ButTUp.Click += new System.EventHandler(this.ButTUp_Click);
            // 
            // TrckTTrigger
            // 
            this.TrckTTrigger.Location = new System.Drawing.Point(56, 769);
            this.TrckTTrigger.Margin = new System.Windows.Forms.Padding(1);
            this.TrckTTrigger.Maximum = 1000;
            this.TrckTTrigger.Name = "TrckTTrigger";
            this.TrckTTrigger.Size = new System.Drawing.Size(708, 45);
            this.TrckTTrigger.TabIndex = 67;
            this.TrckTTrigger.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.TrckTTrigger.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TrckTTrigger_MouseUp);
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
            this.CmbTimeDiv.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CmbTimeDiv_KeyUp);
            this.CmbTimeDiv.Validating += new System.ComponentModel.CancelEventHandler(this.CmbTimeDiv_Validating);
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
            this.CmbVoltDiv.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CmbVoltDiv_KeyUp);
            this.CmbVoltDiv.Validating += new System.ComponentModel.CancelEventHandler(this.CmbVoltDiv_Validating);
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
            this.ChkoptionExtrapolated.Location = new System.Drawing.Point(869, 351);
            this.ChkoptionExtrapolated.Name = "ChkoptionExtrapolated";
            this.ChkoptionExtrapolated.Size = new System.Drawing.Size(110, 17);
            this.ChkoptionExtrapolated.TabIndex = 2;
            this.ChkoptionExtrapolated.Text = "Extrapolate points";
            this.ChkoptionExtrapolated.UseVisualStyleBackColor = true;
            this.ChkoptionExtrapolated.CheckedChanged += new System.EventHandler(this.ChkoptionExtrapolated_CheckedChanged);
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
            this.ChkOptionFilterOutliers.Location = new System.Drawing.Point(878, 514);
            this.ChkOptionFilterOutliers.Name = "ChkOptionFilterOutliers";
            this.ChkOptionFilterOutliers.Size = new System.Drawing.Size(84, 17);
            this.ChkOptionFilterOutliers.TabIndex = 3;
            this.ChkOptionFilterOutliers.Text = "Filter outliers";
            this.ChkOptionFilterOutliers.UseVisualStyleBackColor = true;
            this.ChkOptionFilterOutliers.CheckedChanged += new System.EventHandler(this.ChkOptionFilterOutliers_CheckedChanged);
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
            this.pictureBox1.Location = new System.Drawing.Point(1094, 2);
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
            this.label17.Location = new System.Drawing.Point(1094, 36);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(104, 15);
            this.label17.TabIndex = 43;
            this.label17.Text = "Isochronous USB";
            // 
            // label18
            // 
            this.label18.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label18.Font = new System.Drawing.Font("Candara", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(1094, 52);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(104, 21);
            this.label18.TabIndex = 44;
            this.label18.Text = "Multithreading";
            // 
            // label19
            // 
            this.label19.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label19.Font = new System.Drawing.Font("Candara", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(1094, 67);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(104, 21);
            this.label19.TabIndex = 45;
            this.label19.Text = "C WinUSB DLL";
            // 
            // label20
            // 
            this.label20.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label20.Font = new System.Drawing.Font("Candara", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(1094, 83);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(104, 21);
            this.label20.TabIndex = 46;
            this.label20.Text = "C OpenGL DLL";
            // 
            // label21
            // 
            this.label21.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label21.Font = new System.Drawing.Font("Candara", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(1094, 97);
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
            this.groupBox1.Location = new System.Drawing.Point(868, 449);
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
            this.groupBox2.Location = new System.Drawing.Point(868, 387);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(279, 56);
            this.groupBox2.TabIndex = 49;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label23);
            this.groupBox3.Controls.Add(this.CmbStatRefresh);
            this.groupBox3.Controls.Add(this.LblKSamplePerSec);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.LblTmrSuspended);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.CmbmsSleep);
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
            // TmrRefresh
            // 
            this.TmrRefresh.Interval = 1000;
            this.TmrRefresh.Tick += new System.EventHandler(this.TmrRefresh_Tick);
            // 
            // TxtUSB2
            // 
            this.TxtUSB2.Location = new System.Drawing.Point(1216, 12);
            this.TxtUSB2.Multiline = true;
            this.TxtUSB2.Name = "TxtUSB2";
            this.TxtUSB2.Size = new System.Drawing.Size(262, 100);
            this.TxtUSB2.TabIndex = 61;
            // 
            // butUSB2
            // 
            this.butUSB2.Location = new System.Drawing.Point(868, 34);
            this.butUSB2.Name = "butUSB2";
            this.butUSB2.Size = new System.Drawing.Size(85, 78);
            this.butUSB2.TabIndex = 62;
            this.butUSB2.Text = "Connect to USB device";
            this.butUSB2.UseVisualStyleBackColor = true;
            this.butUSB2.Click += new System.EventHandler(this.butUSB2_Click);
            // 
            // ChkListUSBInit
            // 
            this.ChkListUSBInit.BackColor = System.Drawing.SystemColors.Control;
            this.ChkListUSBInit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ChkListUSBInit.Enabled = false;
            this.ChkListUSBInit.FormattingEnabled = true;
            this.ChkListUSBInit.Items.AddRange(new object[] {
            "USB Device found",
            "Descriptor retrieved",
            "Interface retrieved",
            "Pipe retrieved",
            "Interval retrieved",
            "Transfer characteristics set",
            "Overlapped structure set",
            "Overlapped events set",
            "Isochronous packets set",
            "Isochronous buffer registered",
            "Pipe reset",
            "End at Frame check succesfull"});
            this.ChkListUSBInit.Location = new System.Drawing.Point(868, 150);
            this.ChkListUSBInit.Name = "ChkListUSBInit";
            this.ChkListUSBInit.Size = new System.Drawing.Size(177, 180);
            this.ChkListUSBInit.TabIndex = 63;
            // 
            // ChkListUSBState
            // 
            this.ChkListUSBState.BackColor = System.Drawing.SystemColors.Control;
            this.ChkListUSBState.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ChkListUSBState.Enabled = false;
            this.ChkListUSBState.FormattingEnabled = true;
            this.ChkListUSBState.Items.AddRange(new object[] {
            "Initialized",
            "Started",
            "Requested to stop",
            "Stopped",
            "Errored"});
            this.ChkListUSBState.Location = new System.Drawing.Point(959, 37);
            this.ChkListUSBState.Name = "ChkListUSBState";
            this.ChkListUSBState.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ChkListUSBState.Size = new System.Drawing.Size(113, 75);
            this.ChkListUSBState.TabIndex = 64;
            // 
            // ChkListOGLSuspended
            // 
            this.ChkListOGLSuspended.BackColor = System.Drawing.SystemColors.Control;
            this.ChkListOGLSuspended.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ChkListOGLSuspended.Enabled = false;
            this.ChkListOGLSuspended.FormattingEnabled = true;
            this.ChkListOGLSuspended.Items.AddRange(new object[] {
            "Screen Suspended"});
            this.ChkListOGLSuspended.Location = new System.Drawing.Point(868, 336);
            this.ChkListOGLSuspended.Name = "ChkListOGLSuspended";
            this.ChkListOGLSuspended.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ChkListOGLSuspended.Size = new System.Drawing.Size(130, 15);
            this.ChkListOGLSuspended.TabIndex = 65;
            // 
            // ButMemoryMode
            // 
            this.ButMemoryMode.Location = new System.Drawing.Point(1136, 195);
            this.ButMemoryMode.Name = "ButMemoryMode";
            this.ButMemoryMode.Size = new System.Drawing.Size(63, 35);
            this.ButMemoryMode.TabIndex = 66;
            this.ButMemoryMode.Text = "Start Memory Mode";
            this.ButMemoryMode.UseVisualStyleBackColor = true;
            this.ButMemoryMode.Click += new System.EventHandler(this.ButMemoryMode_Click);
            // 
            // GrpBoxUSBMode
            // 
            this.GrpBoxUSBMode.Controls.Add(this.RdButUSBMode_Memory);
            this.GrpBoxUSBMode.Controls.Add(this.RdButUSBMode_Triggered);
            this.GrpBoxUSBMode.Controls.Add(this.RdButUSBMode_Roll);
            this.GrpBoxUSBMode.Controls.Add(this.RdButUSBMode_Standard);
            this.GrpBoxUSBMode.Location = new System.Drawing.Point(1233, 137);
            this.GrpBoxUSBMode.Name = "GrpBoxUSBMode";
            this.GrpBoxUSBMode.Size = new System.Drawing.Size(139, 143);
            this.GrpBoxUSBMode.TabIndex = 67;
            this.GrpBoxUSBMode.TabStop = false;
            // 
            // RdButUSBMode_Memory
            // 
            this.RdButUSBMode_Memory.AutoSize = true;
            this.RdButUSBMode_Memory.Location = new System.Drawing.Point(12, 92);
            this.RdButUSBMode_Memory.Name = "RdButUSBMode_Memory";
            this.RdButUSBMode_Memory.Size = new System.Drawing.Size(92, 17);
            this.RdButUSBMode_Memory.TabIndex = 3;
            this.RdButUSBMode_Memory.TabStop = true;
            this.RdButUSBMode_Memory.Text = "Memory Mode";
            this.RdButUSBMode_Memory.UseVisualStyleBackColor = true;
            this.RdButUSBMode_Memory.CheckedChanged += new System.EventHandler(this.RdButUSBMode_Memory_CheckedChanged);
            // 
            // RdButUSBMode_Triggered
            // 
            this.RdButUSBMode_Triggered.AutoSize = true;
            this.RdButUSBMode_Triggered.Location = new System.Drawing.Point(12, 69);
            this.RdButUSBMode_Triggered.Name = "RdButUSBMode_Triggered";
            this.RdButUSBMode_Triggered.Size = new System.Drawing.Size(100, 17);
            this.RdButUSBMode_Triggered.TabIndex = 2;
            this.RdButUSBMode_Triggered.TabStop = true;
            this.RdButUSBMode_Triggered.Text = "Triggered Mode";
            this.RdButUSBMode_Triggered.UseVisualStyleBackColor = true;
            this.RdButUSBMode_Triggered.CheckedChanged += new System.EventHandler(this.RdButUSBMode_Triggered_CheckedChanged);
            // 
            // RdButUSBMode_Roll
            // 
            this.RdButUSBMode_Roll.AutoSize = true;
            this.RdButUSBMode_Roll.Location = new System.Drawing.Point(12, 46);
            this.RdButUSBMode_Roll.Name = "RdButUSBMode_Roll";
            this.RdButUSBMode_Roll.Size = new System.Drawing.Size(73, 17);
            this.RdButUSBMode_Roll.TabIndex = 1;
            this.RdButUSBMode_Roll.TabStop = true;
            this.RdButUSBMode_Roll.Text = "Roll Mode";
            this.RdButUSBMode_Roll.UseVisualStyleBackColor = true;
            this.RdButUSBMode_Roll.CheckedChanged += new System.EventHandler(this.RdButUSBMode_Roll_CheckedChanged);
            // 
            // RdButUSBMode_Standard
            // 
            this.RdButUSBMode_Standard.AutoSize = true;
            this.RdButUSBMode_Standard.Location = new System.Drawing.Point(12, 23);
            this.RdButUSBMode_Standard.Name = "RdButUSBMode_Standard";
            this.RdButUSBMode_Standard.Size = new System.Drawing.Size(98, 17);
            this.RdButUSBMode_Standard.TabIndex = 0;
            this.RdButUSBMode_Standard.TabStop = true;
            this.RdButUSBMode_Standard.Text = "Standard Mode";
            this.RdButUSBMode_Standard.UseVisualStyleBackColor = true;
            this.RdButUSBMode_Standard.CheckedChanged += new System.EventHandler(this.RdButUSBMode_Standard_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1582, 858);
            this.Controls.Add(this.GrpBoxUSBMode);
            this.Controls.Add(this.ButMemoryMode);
            this.Controls.Add(this.ChkListOGLSuspended);
            this.Controls.Add(this.butUSB2);
            this.Controls.Add(this.ChkoptionExtrapolated);
            this.Controls.Add(this.ChkListUSBState);
            this.Controls.Add(this.ChkListUSBInit);
            this.Controls.Add(this.TxtUSB2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ChkOptionFilterOutliers);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "YetAnother USB Oscilloscope";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrckVTrigger)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrckTTrigger)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.GrpBoxUSBMode.ResumeLayout(false);
            this.GrpBoxUSBMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
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
        private Label label8;
        private Label LblADC_Res_V;
        private CheckBox ChkoptionExtrapolated;
        private ComboBox CmbmsSleep;
        private Label label14;
        private Label label15;
        private Label LblTmrSuspended;
        private CheckBox ChkOptionFilterOutliers;
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
        private Timer TmrRefresh;
        private Label label23;
        private ComboBox CmbStatRefresh;
        private TextBox TxtUSB2;
        private Button butUSB2;
        private CheckedListBox ChkListUSBInit;
        private CheckedListBox ChkListUSBState;
        private CheckedListBox ChkListOGLSuspended;
        private Button ButMemoryMode;
        private Button ButTUp;
        private Button ButTDown;
        private TrackBar TrckTTrigger;
        private TrackBar TrckVTrigger;
        private GroupBox GrpBoxUSBMode;
        private RadioButton RdButUSBMode_Triggered;
        private RadioButton RdButUSBMode_Roll;
        private RadioButton RdButUSBMode_Standard;
        private RadioButton RdButUSBMode_Memory;
    }
}



internal static class NativeMethods
{

    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool USB2_Device_Initialize(UInt16[] pBuffer, UInt32 BufferSize8, UInt32 BackBuffersize8, UInt32 PacketSize, UInt32 MicroFrameSize);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Start(UInt32 TaktTime, ulong FrameNumber = 0);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Stop();
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Dispose();
 
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Status_Started(IntPtr pStarted);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Status_Stopped(IntPtr pStopped);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Status_RequestToStop(IntPtr pRequestToStop);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Status_Initialized(IntPtr pInitialized);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Status_Errored(IntPtr pErrored);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Status_Action_DeviceFound(IntPtr pDeviceFound);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Status_Action_GetDescriptor(IntPtr pGetDescriptor);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Status_Action_GetInterface(IntPtr pGetInterface);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Status_Action_GetIsochPipe(IntPtr pGetIsochPipe);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Status_Action_GetInterval(IntPtr pGetInterval);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Status_Action_SetTransferChars(IntPtr pSetTransferChars);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Status_Action_SetOverlappedStructure(IntPtr pSetOverlappedStructure);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Status_Action_SetOverlappedEvents(IntPtr pSetOverlappedEvents);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Status_Action_SetIsochPackets(IntPtr pSetIsochPackets);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Status_Action_RegisterIsochBuffer(IntPtr pRegisterIsochBuffer);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Status_Action_ResetPipe(IntPtr pResetPipe);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Status_Action_EndAtFrame(IntPtr pEndAtFrame);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Status_Action_ReadIsochPipe(IntPtr pReadIsochPipe);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_BackBufferFilled(IntPtr pBackBufferFilled);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_BackBufferLocked(IntPtr pBackBufferLocked);

    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void OGL_Window_Init(UInt32 win_W, float[] vertexBufferData, UInt32 vertexBufferDataSize, float[] vertexBufferTrigger, UInt32 vertexBufferTriggerSize);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void OGL_Window_Dispose();
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void OGL_Window_SetPos(int x, int y);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetWin32Window();
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void OGL_Suspended(IntPtr pSuspended);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void OGL_ScreenDrawn(byte ScreenDrawn); //special case with conditional variable on mutex passed by value
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void OGL_pScreenDrawn(IntPtr pScreenDrawn); 
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void OGL_Window_Frames(IntPtr pWindow_Frames);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void OGL_Extrapolate(IntPtr pExtrapolate);
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_Osc_v7.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void OGL_LastDataPosition(IntPtr pLastDataPosition);
}

