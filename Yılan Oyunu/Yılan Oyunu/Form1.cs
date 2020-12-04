using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace Yılan_Oyunu
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Keys vKeys); // Tuş kordinasyonu için
        string yon = ""; // yılan yön değiştirdiğinde kuruğunun onu takip etmesi için
        PictureBox[] snakesss = new PictureBox[50]; // Yılanın kuyrukları
        
        private void hareket_Tick(object sender, EventArgs e) {

            
            if (GetAsyncKeyState(Keys.Up) >0) //Yukarı Yön Tuşuna Tıklama
            {
                yon = "Up";
                oyunKontrolTimer.Enabled = true; // Duvara ve kuyruğa çarpamayı Kontrol Etme
                sureTimer.Enabled = true; // Süreyi Başlatıyoruz
            }
            else if(GetAsyncKeyState(Keys.Down) > 0) // Aşağı Yön Tuşu
            {
                yon = "Down";
                oyunKontrolTimer.Enabled = true;
                sureTimer.Enabled = true;
            }
            else if (GetAsyncKeyState(Keys.Right) > 0) // Sağa Yön Tuşu
            {
                yon = "Right";
                oyunKontrolTimer.Enabled = true;
                sureTimer.Enabled = true;
            }
            else if (GetAsyncKeyState(Keys.Left) > 0) // Sol Yön Tuşu
            {
                yon = "Left";
                oyunKontrolTimer.Enabled = true;
                sureTimer.Enabled = true;
            }
            hareketEttir(yon); // yılanı hareket ettiriyoruz
            
        }
        int sira = 0,falx = 0; // sira sırayla kutrukları hareket ettiriyor falx kuyruk eklendiğinde yılanın boyunu oluşturur.
        int konumx, konumy;
        private void hareketEttir(string yon)
        {
            if(sira == uzunluk) 
            {
                sira = 0; // kuyruk yönü bitince başa dönüyor 
                falx = uzunluk; // yeni kuyruk eklenen yılanın boyu
            }

            // Sürekli Hareket Ettirme
            if (yon == "Up")
            {
                snakesss[sira].Location = new Point(konumx, konumy - snakePictureBox.Height); // Yılanı Hareket ettirme
                konumx = snakesss[sira].Location.X; //Yılanın yeni konumu
                konumy = snakesss[sira].Location.Y; // Yılanın yeni konumu
                sira++;
                         
            }
            else if(yon == "Down")
            {
                snakesss[sira].Location = new Point(konumx, konumy + snakePictureBox.Height);
                konumx = snakesss[sira].Location.X;
                konumy = snakesss[sira].Location.Y;
                sira++;
            }
            else if(yon == "Right")
            {
                snakesss[sira].Location = new Point(konumx + snakePictureBox.Width, konumy);
                konumx = snakesss[sira].Location.X;
                konumy = snakesss[sira].Location.Y;
                sira++;
            }
            else if(yon == "Left")
            {
                snakesss[sira].Location = new Point(konumx - snakePictureBox.Width, konumy);
                konumx = snakesss[sira].Location.X;
                konumy = snakesss[sira].Location.Y;
                sira++;
            }
            
        }
        int uzunluk = 3; // Yılanın Uzunluğu
        bool food = false; // Yemek Yedi mi 
        bool oyunBasladiMi = false; // Oyun Başladı mı
        private void yemTimer_Tick(object sender, EventArgs e)
        {
            if(oyunBasladiMi)
            {

                Random rastgele = new Random();

                for (int i = 0; i < uzunluk; i++)
                {
                    if (snakesss[i].Location == yemPictureBox.Location) // Yılan Yemi yedi mi
                    {
                        food = true; //yemek yerse food true olur
                        break;
                    }
                    if (snakePictureBox.Location == yemPictureBox.Location)
                    {
                        food = true;
                    }

                }

                if (food)
                {
                    /// Köşediki yemekleri yeme ///
                    if ((yemPictureBox.Location.X == 0 && yemPictureBox.Location.Y == 0) || (yemPictureBox.Location.X == 575 && yemPictureBox.Location.Y == 0))
                    {
                        toplamPuan += 10;
                    }
                    else if ((yemPictureBox.Location.X == 0 && yemPictureBox.Location.Y == 468) || (yemPictureBox.Location.X == 575 && yemPictureBox.Location.Y == 468))
                    {
                        toplamPuan += 10;
                    }
                    ///////////////

                    /// Saniye başına puan hesaplama ///
                    if (puanSaniye != 0)
                    {
                        if (puanSaniye <= 100) // 100 Saniyeyi geçerse puan verilmez
                        {
                            toplamPuan += (100.0 / puanSaniye);
                        }
                    }
                    else
                    {
                        toplamPuan += 100;
                    }
                    toplamPuan = Math.Round(toplamPuan, 1); // yuvarlama
                    puanLabel.Text = toplamPuan.ToString();

                    puanSaniye = 0;
                    ///////////////////////

                    // Yem için yeni konum belirleme
                    yemm();
                    yemPictureBox.SendToBack();

                    // Yeni yılan kuyruğu ekleme
                    PictureBox kutucuk = new PictureBox();
                    kutucuk.Width = snakePictureBox.Width;
                    kutucuk.Height = snakePictureBox.Height;
                    kutucuk.Image = Properties.Resources.squaree;
                    kutucuk.SizeMode = PictureBoxSizeMode.StretchImage;
                    kutucuk.BringToFront();

                    // Yeni eklenen yılan kuruğunun konumunu belirleme
                    if (yon == "Up")
                    {
                        kutucuk.Location = new Point(snakesss[uzunluk - 1].Location.X, snakesss[uzunluk - 1].Location.Y + snakePictureBox.Height);
                    }
                    else if (yon == "Down")
                    {
                        kutucuk.Location = new Point(snakesss[uzunluk - 1].Location.X, snakesss[uzunluk - 1].Location.Y - snakePictureBox.Height);
                    }
                    else if (yon == "Right")
                    {
                        kutucuk.Location = new Point(snakesss[uzunluk - 1].Location.X - snakesss[uzunluk - 1].Width, snakePictureBox.Location.Y);
                    }
                    else if (yon == "Left")
                    {
                        kutucuk.Location = new Point(snakesss[uzunluk - 1].Location.X + snakesss[uzunluk - 1].Width, snakePictureBox.Location.Y);
                    }


                    panel1.Controls.Add(kutucuk); // Yılan kuyruğunu arayüze ekliyoruz
                    snakesss[uzunluk] = kutucuk;
                    uzunluk += 1; // uzunluk arttı
                     
                }
                food = false;

            }
           
        }
        private void oyunKontrolTimer_Tick(object sender, EventArgs e)
        {
            bool deneme = false; // duvara yada kuyruğa vurdu mu 
            for (int i = 0; i < falx; i++) // duvar kontrölü
            {
                if (snakesss[i].Location.X < 0 || snakesss[i].Location.Y < 0 || snakesss[i].Location.X > 577 || snakesss[i].Location.Y > 470)
                {
                    deneme = true;
                    break;
                }
                
                //////////////////////////
            }

            for(int t = 0; t<falx; t++) // kuyruğa vurdu mu kontrolü
            {
                for (int j = 0; j < falx; j++)
                {
                    if (t != j )
                    {
                        if (snakesss[j].Location == snakesss[t].Location)
                        {
                            deneme = true;
                            break;
                        }
                    }
                }
            }


            if (deneme) // duvara yada kuyruğa vurdu mu 
            {
                gameOver(); // Oyun Biiti 
                falx = 0;
                oyunKontrolTimer.Interval = 5000;
                DialogResult dialog = new DialogResult();
                dialog = MessageBox.Show("Çarptınız ! Oyun Yeniden Başlasın Mı ?", "Yılan Oyunu", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
 
                if (dialog == DialogResult.Yes)
                {
                    refleshGame(); // yeniden Başlatma
                }
                else
                {
                    MessageBox.Show("Oyun Kapatılıyor...","Yılan Oyunu", MessageBoxButtons.OK,MessageBoxIcon.Information);
                    Application.Exit();
                }

                oyunKontrolTimer.Interval = 1;
                

                oyunKontrolTimer.Stop();
            }
        }
        int sayacSaniye = 0,sayacDakika = 0 ; // süre hesaplama

        private void pauseTimer_Tick(object sender, EventArgs e)
        {
            pauseTimer.Interval = 100;
            if (GetAsyncKeyState(Keys.B) > 0) // B ye başınca oyunu durdurma
            {
                
               if((radioButton1.Checked == true || radioButton2.Checked == true) && kisi != "") // kişi kaydetme ve zorluk seçme uyarısı
                {
                    oyunBasladiMi = true;
                    textBox1.Enabled = false;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;


                    panel1.Controls.Remove(pictureBox4); // panelden itemleri kaldırdık
                    panel1.Controls.Remove(label3);
                    panel1.Controls.Remove(label4);
                    panel1.Controls.Remove(label5);
                    panel1.Controls.Remove(label6);
                    konumx = snakePictureBox.Location.X;
                    konumy = snakePictureBox.Location.Y;
                    snakesss[0] = snakePictureBox;
                    snakesss[1] = pictureBox2;
                    snakesss[2] = pictureBox3;

                    pictureBox3.Visible = true;
                    pictureBox2.Visible = true;
                    snakePictureBox.Visible = true;
                    yemPictureBox.Visible = true;

                    yemTimer.Enabled = true;
                    hareket.Enabled = true;
                }
               else
                {
                    pauseTimer.Interval = 5000;
                    MessageBox.Show("Lütfen Önce Oyun Derecesini Seçin ve Adınızı Girerek Kişiyi Kaydet Butonuna Tıklayın !!!","Yılan Oyunu",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    pauseTimer.Interval = 1;
                }

               

            }
            if(oyunBasladiMi == true)
            {
                if (GetAsyncKeyState(Keys.D) > 0) // Oyunu Yeniden Başlatma
                {
                    gameOver();
                    refleshGame();
                    pauseTimer.Interval = 1000;
                }
            }

        }
        int puanSaniye = 0; // saniye Tutma
        double toplamPuan = 0.0; // Toplan Puan
        string kisi = "";
        private void sureTimer_Tick(object sender, EventArgs e)
        {
            sayacSaniye++; // Saniye arttır
            puanSaniye++; // puan için saniye arttırma

            if(sayacSaniye < 60 )
            {
                if(sayacSaniye < 10) // saniye 10 dan küçükse başına sıfır ekleme
                {
                    if(sayacDakika < 10) // Dakika 10 dan küçükse başına 0 ekleme
                    {
                        sureLabel.Text = "0" + sayacDakika.ToString() + ":0" + sayacSaniye;
                    }
                    else
                    {
                        sureLabel.Text = sayacDakika.ToString() + ":0" + sayacSaniye;
                    }
                }
                else
                {
                    if (sayacDakika < 10) // Dakika 10 dan küçükse başına 0 ekleme
                    {
                        sureLabel.Text = "0" + sayacDakika.ToString() + ":" + sayacSaniye;
                    }
                    else
                    {
                        sureLabel.Text = sayacDakika.ToString() + ":" + sayacSaniye; // Labele Süreyi yansıtma
                    }
                }
                
            }
            else
            {
                sayacDakika++; // dakika arttırma
                sayacSaniye = 0;
               if(sayacDakika < 10)
                {
                    sureLabel.Text = "0" + sayacDakika.ToString() + ":0" + sayacSaniye; 
                }
               else
                {
                    sureLabel.Text = sayacDakika.ToString() + ":0" + sayacSaniye;
                }
             
            }

        }

        private void gameOver() // Oyun Bitti Fonksiyonu
        {
            
            dosyayaYaz(); // Dosyaya Kullanıcıyı Yazıyoruz
            yemTimer.Enabled = false;
            hareket.Enabled = false;
            sureTimer.Enabled = false;
            oyunBasladiMi = false;

            

        }

        private void refleshGame() // Oyunu Yeniden başlatma fonksiyonu
        {

            for (int i = 3; i < uzunluk; i++) //Yılanı panelden silme
            {
                panel1.Controls.Remove(snakesss[i]);
                snakesss[i] = null;
            }

            //Değişkenleri Sıfırlıyoruz
            uzunluk = 3;
            yon = "";
            sira = 0;
            food = false;
            sayacSaniye = 0;
            sayacDakika = 0;
            puanSaniye = 0;
            toplamPuan = 0;
            puanLabel.Text = toplamPuan.ToString();
            sureLabel.Text = "00:00";
            oyunBasladiMi = true;
            skorSayac = 0;
            filesRead = false;
            falx = 0;

            yemm(); // Rastgele yem atama
            pictureBox3.Location = new Point(0, 0);
            pictureBox2.Location = new Point(25, 0);
            snakePictureBox.Location = new Point(50, 0);
            konumx = snakePictureBox.Location.X;
            konumy = snakePictureBox.Location.Y;


            yemTimer.Enabled = true;
            hareket.Enabled = true;

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            hareket.Interval = 500; // Oyunun Zorluğuna Göre Yılanın Hıznı Ayarlama
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            hareket.Interval = 200; // Zor Oyun
        }

        private void button3_Click(object sender, EventArgs e) // Kişiyi Kaydetme Butonu
        {
            kisi = textBox1.Text;

            MessageBox.Show(kisi + " Kişisi Başarıyla Kaydedildi !", "Yılan Oyunu", MessageBoxButtons.OK,MessageBoxIcon.Information);

        }

        public static string[] skorlar = new string[50]; // Eski skorları tutan dizi
        int skorSayac = 0; //Kaç tane eski skor olduğunu tutan değişken

        void dosyadanOku() // Txt belgesinden verileri Okuma
        {
            
            string dizin = "C:\\Windows\\Temp\\Yilan_Oyunu_Skorlar.txt"; // Skorları kaydettiğimiz dizin
            if(File.Exists(dizin))
            {
                skorSayac = 0;
                FileStream fs = new FileStream(dizin, FileMode.Open, FileAccess.Read);
                StreamReader sw = new StreamReader(fs);
                string satir = sw.ReadLine();
                while (satir != null)
                {
                    skorlar[skorSayac] = satir; // txt belgesinden okuduğumuz değerleri skorlar dizisinde tuttuk
                    satir = sw.ReadLine();
                    skorSayac++;
                }
                sw.Close();
                fs.Close();
                Array.Resize(ref skorlar, skorSayac);
            }

            filesRead = true;

        }
        bool filesRead = false; // Dosya okundu mu
        void dosyayaYaz() // Dosyaya skorları yazma
        {
            if(filesRead == false) // doosya okundu mu
            {
                dosyadanOku();
            }

            string dizin = "C:\\Windows\\Temp\\Yilan_Oyunu_Skorlar.txt"; // Dizin
            skorSayac++;
            Array.Resize(ref skorlar, skorSayac);
            skorlar[skorSayac - 1] = kisi + " " + sureLabel.Text + " Skor : " + puanLabel.Text;
            System.IO.File.Delete(dizin);

            FileStream fs = new FileStream(dizin, FileMode.OpenOrCreate, FileAccess.Write);

            StreamWriter sw = new StreamWriter(fs);
            for (int i = 0; i < skorSayac; i++)
            {
                sw.WriteLine(skorlar[i]); // skorları tt belgesine yazma
            }

            sw.Flush();
            sw.Close();
            fs.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bu Proje Ramazan HARMANER Tarafından Geliştirilmiştir." +
               "\nİletişim : ramazanharmaner@gmail.com\n" +
               "\nNasıl Oynanır ? \n1-) Oyana Başlamadan önce Zorluk Derecesi Seçilir ve Oyuncu İsmi Girilerek Kaydedilir." +
               "\n2-) B Tuşuna Basılarak Oyun Başlanır. Yine B Tuşuna Basılarak Oyun Durdururup Kaldığı Yerden Devam Ettirilebilir." +
               "\n3-) D Tuşuna Basaak Oyunu Yeniden Başlatabilirsiniz." +
               "\n4-) Oyun Yön Tuşları İle Oynanır." +
               "\n5-) Yılanın Kuyruğuna Ya da Duvara Çarparsanız Oyunu Kaybedersiniz." +
               "\n6-) Köşede ki Yemleri Yemeniz Durumunda Ekstradan 10 Puan Kazanırsınız.",
               "Yılan Oyunu - Geliştirici & Nasıl Oynanır ?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string dizin = "C:\\Windows\\Temp\\Yilan_Oyunu_Skorlar.txt";
            if (File.Exists(dizin))
            {
                System.Diagnostics.Process.Start(dizin); // txt belgesini açma
            }
            else
            {
                MessageBox.Show("Önceki Kayıt Bulunamadı.", "Yılan Oyunu", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void yemm() // yem kordinatlarını belirleme
        {
            Random rastgele = new Random();
            int yatay = rastgele.Next(0, 24);
            int dikey = rastgele.Next(0, 19);
            yemPictureBox.Location = new Point((yemPictureBox.Width * yatay), (yemPictureBox.Height * dikey));
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            yemm(); // yem dağıtma
            konumx = snakePictureBox.Location.X;
            konumy = snakePictureBox.Location.Y;
            snakesss[0] = snakePictureBox;
            snakesss[1] = pictureBox2;
            snakesss[2] = pictureBox3;
            pictureBox3.Visible = false;
            pictureBox2.Visible = false;
            snakePictureBox.Visible = false;
            yemPictureBox.Visible = false;

        }
    }
}
