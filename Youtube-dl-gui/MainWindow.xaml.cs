using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace Youtube_dl_gui
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		string OutputDirectory = Properties.Settings.Default["OutputDirectory"] as string;
		public MainWindow()
		{
			InitializeComponent();
			OutputDirectoryText.Text = OutputDirectory;
			UpdateYoutubeDL();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{


			if (string.IsNullOrEmpty(videoURL.Text))
			{
				System.Windows.Forms.MessageBox.Show("Entrez l'URL de la vidéo à convertir", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			else
			{
				RunConvert(videoURL.Text);
			}
		}

		private void UpdateYoutubeDL()
		{
			var cmd = new ProcessStartInfo();
			cmd.FileName = $@"youtube-dl.exe";
			cmd.Arguments = $@"--update";
			cmd.RedirectStandardInput = true;
			cmd.RedirectStandardOutput = true;
			cmd.CreateNoWindow = true;
			cmd.UseShellExecute = false;
			cmd.WindowStyle = ProcessWindowStyle.Hidden;

			try
			{
				Process.Start(cmd);
			}
			catch (Exception)
			{

			}
		}

		private bool RunConvert(string url)
		{
			if (string.IsNullOrEmpty(url))
			{
				System.Windows.Forms.MessageBox.Show("Entrez l'URL de la vidéo à convertir", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}
			else
			{
				var cmd = new ProcessStartInfo();
				cmd.FileName = $@"youtube-dl.exe";
				cmd.Arguments = $@"--extract-audio --audio-format mp3 {url} -o {OutputDirectory}\%(title)s.%(ext)s";
				cmd.RedirectStandardInput = true;
				cmd.RedirectStandardOutput = true;
				cmd.CreateNoWindow = true;
				cmd.UseShellExecute = false;
				cmd.WindowStyle = ProcessWindowStyle.Hidden;

				try
				{
					Process.Start(cmd);
					return true;
				}
				catch
				{
					return false;
				}
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Process.Start(OutputDirectory);
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			using (var fbd = new FolderBrowserDialog())
			{
				DialogResult result = fbd.ShowDialog();

				if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
				{
					string[] files = Directory.GetFiles(fbd.SelectedPath);
					Properties.Settings.Default["OutputDirectory"] = fbd.SelectedPath;
					Properties.Settings.Default.Save();
					OutputDirectory = fbd.SelectedPath;
					OutputDirectoryText.Text = OutputDirectory;
				}
			}
		}
	}
}
