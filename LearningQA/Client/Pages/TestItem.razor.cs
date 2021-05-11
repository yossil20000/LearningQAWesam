using Castle.Core.Internal;

using LearningQA.Client.ViewModel;
using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using YLBlazor;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;
using System.Globalization;

namespace LearningQA.Client.Pages
{
	public partial class TestItem : ComponentBase
	{
		[Inject]
		IJSRuntime JSRuntime { get; set; }
		[Inject]
		ITestItemViewModel testItemViewModel { get; set; }
		[Parameter] public int testItemId { get; set; }
		[Parameter] public int testId { get; set; }
		private bool IsInitialize { get; set; } = false;
		private CanvasClassJsInterop canvasClassJsInterop = null;
		private string message = "";
		private string drawMessage = "";
		private bool mouseUp = false;
		private bool mouseDown = false;
		private MouseEventArgs lastMouseEventArgs = new MouseEventArgs();
		private MouseEventArgs lastMouseDownEventArgs = new MouseEventArgs();
		private bool newLine = true;
		private bool bRenderSupp = false;
		private bool bImageChanged = false;
		private bool answereExpend { get; set; } = false;
		private bool supplementExpand { get; set; } = true;
		private bool supplementFullExpand { get; set; } = true;
		private bool IsTestMode { get; set; } = true;
		private string SupplementExpandClass = "";
		private string SupplementToggleClass = "";
		public TestItem()
		{

		}
		protected override async  Task OnInitializedAsync()
		{
			canvasClassJsInterop = new CanvasClassJsInterop(JSRuntime);
			await testItemViewModel?.RetriveTestItemInfos(testItemId);
			IsInitialize = true;
			await base.OnInitializedAsync();
			StateHasChanged();
		}
		protected override  async Task OnParametersSetAsync()
		{
			
			if (testItemId > 0)
			{
				
				await testItemViewModel.OnTestItemId(testItemId);
				Console.WriteLine($"OnParametersSetAsync TestIdemId: {testItemId} {ViewModelPersist.SelectedSubjecte}");
				
			}
			else if(testId > 0)
			{				
				Console.WriteLine($"OnParametersSetAsync TestId: {testId} ExamState:{ViewModelPersist.ExamState} ");
			}
			await base.OnParametersSetAsync();

		}
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if(bImageChanged && canvasClassJsInterop != null)
			{
				
				_ = await canvasClassJsInterop.UpdateImage("canvasTestItem", "canvasImgTestItem");
				bImageChanged = false;
			}
		}
		private async Task ShowMessage()
		{
			await canvasClassJsInterop.Prompt("Hi from event");
		}
		private void UpdateImage()
		{
			bImageChanged = true;
			StateHasChanged();
			
		}
		private bool RenderSupp(bool bRenderAlways = false)
		{
			if (canvasClassJsInterop != null && (bRenderSupp == false || bRenderAlways))
			{
				_ = canvasClassJsInterop.InitCanvas("canvasTestItem", "canvasImgTestItem");
				bRenderSupp = true;
				ViewModelPersist.OnChanged(UpdateImage);
			}
				
			return true;
		}
		private void OnAnswerExpandToggle()
		{
			answereExpend = !answereExpend;
		}
		private void OnSupplementExpandToggle()
		{
			supplementExpand = !supplementExpand;
		}
		private void OnSupplementFullExpandToggle()
		{
			supplementFullExpand = !supplementFullExpand;
		}
		private void OnMarkCurrentQuestion()
		{
			ViewModelPersist.MarkCurrentQuestion();
		}
		private bool IsOptionChecked(QUestionSql questionSql, string tenantId)
		{
			//return questionSql?.Options.Where(x => x.TenantId == tenantId).FirstOrDefault()?.IsTrue ?? false;
			try
			{
				if (tenantId.IsNullOrEmpty())
				{
					Console.WriteLine("IsOptionChecked TenantId null or empry");
					return false;

				}
				var answer = ViewModelPersist.CurrentTest.Answers.Where(x => x.QUestionSql.Id == questionSql.Id).FirstOrDefault();
				if(answer != null)
				{
					if(answer.SelectedAnswer == null)
					{
						answer.SelectedAnswer = new List<AnswareOption<int>>();
						Console.WriteLine($"Answer: {questionSql.QuestionNumber} with Id:{answer.Id} in question:{questionSql.Id} in test:{ViewModelPersist.CurrentTest.Id} answerOption was null");
					}

					var selectes = answer.SelectedAnswer.Where(x => x.TenantId == tenantId);
					if(selectes != null)
					return selectes.Any();
				}
				return false;
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.Message);
				return false;
			}
		}
		private async Task ClearCanvas()
		{


			await canvasClassJsInterop.ClearDraw("canvasTestItem");

		}
		private async Task CanvasOnMoseMove(MouseEventArgs ea)
		{
			message = $"Mouse: Client:{lastMouseEventArgs.ClientX} offsetx:{lastMouseEventArgs.OffsetX} ScreenX:{lastMouseEventArgs.ScreenX}  DX:{ea.ClientX - lastMouseEventArgs.ClientX}";
			lastMouseEventArgs = ea;
			//Console.WriteLine(message);
			//if(ea.AltKey)
			//	await canvasClassJsInterop.DrawPreview((int)ea.ClientX, (int)ea.ClientY);
			await Task.CompletedTask;
		}
		private async Task  NewLine()
		{
			newLine = true;
			await canvasClassJsInterop.NewLine("canvasTestItem");

		}
		private async Task ClearDraw()
		{

		}
		private async Task CanvasMouseUp(MouseEventArgs ea)
		{
			mouseUp = true;
			await Task.CompletedTask;
		}
		private async Task CanvasMousedown(MouseEventArgs ea)
		{

			message = $"Mouse Down: Client:{lastMouseEventArgs.ClientX} , {lastMouseEventArgs.ClientY} offsetx:{lastMouseEventArgs.OffsetX}, {lastMouseEventArgs.OffsetY} ScreenX:{lastMouseEventArgs.ScreenX} , {lastMouseEventArgs.ScreenY}  DX:{ea.ClientX - lastMouseEventArgs.ClientX}";

			Console.WriteLine(message); 
			mouseDown = true;
			if (!newLine)
			{
				await Draw(lastMouseDownEventArgs, ea);
			}
			lastMouseDownEventArgs = ea;
			newLine = false;
			await Task.CompletedTask;
		}
		private async Task Draw(MouseEventArgs firstPoint, MouseEventArgs secondPoint)
		{
			try
			{
				drawMessage = $"Draw Line:(({firstPoint.ClientX},{firstPoint.ClientY})({secondPoint.ClientX}, {secondPoint.ClientY}) )";
				Console.WriteLine(drawMessage);
				//await canvasJsInterop.Draw((int)firstPoint.ClientX, (int)firstPoint.ClientY, (int)secondPoint.ClientX, (int)secondPoint.ClientY);
				await Task.CompletedTask;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			

			
		}
	}
}
