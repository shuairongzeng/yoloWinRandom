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
                MessageBox.Show("��������Yolo��׼�ļ���·����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("��ָ���ĸ�Ŀ¼���Ҳ��� classes.txt �ļ���", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // ��ȡ����ͼƬ�ͱ�ǩ�ļ�
                List<string> imageFiles = Directory.GetFiles(yoloRootPath, "*.jpg").ToList();
                imageFiles.AddRange(Directory.GetFiles(yoloRootPath, "*.jpeg"));
                imageFiles.AddRange(Directory.GetFiles(yoloRootPath, "*.png"));
                List<string> labelFiles = Directory.GetFiles(yoloRootPath, "*.txt").Where(f => !f.EndsWith("classes.txt", StringComparison.OrdinalIgnoreCase)).ToList();

                // ��ȡ�������
                List<string> classNames = File.ReadAllLines(classesFilePath).ToList().Where(c=>!string.IsNullOrWhiteSpace(c)).ToList();
                int numClasses = classNames.Count;

                // ��ͼƬ�ͱ�ǩ���
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
                        Logs($"���棺�Ҳ���ͼƬ '{Path.GetFileName(imageFile)}' ��Ӧ�ı�ǩ�ļ���\r\n");
                    }
                }

                // �����������
                Random random = new Random();
                dataPairs = dataPairs.OrderBy(x => random.Next()).ToList();

                // ���㻮������
                int totalCount = dataPairs.Count;
                int trainCount = (int)(totalCount * 0.7);
                int valCount = (int)(totalCount * 0.2);
                int testCount = totalCount - trainCount - valCount;

                Logs($"����������{totalCount}��ѵ������{trainCount}����֤����{valCount}�����Լ���{testCount}\r\n");

                // �ƶ��ļ���ѵ����
                for (int i = 0; i < trainCount; i++)
                {
                    MoveFileWithOverwriteCheck(dataPairs[i].Item1, Path.Combine(destTrainImagesPath, Path.GetFileName(dataPairs[i].Item1)), "ѵ��ͼƬ");
                    MoveFileWithOverwriteCheck(dataPairs[i].Item2, Path.Combine(destTrainLabelsPath, Path.GetFileName(dataPairs[i].Item2)), "ѵ����ǩ");
                }

                // �ƶ��ļ�����֤��
                for (int i = trainCount; i < trainCount + valCount; i++)
                {
                    MoveFileWithOverwriteCheck(dataPairs[i].Item1, Path.Combine(destValImagesPath, Path.GetFileName(dataPairs[i].Item1)), "��֤ͼƬ");
                    MoveFileWithOverwriteCheck(dataPairs[i].Item2, Path.Combine(destValLabelsPath, Path.GetFileName(dataPairs[i].Item2)), "��֤��ǩ");
                }

                // �ƶ��ļ������Լ�
                for (int i = trainCount + valCount; i < totalCount; i++)
                {
                    MoveFileWithOverwriteCheck(dataPairs[i].Item1, Path.Combine(destTestImagesPath, Path.GetFileName(dataPairs[i].Item1)), "����ͼƬ");
                    MoveFileWithOverwriteCheck(dataPairs[i].Item2, Path.Combine(destTestLabelsPath, Path.GetFileName(dataPairs[i].Item2)), "���Ա�ǩ");
                }

                // �ƶ� classes.txt
                string destClassesFilePath = Path.Combine(dataPath, "classes.txt");
                MoveFileWithOverwriteCheck(classesFilePath, destClassesFilePath, "����ļ�");

                // ���� data/my_dataset.yaml �ļ�
                List<string> classessList = new List<string>();
                classessList.Add( $"train: {Path.Combine("datasets", "my_dataset", "images", "train")}");
                classessList.Add($"val: {Path.Combine("datasets", "my_dataset", "images", "val") }");
                classessList.Add($"test: {Path.Combine("datasets", "my_dataset", "images", "test") }");
                classessList.Add($"nc: {numClasses }");
                classessList.Add($"names: {string.Join(", ", classNames.Select(n => "'" + n + "'"))}");
                
                File.WriteAllLines(dataConfigFilePath, classessList);
                Logs($"�������ݼ������ļ���{dataConfigFilePath}\r\n");

                MessageBox.Show("���ݻ��ֺ��ļ��ƶ���ɣ���鿴������־��", "���", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                Logs($"��������{ex.Message}\r\n");
                MessageBox.Show($"��������{ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void MoveFiles(string sourceImagesPath, string sourceLabelsPath, string destImagesPath, string destLabelsPath, string setName)
        {
            if (Directory.Exists(sourceImagesPath))
            {
                Logs($"��ʼ�ƶ� {setName} ����...\r\n");
                foreach (string imagePath in Directory.GetFiles(sourceImagesPath))
                {
                    string imageNameWithoutExtension = Path.GetFileNameWithoutExtension(imagePath);
                    string imageExtension = Path.GetExtension(imagePath).ToLower();

                    if (imageExtension == ".jpg" || imageExtension == ".jpeg" || imageExtension == ".png")
                    {
                        string destImagePath = Path.Combine(destImagesPath, Path.GetFileName(imagePath));
                        string sourceLabelPath = Path.Combine(sourceLabelsPath, imageNameWithoutExtension + ".txt");
                        string destLabelPath = Path.Combine(destLabelsPath, imageNameWithoutExtension + ".txt");

                        MoveFileWithOverwriteCheck(imagePath, destImagePath, setName + "ͼƬ");

                        if (File.Exists(sourceLabelPath))
                        {
                            MoveFileWithOverwriteCheck(sourceLabelPath, destLabelPath, setName + "��ǩ");
                        }
                        else
                        {
                            Logs($"���棺�Ҳ��� {setName} ͼƬ '{Path.GetFileName(imagePath)}' ��Ӧ�ı�ǩ�ļ���\r\n");
                        }
                    }
                }
                Logs($"{setName} �����ƶ���ɡ�\r\n");
            }
            else if (!string.IsNullOrEmpty(sourceImagesPath))
            {
                Logs($"���棺{setName} ͼƬ·�������ڣ�{sourceImagesPath}\r\n");
            }

            if (!Directory.Exists(sourceLabelsPath) && !string.IsNullOrEmpty(sourceLabelsPath))
            {
                Logs($"���棺{setName} ��ǩ·�������ڣ�{sourceLabelsPath}\r\n");
            }
        }

        private bool? overwriteAll = null; // null ��ʾ��δ������true ��ʾȫ�����ǣ�false ��ʾȫ������

        private void MoveFileWithOverwriteCheck(string sourceFile, string destinationFile, string fileType)
        {
            if (overwriteAll == true)
            {
                try
                {
                    File.Copy(sourceFile, destinationFile, true);
                    Logs($"���� {fileType} �ļ���'{Path.GetFileName(sourceFile)}' -> '{Path.GetFileName(destinationFile)}'\r\n");
                }
                catch (Exception ex)
                {
                    Logs($"�����ƶ� {fileType} �ļ� '{Path.GetFileName(sourceFile)}' ʧ�ܣ�{ex.Message}\r\n");
                }
                return;
            }

            if (overwriteAll == false)
            {
                Logs($"���� {fileType} �ļ���'{Path.GetFileName(sourceFile)}'���û���ѡ���������С�\r\n");
                return;
            }

            if (File.Exists(destinationFile))
            {
                DialogResult result = MessageBox.Show($"Ŀ��λ���Ѵ��� {fileType} �ļ� '{Path.GetFileName(destinationFile)}'���Ƿ񸲸ǣ�\r\n������ǡ����ǵ�ǰ�ļ����������������ǰ�ļ���\r\n�������(ȫ��)���������к�����ͻ�ļ����������(ȫ��)���������к�����ͻ�ļ���", "�ļ��Ѵ���", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        File.Copy(sourceFile, destinationFile, true);
                        Logs($"���� {fileType} �ļ���'{Path.GetFileName(sourceFile)}' -> '{Path.GetFileName(destinationFile)}'\r\n");
                    }
                    catch (Exception ex)
                    {
                        Logs($"�����ƶ� {fileType} �ļ� '{Path.GetFileName(sourceFile)}' ʧ�ܣ�{ex.Message}\r\n");
                    }
                }
                else if (result == DialogResult.No)
                {
                    Logs($"���� {fileType} �ļ���'{Path.GetFileName(sourceFile)}'��\r\n");
                }
                else if (result == DialogResult.Cancel) // ���ǽ� Cancel ��Ϊ��ȫ�����ǡ�
                {
                    overwriteAll = true;
                    try
                    {
                        File.Copy(sourceFile, destinationFile, true);
                        Logs($"���� {fileType} �ļ���'{Path.GetFileName(sourceFile)}' -> '{Path.GetFileName(destinationFile)}'\r\n");
                    }
                    catch (Exception ex)
                    {
                        Logs($"�����ƶ� {fileType} �ļ� '{Path.GetFileName(sourceFile)}' ʧ�ܣ�{ex.Message}\r\n");
                    }
                }
                else if (result == DialogResult.Abort) // ��������Ҫ���һ������(ȫ��)�����߼�������Ҫ�޸� MessageBox �İ�ť
                {
                    overwriteAll = false;
                    Logs($"���� {fileType} �ļ���'{Path.GetFileName(sourceFile)}'���û���ѡ���������С�\r\n");
                }
            }
            else
            {
                try
                {
                    File.Copy(sourceFile, destinationFile, false);
                    Logs($"�ƶ� {fileType} �ļ���'{Path.GetFileName(sourceFile)}' -> '{Path.GetFileName(destinationFile)}'\r\n");
                }
                catch (Exception ex)
                {
                    Logs($"�����ƶ� {fileType} �ļ� '{Path.GetFileName(sourceFile)}' ʧ�ܣ�{ex.Message}\r\n");
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

                Logs($"����Ŀ¼�ṹ�ɹ���{datasetPath}\r\n");
            }
            catch (Exception ex)
            {
                Logs($"����Ŀ¼�ṹʧ�ܣ�{ex.Message}\r\n");
                MessageBox.Show($"����Ŀ¼�ṹʧ�ܣ�{ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}