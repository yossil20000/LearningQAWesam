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

namespace LearningQA.Client.Pages
{
	public partial class TestItem : ComponentBase
	{
		[Inject]
		IJSRuntime jSRuntime { get; set; }
		[Inject]
		ITestItemViewModel testItemViewModel { get; set; }
		[Parameter] public int testItemId { get; set; }
		[Parameter] public int testId { get; set; }
		private bool IsInitialize { get; set; } = false;
		private CanvasJsInterop canvasJsInterop = null;
		private string message = "";
		private string drawMessage = "";
		private bool mouseUp = false;
		private bool mouseDown = false;
		private MouseEventArgs lastMouseEventArgs = new MouseEventArgs();
		private MouseEventArgs lastMouseDownEventArgs = new MouseEventArgs();
		private bool newLine = true;
		private bool bRenderSupp = false;
		public TestItem()
		{

		}
		protected override async  Task OnInitializedAsync()
		{
			 await testItemViewModel?.RetriveTestItemInfos(testItemId);
			 canvasJsInterop = new CanvasJsInterop(jSRuntime);
			//if(testItemId > 0)
			//{
			//	Console.WriteLine($"OnInitializedAsync TestIdemId: {testItemId}");
			//	await testItemViewModel.OnTestItemId(testItemId);
			//}
			//else if(test > 0)
			//{

			//}
			IsInitialize = true;
			
			
		}
		protected override  async Task OnParametersSetAsync()
		{
			
			if (testItemId > 0)
			{
				
				await testItemViewModel.OnTestItemId(testItemId);
				Console.WriteLine($"OnParametersSetAsync TestIdemId: {testItemId} {TestItemViewModelPersist.SelectedSubjecte}");
				
			}
			else if(testId > 0)
			{
				
				Console.WriteLine($"OnParametersSetAsync TestId: {testId} ExamState:{TestItemViewModelPersist.ExamState} ");
				
				//if (testId > 0)
				//{
				//	Console.WriteLine($"OnParametersSetAsync TestId: {testId} ");

				//	await testItemViewModel.OnTestId(testId);
				//	TestItemViewModelPersist.ExamState = ExamState.ExamReview;
				//	StateHasChanged();
				//}
			}
			await base.OnParametersSetAsync();

		}
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			//TestItemViewModelPersist.OnChanged(
			//	() => base.StateHasChanged());
			
			
		}

		private bool RenderSupp()
		{
			if (canvasJsInterop != null && bRenderSupp == false)
			{
				_ = canvasJsInterop.InitCanvas("can", "canvasimg");
				bRenderSupp = true;
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
			TestItemViewModelPersist.MarkCurrentQuestion();
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
				var answer = TestItemViewModelPersist.CurrentTest.Answers.Where(x => x.QUestionSql.Id == questionSql.Id).FirstOrDefault();
				if(answer != null)
				{
					if(answer.SelectedAnswer == null)
					{
						answer.SelectedAnswer = new List<AnswareOption<int>>();
						Console.WriteLine($"Answer: {questionSql.QuestionNumber} with Id:{answer.Id} in question:{questionSql.Id} in test:{TestItemViewModelPersist.CurrentTest.Id} answerOption was null");
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


			await canvasJsInterop.ClearCanvas(false);

		}
		private async Task CanvasOnMoseMove(MouseEventArgs ea)
		{
			message = $"Mouse: Client:{lastMouseEventArgs.ClientX} offsetx:{lastMouseEventArgs.OffsetX} ScreenX:{lastMouseEventArgs.ScreenX}  DX:{ea.ClientX - lastMouseEventArgs.ClientX}";
			lastMouseEventArgs = ea;
			//Console.WriteLine(message);
			await Task.CompletedTask;
		}
		private void NewLine()
		{
			newLine = true;
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
			drawMessage = $"Draw Line:(({firstPoint.ClientX},{firstPoint.ClientY})({secondPoint.ClientX}, {secondPoint.ClientY}) )";
			await canvasJsInterop.Draw((int)firstPoint.ClientX, (int)firstPoint.ClientY, (int)secondPoint.ClientX, (int)secondPoint.ClientY);
			await Task.CompletedTask;
			if (canvasJsInterop != null)
			{
				
			}

			
		}
	}
}
