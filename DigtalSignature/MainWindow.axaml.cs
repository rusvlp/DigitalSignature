using System.Security.Cryptography;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace DigtalSignature;

public partial class MainWindow : Window
{

    public TextBox SourceData;
    public TextBox CheckData;
    public TextBlock ResultTb;
    public Button CheckButton;
    
    public byte[] SignatureBytes;
   // public byte[] HashBytes;
    DSAParameters dsaparams;
    
    public MainWindow()
    {
        InitializeComponent();
        SourceData = this.FindControl<TextBox>("Source");
        CheckData = this.FindControl<TextBox>("Check");
        ResultTb = this.FindControl<TextBlock>("Result");
        CheckButton = this.FindControl<Button>("CheckBtn");

        CheckButton.IsEnabled = false;
    }

    private void SignHandler(object? sender, RoutedEventArgs e)
    {
        SHA1 sha1 = new SHA1CryptoServiceProvider();
        byte[] messagebytes = Encoding.UTF8.GetBytes (SourceData.Text);
        byte[] hashbytes = sha1.ComputeHash(messagebytes);
        DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();
        SignatureBytes = dsa.SignHash(hashbytes, "1.3.14.3.2.26");
        dsaparams = dsa.ExportParameters(false);
        
        CheckButton.IsEnabled = true;
    }

    private void CheckHandler(object? sender, RoutedEventArgs e)
    {
        SHA1 sha1 = new SHA1CryptoServiceProvider();
        byte[] messagebytes = Encoding.UTF8.GetBytes(CheckData.Text);
        byte[] hashbytes = sha1.ComputeHash(messagebytes);
        DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();
        dsa.ImportParameters(dsaparams);
        if (dsa.VerifyHash(hashbytes, "1.3.14.3.2.26", SignatureBytes))
        {
            ResultTb.Text = "Проверка пройдена";
        }
        else
        {
            ResultTb.Text = "Проверка НЕ пройдена";
        }

    }
}