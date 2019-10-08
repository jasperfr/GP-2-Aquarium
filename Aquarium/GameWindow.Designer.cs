namespace Aquarium
{
    partial class GameWindow
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
            this.TickHandle = new System.Windows.Forms.Timer(this.components);
            this.DrawPanel = new System.Windows.Forms.Panel();
            this.LabelDebug = new System.Windows.Forms.Label();
            this.LblDebugInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TickHandle
            // 
            this.TickHandle.Tick += new System.EventHandler(this.TickHandle_Tick);
            // 
            // DrawPanel
            // 
            this.DrawPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DrawPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(30)))), ((int)(((byte)(73)))));
            this.DrawPanel.Location = new System.Drawing.Point(73, 73);
            this.DrawPanel.Margin = new System.Windows.Forms.Padding(64);
            this.DrawPanel.Name = "DrawPanel";
            this.DrawPanel.Size = new System.Drawing.Size(1118, 535);
            this.DrawPanel.TabIndex = 0;
            this.DrawPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawPanel_Paint);
            // 
            // LabelDebug
            // 
            this.LabelDebug.AutoSize = true;
            this.LabelDebug.Font = new System.Drawing.Font("Minecraftia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelDebug.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LabelDebug.Location = new System.Drawing.Point(12, 9);
            this.LabelDebug.Name = "LabelDebug";
            this.LabelDebug.Size = new System.Drawing.Size(134, 28);
            this.LabelDebug.TabIndex = 1;
            this.LabelDebug.Text = "(debug info)";
            // 
            // LblDebugInfo
            // 
            this.LblDebugInfo.AutoSize = true;
            this.LblDebugInfo.BackColor = System.Drawing.Color.Transparent;
            this.LblDebugInfo.Font = new System.Drawing.Font("Minecraftia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblDebugInfo.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LblDebugInfo.Location = new System.Drawing.Point(12, 37);
            this.LblDebugInfo.Name = "LblDebugInfo";
            this.LblDebugInfo.Size = new System.Drawing.Size(122, 28);
            this.LblDebugInfo.TabIndex = 2;
            this.LblDebugInfo.Text = "(game info)";
            // 
            // GameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(11)))), ((int)(((byte)(22)))));
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.LblDebugInfo);
            this.Controls.Add(this.LabelDebug);
            this.Controls.Add(this.DrawPanel);
            this.DoubleBuffered = true;
            this.Name = "GameWindow";
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GameWindow_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Timer TickHandle;
        public System.Windows.Forms.Panel DrawPanel;
        private System.Windows.Forms.Label LabelDebug;
        public System.Windows.Forms.Label LblDebugInfo;
    }
}

