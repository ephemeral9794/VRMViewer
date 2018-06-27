using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using VRMLoader;

namespace VRMViewer
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void OnFileOpen(object sender, EventArgs e) {
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.FileName = "";
			ofd.InitialDirectory = @".\";
			ofd.Filter = "VRMファイル(*.vrm)|*.vrm|glTF Binaryファイル(*.glb)|*.glb|すべてのファイル(*.*)|*.*";
			ofd.FilterIndex = 1;
			ofd.Title = "開くファイルを選択してください";
			ofd.RestoreDirectory = true;

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				Console.WriteLine(ofd.FileName);
				var model = Loader.Load(ofd.OpenFile());
				var json = Loader.SerializeModelInfo(model.Info);
			}
		}

		private void OnFileClose(object sender, EventArgs e) {

		}

		private void OnQuitApplication(object sender, EventArgs e) {
			Application.Exit();
		}

		private void OnAbout(object sender, EventArgs e) {
			MessageBox.Show("VRM Viewer ver.0.01", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		//glControlの起動時に実行される。
		private void OnLoad(object sender, EventArgs e)
		{
			GL.ClearColor(Color4.White);
			GL.Enable(EnableCap.DepthTest);
		}

		//glControlのサイズ変更時に実行される。
		private void OnResize(object sender, EventArgs e)
		{
			GL.Viewport(0, 0, glControl.Size.Width, glControl.Size.Height);
			GL.MatrixMode(MatrixMode.Projection);
			Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, (float)glControl.Size.Width / (float)glControl.Size.Height, 1.0f, 64.0f);
			GL.LoadMatrix(ref projection);
		}

		//glControlの描画時に実行される。
		private void OnPaint(object sender, PaintEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.MatrixMode(MatrixMode.Modelview);
			Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
			GL.LoadMatrix(ref modelview);

			GL.Begin(PrimitiveType.Quads);

			GL.Color4(Color4.White);
			GL.Vertex3(-1.0f, 1.0f, 4.0f);
			GL.Color4(Color4.Red);
			GL.Vertex3(-1.0f, -1.0f, 4.0f);
			GL.Color4(Color4.Lime);
			GL.Vertex3(1.0f, -1.0f, 4.0f);
			GL.Color4(Color4.Blue);
			GL.Vertex3(1.0f, 1.0f, 4.0f);

			GL.End();
			glControl.SwapBuffers();
		}
	}
}
