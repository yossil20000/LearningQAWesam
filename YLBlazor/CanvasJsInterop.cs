using Microsoft.JSInterop;

using System;
using System.ComponentModel;
using System.Threading.Tasks;
//https://medium.com/@doomgoober/resizing-canvas-vector-graphics-without-aliasing-7a1f9e684e4d#id_token=eyJhbGciOiJSUzI1NiIsImtpZCI6ImZlZDgwZmVjNTZkYjk5MjMzZDRiNGY2MGZiYWZkYmFlYjkxODZjNzMiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJuYmYiOjE2MTQwNzEzNjcsImF1ZCI6IjIxNjI5NjAzNTgzNC1rMWs2cWUwNjBzMnRwMmEyamFtNGxqZGNtczAwc3R0Zy5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsInN1YiI6IjEwMTM0MzI3ODk5MzI0NTg0NDY4OCIsImVtYWlsIjoieW9zLjE5NjVAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOnRydWUsImF6cCI6IjIxNjI5NjAzNTgzNC1rMWs2cWUwNjBzMnRwMmEyamFtNGxqZGNtczAwc3R0Zy5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsIm5hbWUiOiJZb3NzaSBMZXZ5IiwicGljdHVyZSI6Imh0dHBzOi8vbGgzLmdvb2dsZXVzZXJjb250ZW50LmNvbS9hLS9BT2gxNEdnWmY0NzFQRHp4TmdGWGx2VFVNU0xOSjJ0cWJaSDVYSkV3U1VCTj1zOTYtYyIsImdpdmVuX25hbWUiOiJZb3NzaSIsImZhbWlseV9uYW1lIjoiTGV2eSIsImlhdCI6MTYxNDA3MTY2NywiZXhwIjoxNjE0MDc1MjY3LCJqdGkiOiI0NjE0Y2JmZDkwYTJlNDYxYjE1ZmNkOTZjMDgxNWY0MjI0YmE0ZDk0In0.TD8A-Lh7oJKZpeQRYeh4BtnT_9NKe6MQeoTW3m49sWQISq6KWAJ-CFh5ZK7dtq0eVVqXNGsXegYVrAAJSHUAftWQP5cT--Jen8xVWndyZxFZcg-htwIU5drAKA1NY17VBVfMiY5NULs-hCtLeu0xU0QXi1GOWiGfnGOK0A_ml98xzqVNrZbiT32JESVGnzR8XpLkgCh4ohrOkbESyLur8MMFGWCdUVXeULwrZBzUIuRgMu5LYssvppYBqT54sZevLPEZeIxFgLyXFC3iTJyIPuVdNsMeeOGakvODwcK_KkUwYBUjzR9jJ9Pj1SaJDXtI1R9Q67BAIrZOYrqRN4hzcg
namespace YLBlazor
{
	// This class provides an example of how JavaScript functionality can be wrapped
	// in a .NET class for easy consumption. The associated JavaScript module is
	// loaded on demand when first needed.
	//
	// This class can be registered as scoped DI service and then injected into Blazor
	// components for use.
	//15/03

	/// <summary>
	/// CanvasJsInterop for manipulate Canvas drawing
	/// </summary>
	public class CanvasJsInterop : IAsyncDisposable
	{
		private readonly Lazy<Task<IJSObjectReference>> moduleTask;

		public CanvasJsInterop(IJSRuntime jsRuntime)
		{
			moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
			   "import", "./_content/YLBlazor/canvas.js").AsTask());
		}

		public async ValueTask<string> Prompt(string message)
		{
			var module = await moduleTask.Value;
			return await module.InvokeAsync<string>("showPrompt", message);
		}
		public async ValueTask<string> UpdateImage(string imageId)
		{
			var module = await moduleTask.Value;
			return await module.InvokeAsync<string>("UpdateImage", imageId);
		}
		
		public async ValueTask<string> NewLine()
		{
			var module = await moduleTask.Value;
			return await module.InvokeAsync<string>("newline");

		}
		public async ValueTask<string> ClearDraw()
		{
			var module = await moduleTask.Value;
			return await module.InvokeAsync<string>("ClearDraw");
		}
		public async ValueTask<string> ClearCanvas(bool isConfirm)
		{
			var module = await moduleTask.Value;
			return await module.InvokeAsync<string>("erase", isConfirm);
		}
		public async ValueTask<string> InitCanvas(string id, string imageId)
		{
			var module = await moduleTask.Value;
			return await module.InvokeAsync<string>("init", id, imageId);
		}
		public async ValueTask<string> Draw(int prevX, int prevY, int currX, int currY)
		{
			try
			{
				var module = await moduleTask.Value;
				return await module.InvokeAsync<string>("draw", prevX, prevY, currX, currY);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return "";
		}
		public async ValueTask<string> DrawPreview(int x,int y)
		{
			var module = await moduleTask.Value;
			return await module.InvokeAsync<string>("drawPreview", x, y);
		}
		public async ValueTask DisposeAsync()
		{
			if (moduleTask.IsValueCreated)
			{
				var module = await moduleTask.Value;
				await module.DisposeAsync();
			}
		}
	}
}
