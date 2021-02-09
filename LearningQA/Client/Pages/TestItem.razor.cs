using Castle.Core.Internal;

using LearningQA.Client.ViewModel;
using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningQA.Client.Pages
{
	public partial class TestItem : ComponentBase
	{
		[Inject]
		ITestItemViewModel testItemViewModel { get; set; }
		[Parameter] public int testItemId { get; set; }
		[Parameter] public int testId { get; set; }
		private bool IsInitialize { get; set; } = false;
		public TestItem()
		{

		}
		protected override async  Task OnInitializedAsync()
		{
			 await testItemViewModel?.RetriveTestItemInfos(testItemId);
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
		protected override void OnAfterRender(bool firstRender)
		{
			//TestItemViewModelPersist.OnChanged(
			//	() => base.StateHasChanged());
			base.OnAfterRender(firstRender);
		}


		private void OnAnswerExpandToggle()
		{
			answereExpend = !answereExpend;
		}
		private void OnSupplementExpandToggle()
		{
			supplementExpand = !supplementExpand;
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
	}
}
