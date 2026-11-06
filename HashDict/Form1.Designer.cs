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
            labelStats = new Label();
            listBoxBuckets = new ListBox();
            labelStatus = new Label();
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
            buttonClear.Size = new Size(100, 32);
            buttonClear.TabIndex = 5;
            buttonClear.Text = "Wyczyść";
            buttonClear.UseVisualStyleBackColor = true;
            buttonClear.Click += buttonClear_Click;
            //
            // labelStats
            //
            labelStats.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            labelStats.Location = new Point(16, 104);
            labelStats.Name = "labelStats";
            labelStats.Size = new Size(668, 24);
            //
            // listBoxBuckets
            //
            listBoxBuckets.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBoxBuckets.Font = new Font("Consolas", 10F);
            listBoxBuckets.HorizontalScrollbar = true;
            listBoxBuckets.IntegralHeight = false;
            listBoxBuckets.Location = new Point(16, 134);
            listBoxBuckets.Name = "listBoxBuckets";
            listBoxBuckets.Size = new Size(668, 290);
            listBoxBuckets.TabIndex = 6;
            //
            // labelStatus
            //
            labelStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            labelStatus.Location = new Point(16, 434);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(668, 24);
            //
            // Form1
            //
            AcceptButton = buttonAdd;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 470);
            Controls.Add(labelKey);
            Controls.Add(textBoxKey);
            Controls.Add(labelValue);
            Controls.Add(textBoxValue);
            Controls.Add(buttonAdd);
            Controls.Add(buttonFind);
            Controls.Add(buttonRemove);
            Controls.Add(buttonClear);
            Controls.Add(labelStats);
            Controls.Add(listBoxBuckets);
            Controls.Add(labelStatus);
            MinimumSize = new Size(700, 400);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Słownik z haszowaniem (łańcuchowa obsługa kolizji)";
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
        private Label labelStats;
        private ListBox listBoxBuckets;
        private Label labelStatus;
    }
}
