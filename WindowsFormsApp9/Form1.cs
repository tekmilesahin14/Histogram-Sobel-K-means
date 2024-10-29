using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp9
{
    public enum ColorChannel
    {
        Red,
        Green,
        Blue
    }
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button3_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image != null)
            {
                if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() == "Gri Yap")
                {
                    if (comboBox2.SelectedIndex == -1)
                    {
                        MessageBox.Show("Lütfen sayı değerini seçiniz!!");
                        return;
                    }
                    Bitmap image = new Bitmap(pictureBox1.Image);
                    Bitmap gri = griYap(image);
                    pictureBox3.Image = gri;

                    int[] histogramR = CalculateHistogram((Bitmap)pictureBox1.Image, ColorChannel.Red);
                    int[] histogramG = CalculateHistogram((Bitmap)pictureBox1.Image, ColorChannel.Green);
                    int[] histogramB = CalculateHistogram((Bitmap)pictureBox1.Image, ColorChannel.Blue);

                    // Histogramı ekrana yazdır
                    PrintHistogram(histogramR, "Red");
                    PrintHistogram(histogramG, "Green");
                    PrintHistogram(histogramB, "Blue");
                }
                else if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() == "Histogram")
                {
                    if (comboBox2.SelectedIndex == -1)
                    {
                        MessageBox.Show("Lütfen sayı değerini seçiniz!!");
                        return;
                    }
                    Bitmap image = new Bitmap(pictureBox1.Image);
                    Bitmap hist = griYap(image);
                    pictureBox3.Image = hist;

                    int[] histogramR = CalculateHistogram((Bitmap)pictureBox1.Image, ColorChannel.Red);
                    int[] histogramG = CalculateHistogram((Bitmap)pictureBox1.Image, ColorChannel.Green);
                    int[] histogramB = CalculateHistogram((Bitmap)pictureBox1.Image, ColorChannel.Blue);

                    // Histogramı ekrana yazdır
                    PrintHistogram(histogramR, "Red");
                    PrintHistogram(histogramG, "Green");
                    PrintHistogram(histogramB, "Blue");

                }
                else if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() == "Kmeans int")
                {

                    Bitmap image = new Bitmap(pictureBox1.Image);
                    Bitmap kmeansIntUygula= griYap(image);
                    pictureBox3.Image = kmeansIntUygula;

                    int[] histogramR = CalculateHistogram((Bitmap)pictureBox1.Image, ColorChannel.Red);
                    int[] histogramG = CalculateHistogram((Bitmap)pictureBox1.Image, ColorChannel.Green);
                    int[] histogramB = CalculateHistogram((Bitmap)pictureBox1.Image, ColorChannel.Blue);

                    // Histogramı ekrana yazdır
                    PrintHistogram(histogramR, "Red");
                    PrintHistogram(histogramG, "Green");
                    PrintHistogram(histogramB, "Blue");
                }
                else if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() == "Kmeans RGB")
                {

                    if (int.TryParse(comboBox2.SelectedItem?.ToString(), out int kDegeri))
                    {
                        Bitmap image = new Bitmap(pictureBox1.Image);
                        Bitmap kmeansRGB = KMeansrgbUygula(image, kDegeri);
                        pictureBox3.Image = kmeansRGB;
                    }
                    int[] histogramR = CalculateHistogram((Bitmap)pictureBox1.Image, ColorChannel.Red);
                    int[] histogramG = CalculateHistogram((Bitmap)pictureBox1.Image, ColorChannel.Green);
                    int[] histogramB = CalculateHistogram((Bitmap)pictureBox1.Image, ColorChannel.Blue);

                    // Histogramı ekrana yazdır
                    PrintHistogram(histogramR, "Red");
                    PrintHistogram(histogramG, "Green");
                    PrintHistogram(histogramB, "Blue");
                }
                else if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() == "Sobel")
                {
                    if (comboBox2.SelectedIndex == -1)
                    {
                        MessageBox.Show("Lütfen sayı değerini seçiniz!!");
                        return;
                    }

                    if (pictureBox1.Image != null)
                    {
                        Bitmap image = new Bitmap(pictureBox1.Image);
                        Bitmap sobelResult = ApplySobelFilter(image);
                        pictureBox3.Image = sobelResult;

                        int[] histogramR = CalculateHistogram((Bitmap)pictureBox1.Image, ColorChannel.Red);
                        int[] histogramG = CalculateHistogram((Bitmap)pictureBox1.Image, ColorChannel.Green);
                        int[] histogramB = CalculateHistogram((Bitmap)pictureBox1.Image, ColorChannel.Blue);

                        // Histogramı ekrana yazdır
                        PrintHistogram(histogramR, "Red");
                        PrintHistogram(histogramG, "Green");
                        PrintHistogram(histogramB, "Blue");
                    }
                }
            }
        }
        private (Bitmap, int[]) ApplySobelFilter1(Bitmap image)
        {
            Bitmap resultImage = new Bitmap(image.Width, image.Height);

            int[,] sobelX = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] sobelY = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };

            int[] labeledHistogram = new int[256];

            for (int y = 1; y < image.Height - 1; y++)
            {
                for (int x = 1; x < image.Width - 1; x++)
                {
                    int gx = 0, gy = 0;

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            Color pixel = image.GetPixel(x - 1 + j, y - 1 + i);
                            int grayValue = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);

                            gx += sobelX[i, j] * grayValue;
                            gy += sobelY[i, j] * grayValue;
                        }
                    }
             //Kenarların olduğu yerde yüksek değerleri alır,belirgin kenarları vurgular ve diğer kenarları bastırır gradient
                    int gradient = (int)Math.Sqrt(gx * gx + gy * gy);
                    gradient = Math.Min(255, Math.Max(0, gradient));

                    labeledHistogram[gradient]++; // Gri tonlamalı değere dayalı olarak etiketli histogramu güncelle
                    resultImage.SetPixel(x, y, Color.FromArgb(gradient, gradient, gradient));
                }
            }
            return (resultImage, labeledHistogram);
        }
        private Bitmap ConvertToGrayscale(Bitmap original)
        {
            Bitmap grayscaleImage = new Bitmap(original.Width, original.Height);

            for (int x = 0; x < original.Width; x++)
            {
                for (int y = 0; y < original.Height; y++)
                {
                    Color originalColor = original.GetPixel(x, y);
                    int grayValue = (int)(originalColor.R * 0.3 + originalColor.G * 0.59 + originalColor.B * 0.11);
                    Color grayColor = Color.FromArgb(grayValue, grayValue, grayValue);
                    grayscaleImage.SetPixel(x, y, grayColor);
                }
            }
            return grayscaleImage;
        }
        private void DisplayChart(Bitmap image)
        {
            chart1.Series.Clear();

            Series series = new Series("Histogram");
            series.ChartType = SeriesChartType.Column;

            int[] histogram = new int[256];

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int grayValue = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);
                    histogram[grayValue]++;
                }
            }
            for (int i = 0; i < 256; i++)
            {
                series.Points.AddXY(i, histogram[i]);
            }
            chart1.Series.Add(series);
        }

        private Bitmap KMeansrgbUygula(Bitmap image, int k)
        {
            Bitmap resultImage = new Bitmap(image.Width, image.Height);
            Random random = new Random();

            // Rastgele renklerle kümeleri başlat
            Color[] clusterColors = new Color[k];
            for (int i = 0; i < k; i++)
            {
                clusterColors[i] = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            }

            // İterasyon sayısı
            int maxIterations = 100;

            // Her piksel için küme atamaları
            int[,] clusterAssignments = new int[image.Width, image.Height];

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                // Her pikseli en yakın kümeye ata
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        Color pixel = image.GetPixel(x, y);
                        int enYakinKumeIndex = EnYakinKumeBulRGB(pixel, clusterColors);
                        clusterAssignments[x, y] = enYakinKumeIndex;
                    }
                }

                // Küme renklerini güncelle
                for (int kumeIndex = 0; kumeIndex < k; kumeIndex++)
                {
                    int toplamKirmizi = 0, toplamYesil = 0, toplamMavi = 0;
                    int kumeBoyutu = 0;

                    for (int y = 0; y < image.Height; y++)
                    {
                        for (int x = 0; x < image.Width; x++)
                        {
                            if (clusterAssignments[x, y] == kumeIndex)
                            {
                                Color pixel = image.GetPixel(x, y);
                                toplamKirmizi += pixel.R;
                                toplamYesil += pixel.G;
                                toplamMavi += pixel.B;
                                kumeBoyutu++;
                            }
                        }
                    }

                    if (kumeBoyutu > 0)
                    {
                        clusterColors[kumeIndex] = Color.FromArgb(
                            toplamKirmizi / kumeBoyutu,
                            toplamYesil / kumeBoyutu,
                            toplamMavi / kumeBoyutu);
                    }
                }
            }

            // Son renkleri sonuç resmine ata
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    resultImage.SetPixel(x, y, clusterColors[clusterAssignments[x, y]]);
                }
            }

            return resultImage;
        }


        /*private Bitmap KMeansrgbUygula(Bitmap image, int k)
        {
            // K-means kümeleme mantığını burada uygulayabiliriz
            // Örnek: Boş bir sonuç resmi oluşturun
            Bitmap resultImage = new Bitmap(image.Width, image.Height);

            // Örnek: Her küme için rastgele renkler atayın
            Random random = new Random();
            Color[] clusterColors = new Color[k];
            for (int i = 0; i < k; i++)
            {
                clusterColors[i] = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            }
            // Örnek: Her pikseli en yakın küme rengine atayın
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixel = image.GetPixel(x, y);

                    // Bu kısmı gerçek K-means mantığınızla değiştirin
                    int enYakinKumeIndex = EnYakinKumeBulRGB(pixel, clusterColors);

                    // Pikseli en yakın küme rengine atayın
                    resultImage.SetPixel(x, y, clusterColors[enYakinKumeIndex]);
                }
            }
            return resultImage;
        }*/
        private int EnYakinKumeBulRGB(Color pixel, Color[] clusterColors)
        {
            // En yakın küme indeksini bulan gerçek mantığınızla değiştirin
            int enYakinKumeIndex = 0;
            double minMesafe = double.MaxValue;

            for (int i = 0; i < clusterColors.Length; i++)
            {
                double mesafe = RenkMesafesiHesaplaRGB(pixel, clusterColors[i]);
                if (mesafe < minMesafe)
                {
                    minMesafe = mesafe;
                    enYakinKumeIndex = i;
                }
            }
            return enYakinKumeIndex;
        }
        private double RenkMesafesiHesaplaRGB(Color renk1, Color renk2)
        {
            // Gerçek renk mesafesi hesaplama mantığınızla değiştirin
            double mesafe = Math.Sqrt(Math.Pow(renk1.R - renk2.R, 2) + Math.Pow(renk1.G - renk2.G, 2) + Math.Pow(renk1.B - renk2.B, 2));
            return mesafe;
        }
        private Bitmap KMeansIntUygula(Bitmap image, int k)
        {
            // K-means kümeleme mantığını burada uygularız
            // Örnek: Boş bir sonuç resmi oluşturun
            Bitmap resultImage = new Bitmap(image.Width, image.Height);

            // Örnek: Her küme için rastgele renkler atayın
            Random random = new Random();
            Color[] clusterColors = new Color[k];
            for (int i = 0; i < k; i++)
            {
                clusterColors[i] = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            }

            // Örnek: Her pikseli en yakın küme rengine atayın
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixel = image.GetPixel(x, y);

                    // Bu kısmı gerçek K-means mantığınızla değiştirin
                    int enYakinKumeIndex = EnYakinKumeBul(pixel, clusterColors);

                    // Pikseli en yakın küme rengine atayın
                    resultImage.SetPixel(x, y, clusterColors[enYakinKumeIndex]);
                }
            }
            return resultImage;
        }
        private int EnYakinKumeBul(Color pixel, Color[] clusterColors)
        {
            // En yakın küme indeksini bulan gerçek mantığınızla değiştirin
            int enYakinKumeIndex = 0;
            double minMesafe = double.MaxValue;

            for (int i = 0; i < clusterColors.Length; i++)
            {
                double mesafe = RenkMesafesiHesapla(pixel, clusterColors[i]);
                if (mesafe < minMesafe)
                {
                    minMesafe = mesafe;
                    enYakinKumeIndex = i;
                }
            }
            return enYakinKumeIndex;
        }
        private double RenkMesafesiHesapla(Color renk1, Color renk2)
        {
            // Gerçek renk mesafesi hesaplama mantığınızla değiştirin
            return Math.Sqrt(Math.Pow(renk1.R - renk2.R, 2) + Math.Pow(renk1.G - renk2.G, 2) + Math.Pow(renk1.B - renk2.B, 2));
        }
        private int[] CalculateHistogram(Bitmap image, ColorChannel channel)
        {
            int[] histogram = new int[256];

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixel = image.GetPixel(x, y);
                    int value = 0;

                    switch (channel)
                    {
                        case ColorChannel.Red:
                            value = pixel.R;
                            break;
                        case ColorChannel.Green:
                            value = pixel.G;
                            break;
                        case ColorChannel.Blue:
                            value = pixel.B;
                            break;
                    }

                    histogram[value]++;
                }
            }
            return histogram;
        }
        private void PrintHistogram(int[] histogram, string channelName)
        {
            chart1.Series.Clear();
            chart1.Series.Add(channelName);

            for (int i = 0; i < histogram.Length; i++)
            {
                chart1.Series[channelName].Points.AddXY(i, histogram[i]);
            }
            int totalPixels = 0;
            int totalRed = 0, totalGreen = 0, totalBlue = 0;

            Bitmap bmp = (Bitmap)pictureBox1.Image;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color pixelColor = bmp.GetPixel(x, y);

                    totalPixels++;
                    totalRed += pixelColor.R;
                    totalGreen += pixelColor.G;
                    totalBlue += pixelColor.B;
                }
            }
            int averageRed = totalRed / totalPixels;
            int averageGreen = totalGreen / totalPixels;
            int averageBlue = totalBlue / totalPixels;

            listBox1.Items.Clear();
            listBox1.Items.Add("Pixel Count: " + totalPixels);
            listBox1.Items.Add("Average T-R: " + averageRed);
            listBox1.Items.Add("Average T-G: " + averageGreen);
            listBox1.Items.Add("Average T-B: " + averageBlue);

            int totalPixels2 = 0;
            int totalRed2 = 0, totalGreen2 = 0, totalBlue2 = 0;

            Bitmap bmp2 = (Bitmap)pictureBox3.Image;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color pixelColor = bmp.GetPixel(x, y);

                    totalPixels2++;
                    totalRed2 += pixelColor.R;
                    totalGreen2 += pixelColor.G;
                    totalBlue2 += pixelColor.B;
                }
                int averageRed2 = totalRed2 / totalPixels2;
                int averageGreen2 = totalGreen2 / totalPixels2;
                int averageBlue2 = totalBlue2 / totalPixels2;

                listBox2.Items.Clear();
                listBox2.Items.Add("Pixel Count: " + totalPixels2);
                listBox2.Items.Add("Average T-R: " + averageRed2);
                listBox2.Items.Add("Average T-G: " + averageGreen2);
                listBox2.Items.Add("Average T-B: " + averageBlue2);


                label5.Text = "Toplam Piksel:" + totalPixels.ToString() + totalPixels2.ToString();
                label6.Text = totalRed.ToString();
                label7.Text = totalGreen.ToString();
                label8.Text = totalBlue.ToString();

                label11.Text = totalRed2.ToString();
                label10.Text = totalGreen2.ToString();
                label9.Text = totalBlue2.ToString();
            }
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Resim dosyalarını seçmesine izin ver
            openFileDialog.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Tüm Dosyalar|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Seçilen resmi PictureBox'a yükle
                pictureBox1.Image = new Bitmap(openFileDialog.FileName);
            }
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
        }
        private int[] HistogramHesapla(Bitmap resim, ColorChannel kanal)
        {
            int[] histogram = new int[256];

            for (int y = 0; y < resim.Height; y++)
            {
                for (int x = 0; x < resim.Width; x++)
                {
                    Color piksel = resim.GetPixel(x, y);
                    int deger = 0;

                    switch (kanal)
                    {
                        case ColorChannel.Red:
                            deger = piksel.R;
                            break;
                        case ColorChannel.Green:
                            deger = piksel.G;
                            break;
                        case ColorChannel.Blue:
                            deger = piksel.B;
                            break;
                    }
                    histogram[deger]++;
                }
            }
            return histogram;
       }
        private int[] EtiketliHistogramHesapla(Bitmap resim, ColorChannel kanal)
        {
            int[] histogram = new int[256];

            for (int y = 0; y < resim.Height; y++)
            {
                for (int x = 0; x < resim.Width; x++)
                {
                    Color piksel = resim.GetPixel(x, y);
                    int deger = 0;

                    switch (kanal)
                    {
                        case ColorChannel.Red:
                            deger = piksel.R;
                            break;
                        case ColorChannel.Green:
                            deger = piksel.G;
                            break;
                        case ColorChannel.Blue:
                            deger = piksel.B;
                            break;
                    }

                    histogram[deger]++;
                }
            }
            // Etiketli histogramı hesapla
            int[] etiketliHistogram = EtiketliHistogramHesapla(histogram);

            return etiketliHistogram;
        }
        private int[] EtiketliHistogramHesapla(int[] histogram)
        {
            int[] etiketliHistogram = new int[histogram.Length];
            int toplam = 0;

            for (int i = 0; i < histogram.Length; i++)
            {
                toplam += histogram[i];
                etiketliHistogram[i] = toplam;
            }

            return etiketliHistogram;
        }
        private void button2_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image != null)
            {
                Bitmap resim = new Bitmap(pictureBox1.Image);

                if (comboBox1.SelectedItem != null)
                {
                    ColorChannel seciliKanal = ColorChannel.Red; // İhtiyaca göre değiştirilebilir

                    int[] etiketliHistogram;

                    switch (comboBox1.SelectedItem.ToString())
                    {
                        case "Histogram":
                            // Seçilen renk kanalı için etiketli histogramu hesapla
                            etiketliHistogram = EtiketliHistogramHesapla(resim, seciliKanal);
                            break;
                        case "Gri Yap":
                            // Resmi gri tonlamaya çevir
                            Bitmap griResim = griYap(resim);
                            pictureBox3.Image = griResim;

                            // Gri tonlamalı resim için etiketli histogramu hesapla
                            etiketliHistogram = EtiketliHistogramHesapla(griResim, seciliKanal);
                            break;
                        case "Kmeans int":
                            // K-means kümeleme uygula ve sonuç resmi al
                            Bitmap kmeansIntSonucu = KMeansIntUygula(resim, comboBox2.SelectedIndex);
                            pictureBox3.Image = kmeansIntSonucu;

                            // K-means kümeleme sonucu için etiketli histogramu hesapla
                            etiketliHistogram = EtiketliHistogramHesapla(kmeansIntSonucu, seciliKanal);
                            break;
                        case "Kmeans RGB":
                            // RGB uzayında K-means kümeleme uygula (ihtiyaca göre değiştirilebilir)
                            Bitmap kmeansRgbSonucu = KMeansrgbUygula(resim, comboBox2.SelectedIndex);
                            pictureBox3.Image = kmeansRgbSonucu;

                            // K-means kümeleme sonucu için etiketli histogramu hesapla
                            etiketliHistogram = EtiketliHistogramHesapla(kmeansRgbSonucu, seciliKanal);
                            break;
                        case "Sobel":
                            // Sobel filtresini uygula ve Sobel filtrelenmiş resmi ve etiketli histogramı al
                            (Bitmap sobelSonucu, int[] etiketliSobelHistogram) = ApplySobelFilter1(resim);
                            pictureBox3.Image = sobelSonucu;

                            // Sobel histogramını ve etiketli histogramı yazdır
                            int[] sobelHistogramu = HistogramHesapla(sobelSonucu, ColorChannel.Red);

                            etiketliHistogram = etiketliSobelHistogram;
                            break;
                        default:
                            // Diğer durumları ele al (gerekiyorsa)
                            etiketliHistogram = new int[256]; // Varsayılan olarak boş bir histogram
                            break;
                    }
                    HistogramGoster(etiketliHistogram, "Etiketli Histogram", chart2);
                }
            }
        }
        private void HistogramGoster(int[] histogram, string kanalAdi, Chart chart)
        {
            chart.Series.Clear();
            chart.Series.Add(kanalAdi);

            for (int i = 0; i < histogram.Length; i++)
            {
                chart.Series[kanalAdi].Points.AddXY(i, histogram[i]);
            }
        }
        public enum RenkKanali
        {
            Kirmizi,
            Yesil,
            Mavi
        }
        private Bitmap ApplySobelFilter(Bitmap image)
        {
            Bitmap resultImage = new Bitmap(image.Width, image.Height);

            int[,] sobelX = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] sobelY = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };

            int[] labeledHistogram = new int[256];

            for (int y = 1; y < image.Height - 1; y++)
            {
                for (int x = 1; x < image.Width - 1; x++)
                {
                    int gx = 0, gy = 0;

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            Color pixel = image.GetPixel(x - 1 + j, y - 1 + i);
                            int grayValue = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);

                            gx += sobelX[i, j] * grayValue;
                            gy += sobelY[i, j] * grayValue;
                        }
                    }
                    //Kenarların olduğu yerde yüksek değerleri alır,belirgin kenarları vurgular ve diğer kenarları bastırır gradient
                    int gradient = (int)Math.Sqrt(gx * gx + gy * gy);
                    gradient = Math.Min(255, Math.Max(0, gradient));

                    labeledHistogram[gradient]++;
                    resultImage.SetPixel(x, y, Color.FromArgb(gradient, gradient, gradient));
                }
            }
            return resultImage;
        }
        private void button4_Click(object sender, EventArgs e)
        {

            Bitmap image = new Bitmap(pictureBox1.Image);
            Bitmap gri = griYap(image);

            pictureBox3.Image = gri;
        }
        private Bitmap griYap(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {

                    int deger = (bmp.GetPixel(j, i).R + bmp.GetPixel(j, i).G + bmp.GetPixel(j, i).B) / 3;

                    deger = Math.Max(0, Math.Min(deger, 255));

                    Color renk = Color.FromArgb(deger, deger, deger);

                    bmp.SetPixel(j, i, renk);
                }
            }
            return bmp;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
