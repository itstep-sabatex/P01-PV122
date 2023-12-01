using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArchiverDemo
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

		}
        public string FolderPath { get; set; }
		public bool Run(string script, string? workingDirectory = null)
		{
			var proc = new Process();
			proc.StartInfo.FileName = "cmd.exe";
			proc.StartInfo.Arguments = $"/C {script}";
			proc.StartInfo.UseShellExecute = false;
			proc.StartInfo.RedirectStandardOutput = true;
			proc.StartInfo.RedirectStandardError = true;
			if (string.IsNullOrWhiteSpace(workingDirectory))
			{
				proc.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
			}
			else
			{
				proc.StartInfo.WorkingDirectory = workingDirectory;
			}
			proc.OutputDataReceived += (a, b) =>
			{
				if (b.Data != null)
				{
					Console.WriteLine(b.Data);
				}
				
			};
			proc.ErrorDataReceived += (a, b) =>
			{
				if (b.Data != null)
				{

					Console.WriteLine(b.Data);
				}
			};
			proc.StartInfo.CreateNoWindow = true;
			proc.Start();
			proc.BeginOutputReadLine();
			proc.BeginErrorReadLine();
			proc.WaitForExit();
			return proc.ExitCode == 0 || proc.ExitCode == 1000;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			//var thread = new Thread(new ThreadStart(()=> Run($"tar -czvf {FolderPath}.tar.gz  {FolderPath}")));
			//thread.Start();
			//thread.Join();
			Task.Run(() => {
			var folderName = "/"+FolderPath.Substring(System.IO.Path.GetPathRoot(FolderPath).Length).Replace('\\','/');
			string archiveName = folderName.Split('/').Last();
				
				
				//Run($"tar -czvf {FolderPath}.tar.gz  {FolderPath}");
				using (Process process = new Process())
				{
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.FileName = "cmd.exe";
					//process.StartInfo.Arguments = $"/C tar -czvf {archiveName}.tar.gz  {archiveName}";
					process.StartInfo.Arguments = $"/C dir";

					process.StartInfo.UseShellExecute = false;
					process.StartInfo.RedirectStandardError = true;
					process.StartInfo.RedirectStandardOutput = true;
					process.StartInfo.WorkingDirectory = Directory.GetParent(FolderPath).FullName;
					process.OutputDataReceived += (sender, args) =>
					{
						if (args.Data != null)
						{
							Dispatcher.Invoke(() =>
							{
								textBox.AppendText($"{args.Data}\n");
								textBox.ScrollToEnd();
							});
						}
					};
					process.ErrorDataReceived += (sender, args) =>
					{
						if (args.Data != null)
						{
							Dispatcher.Invoke(() =>
							{
								textBox.AppendText($"Error:  {args.Data} \n");
								textBox.ScrollToEnd();
							});
						}

					};


					process.Start();
					process.BeginOutputReadLine();
					process.BeginErrorReadLine();
					process.WaitForExit();
					var result = process.ExitCode;
					Dispatcher.Invoke(() =>
					{
						textBox.AppendText($"End process \n");
						textBox.ScrollToEnd();
					});


				}
			});
		}
	}
}