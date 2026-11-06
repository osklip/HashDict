namespace HashDict
{
    partial class Form1
    {
        private System.ComponentModel.IContainer? components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            labelKey = new Label();
            textBoxKey = new TextBox();
            labelValue = new Label();
            textBoxValue = new TextBox();
            buttonAdd = new Button();
            buttonFind = new Button();
            buttonRemove = new Button();
            buttonClear = new Button();
            labelGenerate = new Label();
            numericUpDownCount = new NumericUpDown();
            buttonGenerate = new Button();
            labelStats = new Label();
            listBoxBuckets = new ListBox();
            labelStatus = new Label();
            ((System.ComponentModel.ISupportInitialize)numericUpDownCount).BeginInit();
            SuspendLayout();
            //
            // labelKey
            //
            labelKey.AutoSize = true;
            labelKey.Location = new Point(16, 20);
            labelKey.Name = "labelKey";
            labelKey.Text = "Klucz:";
            //
            // textBoxKey
            //
            textBoxKey.Location = new Point(90, 16);
            textBoxKey.Name = "textBoxKey";
            textBoxKey.Size = new Size(220, 27);
            textBoxKey.TabIndex = 0;
            //
            // labelValue
            //
            labelValue.AutoSize = true;
            labelValue.Location = new Point(330, 20);
            labelValue.Name = "labelValue";
            labelValue.Text = "Wartość:";
            //
            // textBoxValue
            //
            textBoxValue.Location = new Point(410, 16);
            textBoxValue.Name = "textBoxValue";
            textBoxValue.Size = new Size(260, 27);
            textBoxValue.TabIndex = 1;
            //
            // buttonAdd
            //
            buttonAdd.Location = new Point(16, 56);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(130, 32);
            buttonAdd.TabIndex = 2;
            buttonAdd.Text = "Dodaj / zmień";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            //
            // buttonFind
            //
            buttonFind.Location = new Point(156, 56);
            buttonFind.Name = "buttonFind";
            buttonFind.Size = new Size(100, 32);
            buttonFind.TabIndex = 3;
            buttonFind.Text = "Szukaj";
            buttonFind.UseVisualStyleBackColor = true;
            buttonFind.Click += buttonFind_Click;
            //
            // buttonRemove
            //
            buttonRemove.Location = new Point(266, 56);
            buttonRemove.Name = "buttonRemove";
            buttonRemove.Size = new Size(100, 32);
            buttonRemove.TabIndex = 4;
            buttonRemove.Text = "Usuń";
            buttonRemove.UseVisualStyleBackColor = true;
            buttonRemove.Click += buttonRemove_Click;
            //
            // buttonClear
            //
            buttonClear.Location = new Point(376, 56);
            buttonClear.Name = "buttonClear";
            buttonClear.Size = new Size(150, 32);
            buttonClear.TabIndex = 5;
            buttonClear.Text = "Wyczyść słownik";
            buttonClear.UseVisualStyleBackColor = true;
            buttonClear.Click += buttonClear_Click;
            //
            // labelGenerate
            //
            labelGenerate.AutoSize = true;
            labelGenerate.Location = new Point(16, 108);
            labelGenerate.Name = "labelGenerate";
            labelGenerate.Text = "Losowe pary:";
            //
            // numericUpDownCount
            //
            numericUpDownCount.Location = new Point(120, 104);
            numericUpDownCount.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numericUpDownCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownCount.Name = "numericUpDownCount";
            numericUpDownCount.Size = new Size(100, 27);
            numericUpDownCount.TabIndex = 6;
            numericUpDownCount.ThousandsSeparator = true;
            numericUpDownCount.Value = new decimal(new int[] { 20, 0, 0, 0 });
            //
            // buttonGenerate
            //
            buttonGenerate.Location = new Point(230, 100);
            buttonGenerate.Name = "buttonGenerate";
            buttonGenerate.Size = new Size(180, 32);
            buttonGenerate.TabIndex = 7;
            buttonGenerate.Text = "Dodaj losowe pary";
            buttonGenerate.UseVisualStyleBackColor = true;
            buttonGenerate.Click += buttonGenerate_Click;
            //
            // labelStats
            //
            labelStats.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            labelStats.Location = new Point(16, 146);
            labelStats.Name = "labelStats";
            labelStats.Size = new Size(868, 24);
            //
            // listBoxBuckets
            //
            listBoxBuckets.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBoxBuckets.Font = new Font("Consolas", 10F);
            listBoxBuckets.HorizontalScrollbar = true;
            listBoxBuckets.IntegralHeight = false;
            listBoxBuckets.Location = new Point(16, 176);
            listBoxBuckets.Name = "listBoxBuckets";
            listBoxBuckets.Size = new Size(868, 380);
            listBoxBuckets.TabIndex = 8;
            //
            // labelStatus
            //
            labelStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            labelStatus.Location = new Point(16, 566);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(868, 24);
            //
            // Form1
            //
            AcceptButton = buttonAdd;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 600);
            Controls.Add(labelKey);
            Controls.Add(textBoxKey);
            Controls.Add(labelValue);
            Controls.Add(textBoxValue);
            Controls.Add(buttonAdd);
            Controls.Add(buttonFind);
            Controls.Add(buttonRemove);
            Controls.Add(buttonClear);
            Controls.Add(labelGenerate);
            Controls.Add(numericUpDownCount);
            Controls.Add(buttonGenerate);
            Controls.Add(labelStats);
            Controls.Add(listBoxBuckets);
            Controls.Add(labelStatus);
            MinimumSize = new Size(700, 400);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Słownik z haszowaniem (łańcuchowa obsługa kolizji)";
            ((System.ComponentModel.ISupportInitialize)numericUpDownCount).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelKey;
        private TextBox textBoxKey;
        private Label labelValue;
        private TextBox textBoxValue;
        private Button buttonAdd;
        private Button buttonFind;
        private Button buttonRemove;
        private Button buttonClear;
        private Label labelGenerate;
        private NumericUpDown numericUpDownCount;
        private Button buttonGenerate;
        private Label labelStats;
        private ListBox listBoxBuckets;
        private Label labelStatus;
    }
}
