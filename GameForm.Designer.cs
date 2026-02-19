namespace Gardener
{
    partial class GameForm
    {
        private System.ComponentModel.IContainer components = null;
        private OpenTK.GLControl.GLControl glControl;
        private System.Windows.Forms.Label lblPlayer1Fruits;
        private System.Windows.Forms.Label lblPlayer2Fruits;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.glControl = new OpenTK.GLControl.GLControl();
            this.lblPlayer1Fruits = new System.Windows.Forms.Label();
            this.lblPlayer2Fruits = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // glControl
            // 
            this.glControl.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            this.glControl.APIVersion = new System.Version(3, 3, 0, 0);
            this.glControl.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            this.glControl.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            this.glControl.IsEventDriven = true;
            this.glControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl.Location = new System.Drawing.Point(0, 0);
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(1582, 853);
            this.glControl.TabIndex = 0;
            // 
            // lblPlayer1Fruits
            // 
            this.lblPlayer1Fruits.AutoSize = true;
            this.lblPlayer1Fruits.Location = new System.Drawing.Point(10, 257);
            this.lblPlayer1Fruits.Name = "lblPlayer1Fruits";
            this.lblPlayer1Fruits.Size = new System.Drawing.Size(25, 20);
            this.lblPlayer1Fruits.TabIndex = 1;
            this.lblPlayer1Fruits.Text = "0";
            this.lblPlayer1Fruits.ForeColor = System.Drawing.Color.White;
            this.lblPlayer1Fruits.BackColor = System.Drawing.ColorTranslator.FromHtml("#a3a7cc");
            this.lblPlayer1Fruits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter; // Центрируем текст
            // 
            // lblPlayer2Fruits
            // 
            this.lblPlayer2Fruits.AutoSize = true;
            this.lblPlayer2Fruits.Location = new System.Drawing.Point(801, 257);
            this.lblPlayer2Fruits.Name = "lblPlayer2Fruits";
            this.lblPlayer2Fruits.Size = new System.Drawing.Size(25, 20);
            this.lblPlayer2Fruits.TabIndex = 2;
            this.lblPlayer2Fruits.Text = "0";
            this.lblPlayer2Fruits.ForeColor = System.Drawing.Color.White;
            this.lblPlayer2Fruits.BackColor = System.Drawing.ColorTranslator.FromHtml("#a3a7cc");
            this.lblPlayer2Fruits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter; // Центрируем текст
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1582, 853);
            this.Controls.Add(this.lblPlayer1Fruits);
            this.Controls.Add(this.lblPlayer2Fruits);
            this.Controls.Add(this.glControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "GameForm";
            this.Text = "Gardener";
            this.ResumeLayout(false);
        }
    }
}