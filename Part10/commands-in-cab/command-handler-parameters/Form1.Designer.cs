namespace command_handler_parameters
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.commandButton = new System.Windows.Forms.ToolStripButton();
            this.eventButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.commandButton,
            this.eventButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // commandButton
            // 
            this.commandButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.commandButton.Image = ((System.Drawing.Image)(resources.GetObject("commandButton.Image")));
            this.commandButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.commandButton.Name = "commandButton";
            this.commandButton.Size = new System.Drawing.Size(106, 22);
            this.commandButton.Text = "command button";
            // 
            // eventButton
            // 
            this.eventButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.eventButton.Image = ((System.Drawing.Image)(resources.GetObject("eventButton.Image")));
            this.eventButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.eventButton.Name = "eventButton";
            this.eventButton.Size = new System.Drawing.Size(108, 22);
            this.eventButton.Text = ".NET event button";
            this.eventButton.Click += new System.EventHandler(this.eventButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        internal System.Windows.Forms.ToolStripButton commandButton;
        internal System.Windows.Forms.ToolStripButton eventButton;
    }
}

