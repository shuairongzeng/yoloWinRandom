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
            string yoloRootPath = txt_createDirPath.Text.Trim();
            if (string.IsNullOrEmpty(yoloRootPath))
            {
                MessageBox.Show("请先输入Yolo标准文件夹路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string datasetPath = Path.Combine(yoloRootPath, "datasets", "my_dataset");
            string destTrainImagesPath = Path.Combine(datasetPath, "images", "train");
            string destTrainLabelsPath = Path.Combine(datasetPath, "labels", "train");
            string destValImagesPath = Path.Combine(datasetPath, "images", "val");
            string destValLabelsPath = Path.Combine(datasetPath, "labels", "val");
            string destTestImagesPath = Path.Combine(datasetPath, "images", "test");
            string destTestLabelsPath = Path.Combine(datasetPath, "labels", "test");
            string dataPath = Path.Combine(yoloRootPath, "data");
            string classesFilePath = Path.Combine(yoloRootPath, "classes.txt");
            string dataConfigFilePath = Path.Combine(dataPath, "my_dataset.yaml");

            if (!File.Exists(classesFilePath))
            {
                MessageBox.Show("在指定的根目录下找不到 classes.txt 文件！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // 读取所有图片和标签文件
                List<string> imageFiles = Directory.GetFiles(yoloRootPath, "*.jpg").ToList();
                imageFiles.AddRange(Directory.GetFiles(yoloRootPath, "*.jpeg"));
                imageFiles.AddRange(Directory.GetFiles(yoloRootPath, "*.png"));
                List<string> labelFiles = Directory.GetFiles(yoloRootPath, "*.txt").Where(f => !f.EndsWith("classes.txt", StringComparison.OrdinalIgnoreCase)).ToList();

                // 读取类别名称
                List<string> classNames = File.ReadAllLines(classesFilePath).ToList().Where(c=>!string.IsNullOrWhiteSpace(c)).ToList();
                int numClasses = classNames.Count;

                // 将图片和标签配对
                var dataPairs = new List<Tuple<string, string>>();
                foreach (var imageFile in imageFiles)
                {
                    string imageNameWithoutExtension = Path.GetFileNameWithoutExtension(imageFile);
                    string expectedLabelFile = Path.Combine(yoloRootPath, imageNameWithoutExtension + ".txt");
                    if (File.Exists(expectedLabelFile))
                    {
                        dataPairs.Add(Tuple.Create(imageFile, expectedLabelFile));
                    }
                    else
                    {
                        Logs($"警告：找不到图片 '{Path.GetFileName(imageFile)}' 对应的标签文件。\r\n");
                    }
                }

                // 随机打乱数据
                Random random = new Random();
                dataPairs = dataPairs.OrderBy(x => random.Next()).ToList();

                // 计算划分数量
                int totalCount = dataPairs.Count;
                int trainCount = (int)(totalCount * 0.7);
                int valCount = (int)(totalCount * 0.2);
                int testCount = totalCount - trainCount - valCount;

                Logs($"总数据量：{totalCount}，训练集：{trainCount}，验证集：{valCount}，测试集：{testCount}\r\n");

                // 移动文件到训练集
                for (int i = 0; i < trainCount; i++)
                {
                    MoveFileWithOverwriteCheck(dataPairs[i].Item1, Path.Combine(destTrainImagesPath, Path.GetFileName(dataPairs[i].Item1)), "训练图片");
                    MoveFileWithOverwriteCheck(dataPairs[i].Item2, Path.Combine(destTrainLabelsPath, Path.GetFileName(dataPairs[i].Item2)), "训练标签");
                }

                // 移动文件到验证集
                for (int i = trainCount; i < trainCount + valCount; i++)
                {
                    MoveFileWithOverwriteCheck(dataPairs[i].Item1, Path.Combine(destValImagesPath, Path.GetFileName(dataPairs[i].Item1)), "验证图片");
                    MoveFileWithOverwriteCheck(dataPairs[i].Item2, Path.Combine(destValLabelsPath, Path.GetFileName(dataPairs[i].Item2)), "验证标签");
                }

                // 移动文件到测试集
                for (int i = trainCount + valCount; i < totalCount; i++)
                {
                    MoveFileWithOverwriteCheck(dataPairs[i].Item1, Path.Combine(destTestImagesPath, Path.GetFileName(dataPairs[i].Item1)), "测试图片");
                    MoveFileWithOverwriteCheck(dataPairs[i].Item2, Path.Combine(destTestLabelsPath, Path.GetFileName(dataPairs[i].Item2)), "测试标签");
                }

                // 移动 classes.txt
                string destClassesFilePath = Path.Combine(dataPath, "classes.txt");
                MoveFileWithOverwriteCheck(classesFilePath, destClassesFilePath, "类别文件");

                // 创建 data/my_dataset.yaml 文件
                List<string> classessList = new List<string>();
                classessList.Add( $"train: {Path.Combine("datasets", "my_dataset", "images", "train")}");
                classessList.Add($"val: {Path.Combine("datasets", "my_dataset", "images", "val") }");
                classessList.Add($"test: {Path.Combine("datasets", "my_dataset", "images", "test") }");
                classessList.Add($"nc: {numClasses }");
                classessList.Add($"names: {string.Join(", ", classNames.Select(n => "'" + n + "'"))}");
                
                File.WriteAllLines(dataConfigFilePath, classessList);
                Logs($"创建数据集配置文件：{dataConfigFilePath}\r\n");

                MessageBox.Show("数据划分和文件移动完成！请查看操作日志。", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                Logs($"发生错误：{ex.Message}\r\n");
                MessageBox.Show($"发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void MoveFiles(string sourceImagesPath, string sourceLabelsPath, string destImagesPath, string destLabelsPath, string setName)
        {
            if (Directory.Exists(sourceImagesPath))
            {
                Logs($"开始移动 {setName} 数据...\r\n");
                foreach (string imagePath in Directory.GetFiles(sourceImagesPath))
                {
                    string imageNameWithoutExtension = Path.GetFileNameWithoutExtension(imagePath);
                    string imageExtension = Path.GetExtension(imagePath).ToLower();

                    if (imageExtension == ".jpg" || imageExtension == ".jpeg" || imageExtension == ".png")
                    {
                        string destImagePath = Path.Combine(destImagesPath, Path.GetFileName(imagePath));
                        string sourceLabelPath = Path.Combine(sourceLabelsPath, imageNameWithoutExtension + ".txt");
                        string destLabelPath = Path.Combine(destLabelsPath, imageNameWithoutExtension + ".txt");

                        MoveFileWithOverwriteCheck(imagePath, destImagePath, setName + "图片");

                        if (File.Exists(sourceLabelPath))
                        {
                            MoveFileWithOverwriteCheck(sourceLabelPath, destLabelPath, setName + "标签");
                        }
                        else
                        {
                            Logs($"警告：找不到 {setName} 图片 '{Path.GetFileName(imagePath)}' 对应的标签文件。\r\n");
                        }
                    }
                }
                Logs($"{setName} 数据移动完成。\r\n");
            }
            else if (!string.IsNullOrEmpty(sourceImagesPath))
            {
                Logs($"警告：{setName} 图片路径不存在：{sourceImagesPath}\r\n");
            }

            if (!Directory.Exists(sourceLabelsPath) && !string.IsNullOrEmpty(sourceLabelsPath))
            {
                Logs($"警告：{setName} 标签路径不存在：{sourceLabelsPath}\r\n");
            }
        }

        private bool? overwriteAll = null; // null 表示尚未决定，true 表示全部覆盖，false 表示全部跳过

        private void MoveFileWithOverwriteCheck(string sourceFile, string destinationFile, string fileType)
        {
            if (overwriteAll == true)
            {
                try
                {
                    File.Copy(sourceFile, destinationFile, true);
                    Logs($"覆盖 {fileType} 文件：'{Path.GetFileName(sourceFile)}' -> '{Path.GetFileName(destinationFile)}'\r\n");
                }
                catch (Exception ex)
                {
                    Logs($"错误：移动 {fileType} 文件 '{Path.GetFileName(sourceFile)}' 失败：{ex.Message}\r\n");
                }
                return;
            }

            if (overwriteAll == false)
            {
                Logs($"跳过 {fileType} 文件：'{Path.GetFileName(sourceFile)}'，用户已选择跳过所有。\r\n");
                return;
            }

            if (File.Exists(destinationFile))
            {
                DialogResult result = MessageBox.Show($"目标位置已存在 {fileType} 文件 '{Path.GetFileName(destinationFile)}'，是否覆盖？\r\n点击“是”覆盖当前文件，点击“否”跳过当前文件。\r\n点击“是(全部)”覆盖所有后续冲突文件，点击“否(全部)”跳过所有后续冲突文件。", "文件已存在", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        File.Copy(sourceFile, destinationFile, true);
                        Logs($"覆盖 {fileType} 文件：'{Path.GetFileName(sourceFile)}' -> '{Path.GetFileName(destinationFile)}'\r\n");
                    }
                    catch (Exception ex)
                    {
                        Logs($"错误：移动 {fileType} 文件 '{Path.GetFileName(sourceFile)}' 失败：{ex.Message}\r\n");
                    }
                }
                else if (result == DialogResult.No)
                {
                    Logs($"跳过 {fileType} 文件：'{Path.GetFileName(sourceFile)}'。\r\n");
                }
                else if (result == DialogResult.Cancel) // 我们将 Cancel 作为“全部覆盖”
                {
                    overwriteAll = true;
                    try
                    {
                        File.Copy(sourceFile, destinationFile, true);
                        Logs($"覆盖 {fileType} 文件：'{Path.GetFileName(sourceFile)}' -> '{Path.GetFileName(destinationFile)}'\r\n");
                    }
                    catch (Exception ex)
                    {
                        Logs($"错误：移动 {fileType} 文件 '{Path.GetFileName(sourceFile)}' 失败：{ex.Message}\r\n");
                    }
                }
                else if (result == DialogResult.Abort) // 假设你想要添加一个“否(全部)”的逻辑，你需要修改 MessageBox 的按钮
                {
                    overwriteAll = false;
                    Logs($"跳过 {fileType} 文件：'{Path.GetFileName(sourceFile)}'，用户已选择跳过所有。\r\n");
                }
            }
            else
            {
                try
                {
                    File.Copy(sourceFile, destinationFile, false);
                    Logs($"移动 {fileType} 文件：'{Path.GetFileName(sourceFile)}' -> '{Path.GetFileName(destinationFile)}'\r\n");
                }
                catch (Exception ex)
                {
                    Logs($"错误：移动 {fileType} 文件 '{Path.GetFileName(sourceFile)}' 失败：{ex.Message}\r\n");
                }
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
            string yoloRootPath = txt_createDirPath.Text.Trim();
            if (string.IsNullOrEmpty(yoloRootPath))
            {
                selectCreateBathPath();
            }
            yoloRootPath = txt_createDirPath.Text.Trim();
            string datasetsPath = Path.Combine(yoloRootPath, "datasets");
            string datasetName = "my_dataset";
            string datasetPath = Path.Combine(datasetsPath, datasetName);
            string imagesTrainPath = Path.Combine(datasetPath, "images", "train");
            string imagesValPath = Path.Combine(datasetPath, "images", "val");
            string imagesTestPath = Path.Combine(datasetPath, "images", "test");
            string labelsTrainPath = Path.Combine(datasetPath, "labels", "train");
            string labelsValPath = Path.Combine(datasetPath, "labels", "val");
            string labelsTestPath = Path.Combine(datasetPath, "labels", "test");
            string dataPath = Path.Combine(yoloRootPath, "data");

            try
            {
                Directory.CreateDirectory(imagesTrainPath);
                Directory.CreateDirectory(imagesValPath);
                Directory.CreateDirectory(imagesTestPath);
                Directory.CreateDirectory(labelsTrainPath);
                Directory.CreateDirectory(labelsValPath);
                Directory.CreateDirectory(labelsTestPath);
                Directory.CreateDirectory(dataPath);

                Logs($"创建目录结构成功：{datasetPath}\r\n");
            }
            catch (Exception ex)
            {
                Logs($"创建目录结构失败：{ex.Message}\r\n");
                MessageBox.Show($"创建目录结构失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}