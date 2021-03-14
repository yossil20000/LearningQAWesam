using Microsoft.JSInterop;

using System;
using System.ComponentModel;
using System.Threading.Tasks;
//https://medium.com/@doomgoober/resizing-canvas-vector-graphics-without-aliasing-7a1f9e684e4d#id_token=eyJhbGciOiJSUzI1NiIsImtpZCI6ImZlZDgwZmVjNTZkYjk5MjMzZDRiNGY2MGZiYWZkYmFlYjkxODZjNzMiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJuYmYiOjE2MTQwNzEzNjcsImF1ZCI6IjIxNjI5NjAzNTgzNC1rMWs2cWUwNjBzMnRwMmEyamFtNGxqZGNtczAwc3R0Zy5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsInN1YiI6IjEwMTM0MzI3ODk5MzI0NTg0NDY4OCIsImVtYWlsIjoieW9zLjE5NjVAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOnRydWUsImF6cCI6IjIxNjI5NjAzNTgzNC1rMWs2cWUwNjBzMnRwMmEyamFtNGxqZGNtczAwc3R0Zy5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsIm5hbWUiOiJZb3NzaSBMZXZ5IiwicGljdHVyZSI6Imh0dHBzOi8vbGgzLmdvb2dsZXVzZXJjb250ZW50LmNvbS9hLS9BT2gxNEdnWmY0NzFQRHp4TmdGWGx2VFVNU0xOSjJ0cWJaSDVYSkV3U1VCTj1zOTYtYyIsImdpdmVuX25hbWUiOiJZb3NzaSIsImZhbWlseV9uYW1lIjoiTGV2eSIsImlhdCI6MTYxNDA3MTY2NywiZXhwIjoxNjE0MDc1MjY3LCJqdGkiOiI0NjE0Y2JmZDkwYTJlNDYxYjE1ZmNkOTZjMDgxNWY0MjI0YmE0ZDk0In0.TD8A-Lh7oJKZpeQRYeh4BtnT_9NKe6MQeoTW3m49sWQISq6KWAJ-CFh5ZK7dtq0eVVqXNGsXegYVrAAJSHUAftWQP5cT--Jen8xVWndyZxFZcg-htwIU5drAKA1NY17VBVfMiY5NULs-hCtLeu0xU0QXi1GOWiGfnGOK0A_ml98xzqVNrZbiT32JESVGnzR8XpLkgCh4ohrOkbESyLur8MMFGWCdUVXeULwrZBzUIuRgMu5LYssvppYBqT54sZevLPEZeIxFgLyXFC3iTJyIPuVdNsMeeOGakvODwcK_KkUwYBUjzR9jJ9Pj1SaJDXtI1R9Q67BAIrZOYrqRN4hzcg


namespace YLBlazor
{
	public class CanvasClassJsInterop : IAsyncDisposable
	{
		private readonly Lazy<Task<IJSObjectReference>> moduleTask;
		public CanvasClassJsInterop(IJSRuntime jsRuntime)
		{
			moduleTask = new (() => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/YLBlazor/canvasclass.js").AsTask());
		}
		public async ValueTask<string> Prompt(string message)
		{
			var module = await moduleTask.Value;
			return await module.InvokeAsync<string>("showPrompt", message);
		}
		public async ValueTask<string> UpdateImage(string canvasId, string imageId)
		{
			var module = await moduleTask.Value;
			return await module.InvokeAsync<string>("setCanvasImage", canvasId, imageId);
		}
		public async ValueTask<string> NewLine(string canvasId)
		{
			var module = await moduleTask.Value;
			return await module.InvokeAsync<string>("OnNewline", canvasId);

		}
		public async ValueTask<string> ClearDraw(string canvasId)
		{
			var module = await moduleTask.Value;
			return await module.InvokeAsync<string>("OnClearDraw", canvasId);
		}
		//public async ValueTask<string> ClearCanvas(bool isConfirm)
		//{
		//	var module = await moduleTask.Value;
		//	return await module.InvokeAsync<string>("OnClearDraw", isConfirm);
		//}
		public async ValueTask<string> InitCanvas(string canvasId, string imageId)
		{
			var module = await moduleTask.Value;
			return await module.InvokeAsync<string>("initCanvas", canvasId, imageId);
		}
		public async ValueTask<string> UnDo(string canvasId)
		{
			var module = await moduleTask.Value;
			return await module.InvokeAsync<string>("OnUnDo", canvasId);
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
