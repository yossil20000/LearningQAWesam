using LearningQA.Client.PageBase;
using LearningQA.Client.ViewModel;
using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace LearningQA.Client.Pages
{
	public partial class TestList : ComponentBase
	{
		[Inject]
		IExamViewModel ExamVM { get; set; }
		[Inject]
		NavigationManager navigationManager { get; set; }
		[Inject]
		ExamViewModelPersist ExamViewModelPersist { get; set; }
		private bool IsViewExamsList = false;
		private bool IsInitialize { get; set; } = false;
		List<ExamInfoModel> TestsInfo { get; set; } = new List<ExamInfoModel>();
		protected override async Task OnInitializedAsync()
		{
			await ExamVM?.RetriveTestItemInfos(0);
			IsInitialize = true;
			await base.OnInitializedAsync();
			StateHasChanged();
		}
		protected override async Task OnParametersSetAsync()
		{

			await base.OnParametersSetAsync();
		}
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);
		}
		private  async Task OnLoadExams()
		{
			await ExamVM.OnLoadCommand();
			IsViewExamsList = true;
			StateHasChanged();
		}
		private void OnExamSelect(int testId)
		{
			//TestItemViewModelPersist.ExamState = ExamState.ExamReview;
			navigationManager.NavigateTo($"testitem/test/{testId}");
		}
		private void OnAnswerExpandToggle()
		{
			answereExpend = !answereExpend;
		}
		private void OnSupplementExpandToggle()
		{
			supplementExpand = !supplementExpand;
		}
		private bool IsOptionChecked(QUestionSql questionSql, string tenantId)
		{
			//return questionSql?.Options.Where(x => x.TenantId == tenantId).FirstOrDefault()?.IsTrue ?? false;
			try
			{
				if (string.IsNullOrEmpty(tenantId))
				{
					Console.WriteLine("IsOptionChecked TenantId null or empry");
					return false;

				}
				var answer = ExamViewModelPersist.CurrentTest.Answers.Where(x => x.QUestionSql.Id == questionSql.Id).FirstOrDefault();
				if (answer != null)
				{
					if (answer.SelectedAnswer == null)
					{
						answer.SelectedAnswer = new List<AnswareOption<int>>();
						Console.WriteLine($"Answer: {questionSql.QuestionNumber} with Id:{answer.Id} in question:{questionSql.Id} in test:{ExamViewModelPersist.CurrentTest.Id} answerOption was null");
					}

					var selectes = answer.SelectedAnswer.Where(x => x.TenantId == tenantId);
					if (selectes != null)
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
