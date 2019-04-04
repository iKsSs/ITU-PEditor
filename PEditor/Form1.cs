using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace PEditor
{
    public partial class mainForm : Form
    {
        private static String histogramMenuLabel = "Show histogram";
        private static String navigationMenuLabel = "Show navigation";

        private Bitmap bitmap;
        private Histogram hist;
        private Navigation navig;
        private Filters filters;
        private List<Bitmap> backBitmap;

        private string filename;
        private bool edited;
        public mainForm()
        {
            InitializeComponent();

            backBitmap = new List<Bitmap>();
            edited = false;
        }

        public void renderCurrentView(object sender = null)
        {
            hist.createHistogram();
            histogram.Image = hist.drawHistogram();
            histogram.Refresh();

            navigation.Image = navig.drawNavigation();
            navigation.Refresh();

            //filters.refreshRef(ref bitmap);
            picturePanel.Image = bitmap;
            //this.Invalidate();
            picturePanel.Refresh();
        }


        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog.FileName;

                open();
            }
        }

        private void enableFunc()
        {
            btnSave.Enabled = btnLeft.Enabled = btnRight.Enabled = btnHor.Enabled = btnVert.Enabled = true;
            saveAsToolStripMenuItem.Enabled = saveToolStripMenuItem.Enabled = true;
            editToolStripMenuItem.Enabled = adjustToolStripMenuItem.Enabled = effectsToolStripMenuItem.Enabled = true;
        }

        private void open()
        {
            enableFunc();

            bitmap = new Bitmap(filename);
            hist = new Histogram(bitmap);
            navig = new Navigation(bitmap);
            filters = new Filters(ref bitmap);

            picturePanel.Image = bitmap;

            // INFO{2} histrogram is default view so there is no need to cal renderCurrentView();
            //hist.createHistogram();
            //histogram.Image = hist.drawHistogram();
            renderCurrentView();
            //back step 1

            backBitmap.Add(new Bitmap(bitmap));
            backBox.Items.Add("Open image");

        }

        private void btnBrightnessOk_Click(object sender, EventArgs e)
        {
            int brightness = trackBrightness.Value;
            int contrast = trackContrast.Value;

            if (filters == null)
                return;

            edited = true;

            //brightness
            filters.setBrightness(brightness);
            this.backBitmap.Add(new Bitmap(this.bitmap));
            this.backBox.Items.Add("Brightness");

            //contrast
            filters.setContrast(contrast);

            this.backBitmap.Add(new Bitmap(this.bitmap));
            this.backBox.Items.Add("Contrast");

            renderCurrentView();

            trackBrightness.Value = 0;
            numericBrightness.Value = 0;
            trackContrast.Value = 0;
            numericContrast.Value = 0;
        }

        private void trackBrightness_Scroll(object sender, EventArgs e)
        {
            numericBrightness.Value = trackBrightness.Value;
        }

        private void numericBrightness_ValueChanged(object sender, EventArgs e)
        {
            trackBrightness.Value = (int)numericBrightness.Value;
        }

        private void trackContrast_Scroll(object sender, EventArgs e)
        {
            numericContrast.Value = trackContrast.Value;
        }

        private void numericContrast_ValueChanged(object sender, EventArgs e)
        {
            trackContrast.Value = (int)numericContrast.Value;
        }

        private void grayScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filters == null)
                return;

            edited = true;

            progressBar1.Value = 0;

            filters.toGrayScale();

            this.backBitmap.Add(new Bitmap(this.bitmap));
            this.backBox.Items.Add("Gray scale");

            progressBar1.Value = 50;

            renderCurrentView();

            progressBar1.Value = 100;
        }


        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            renderCurrentView();
        }

        private void negativeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filters == null)
                return;

            edited = true;

            progressBar1.Value = 0;

            filters.toInvert();

            progressBar1.Value = 50;

            this.backBitmap.Add(new Bitmap(this.bitmap));
            this.backBox.Items.Add("Negative");

            renderCurrentView();

            progressBar1.Value = 100;
        }

        private void blackAndWhiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filters == null)
                return;

            edited = true;

            BlackAndWhiteSettings bs = new BlackAndWhiteSettings();

            progressBar1.Value = 0;

            DialogResult result = bs.ShowDialog();

            if (result == DialogResult.Yes)
            {
                progressBar1.Value = 10;

                filters.toBlackAndWhite(bs.Threshold);

                this.backBitmap.Add(new Bitmap(this.bitmap));
                this.backBox.Items.Add("Black and White");

                progressBar1.Value = 50;

                renderCurrentView();

                progressBar1.Value = 100;
            }
        }

        private void showNavigationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            renderCurrentView(sender);
        }

        private void showHistogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            renderCurrentView(sender);
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            // zoom +10%
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            save();
        }

        private void save()
        {
            edited = false;

            bitmap.Save(filename + "_edit");
        }

        private void saveAs()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "BMP soubor (*.BMP)|*.bmp|All Files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bitmap.Save(saveFileDialog1.FileName);
            }
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            picturePanel.SizeMode = PictureBoxSizeMode.Normal;
        }

        private void zoomToFitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            picturePanel.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void sidePanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel1.Visible)
            {
                panel1.Visible = false;
                sidePanelToolStripMenuItem.Checked = false;
            }
            else
            {
                panel1.Visible = true;
                sidePanelToolStripMenuItem.Checked = true;
            }
        }

        private void toolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mainMenu.Visible)
            {
                mainMenu.Visible = false;
                toolbarToolStripMenuItem.Checked = false;
            }
            else
            {
                mainMenu.Visible = true;
                toolbarToolStripMenuItem.Checked = true;
            }
        }

        private void mainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            filename = files.Last();

            open();
        }

        private void mainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void blurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filters == null)
                return;

            edited = true;

            BlurSettings bs = new BlurSettings();
            progressBar1.Value = 0;

            DialogResult result = bs.ShowDialog();

            if (result == DialogResult.Yes)
            {
                progressBar1.Value = 10;

                filters.blur(bs.Threshold);

                this.backBitmap.Add(new Bitmap(this.bitmap));
                this.backBox.Items.Add("Blur");

                progressBar1.Value = 50;

                renderCurrentView();

                progressBar1.Value = 100;
            }
        }

        private void brightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void backBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selected = this.backBox.SelectedIndex;
            bitmap = new Bitmap(backBitmap[selected]);

            filters.setRef(ref bitmap);

            hist = new Histogram(bitmap);
            navig = new Navigation(bitmap);
            renderCurrentView();
        }

        private void oldPhotographToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edited = true;
        }

        private void coloredNoiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edited = true;
        }

        private void resizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edited = true;
        }

        private void rotateLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filters == null)
                return;

            filters.setRotateFlip(2);

            edited = true;

            renderCurrentView();

            edited = true;
        }

        private void rotateRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filters == null)
                return;

            filters.setRotateFlip(1);

            edited = true;

            renderCurrentView();

            edited = true;
        }

        private void rotateAndFlipToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void flipHorizontallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filters == null)
                return;

            filters.setRotateFlip(4);

            edited = true;

            renderCurrentView();

            edited = true;

            edited = true;
        }

        private void flipVerticallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filters == null)
                return;

            filters.setRotateFlip(3);

            edited = true;

            renderCurrentView();

            edited = true;

            edited = true;
        }

        private void rotateBy180ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filters == null)
                return;

            filters.setRotateFlip(5);

            edited = true;

            renderCurrentView();

            edited = true;

            edited = true;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (filters == null)
                return;

            filters.setRotateFlip(2);
            
            edited = true;

            renderCurrentView();
        }

        private void backBox_DrawItem(object sender, DrawItemEventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog.FileName;

                open();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAs();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (filters == null)
                return;

            filters.setRotateFlip(1);

            edited = true;

            renderCurrentView();

            edited = true;
        }

        private void btnVert_Click(object sender, EventArgs e)
        {
            if (filters == null)
                return;

            filters.setRotateFlip(4);

            edited = true;

            renderCurrentView();

            edited = true;

            edited = true;
        }

        private void btnHor_Click(object sender, EventArgs e)
        {
            if (filters == null)
                return;

            filters.setRotateFlip(3);

            edited = true;

            renderCurrentView();

            edited = true;

            edited = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (edited)
            {
                DialogResult result = MessageBox.Show("Save?", "Save your photo?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    save();
                }
            }
        }
    }
}
