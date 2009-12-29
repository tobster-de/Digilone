namespace Digilone
{
    partial class MainForm
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
            this.boardControl = new Components.BoardControl();
            this.SuspendLayout();
            // 
            // boardControl
            // 
            this.boardControl.BackgroundImage = global::Digilone.Properties.Resources.abalone_back_neu2;
            this.boardControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.boardControl.BlackMarble = global::Digilone.Properties.Resources.black2;
            this.boardControl.DoubleBuffered = true;
            this.boardControl.Location = new System.Drawing.Point(0, 0);
            this.boardControl.Margin = new System.Windows.Forms.Padding(0);
            this.boardControl.MoveDownLeftMarble = global::Digilone.Properties.Resources.direction5;
            this.boardControl.MoveDownRightMarble = global::Digilone.Properties.Resources.direction6;
            this.boardControl.MoveLeftMarble = global::Digilone.Properties.Resources.direction3;
            this.boardControl.MoveRightMarble = global::Digilone.Properties.Resources.direction4;
            this.boardControl.MoveUpLeftMarble = global::Digilone.Properties.Resources.direction1;
            this.boardControl.MoveUpRightMarble = global::Digilone.Properties.Resources.direction2;
            this.boardControl.Name = "boardControl";
            this.boardControl.SelectMarble = global::Digilone.Properties.Resources.select;
            this.boardControl.Size = new System.Drawing.Size(520, 469);
            this.boardControl.TabIndex = 0;
            this.boardControl.WhiteMarble = global::Digilone.Properties.Resources.white2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 469);
            this.Controls.Add(this.boardControl);
            this.Name = "MainForm";
            this.Text = "Digilone";
            this.ResumeLayout(false);

        }

        #endregion

        private Components.BoardControl boardControl;

    }
}

