using System.Diagnostics;

namespace yoloWinRandom
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Random randomNum = new Random();

        private void btn_go_Click(object sender, EventArgs e)
        {
            // 设置分割比例
            var thridValue = Convert.ToInt32(txt_thrid.Text) / 10.0;
            var sencondValue = (Convert.ToInt32(txt_second.Text) + 1) / 10.0;


            var basePath = txt_createDirPath.Text;

            var trinPath = Path.Combine(basePath, "train");

            var imgPath = Path.Combine(trinPath, "images");
            var imgFiles = Directory.GetFiles(imgPath).ToList();

            var labelPath = Path.Combine(trinPath, "labels");
            var lableFiles = Directory.GetFiles(labelPath).ToList();


            foreach (var imgFile in imgFiles)
            {

                FileInfo imgFileInfo = new FileInfo(imgFile);

                var fileName = imgFileInfo.Name.Replace(imgFileInfo.Extension, "");


                var movePathTest = Path.Combine(basePath, "test");
                var imgmovePathTest = Path.Combine(movePathTest, "images");
                var labelmovePathTest = Path.Combine(movePathTest, "labels");


                var movePathValid = Path.Combine(basePath, "val");
                var imgmovePathValid = Path.Combine(movePathValid, "images");
                var labelmovePathValid = Path.Combine(movePathValid, "labels");


                var lableSearchPath = lableFiles.Where(c => c.Contains(fileName)).FirstOrDefault();
                if (lableSearchPath != null)
                {
                    var lableFileInfo = new FileInfo(lableSearchPath);

                    var rnd = randomNum.NextDouble();


                    //Logs($"图片/标签名称：{imgFileInfo.Name}/txt 比例：{rnd}");


                    if (rnd < thridValue)
                    {
                        imgFileInfo.MoveTo(Path.Combine(imgmovePathTest, imgFileInfo.Name));
                        lableFileInfo.MoveTo(Path.Combine(labelmovePathTest, lableFileInfo.Name));


                        Logs($"图片/标签名称：{imgFileInfo.Name}/txt 比例：{rnd} 移动到test目录");
                        //imgFileInfo.Delete();
                        //lableFileInfo.Delete();

                    }
                    else if (rnd < sencondValue)
                    {
                        imgFileInfo.MoveTo(Path.Combine(imgmovePathValid, imgFileInfo.Name));
                        lableFileInfo.MoveTo(Path.Combine(labelmovePathValid, lableFileInfo.Name));
                        Logs($"图片/标签名称：{imgFileInfo.Name}/txt 比例：{rnd} 移动到val目录");

                        //imgFileInfo.Delete();
                        //lableFileInfo.Delete();
                    }
                    else
                    {
                        Logs($"图片/标签名称：{imgFileInfo.Name}/txt 比例：{rnd} 保持不变");
                    }



                }

                var dds = 1;

            }




        }

        private void Logs(string content)
        {
            this.Invoke(() =>
            {
                txt_logs.AppendText(content);
                txt_logs.AppendText(Environment.NewLine);
            });
        }

        private void txt_createDirPath_Click(object sender, EventArgs e)
        {
            selectCreateBathPath();
        }

        private void selectCreateBathPath()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = txt_createDirPath.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txt_createDirPath.Text = folderBrowserDialog.SelectedPath;
                //btn_createDir_Click(null, null);
            }
        }

        private void btn_createDir_Click(object sender, EventArgs e)
        {
            var basePath = txt_createDirPath.Text;
            if (string.IsNullOrEmpty(basePath))
            {
                selectCreateBathPath();
            }

            basePath = txt_createDirPath.Text;

            // train
            var trainBasePath = Path.Combine(basePath, "train");
            if (!Directory.Exists(trainBasePath))
            {
                Directory.CreateDirectory(trainBasePath);
            }

            var trainImgPath = Path.Combine(trainBasePath, "images");
            if (!Directory.Exists(trainImgPath))
            {
                Directory.CreateDirectory(trainImgPath);
            }

            var trainLabelPath = Path.Combine(trainBasePath, "labels");
            if (!Directory.Exists(trainLabelPath))
            {
                Directory.CreateDirectory(trainLabelPath);
            }

            // val
            var valBasePath = Path.Combine(basePath, "val");
            if (!Directory.Exists(trainBasePath))
            {
                Directory.CreateDirectory(valBasePath);
            }

            var valImgPath = Path.Combine(valBasePath, "images");
            if (!Directory.Exists(valImgPath))
            {
                Directory.CreateDirectory(valImgPath);
            }

            var valLabelPath = Path.Combine(valBasePath, "labels");
            if (!Directory.Exists(valLabelPath))
            {
                Directory.CreateDirectory(valLabelPath);
            }

            // test
            var testBasePath = Path.Combine(basePath, "test");
            if (!Directory.Exists(testBasePath))
            {
                Directory.CreateDirectory(testBasePath);
            }

            var testImgPath = Path.Combine(testBasePath, "images");
            if (!Directory.Exists(testImgPath))
            {
                Directory.CreateDirectory(testImgPath);
            }

            var testLabelPath = Path.Combine(testBasePath, "labels");
            if (!Directory.Exists(testLabelPath))
            {
                Directory.CreateDirectory(testLabelPath);
            }

            Process.Start("explorer.exe", basePath);
        }
    }
}