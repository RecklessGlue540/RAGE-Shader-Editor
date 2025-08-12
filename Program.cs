using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ShaderEditor
{
	internal class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				if (MessageBox.Show("Usage: ShaderEditor.exe <filename>\nOr you can select a file here.\nSelect OK to launch the file selection dialog.", "RAGE Shader Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					OpenFileDialog openFileDialog = new OpenFileDialog();
					openFileDialog.Filter = "RAGE shader (*.fxc)|*.fxc|RAGE shader .xml (*.xml)|*.xml";
					openFileDialog.FilterIndex = 1;
					openFileDialog.RestoreDirectory = true;

					if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        args = new string[1]; args[0] = openFileDialog.FileName;
                    }
					else
                    {
                        return;
                    }
				}
				else
                {
                    return;
                }
			}
			if (args.Length != 1)
			{
				if (args[0] == "/conv")
				{
					string inputGame = "";
					string outputGame = "";
					string inShader = "";
					string outShader = "";
					for (int a = 1; a < args.Length; a++)
					{
						if (args[a] == "/IG") inputGame = args[a+1];
						else if (args[a] == "/OG") outputGame = args[a + 1];
						else if (args[a] == "/IS") inShader = args[a + 1];
						else if (args[a] == "/OS") outShader = args[a + 1];
					}
					if (inputGame==null|| outputGame == null || inShader == null || outShader == null)
                    {
						throw new Exception("Wrong argument count");
                    }
					if (inputGame == "MaxPayne3" && outputGame == "IV")
					{
						IV.FXC_MP3_TO_IV(inShader, outShader);
					}
					else throw new Exception("Unsupported conversion");
				}
				else
				{
					throw new Exception($"{args[0]} is an unk argument");
				}
			}
			else
			{
				FileInfo.fileName = args[0];
				//BinaryReader br;
				if (FileInfo.fileMask == ".fxc")
				{
					byte[] buffer;
					if (DataUtils.FileExists(FileInfo.fileName)) buffer = File.ReadAllBytes(FileInfo.fileName);
					else throw new Exception($"Usage: ShaderEditor.exe <filename>");
					MemoryStream ms = new MemoryStream(buffer);
					BinaryReader br = new BinaryReader(ms);
					uint magic = br.ReadUInt32();
					br.BaseStream.Position = 0;
					if (FileInfo.fileMask == ".fxc")
					{
						if (magic == 1635280754) // GTAIV & MCLA
                        {
							IV.ReadFXC(br);
                        }
						else if (magic == 1702389618)
						{
							int result = ShaderUtils.CheckMagicMP3(br);

							if (result == 2) // Max Payne 3
							{
								MP3.ReadFXC(br);
							}

						}
					}
				}
				else if (FileInfo.fileMask == ".xml")
				{
					XmlReaderSettings settings = new XmlReaderSettings();
					settings.IgnoreComments = true;
					settings.IgnoreProcessingInstructions = true;
					settings.IgnoreWhitespace = true;
					uint magic;
					XmlReader reader = XmlReader.Create(FileInfo.fileName, settings);
					reader.Read();
					IV.ReadStartElement(reader, "Effect");
					uint.TryParse(reader.ReadStringValue("Magic"), out magic);
					string tempstring = reader.ReadStringValue("Game");

					if (tempstring == "IV")
                    {
                        Settings.game = (int)DataUtils.eGame.IV; IV.ReadXML(reader);
                    }
				}
			}

			//byte[] shader = File.ReadAllBytes("gta_normal_spec_79h_438h.fxc");
			//char[] chars = Encoding.Unicode.GetChars(shader);
			//Console.WriteLine(Add(27, 28));
			//Console.WriteLine(Disassemble(shader, shader.Length));
			/*	string shaderASM = Disassemble(shader, shader.Length);
				File.WriteAllText("shader.asm", shaderASM);

				string shader2 = File.ReadAllText("shader.asm");

				byte[] gg = Encoding.ASCII.GetBytes(File.ReadAllText("shader.asm"));

				char[] shaderTXT = Encoding.ASCII.GetChars(gg);

				byte[] _OutputBuffer = new byte[GetASMSize(shader2, (uint)shaderTXT.Length)];
				Assemble(shader2, (uint)shaderTXT.Length, _OutputBuffer, _OutputBuffer.Length);*/

			//File.WriteAllText("shader.fxc", newShader);
			//File.WriteAllBytes("shader.fxc", _OutputBuffer);

			//Disassemble(shader, (uint)shader.Length, buffer);
		}
	}
}